using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Draken.API.Infrastructure
{
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var requiredScopes = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<ProducesResponseTypeAttribute>()
                .Distinct();

            var bearerScheme = new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            };

            if (requiredScopes.Any(x => x.StatusCode == 401))
            {
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    { bearerScheme, new List<string>() }
                });
            }
        }
    }
}
