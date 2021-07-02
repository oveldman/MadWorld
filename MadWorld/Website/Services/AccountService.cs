using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Website.Services.Interfaces;
using Website.Shared.Models;
using Website.Shared.Models.Account;

namespace Website.Services
{
    public class AccountService : AuthenticatedBaseService, IAccountService
    {
        public AccountService(IHttpClientFactory clientFactory, AuthenticationStateProvider state, NavigationManager navigation) : base(clientFactory, state, navigation) { }
        public async Task<BaseModel> ChangePassword(PasswordRequest passwordRequest)
        {
            return await SendPutRequest<BaseModel, PasswordRequest>("account/changepassword", passwordRequest);
        }

        public async Task<NewTwoFactorResponse> GetTwoFactorInfo()
        {
            return await SendGetRequest<NewTwoFactorResponse>("account/GetNewTwoFactorAuthentication");
        }

        public async Task<NewTwoFactorResponse> TurnTwoFactorOn(TwoFactorRequest twofactorRequest)
        {
            return await SendPostRequest<NewTwoFactorResponse, TwoFactorRequest>("account/SetTwoFactorOn", twofactorRequest);
        }

        public async Task<NewTwoFactorResponse> TurnTwoFactorOff()
        {
            return await SendGetRequest<NewTwoFactorResponse>("account/SetTwoFactorOff");
        }
    }
}
