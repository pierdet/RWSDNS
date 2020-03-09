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
    public class CnameRecordController : ControllerBase
    {
        private readonly IDNSService _dns;
        public CnameRecordController(IDNSService dns)
        {
            _dns = dns;
        }
        [HttpPost]
        public ActionResult<CnameRecordItem> AddCnameRecord(CnameRecordItem item)
        {
            var result = _dns.AddCnameRecord(item);
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
        public ActionResult DeleteCnameRecord(CnameRecordItem item)
        {
            var result = _dns.DeleteCnameRecord(item);
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