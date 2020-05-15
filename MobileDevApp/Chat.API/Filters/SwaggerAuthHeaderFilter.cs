using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Chat.API.Filters
{
    public class SwaggerAuthHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.CustomAttributes().Any(a => a.GetType().Equals(typeof(AllowAnonymousAttribute))))
                return;

            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Authentication",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema { Type = "string" },
                Required = true
            });
        }
    }
}
