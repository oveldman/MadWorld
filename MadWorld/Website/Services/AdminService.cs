using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Website.Services.Interfaces;
using Website.Services.Models;
using Website.Shared.Models;
using Website.Shared.Models.Admin;

namespace Website.Services
{
    public class AdminService : AuthenticatedBaseService, IAdminService
    {
        public AdminService(IHttpClientFactory clientFactory, AuthenticationStateProvider state, NavigationManager navigation) : base(clientFactory, state, navigation) { }

        public async Task<AdminModel> GetIndex()
        {
            return await SendGetRequest<AdminModel>("admin");
        }

        public async Task<List<UserModel>> GetUsers()
        {
            return await SendGetRequest<List<UserModel>>("admin/getallaccounts");
        }

        public async Task<UserModel> GetUser(Guid id)
        {
            List<UrlParameter> parameters = new()
            {
                new UrlParameter
                {
                    Name = "id",
                    Value = id.ToString()
                }
            };

            return await SendGetRequest<UserModel>("admin/getaccount", parameters);
        }

        public async Task<BaseModel> SaveUser(UserModel user)
        {
            return await SendPostRequest<BaseModel, UserModel>("admin/saveaccount", user);
        }
    }
}
