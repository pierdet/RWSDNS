using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

namespace RWSDNS.Api.OpenApi
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCustomApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(
                options =>
                {
                    options.ReportApiVersions = true;
                    options.DefaultApiVersion = new ApiVersion(1,0);
                    options.AssumeDefaultVersionWhenUnspecified = true;
                }
            );

            services.AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });
            
            return services;
        }
        
        public static IServiceCollection AddOpenApi(this IServiceCollection services)
        {
            services.ConfigureOptions<ConfigureSwaggerOptions>();
            services.AddSwaggerGen(
                options =>
                {
                    options.OperationFilter<SwaggerDefaultValues>();
                });

            return services;
        }

        public static IApplicationBuilder UseOpenApi(this IApplicationBuilder app)
        {
            var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                foreach(var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });

            return app;
        }
    }
}