using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System;
using Api.Mappers;
using Api.Core.Domain;
using Api.Models;

namespace Api.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API V1", Version = "v1" });

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

    public static void ConfigureMappers(this IServiceCollection services)
    {
        // Universal service
        services.AddSingleton<IMappingService, ServiceLocatorMappingService>();

        // For Audits
        services.AddSingleton<IMapper<Audit, AuditDto>, AuditMapper>();
        services.AddSingleton<IMapper<AuditForCreationDto, Audit>, AuditMapper>();
        services.AddSingleton<IMapper<IEnumerable<Audit>, IEnumerable<AuditListDto>>, AuditMapper>();

        // For Questions
        services.AddSingleton<IMapper<Question, QuestionDto>, QuestionMapper>();

        // For Answers
        services.AddSingleton<IMapper<Answer, AnswerDto>, AnswerMapper>();
        services.AddSingleton<IMapper<AnswerForCreationDto, Answer>, AnswerMapper>();

        // For Actions
        services.AddSingleton<IMapper<AuditAction, AuditActionDto>, AuditActionMapper>();
        services.AddSingleton<IMapper<AuditActionForCreationDto, AuditAction>, AuditActionMapper>();
    }
}
