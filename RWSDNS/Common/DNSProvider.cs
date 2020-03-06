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
        public ApiResult DeleteARecord(string zone, string hostname, string ipAddress)
        {
            ManagementScope mgmtScope = new ManagementScope(@"\\.\Root\MicrosoftDNS");
            ManagementClass mgmtClass = null;
            ManagementBaseObject mgmtParams = null;
            ManagementObjectSearcher mgmtSearch = null;
            ManagementObjectCollection mgmtDNSRecords = null;
            string strQuery;

            strQuery = string.Format("SELECT * FROM MicrosoftDNS_AType WHERE OwnerName = '{0}.{1}'", hostname, zone);

            mgmtScope.Connect();

            mgmtSearch = new ManagementObjectSearcher(mgmtScope, new ObjectQuery(strQuery));

            mgmtDNSRecords = mgmtSearch.Get();

            // Multiple A records with the same record name, but different IPv4 addresses, skip.
            if (mgmtDNSRecords.Count > 1)
            {
                // @Todo - Implement Delete functionality
                return new ApiResult { Success = false };
            }
            // Existing A record found, update record.
            else if (mgmtDNSRecords.Count == 1)
            {
                // Todo - Implement Delete functionality
                return new ApiResult { Success = false };
            }
            // A record does not exist, create new record.
            else
            {
                // Todo - Implement Delete functionality
                return new ApiResult { Success = false };
            }
        }
        public ApiResult UpdateARecord(string zone, string hostname, string ipAddress)
        {
            // Thanks https://blog.mikejmcguire.com/2014/06/15/creating-and-updating-dns-records-in-microsoft-dns-servers-with-c-net-and-wmi/!
            var mgmtScope = new ManagementScope(@"\\.\Root\MicrosoftDNS");
            ManagementBaseObject mgmtParams = null;
            string strQuery = $"SELECT * FROM MicrosoftDNS_AType WHERE OwnerName = {hostname}.{zone}";

            mgmtScope.Connect();
            var mgmtSearch = new ManagementObjectSearcher(mgmtScope, new ObjectQuery(strQuery));
            var mgmtDNSRecords = mgmtSearch.Get();

            // Multiple A records with the same record name, but different IPv4 addresses, skip.
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
            // A record does not exist, create new record.
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
    }
}
