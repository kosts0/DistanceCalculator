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
                ["latitude"] = new OpenApiDouble(55.594692), 
                ["longitude"] = new OpenApiDouble(37.427561)
            },
            ["pointB"] = new OpenApiObject
            {
                ["latitude"] = new OpenApiDouble(55.762242),
                ["longitude"] = new OpenApiDouble(37.404249)
            }
        };
    }
}
