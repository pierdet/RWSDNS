using Microsoft.AspNetCore.Mvc;
using RWSDNS.Api.Auth;
using RWSDNS.Api.Models;
using RWSDNS.Api.Models.Response;
using RWSDNS.Api.Services;

namespace RWSDNS.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/dns/[controller]")]
    [ApiController]
    [ApiKeyAuth]
    public class ARecordController : ControllerBase
    {
        private readonly IDNSService _dns;
        public ARecordController(IDNSService dns)
        {
            _dns = dns;
        }
        [HttpPost]
        public IActionResult AddARecord(ARecordItem item)
        {
            var result = _dns.AddARecord(item);
            if (result.Success)
            {
                return Ok(new StatusMessageResponse<ARecordItem>(item, $"A record {item.Hostname} was added!"));
            }
            else
            {
                return NotFound(new StatusMessageResponse<ARecordItem>(item, $"Failed to add {item.Hostname}."));
            }
        }

        [HttpDelete]
        public ActionResult<ARecordItem> DeleteARecord(ARecordItem item)
        {
            var result = _dns.DeleteARecord(item);
            if (result.Success)
            {
                return Ok(new StatusMessageResponse<ARecordItem>(item, $"A record {item.Hostname} was deleted!"));
            }
            else
            {
                return NotFound(new StatusMessageResponse<ARecordItem>(item, $"Failed to delete {item.Hostname}."));
            }
        }
    }
}