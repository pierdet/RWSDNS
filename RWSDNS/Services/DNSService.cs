using RWSDNS.Api.Common;
using RWSDNS.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Threading.Tasks;

namespace RWSDNS.Api.Services
{
    public interface IDNSService
    {
        ApiResult AddARecord(ARecordItem item);
        ApiResult DeleteARecord(ARecordItem item);
        ApiResult AddCnameRecord(CnameRecordItem item);
        ApiResult DeleteCnameRecord(CnameRecordItem item);


    }
    public class DNSService : IDNSService
    {
        private readonly DNSProvider _provider;
        public DNSService(DNSProvider provider)
        {
            _provider = provider;
        } 
        public ApiResult AddARecord(ARecordItem item)
        {
            var result = _provider.UpdateARecord(item.Zone, item.Hostname, item.IPAddress);
            if (result.Success)
            {
                return new ApiResult { Success = true };
            }
            else
            {
                return new ApiResult { Success = false };
            }
        }

        public ApiResult AddCnameRecord(CnameRecordItem item)
        {
            var result = _provider.UpdateCnameRecord(item.Zone, item.Hostname, item.PrimaryName);
            if (result.Success)
            {
                return new ApiResult { Success = true };
            }
            else
            {
                return new ApiResult { Success = false };
            }
        }

        public ApiResult DeleteARecord(ARecordItem item)
        {
            var result = _provider.DeleteARecord(item.Zone, item.Hostname, item.IPAddress);
            if (result.Success)
            {
                return new ApiResult { Success = true };
            }
            else
            {
                return new ApiResult { Success = false };
            }
        }

        public ApiResult DeleteCnameRecord(CnameRecordItem item)
        {
            var result = _provider.DeleteCnameRecord(item.Zone, item.Hostname, item.PrimaryName);
            if (result.Success)
            {
                return new ApiResult { Success = true };
            }
            else
            {
                return new ApiResult { Success = false };
            }
        }
    }
}
