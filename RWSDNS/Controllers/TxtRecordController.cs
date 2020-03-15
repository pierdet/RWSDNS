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
    public class TxtRecordController : ControllerBase
    {
        private readonly IDNSService _dns;
        public TxtRecordController(IDNSService dns)
        {
            _dns = dns;
        }
        [HttpPost]
        public ActionResult<TxtRecordItem> AddTxtRecord(TxtRecordItem item)
        {
            var result = _dns.AddTxtRecord(item);
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
        public ActionResult<TxtRecordItem> DeleteTxtRecord(TxtRecordItem item)
        {
            var result = _dns.DeleteTxtRecord(item);
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