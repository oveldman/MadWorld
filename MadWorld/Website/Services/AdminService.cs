using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Website.Services.Interfaces;
using Website.Shared.Models;

namespace Website.Services
{
    public class AdminService : AuthenticatedBaseService, IAdminService
    {
        public AdminService(IHttpClientFactory clientFactory, AuthenticationStateProvider state) : base(clientFactory, state) { }

        public async Task<AdminModel> GetIndex()
        {
            return await SendGetRequest<AdminModel>("admin");
        }
    }
}
