using RWSDNS.Api.Models;
using System;
using System.Management;

namespace RWSDNS.Api.Common
{
    public class DNSProvider
    {
        public ApiResult DeleteARecord(string zone, string hostname, string ipAddress)
        {
            string strQuery = string.Format("SELECT * FROM MicrosoftDNS_AType WHERE OwnerName = '{0}.{1}'", hostname, zone);

            using var mgmtSearch = CreateManagementObjectSearcher(strQuery);

            var mgmtDNSRecords = mgmtSearch.Get();

            // Multiple A records with the same record name, but different IPv4 addresses, skip.
            if (mgmtDNSRecords.Count >= 1)
            {
                foreach (ManagementObject mgmtDNSRecord in mgmtDNSRecords)
                {
                    mgmtDNSRecord.Delete();
                }
                return new ApiResult { Success = true };
            }
            
            // A record does not exist, return error.
            else
            {
                return new ApiResult { Success = false };
            }
        }
        public ApiResult UpdateARecord(string zone, string hostname, string ipAddress)
        {
            string strQuery = string.Format("SELECT * FROM MicrosoftDNS_AType WHERE OwnerName = '{0}.{1}'", hostname, zone);

            using var mgmtSearch = CreateManagementObjectSearcher(strQuery);
            var mgmtDNSRecords = mgmtSearch.Get();

            // Multiple A records with the same record name, but different IPv4 addresses, skip and return error.
            if (mgmtDNSRecords.Count > 1)
            {
                return new ApiResult { Success = false };
            }
            // Existing A record found, update record.

            if (mgmtDNSRecords.Count == 1)
            {
                foreach (ManagementObject mgmtDNSRecord in mgmtDNSRecords)
                {
                    if (mgmtDNSRecord["RecordData"].ToString() != ipAddress)
                    {
                        var mgmtParams = mgmtDNSRecord.GetMethodParameters("Modify");
                        mgmtParams["IPAddress"] = ipAddress;

                        mgmtDNSRecord.InvokeMethod("Modify", mgmtParams, null);
                    }

                    break;
                }
                return new ApiResult { Success = true };
            }
            // A record does not exist, create new record and return success.
            else
            {
                var mgmtClass = new ManagementClass(mgmtSearch.Scope, new ManagementPath("MicrosoftDNS_AType"), null);

                var mgmtParams = mgmtClass.GetMethodParameters("CreateInstanceFromPropertyData");
                mgmtParams["DnsServerName"] = Environment.MachineName;
                mgmtParams["ContainerName"] = zone;
                mgmtParams["OwnerName"] = string.Format("{0}.{1}", hostname.ToLower(), zone);
                mgmtParams["IPAddress"] = ipAddress;

                mgmtClass.InvokeMethod("CreateInstanceFromPropertyData", mgmtParams, null);
                
                return new ApiResult { Success = true };
            }
        }
        
        public ApiResult UpdateCnameRecord(string zone, string hostname, string primaryName)
        {
            string strQuery = string.Format("SELECT * FROM MicrosoftDNS_CNAMEType WHERE OwnerName = '{0}.{1}'", hostname, zone);

            using var mgmtSearch = CreateManagementObjectSearcher(strQuery);
            var mgmtDNSRecords = mgmtSearch.Get();
            if(mgmtDNSRecords.Count > 1)
            {
                return new ApiResult { Success = false };
            }
            else if(mgmtDNSRecords.Count == 1)
            {
                foreach (ManagementObject mgmtDNSRecord in mgmtDNSRecords)
                {
                    if(mgmtDNSRecord["RecordData"].ToString() != primaryName)
                    {
                        var mgmtParams = mgmtDNSRecord.GetMethodParameters("Modify");
                        mgmtParams["PrimaryName"] = primaryName;

                        mgmtDNSRecord.InvokeMethod("Modify", mgmtParams, null);
                    }
                    break;
                }
                return new ApiResult { Success = true };
            }
            else
            {
                var mgmtClass = new ManagementClass(mgmtSearch.Scope, new ManagementPath("MicrosoftDNS_CNAMEType"), null);

                var mgmtParams = mgmtClass.GetMethodParameters("CreateInstanceFromPropertyData");
                mgmtParams["DnsServerName"] = Environment.MachineName;
                mgmtParams["ContainerName"] = zone;
                mgmtParams["OwnerName"] = string.Format("{0}.{1}", hostname.ToLower(), zone);
                mgmtParams["PrimaryName"] = primaryName;

                mgmtClass.InvokeMethod("CreateInstanceFromPropertyData", mgmtParams, null);

                return new ApiResult { Success = true };
            }
        }

