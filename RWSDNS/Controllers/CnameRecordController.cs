﻿using Microsoft.AspNetCore.Mvc;
using RWSDNS.Api.Auth;
using RWSDNS.Api.Models;
using RWSDNS.Api.Services;

namespace RWSDNS.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/dns/[controller]")]
    [ApiController]
    [ApiKeyAuth]
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