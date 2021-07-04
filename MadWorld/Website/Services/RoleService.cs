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
    public class RoleService : AuthenticatedBaseService, IRoleService
    {
        public RoleService(IHttpClientFactory clientFactory, AuthenticationStateProvider state, NavigationManager navigation) : base(clientFactory, state, navigation) { }

        public async Task<BaseModel> Add(AdminRoleModel role)
        {
            return await SendPostRequest<BaseModel, AdminRoleModel>("adminrole/add", role);
        }

        public async Task<BaseModel> Delete(AdminRoleModel role)
        {
            List<UrlParameter> parameters = new()
            {
                new UrlParameter
                {
                    Name = "id",
                    Value = role.ID
                }
            };

            return await SendDeleteRequest<BaseModel>("adminrole/delete", parameters);
        }

        public async Task<List<AdminRoleModel>> GetAll()
        {
            return await SendGetRequest<List<AdminRoleModel>>("adminrole/getall");
        }
    }
}
