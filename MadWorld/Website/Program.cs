using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Website.Services;
using Website.Services.Interfaces;
using Website.Settings;

namespace Website
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<ITest, Test>();
            builder.Services.AddScoped<IResumeService, ResumeService>();

            if (builder.HostEnvironment.IsDevelopment()) {
                builder.Services.AddHttpClient(ApiUrls.MadWorldApi, client =>
                {
                    client.BaseAddress = new Uri(@"https://localhost:5003/");
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                });
            }
            else
            {
                builder.Services.AddHttpClient(ApiUrls.MadWorldApi, client =>
                {
                    client.BaseAddress = new Uri(@"https://www.mad-world.nl/api/");
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                });
            }

            await builder.Build().RunAsync();
        }
    }
}
