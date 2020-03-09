using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RWSDNS.Api.Models;
using RWSDNS.Api.Services;

namespace RWSDNS.Api.Controllers
{
    [Route("v1/dns/[controller]")]
    [ApiController]
    public class ARecordController : ControllerBase
    {
        private readonly IDNSService _dns;
        public ARecordController(IDNSService dns)
        {
            _dns = dns;
        }
        [HttpPost]
        public ActionResult<ARecordItem> AddARecord(ARecordItem item)
        {
            var result = _dns.AddARecord(item);
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
        public ActionResult<ARecordItem> DeleteARecord(ARecordItem item)
        {
            var result = _dns.DeleteARecord(item);
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