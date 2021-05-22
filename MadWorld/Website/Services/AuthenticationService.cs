using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Website.Services.Interfaces;
using Website.Services.States;
using Website.Settings;
using Website.Shared.Models.Account;
using Website.Shared.Models.Authentication;

namespace Website.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private HttpClient _client;
        private AuthenticationStateProvider _state;
        private ILocalStorageService _localStorage;

        public AuthenticationService(AuthenticationStateProvider authenticationStateProvider, IHttpClientFactory clientFactory, ILocalStorageService localStorage)
        {
            _client = clientFactory.CreateClient(ApiUrls.MadWorldApi);
            _state = authenticationStateProvider;
            _localStorage = localStorage;
        }

        public async Task<LoginResponse> Login(string username, string password)
        {
            var requestBody = new LoginRequest()
            {
                Username = username,
                Password = password
            };

            HttpResponseMessage response = await _client.PostAsJsonAsync("authentication/login", requestBody);
            LoginResponse loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (loginResponse.Succeed && !loginResponse.RequiresTwoFactor) {
                (_state as ApiAuthenticationProvider).LoginNotify(loginResponse);
            }

            return loginResponse;
        }

        public async Task<LoginResponse> VerifyTwoFactor(string token, Guid? session)
        {
            var requestBody = new TwoFactorRequest()
            {
                Token = token,
                Session = session
            };

            HttpResponseMessage response = await _client.PostAsJsonAsync("authentication/twofactor", requestBody);
            LoginResponse loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (loginResponse.Succeed)
            {
                loginResponse = await SetLoginResponseInSession(loginResponse);
                (_state as ApiAuthenticationProvider).LoginNotify(loginResponse);
            }

            return loginResponse;
        }

        public async Task Logout()
        {
            await SetLoginResponseInSession(null);
            (_state as ApiAuthenticationProvider).LogoutNotify();
        }

        private async Task<LoginResponse> SetLoginResponseInSession(LoginResponse response)
        {
            return await (_state as ApiAuthenticationProvider).SetLoginResponseInSession(response);
        }
    }
}
