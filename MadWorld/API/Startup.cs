using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business;
using Business.Interfaces;
using Database;
using Database.Queries;
using Database.Queries.Interfaces;
using Database.Tables.Identity;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace API
{
    public class Startup
    {
        private readonly string AllowedOriginsAPI = "AllowedCalls";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: AllowedOriginsAPI,
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:5001");
                    });
            });

            services.AddDbContext<AuthenticationContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("AuthenticationContext"), b => b.MigrationsAssembly("API")));

            services.AddDefaultIdentity<User>()
                        .AddEntityFrameworkStores<AuthenticationContext>();

            services.AddIdentityServer()
                        .AddApiAuthorization<User, AuthenticationContext>();

            services.AddAuthentication()
                        .AddIdentityServerJwt();

            services.AddDbContext<MadWorldContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("MadWorldContext"), b => b.MigrationsAssembly("API")));

            AddApplicationClassesToScope(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();
            app.UseIdentityServer();

            app.UseCors(AllowedOriginsAPI);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AddApplicationClassesToScope(IServiceCollection services)
        {
            //Business project
            services.AddScoped<IResumeManager, ResumeManager>();

            //Database project
            services.AddScoped<IResumeQueries, ResumeQueries>();
        }
    }
}
