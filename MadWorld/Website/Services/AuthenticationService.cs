using System;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Authorization;
using Website.Services.Interfaces;
using Website.Services.States;
using Website.Settings;

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

        public void Login(string username, string password)
        {
            (_state as ApiAuthenticationProvider).LoginNotify(username);
        }

        public void Logout()
        {
            (_state as ApiAuthenticationProvider).LogoutNotify();
        }
    }
}
