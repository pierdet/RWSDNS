using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWSDNS.Api.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
    {
        private const string ApiKeyHeaderName = "ApiKey";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentialApiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var hashedPotentialApiKey = KeyHash.GetStringSha256Hash(potentialApiKey);

            var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = config.GetValue<string>(key: "ApiKey");
            if (!apiKey.Equals(hashedPotentialApiKey))
            {
                context.Result = new UnauthorizedResult();
                return; 
            }

            await next();
        }
    }
}
