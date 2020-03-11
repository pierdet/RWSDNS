using RWSDNS.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Threading.Tasks;

namespace RWSDNS.Api.Common
{
    public class DNSProvider
    {

        // @Todo dispose of ManagementObjectSearcher in methods
        public ApiResult DeleteARecord(string zone, string hostname, string ipAddress)
        {
            var mgmtScope = new ManagementScope(@"\\.\Root\MicrosoftDNS");
            ManagementObjectSearcher mgmtSearch = null;
            ManagementObjectCollection mgmtDNSRecords = null;

            string strQuery = string.Format("SELECT * FROM MicrosoftDNS_AType WHERE OwnerName = '{0}.{1}'", hostname, zone);

            mgmtScope.Connect();

            mgmtSearch = new ManagementObjectSearcher(mgmtScope, new ObjectQuery(strQuery));

            mgmtDNSRecords = mgmtSearch.Get();

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
            // Thanks https://blog.mikejmcguire.com/2014/06/15/creating-and-updating-dns-records-in-microsoft-dns-servers-with-c-net-and-wmi/!
            var mgmtScope = new ManagementScope(@"\\.\Root\MicrosoftDNS");
            ManagementBaseObject mgmtParams = null;
            string strQuery = string.Format("SELECT * FROM MicrosoftDNS_AType WHERE OwnerName = '{0}.{1}'", hostname, zone);

            mgmtScope.Connect();
            var mgmtSearch = new ManagementObjectSearcher(mgmtScope, new ObjectQuery(strQuery));
            var mgmtDNSRecords = mgmtSearch.Get();

            // Multiple A records with the same record name, but different IPv4 addresses, skip and return error.
            if (mgmtDNSRecords.Count > 1)
            {
                return new ApiResult { Success = false };
            }
            // Existing A record found, update record.
            else if (mgmtDNSRecords.Count == 1)
            {
                foreach (ManagementObject mgmtDNSRecord in mgmtDNSRecords)
                {
                    if (mgmtDNSRecord["RecordData"].ToString() != ipAddress)
                    {
                        mgmtParams = mgmtDNSRecord.GetMethodParameters("Modify");
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
                var mgmtClass = new ManagementClass(mgmtScope, new ManagementPath("MicrosoftDNS_AType"), null);

                mgmtParams = mgmtClass.GetMethodParameters("CreateInstanceFromPropertyData");
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
            var mgmtScope = new ManagementScope(@"\\.\Root\MicrosoftDNS");
            ManagementBaseObject mgmtParams = null;
            string strQuery = string.Format("SELECT * FROM MicrosoftDNS_CNAMEType WHERE OwnerName = '{0}.{1}'", hostname, zone);

            mgmtScope.Connect();
            var mgmtSearch = new ManagementObjectSearcher(mgmtScope, new ObjectQuery(strQuery));
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
                        mgmtParams = mgmtDNSRecord.GetMethodParameters("Modify");
                        mgmtParams["PrimaryName"] = primaryName;

                        mgmtDNSRecord.InvokeMethod("Modify", mgmtParams, null);
                    }
                    break;
                }
                return new ApiResult { Success = true };
            }
            else
            {
                var mgmtClass = new ManagementClass(mgmtScope, new ManagementPath("MicrosoftDNS_CNAMEType"), null);

                mgmtParams = mgmtClass.GetMethodParameters("CreateInstanceFromPropertyData");
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
            var mgmtScope = new ManagementScope(@"\\.\Root\MicrosoftDNS");
            string strQuery = string.Format("SELECT * FROM MicrosoftDNS_CNAMEType WHERE OwnerName = '{0}.{1}'", hostname, zone);

            mgmtScope.Connect();
            var mgmtSearch = new ManagementObjectSearcher(mgmtScope, new ObjectQuery(strQuery));
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
            var mgmtScope = new ManagementScope(@"\\.\Root\MicrosoftDNS");
            ManagementBaseObject mgmtParams = null;
            string strQuery = string.Format("SELECT * FROM MicrosoftDNS_TXTType WHERE OwnerName = '{0}.{1}'", hostname, zone);

            mgmtScope.Connect();
            var mgmtSearch = new ManagementObjectSearcher(mgmtScope, new ObjectQuery(strQuery));
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
                        mgmtParams = mgmtDNSRecord.GetMethodParameters("Modify");
                        mgmtParams["DescriptiveText"] = descriptiveText;

                        mgmtDNSRecord.InvokeMethod("Modify", mgmtParams, null);
                    }
                    break;
                }
                return new ApiResult { Success = true };
            }
            else
            {
                var mgmtClass = new ManagementClass(mgmtScope, new ManagementPath("MicrosoftDNS_TXTType"), null);

                mgmtParams = mgmtClass.GetMethodParameters("CreateInstanceFromPropertyData");
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
            var mgmtScope = new ManagementScope(@"\\.\Root\MicrosoftDNS");
            string strQuery = string.Format("SELECT * FROM MicrosoftDNS_TXTType WHERE OwnerName = '{0}.{1}'", hostname, zone);

            mgmtScope.Connect();
            var mgmtSearch = new ManagementObjectSearcher(mgmtScope, new ObjectQuery(strQuery));
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
            var mgmtScope = new ManagementScope(@"\\.\Root\MicrosoftDNS");
            ManagementBaseObject mgmtParams = null;
            string strQuery = string.Format("SELECT * FROM MicrosoftDNS_Zone WHERE ContainerName = '{0}'", zone);

            mgmtScope.Connect();
            var mgmtSearch = new ManagementObjectSearcher(mgmtScope, new ObjectQuery(strQuery));
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
                // @Todo - Fix, doesn't work now.
                var mgmtClass = new ManagementClass(mgmtScope, new ManagementPath("MicrosoftDNS_Zone"), null);

                mgmtParams = mgmtClass.GetMethodParameters("CreateZone");
                mgmtParams["ZoneName"] = zone;
                mgmtParams["ZoneType"] = 0;

                mgmtClass.InvokeMethod("CreateZone", mgmtParams, null);

                return new ApiResult { Success = true };
            }
        }
        public ApiResult DeleteDnsZone(string zone)
        {
            var mgmtScope = new ManagementScope(@"\\.\Root\MicrosoftDNS");
            string strQuery = string.Format("SELECT * FROM MicrosoftDNS_Zone WHERE ContainerName = '{0}'", zone);

            mgmtScope.Connect();
            var mgmtSearch = new ManagementObjectSearcher(mgmtScope, new ObjectQuery(strQuery));
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
    }
}
