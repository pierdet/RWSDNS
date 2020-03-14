using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RWSDNS.Api.Auth;
using RWSDNS.Api.Auth.Tools;
using RWSDNS.Api.Models;
using RWSDNS.Api.Models.Response;

namespace RWSDNS.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth/[controller]")]
    [ApiController]
    [ApiKeyAuth]
    public class ApiKeyController : ControllerBase
    {
        
        [HttpPost]
        public IActionResult ChangeKey(ApiKeyItem item)
        {
            var hashedKey = KeyHash.GetStringSha256Hash(item.ApiKey);
            var result = ConfigurationWriter.AddOrUpdateAppSetting("ApiKey", hashedKey);
            if (result.Success)
            {
                return Ok(new StatusMessageResponse<ApiKeyItem>(item, "Successfully changed Api Key"));
            }

            return NotFound(new StatusMessageResponse<ApiKeyItem>(item, "Failed to write to configuration file"));
        }
    }
}