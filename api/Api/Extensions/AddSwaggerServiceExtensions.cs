using Api.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
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

            // Add common responses to all endpoints.
            c.OperationFilter<CommonResponsesOperationFilter>();

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

public class RequireNonNullablePropertiesSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties is null) return;

        var nonNullableProperties = schema.Properties
            .Where(x => !x.Value.Nullable)
            .Select(x => x.Key);

        foreach (var property in nonNullableProperties)
        {
            schema.Required.Add(property);
        }
    }
}

public class CommonResponsesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Responses.TryAdd(
            StatusCodes.Status400BadRequest.ToString(),
            new OpenApiResponse
            {
                Description = "Bad Request",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    [MediaTypeNames.Application.Json] = new OpenApiMediaType
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(typeof(CustomValidationProblemDetails), context.SchemaRepository)
                    }
                }
            }
        );

        operation.Responses.TryAdd(
            StatusCodes.Status500InternalServerError.ToString(),
            new OpenApiResponse
            {
                Description = "Internal Server Error",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    [MediaTypeNames.Application.Json] = new OpenApiMediaType
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(typeof(ProblemDetails), context.SchemaRepository)
                    }
                }
            }
        );

        // Check if the controller or method has [Authorize] attribute.
        var authorizeAttributes = context.MethodInfo.DeclaringType?.GetCustomAttributes(true)
            .Union(context.MethodInfo.GetCustomAttributes(true))
            .OfType<AuthorizeAttribute>()
            .ToList() ?? [];

        // Add 401 Unauthorized response to all operations that are decorated with the [Authorize] attribute.
        if (authorizeAttributes.Count != 0)
        {
            operation.Responses.TryAdd(
                StatusCodes.Status401Unauthorized.ToString(),
                new OpenApiResponse
                {
                    Description = "Unauthorized"
                }
            );
        }
    }
}
