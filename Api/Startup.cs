using Api.DbContexts;
using Api.Domain;
using Api.Extensions;
using Api.Mappers;
using Api.Models;
using Api.Services;
using Api.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Api.Exceptions;
using MediatR;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(opt =>
            {
                // Require authenticated users. Add a default AuthorizeFilter to all endpoints.
                AuthorizationPolicy policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddDbContext<LeanAuditorContext>();

            // Register MediatR services
            services.AddMediatR(typeof(Startup));

            // Mappers - Audits
            services.AddSingleton<IMapper<Audit, AuditDto>, AuditMapper>();
            services.AddSingleton<IMapper<AuditForCreationDto, Audit>, AuditMapper>();
            services.AddSingleton<IMapper<Audit, AuditListDto>, AuditMapper>();

            // Mappers - Questions
            services.AddSingleton<IMapper<Question, QuestionDto>, QuestionMapper>();

            // Mappers - Answers
            services.AddSingleton<IMapper<Answer, AnswerDto>, AnswerMapper>();
            services.AddSingleton<IMapper<AnswerForCreationDto, Answer>, AnswerMapper>();

            // Mappers - Actions
            services.AddSingleton<IMapper<AuditAction, AuditActionDto>, AuditActionMapper>();
            services.AddSingleton<IMapper<AuditActionForCreationDto, AuditAction>, AuditActionMapper>();

            // Mappers - Universal Service
            services.AddSingleton<IMappingService, ServiceLocatorMappingService>();

            services.AddScoped<AuditService, AuditService>();
            services.AddTransient<IPropertyMappingService, PropertyMappingService>();

            // Options pattern
            services.Configure<ConnectionStringOptions>(Configuration.GetSection(
                                        ConnectionStringOptions.Section));
            services.Configure<JwtOptions>(Configuration.GetSection(
                                        JwtOptions.Section));

            services.AddIdentityServices(Configuration);

            // Enable CORS
            services.AddCors(options =>
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
            services.AddHsts(options =>
            {
                options.MaxAge = TimeSpan.FromDays(365);
                options.IncludeSubDomains = true;
                options.Preload = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSecurityHeaders();

            app.UseMiddleware<ExceptionMiddleware>();

            // Handle unexisting endpoints (this middleware doesn't catch exceptions)
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            if (env.IsProduction())
            {
                // Set Strict-Transport-Security response header
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
