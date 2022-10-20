using Api.DbContexts;
using Api.Extensions;
using Api.Mappers;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

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
            services.AddSingleton<AuditMapper, AuditMapper>();
            services.AddSingleton<AuditListMapper, AuditListMapper>();
            services.AddSingleton<AuditActionMapper, AuditActionMapper>();
            services.AddSingleton<AnswerMapper, AnswerMapper>();
            services.AddSingleton<QuestionMapper, QuestionMapper>();
            services.AddScoped<AuditService, AuditService>();
            services.AddTransient<IPropertyMappingService, PropertyMappingService>();

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
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } else
            {
                // Set Strict-Transport-Security response header
                app.UseHsts();
            }

            app.UseSecurityHeaders();

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
