using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Managers;
using API.Managers.Interfaces;
using API.Models;
using Business;
using Business.Interfaces;
using Database;
using Database.Queries;
using Database.Queries.Interfaces;
using Database.Tables.Identity;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TwoFactorAuthNet;

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
            string securityKey = Configuration.GetSection("Secrets:AuthenicationKey")?.Value;
            string twoFactorKey = Configuration.GetSection("Secrets:TwoFactorKey")?.Value;
            string issuerUrl = Configuration.GetSection("Settings:Authentication:IssuerUrl")?.Value;
            string issuer = Configuration.GetSection("Settings:Authentication:Issuer")?.Value;

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                    Array.Empty<string>()
                    }
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: AllowedOriginsAPI,
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:5001", "https://www.mad-world.nl", "https://mad-world.nl")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });

            services.AddDbContext<AuthenticationContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("AuthenticationContext"), b => b.MigrationsAssembly("API")));

            services.AddDefaultIdentity<User>()
                        .AddEntityFrameworkStores<AuthenticationContext>();

            services.AddIdentityServer()
                        .AddApiAuthorization<User, AuthenticationContext>();

            services.AddAuthentication()
                        .AddIdentityServerJwt()
                        .AddJwtBearer(options =>
                        {
                            options.SaveToken = true;
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuer = true,
                                ValidateAudience = true,
                                ValidateLifetime = true,
                                ValidateIssuerSigningKey = true,

                                ValidIssuer = issuerUrl,
                                ValidAudience = issuerUrl,
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey))
                            };
                        });

            services.AddDbContext<MadWorldContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("MadWorldContext"), b => b.MigrationsAssembly("API")));

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });

            AddApplicationClassesToScope(services);

            services.AddScoped<IAuthenticationManager, AuthenticationManager>(serviceProvider =>
            {
                SignInManager<User> signInManager = serviceProvider.GetService<SignInManager<User>>();
                UserManager<User> userManager = serviceProvider.GetService<UserManager<User>>();
                IUserExtremeManager userExtremeManager = serviceProvider.GetService<IUserExtremeManager>();
                TwoFactorAuth twoFactorAuth = serviceProvider.GetService<TwoFactorAuth>();
                return new AuthenticationManager(issuerUrl, twoFactorAuth, securityKey, userExtremeManager, signInManager, userManager);
            });

            //Extern packages
            services.AddScoped<TwoFactorAuth, TwoFactorAuth>(serviceProvider => {
                return new TwoFactorAuth(twoFactorKey);
            });

            ApiSettings.SetSettings(issuer);
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
            //API managers
            services.AddScoped<IAccountManager, AccountManager>();

            //Business project
            services.AddScoped<IStatusManager, StatusManager>();
            services.AddScoped<IResumeManager, ResumeManager>();
            services.AddScoped<IUserExtremeManager, UserExtremeManager>();

            //Database project
            services.AddScoped<IAccountQueries, AccountQueries>();
            services.AddScoped<IGeneralQueries, GeneralGueries>();
            services.AddScoped<IResumeQueries, ResumeQueries>();
        }
    }
}
