using System;
using System.IO;
using System.Linq;
using System.Text;
using API.Managers;
using API.Managers.Interfaces;
using API.Models;
using API.SignalR;
using Business;
using Business.Interfaces;
using Business.Models.PlanningPoker;
using Business.PlanningPoker;
using Business.PlanningPoker.Interfaces;
using Datalayer.Database;
using Datalayer.Database.Logging;
using Datalayer.Database.Queries;
using Datalayer.Database.Queries.Interfaces;
using Datalayer.Database.Tables.Identity;
using Datalayer.FileStorage;
using Datalayer.FileStorage.Interfaces;
using Datalayer.FileStorage.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Exporter;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using TwoFactorAuthNet;

namespace API
{
    public class Startup
    {
        private readonly string AllowedOriginsAPI = "AllowedCalls";

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        /// <summary>
        /// This method to add services to the container
        /// </summary>
        /// <remarks>This method gets called by the runtime. Use this method to add services to the container.</remarks>
        public void ConfigureServices(IServiceCollection services)
        {
            string securityKey = Configuration.GetSection("Secrets:AuthenicationKey")?.Value;
            string twoFactorKey = Configuration.GetSection("Secrets:TwoFactorKey")?.Value;
            string issuerUrl = Configuration.GetSection("Settings:Authentication:IssuerUrl")?.Value;
            string issuer = Configuration.GetSection("Settings:Authentication:Issuer")?.Value;
            string apiUrl = Configuration.GetSection("ApiSettings:Url")?.Value;

            services.AddSignalR();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
                c.AddServer(new OpenApiServer { Url = apiUrl, Description = "Mad-World" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.OperationFilter<SecurityRequirementsOperationFilter>();

                string apiDocumentationPath = Path.Combine(AppContext.BaseDirectory, "API.xml");
                c.IncludeXmlComments(apiDocumentationPath);
            });

            var exporter = this.Configuration.GetValue<string>("UseExporter").ToLowerInvariant();
            switch (exporter)
            {
                case "jaeger":
                    services.AddOpenTelemetryTracing(
                            (builder) => builder
                                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(this.Configuration.GetValue<string>("Jaeger:ServiceName")))
                                .AddAspNetCoreInstrumentation()
                                .AddHttpClientInstrumentation()
                                .AddJaegerExporter(opts =>
                                {
                                    opts.AgentHost = Configuration["Jaeger:AgentHost"];
                                    opts.AgentPort = Convert.ToInt32(Configuration["Jaeger:AgentPort"]);
                                }));

                    services.Configure<JaegerExporterOptions>(this.Configuration.GetSection("Jaeger"));
                    break;
                default:
                    services.AddOpenTelemetryTracing((builder) => builder
                                .AddAspNetCoreInstrumentation()
                                .AddHttpClientInstrumentation()
                                .AddConsoleExporter());

                    // For options which can be bound from IConfiguration.
                    services.Configure<AspNetCoreInstrumentationOptions>(this.Configuration.GetSection("AspNetCoreInstrumentation"));

                    // For options which can be configured from code only.
                    services.Configure<AspNetCoreInstrumentationOptions>(options =>
                    {
                        options.Filter = (req) =>
                        {
                            return true;
                        };
                    });

                    break;
            }

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

            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });

            services.AddDbContext<AuthenticationContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("AuthenticationContext"), b => b.MigrationsAssembly("API")));

            services.AddDefaultIdentity<User>()
                        .AddRoles<IdentityRole>()
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

            services.AddScoped<IAuthenticationManager, AuthenticationManager>(serviceProvider =>
            {
                SignInManager<User> signInManager = serviceProvider.GetService<SignInManager<User>>();
                UserManager<User> userManager = serviceProvider.GetService<UserManager<User>>();
                IUserExtremeManager userExtremeManager = serviceProvider.GetService<IUserExtremeManager>();
                TwoFactorAuth twoFactorAuth = serviceProvider.GetService<TwoFactorAuth>();
                return new AuthenticationManager(issuerUrl, twoFactorAuth, securityKey, userExtremeManager, signInManager, userManager);
            });

            //Extern packages
            services.AddScoped<TwoFactorAuth, TwoFactorAuth>(serviceProvider =>
            {
                return new TwoFactorAuth(twoFactorKey);
            });

            var optionsBuilder = new DbContextOptionsBuilder<MadWorldContext>();
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("MadWorldContext"), b => b.MigrationsAssembly("API"));

            AddApplicationClassesToScope(services, optionsBuilder);

            services.AddLogging(logginerBuilder =>
            {
                logginerBuilder.AddMadWorldLogger(optionsBuilder.Options, MadWorldLoggerConfiguration.GetConfig())
                .AddConsole();
            });

            ApiSettings.SetSettings(apiUrl, issuer);
        }

        /// <summary>
        /// Configure the HTTP request pipeline
        /// </summary>
        /// <remarks>This method gets called by the runtime. Use this method to configure the HTTP request pipeline.</remarks>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            string urlSwaggerJson = $"{ApiSettings.Url}swagger/v1/swagger.json";

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(urlSwaggerJson, "API v1");
                c.DocExpansion(DocExpansion.None);
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();
            app.UseIdentityServer();

            app.UseResponseCompression();
            app.UseCors(AllowedOriginsAPI);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<PlanningPokerHub>("/PlanningPoker");
            });
        }

        private void AddApplicationClassesToScope(IServiceCollection services, DbContextOptionsBuilder<MadWorldContext> builderOptions)
        {
            //Logging
            services.AddScoped(_ => MadWorldLoggerConfiguration.GetConfig());
            services.AddScoped<ILogger, MadWorldDbLogger>();
            services.AddSingleton(_ => MadWorldLoggerConfiguration.GetConfig());
            services.AddSingleton(typeof(ILogger<>), typeof(MadWorldDbLogger<>));

            //API managers
            services.AddScoped<IAccountManager, AccountManager>();

            //API Hub
            services.AddSingleton<PokerSession>();

            //Business project
            services.AddScoped<IStatusManager, StatusManager>();
            services.AddScoped<IResumeManager, ResumeManager>();
            services.AddScoped<IUserExtremeManager, UserExtremeManager>();
            services.AddScoped<ILoggingManager, LoggingManager>();
            services.AddScoped<IPokerManager, PokerManager>();
            services.AddScoped<IFileManager, FileManager>();
            services.AddScoped<IBlogManager, BlogManager>();

            //Database project
            services.AddScoped<IAccountQueries, AccountQueries>();
            services.AddScoped<IGeneralQueries, GeneralGueries>();
            services.AddScoped<IResumeQueries, ResumeQueries>();
            services.AddScoped<ILoggerQueries, LoggerQueries>();
            services.AddScoped<IFileQueries, FileQueries>();
            services.AddScoped<IBlogQueries, BlogQueries>();
            services.AddSingleton<ILoggerQueriesSingleton, LoggerQueries>(_ => new LoggerQueries(new MadWorldContext(builderOptions.Options)));

            //Storage
            services.AddScoped<IDiskManager, DiskManager>();
            services.AddScoped<IStorageExplorer, StorageExplorer>();
            services.AddScoped<IStorageManager, StorageManager>();
            services.AddSingleton(_ => Configuration.GetSection(nameof(StorageSettings)).Get<StorageSettings>());
        }
    }
}
