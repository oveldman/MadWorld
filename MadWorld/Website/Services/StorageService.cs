using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Website.Services.Interfaces;
using Website.Services.Models;
using Website.Shared.Models;

namespace Website.Services
{
    public class StorageService : AuthenticatedBaseService, IStorageService
    {
        public StorageService(IHttpClientFactory clientFactory, AuthenticationStateProvider state, NavigationManager navigation) : base(clientFactory, state, navigation) { }

        public async Task<FileResponse> GetFile(Guid? id)
        {
            List<UrlParameter> parameters = new()
            {
                new UrlParameter
                {
                    Name = "id",
                    Value = id.ToString()
                }
            };

            return await SendGetRequest<FileResponse>("storage/downloadfile", parameters);
        }
    }
}
