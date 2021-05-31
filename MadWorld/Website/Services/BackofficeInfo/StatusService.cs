using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Website.Services.Interfaces;
using Website.Settings;
using Website.Shared.Models.BackendInfo;

namespace Website.Services.BackofficeInfo
{
    public class StatusService : IStatusService
    {
        private HttpClient _client;

        public StatusService(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient(ApiUrls.MadWorldApi);
        }

        public async Task<bool> CheckStatus()
        {
            try
            {
                var result = await _client.GetAsync("/status/CheckConnectionAPI");
                return result.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> CheckDatabaseAuthentication()
        {
            try
            {
                var result = await _client.GetFromJsonAsync<DatabaseStatus>("/status/CheckConnectionDatabaseAuthentication");
                return result.IsOnline;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> CheckDatabaseMadWorld()
        {
            try
            {
                var result = await _client.GetFromJsonAsync<DatabaseStatus>("/status/CheckConnectionDatabaseMadWorld");
                return result.IsOnline;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
