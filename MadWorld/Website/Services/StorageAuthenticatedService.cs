using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Website.Services.Interfaces;
using Website.Shared.Models;

namespace Website.Services
{
    public class StorageAuthenticatedService : AuthenticatedBaseService, IStorageAuthenticatedService
    {
        public StorageAuthenticatedService(IHttpClientFactory clientFactory, AuthenticationStateProvider state, NavigationManager navigation) : base(clientFactory, state, navigation) { }

        public async Task<BaseModel> Create(AddFileRequest request)
        {
            return await SendPostRequest<BaseModel, AddFileRequest>("adminstorage/Create", request);
        }

        public async Task<FilesResponse> GetAll()
        {
            return await SendGetRequest<FilesResponse>("adminstorage/getallfiles");
        }
    }
}
