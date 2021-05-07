using System;
using Website.Shared.Models;
using Website.Services.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Website.Settings;

namespace Website.Services
{
    public class ResumeService : IResumeService
    {
        private HttpClient _client;

        public ResumeService(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient(ApiUrls.MadWorldApi);
        }

        public async Task<ResumeModel> GetResume()
        {
            return await _client.GetFromJsonAsync<ResumeModel>("resume");
        }
    }
}
