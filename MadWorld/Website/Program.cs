using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Website.Services;
using Website.Services.Interfaces;
using Website.Settings;
using Website.Services.States;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Website.Services.BackofficeInfo;
using BlazorTable;
using Microsoft.AspNetCore.SignalR.Client;
using Website.Services.ExternJS;
using BlazorDownloadFile;
using Website.Manager.Interfaces;
using Website.Manager;

namespace Website
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            AddToScope(builder);

            string apiUrl = @"https://www.mad-world.nl/api/";

            if (builder.HostEnvironment.IsDevelopment()) {
                apiUrl = @"https://localhost:5003/";
            }

            builder.Services.AddHttpClient(ApiUrls.MadWorldApi, client =>
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            builder.Services.AddScoped<HubConnection>(_ =>
                    new HubConnectionBuilder()
                .WithUrl($"{apiUrl}PlanningPoker")
                .Build()
            );

            builder.Services.AddOidcAuthentication(options =>
            {
                // Configure your authentication provider options here.
                // For more information, see https://aka.ms/blazor-standalone-auth
                builder.Configuration.Bind("Local", options.ProviderOptions);
            });

            builder.Services.AddApiAuthorization();
            builder.Services.AddBlazoredLocalStorage();

            await builder.Build().RunAsync();
        }

        private static void AddToScope(WebAssemblyHostBuilder builder)
        {
            //Others
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationProvider>();

            //Settings
            builder.Services.AddSingleton(_ => new WebsiteSettings {
                TinyMceKey = "r8w19qob34k0zsdpzc5p48fgblz2brxdked5filakm0chewk"
            });

            //Services
            builder.Services.AddScoped<ITest, Test>();
            builder.Services.AddScoped<ISmartlookService, SmartlookService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<IResumeService, ResumeService>();
            builder.Services.AddScoped<IStatusService, StatusService>();
            builder.Services.AddScoped<ILoggingService, LoggingService>();
            builder.Services.AddScoped<IStorageAuthenticatedService, StorageAuthenticatedService>();
            builder.Services.AddScoped<IStorageAnonymousService, StorageAnonymousService>();

            //Managers
            builder.Services.AddScoped<ICodeEditorManager, EditorManager>();

            //Extern packages
            builder.Services.AddBlazorTable();
            builder.Services.AddBlazorDownloadFile();
        }
    }
}
