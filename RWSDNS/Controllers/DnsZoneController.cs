using Microsoft.AspNetCore.Mvc;
using RWSDNS.Api.Auth;
using RWSDNS.Api.Models;
using RWSDNS.Api.Services;

namespace RWSDNS.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/dns/[controller]")]
    [ApiController]
    [ApiKeyAuth]
    public class DnsZoneController : ControllerBase
    {
        private readonly IDNSService _dns;
        public DnsZoneController(IDNSService dns)
        {
            _dns = dns;
        }

        [HttpPost]
        public ActionResult<DnsZoneItem> AddDnsZone(DnsZoneItem item)
        {
            var result = _dns.AddDnsZone(item);
            if (result.Success)
            {
                return Ok(item);
            }
            else
            {
                return NotFound(item);
            }
        }
        [HttpDelete]
        public ActionResult<DnsZoneItem> DeleteDnsZone(DnsZoneItem item)
        {
            var result = _dns.DeleteDnsZone(item);
            if (result.Success)
            {
                return Ok(item);
            }
            else
            {
                return NotFound(item);
            }
        }
    }
}