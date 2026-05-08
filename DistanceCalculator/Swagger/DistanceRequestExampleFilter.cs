using DistanceCalculator.Dtos;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DistanceCalculator.Swagger;

public class DistanceRequestExampleFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(DistanceRequest))
            return;

        schema.Example = new OpenApiObject
        {
            ["pointA"] = new OpenApiObject
            {
                ["latitude"] = new OpenApiDouble(55.7558),
                ["longitude"] = new OpenApiDouble(37.6173)
            },
            ["pointB"] = new OpenApiObject
            {
                ["latitude"] = new OpenApiDouble(59.9343),
                ["longitude"] = new OpenApiDouble(30.3351)
            }
        };
    }
}
