using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Website.Services.Interfaces;
using Website.Shared.Models;
using Website.Shared.Models.Account;

namespace Website.Services
{
    public class AccountService : AuthenticatedBaseService, IAccountService
    {
        public AccountService(IHttpClientFactory clientFactory, AuthenticationStateProvider state) : base(clientFactory, state) { }
        public async Task<BaseModel> ChangePassword(PasswordRequest passwordRequest)
        {
            return await SendPostRequest<BaseModel, PasswordRequest>("account/changepassword", passwordRequest);
        }
    }
}
