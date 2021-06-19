using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Website.Services.Helper;
using Website.Services.Interfaces;
using Website.Services.Models;
using Website.Settings;
using Website.Shared.Models;

namespace Website.Services
{
    public class StorageService : IStorageService
    {
        private HttpClient _client;

        public StorageService(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient(ApiUrls.MadWorldApi);
        }
        public async Task<FileResponse> GetFile(Guid? id)
        {
            string url = "Storage/DownloadFile";

            List<UrlParameter> parameters = new()
            {
                new UrlParameter
                {
                    Name = "id",
                    Value = id.ToString()
                }
            };

            url = ServiceHelper.BuildUrl(url, parameters);

            return await _client.GetFromJsonAsync<FileResponse>(url);
        }
    }
}
