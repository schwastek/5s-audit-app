using Api.DbContexts;
using Api.Core.Domain;
using Api.Exceptions;
using Api.Extensions;
using Api.Mappers;
using Api.Models;
using Api.Options;
using Api.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services.AddControllers(opt =>
{
    // Require authenticated users. Add a default AuthorizeFilter to all endpoints.
    AuthorizationPolicy policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddDbContext<LeanAuditorContext>();

// Register the Swagger generator
builder.Services.ConfigureSwagger();

// Register MediatR services
builder.Services.AddMediatR(typeof(Program));

// Register mappers
builder.Services.ConfigureMappers();

builder.Services.AddScoped<AuditService, AuditService>();
builder.Services.AddTransient<IPropertyMappingService, PropertyMappingService>();

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
            builder.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders("WWW-Authenticate", "X-Pagination");
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

var app = builder.Build();

// Configure the HTTP request pipeline

app.UseSecurityHeaders();

app.UseMiddleware<ExceptionMiddleware>();

// Handle unexisting endpoints (this middleware doesn't catch exceptions)
app.UseStatusCodePagesWithReExecute("/errors/{0}");

if (app.Environment.IsProduction())
{
    // Set Strict-Transport-Security response header
    app.UseHsts();
}

app.UseHttpsRedirection();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger - ui(HTML, JS, CSS, etc.)
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");

    // Serve UI at `localhost:port` instead of `localhost:port/swagger`
    c.RoutePrefix = string.Empty;
});

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// Initialize data

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        if (app.Environment.IsDevelopment())
        {
            var context = services.GetRequiredService<LeanAuditorContext>();
            var userManager = services.GetRequiredService<UserManager<User>>();

            context.Database.Migrate();

            await UserDataInitializer.Seed(userManager);
            await SampleDataInitializer.Seed(context);
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

app.Run();

// Make the implicit Program class public so it can be referenced from tests
public partial class Program { }