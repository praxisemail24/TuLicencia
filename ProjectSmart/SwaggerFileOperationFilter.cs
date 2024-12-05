using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace SmartLicencia
{
    public class SwaggerFileOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Busca parámetros de tipo IFormFile o IEnumerable<IFormFile>
            var fileParameters = context.MethodInfo.GetParameters()
                .Where(p => p.ParameterType == typeof(IFormFile) || p.ParameterType == typeof(IEnumerable<IFormFile>))
                .ToList();

            if (fileParameters.Any())
            {
                operation.Parameters.Clear();

                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["multipart/form-data"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                Properties = fileParameters.ToDictionary(
                                    param => param.Name,
                                    param => new OpenApiSchema
                                    {
                                        Type = "string",
                                        Format = "binary"
                                    }),
                                Required = new HashSet<string>(fileParameters.Select(p => p.Name))
                            }
                        }
                    }
                };
            }
        }
    }

}
