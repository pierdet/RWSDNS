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
    // @Todo rename to ARecordController
    [Route("v1/[controller]")]
    [ApiController]
    public class DnsController : ControllerBase
    {
        private readonly IDNSService _dns;
        public DnsController(IDNSService dns)
        {
            _dns = dns;
        }
        [HttpPost]
        public ActionResult<ARecordItem> AddARecord(ARecordItem item)
        {
            var result = _dns.Add(item);
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
            var result = _dns.Delete(item);
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