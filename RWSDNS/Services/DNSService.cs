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
        ApiResult Add(ARecordItem item);
        ApiResult Delete(ARecordItem item);


    }
    public class DNSService : IDNSService
    {
        private readonly DNSProvider _provider;
        public DNSService(DNSProvider provider)
        {
            _provider = provider;
        } 
        public ApiResult Add(ARecordItem item)
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

        public ApiResult Delete(ARecordItem item)
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
    }
}
