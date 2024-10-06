using Api.Extensions;
using Core.Identity;
using Core.MediatR;
using Core.Pagination;
using Data.DbContext;
using Data.Options;
using Features.Audit.Get;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services.AddControllers(opt =>
{
    // Require authenticated users. Add a default AuthorizeFilter to all endpoints.
    AuthorizationPolicy policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));

    // In enabled Nullable context, the validation system treats non-nullable parameters or bound properties
    // as if they had a [Required(AllowEmptyStrings = true)] attribute.
    // A missing value for Name in a JSON or form post results in a validation error. Disable it.
    opt.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
}).ConfigureApiBehaviorOptions(opt =>
{
    // Disable automatic HTTP 400 responses for invalid model binding or model validation.
    // Both model binding and model validation occur before the execution of a controller action.
    opt.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddDbContext<LeanAuditorContext>();

// Register the Swagger generator
builder.Services.AddSwagger();

// Register MediatR services
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAuditQueryHandler).Assembly));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

// Register validators
builder.Services.AddValidators();

// Register mappers
builder.Services.AddMappers();

// Register OrderBy mappers
builder.Services.AddOrderByMappers();

// Register pagination service
builder.Services.AddSingleton(typeof(IPaginatedResultFactory<>), typeof(PaginatedResultFactory<>));

// Options pattern
builder.Services.Configure<ConnectionStringOptions>(builder.Configuration.GetSection(
                            ConnectionStringOptions.Section));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(
                            JwtOptions.Section));

builder.Services.AddIdentityServices(builder.Configuration);

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "CorsPolicy",
        builder =>
        {
            // Enable CORS for React client
            builder.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders("WWW-Authenticate");

            // Enable CORS for Angular client
            builder.WithOrigins("https://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders("WWW-Authenticate");
        });
});

// Configure HSTS
// https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl
builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromDays(365);
    options.IncludeSubDomains = true;
    options.Preload = true;
});

// Add global exception handling
builder.Services.AddExceptionHandlers();

var app = builder.Build();

// Configure the HTTP request pipeline

app.UseExceptionHandler();

app.UseSecurityHeaders();

if (app.Environment.IsProduction())
{
    // Set Strict-Transport-Security response header
    app.UseHsts();
}

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();

    // Enable middleware to serve Swagger UI
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    });
}

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Make the implicit Program class public so it can be referenced from tests
public partial class Program { }
