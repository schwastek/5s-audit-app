using Api.Extensions;
using Data.DbContext;
using Data.Options;
using Features.Core.Identity;
using Features.Core.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services.AddControllers(opt =>
{
    // In enabled Nullable context, the validation system treats non-nullable parameters or bound properties
    // as if they had a [Required(AllowEmptyStrings = true)] attribute.
    // A missing value for Name in a JSON or form post results in a validation error. Disable it.
    opt.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
}).ConfigureApiBehaviorOptions(opt =>
{
    // Disable automatic HTTP 400 responses for invalid model binding or model validation.
    // Both model binding and model validation occur before the execution of a controller action.
    // If a non-nullable string with a default value of `null!` is missing in a POST request,
    // it triggers a validation error, leaving the property null.
    // If a required string is missing, a binding error (JSON deserialization failure) occurs,
    // causing the entire request model to be null in the controller action.
    opt.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddAuthorization(opt =>
{
    // Requires all users to be authenticated by default.
    // See: https://learn.microsoft.com/en-us/aspnet/core/security/authorization/secure-data?view=aspnetcore-8.0#require-authenticated-users
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    // The DefaultPolicy is the policy used with the [Authorize] attribute when no policy name is specified.
    opt.DefaultPolicy = policy;
});

builder.Services.AddDbContext<LeanAuditorContext>();

// Register the Swagger generator
builder.Services.AddSwagger();

// Register Mediator services
builder.Services.AddMediator();

// Register validators
builder.Services.AddValidators();

// Register mappers
builder.Services.AddMappers();

// Register OrderBy mappers
builder.Services.AddOrderByMappers();

// Register pagination service
builder.Services.AddTransient(typeof(IPaginatedResultFactory<>), typeof(PaginatedResultFactory<>));

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
    // Once a browser receives the HSTS header from a server, it remembers the setting for the specified max-age duration.
    // During this period, the browser will enforce HTTPS for all requests to the domain,
    // even if the server stops sending the HSTS header or runs in an environment where HTTPS isn't configured.
    options.MaxAge = TimeSpan.FromDays(365);
    // The HSTS policy should also apply to all subdomains.
    options.IncludeSubDomains = true;
    // If preload is set, the browser will always make HTTPS requests, even on the first request.
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
    // Only communicate with the server over HTTPS.
    // The HSTS specification discourages applying HSTS policies to localhost, because the HSTS settings are highly cacheable by browsers.
    // Once cached, reversing HSTS settings requires manual intervention (clearing browser's cache),
    // which can disrupt testing with HTTP or switching between different configurations, domains, or ports.
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
