using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
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

        public AuthenticationService(AuthenticationStateProvider authenticationStateProvider, IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient(ApiUrls.MadWorldApi);
            _state = authenticationStateProvider;
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
                (_state as ApiAuthenticationProvider).LoginNotify(loginResponse);
            }

            return loginResponse;
        }

        public void Logout()
        {
            (_state as ApiAuthenticationProvider).LogoutNotify();
        }
    }
}
