using Api.Contracts.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Api.Extensions;

public static class AddSwaggerServiceExtensions
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API V1", Version = "v1" });

            // Generate `operationId` based on the action name which is the method name.
            // Alternatively, you can add `Name` parameter to [HttpGet].
            c.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["action"]}");

            // camelCase for parameters, "Id" -> "id".
            // Better for generating TypeScript models and services of request parameters.
            c.DescribeAllParametersInCamelCase();

            // `string? Name` => `{ "nullable": true }` in Swagger schema file.
            c.SupportNonNullableReferenceTypes();

            // Enables support for nullable object properties (e.g. nullable Enums).
            c.UseAllOfToExtendReferenceSchemas();

            // Mark non-nullable properties as required.
            c.SchemaFilter<RequireNonNullablePropertiesSchemaFilter>();

            // Set the comments path for the Swagger JSON and UI
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);

            // Add authorization support
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Place to add JWT with Bearer",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Name = "Bearer"
                    },
                    new List<string>()
                }
            });
        });
    }
}

public class AddSwaggerRequiredSchemaFilter : ISchemaFilter
{
    // TODO: Remove this filter along with attribute.
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var properties = context.Type.GetProperties();

        foreach (var property in properties)
        {
            var attribute = property.GetCustomAttribute(typeof(SwaggerRequiredAttribute));

            if (attribute is not null)
            {
                var propertyNameCamelCase = char.ToLowerInvariant(property.Name[0]) + property.Name.Substring(1);
                schema.Required.Add(propertyNameCamelCase);
            }
        }
    }
}

public class RequireNonNullablePropertiesSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var nonNullableProperties = schema.Properties
            .Where(x => !x.Value.Nullable)
            .Select(x => x.Key);

        foreach (var property in nonNullableProperties)
        {
            schema.Required.Add(property);
        }
    }
}