        public ApiResult DeleteCnameRecord(string zone, string hostname, string primaryName)
        {
            string strQuery = string.Format("SELECT * FROM MicrosoftDNS_CNAMEType WHERE OwnerName = '{0}.{1}'", hostname, zone);

            using var mgmtSearch = CreateManagementObjectSearcher(strQuery);
            var mgmtDNSRecords = mgmtSearch.Get();
            if (mgmtDNSRecords.Count >= 1)
            {
                foreach (ManagementObject mgmtDNSRecord in mgmtDNSRecords)
                {
                    mgmtDNSRecord.Delete();
                }
                return new ApiResult { Success = true };
            }

            // A record does not exist, return error.
            else
            {
                return new ApiResult { Success = false };
            }
        }

        public ApiResult UpdateTxtRecord(string zone, string hostname, string descriptiveText)
        {
            string strQuery = string.Format("SELECT * FROM MicrosoftDNS_TXTType WHERE OwnerName = '{0}.{1}'", hostname, zone);

            using var mgmtSearch = CreateManagementObjectSearcher(strQuery);
            var mgmtDNSRecords = mgmtSearch.Get();
            if (mgmtDNSRecords.Count > 1)
            {
                return new ApiResult { Success = false };
            }
            else if (mgmtDNSRecords.Count == 1)
            {
                foreach (ManagementObject mgmtDNSRecord in mgmtDNSRecords)
                {
                    if (mgmtDNSRecord["RecordData"].ToString() != descriptiveText)
                    {
                        var mgmtParams = mgmtDNSRecord.GetMethodParameters("Modify");
                        mgmtParams["DescriptiveText"] = descriptiveText;

                        mgmtDNSRecord.InvokeMethod("Modify", mgmtParams, null);
                    }
                    break;
                }
                return new ApiResult { Success = true };
            }
            else
            {
                var mgmtClass = new ManagementClass(mgmtSearch.Scope, new ManagementPath("MicrosoftDNS_TXTType"), null);

                var mgmtParams = mgmtClass.GetMethodParameters("CreateInstanceFromPropertyData");
                mgmtParams["DnsServerName"] = Environment.MachineName;
                mgmtParams["ContainerName"] = zone;
                mgmtParams["OwnerName"] = string.Format("{0}.{1}", hostname.ToLower(), zone);
                mgmtParams["DescriptiveText"] = descriptiveText;

                mgmtClass.InvokeMethod("CreateInstanceFromPropertyData", mgmtParams, null);

                return new ApiResult { Success = true };
            }
        }

        public ApiResult DeleteTxtRecord(string zone, string hostname, string descriptiveText)
        {
            string strQuery = string.Format("SELECT * FROM MicrosoftDNS_TXTType WHERE OwnerName = '{0}.{1}'", hostname, zone);

            using var mgmtSearch = CreateManagementObjectSearcher(strQuery);
            var mgmtDNSRecords = mgmtSearch.Get();
            if (mgmtDNSRecords.Count >= 1)
            {
                foreach (ManagementObject mgmtDNSRecord in mgmtDNSRecords)
                {
                    mgmtDNSRecord.Delete();
                }
                return new ApiResult { Success = true };
            }

            // A record does not exist, return error.
            else
            {
                return new ApiResult { Success = false };
            }
        }

        public ApiResult AddDnsZone(string zone)
        {
            string strQuery = string.Format("SELECT * FROM MicrosoftDNS_Zone WHERE ContainerName = '{0}'", zone);

            using var mgmtSearch = CreateManagementObjectSearcher(strQuery);
            var mgmtDNSRecords = mgmtSearch.Get();
            if (mgmtDNSRecords.Count > 1)
            {
                return new ApiResult { Success = false };
            }
            else if (mgmtDNSRecords.Count == 1)
            {
                return new ApiResult { Success = false };
            }
            else
            {
                var mgmtClass = new ManagementClass(mgmtSearch.Scope, new ManagementPath("MicrosoftDNS_Zone"), null);

                var mgmtParams = mgmtClass.GetMethodParameters("CreateZone");
                mgmtParams["ZoneName"] = zone;
                mgmtParams["ZoneType"] = 0;

                mgmtClass.InvokeMethod("CreateZone", mgmtParams, null);

                return new ApiResult { Success = true };
            }
        }
        public ApiResult DeleteDnsZone(string zone)
        {
            string strQuery = string.Format("SELECT * FROM MicrosoftDNS_Zone WHERE ContainerName = '{0}'", zone);

            using var mgmtSearch = CreateManagementObjectSearcher(strQuery);
            
            var mgmtDNSRecords = mgmtSearch.Get();
            if (mgmtDNSRecords.Count >= 1)
            {
                foreach (ManagementObject mgmtDNSRecord in mgmtDNSRecords)
                {
                    mgmtDNSRecord.Delete();
                }
                return new ApiResult { Success = true };
            }

            // A record does not exist, return error.
            else
            {
                return new ApiResult { Success = false };
            }
        }


        private ManagementObjectSearcher CreateManagementObjectSearcher(string query)
        {
            var mgmtScope = new ManagementScope(@"\\.\Root\MicrosoftDNS");
            mgmtScope.Connect();
            return new ManagementObjectSearcher(mgmtScope, new ObjectQuery(query));
        }
    }
}
