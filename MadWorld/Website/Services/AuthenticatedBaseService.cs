﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Website.Services.Models;
using Website.Settings;

namespace Website.Services
{
    public abstract class AuthenticatedBaseService
    {
        protected HttpClient _client;
        private AuthenticationStateProvider _state;
        private NavigationManager _navigation;
        private DateTime? authenticationExpired;

        public AuthenticatedBaseService(IHttpClientFactory clientFactory, AuthenticationStateProvider state, NavigationManager navigation)
        {
            _state = state;
            _client = clientFactory.CreateClient(ApiUrls.MadWorldApi);
            _navigation = navigation;
        }

        protected async Task<T> SendGetRequest<T>(string url)
        {
            return await SendGetRequest<T>(url, new List<UrlParameter>());
        }

        protected async Task<T> SendGetRequest<T>(string url, List<UrlParameter> urlParameters)
        {
            if (urlParameters != null && urlParameters.Any())
            {
                url = BuildUrl(url, urlParameters);
            }

            bool isAuthenticated = await SetBearerTokenIfEmpty();

            if (isAuthenticated)
            {
                return await _client.GetFromJsonAsync<T>(url);
            }

            return default;
        }

        protected async Task<T> SendPostRequest<T, Y>(string url, Y request)
        {
            bool isAuthenticated = await SetBearerTokenIfEmpty();

            if (isAuthenticated) {
                HttpResponseMessage response = await _client.PostAsJsonAsync(url, request);
                return await response.Content.ReadFromJsonAsync<T>();
            }

            return default;
        }

        private string BuildUrl(string url, List<UrlParameter> urlParameters)
        {
            if (urlParameters == null || !urlParameters.Any()) return url;

            url = $"{url}?";

            for(int i = 0; i < urlParameters.Count; i++)
            {
                url += $"{urlParameters[i].Name}={urlParameters[i].Value}";

                if (i != (urlParameters.Count - 1))
                {
                    url += "&";
                }
            }

            return url;
        }

        private bool ForceReload()
        {
            _navigation.NavigateTo(WebsiteUrls.Home, true);
            return false;
        }

        private async Task<bool> SetBearerTokenIfEmpty()
        {
            if (DateTime.Now > authenticationExpired) return ForceReload();
            if (_client.DefaultRequestHeaders.Any(h => h.Key
                        .Equals("Authorization", StringComparison.OrdinalIgnoreCase))) return true;

            var currentUserState = await _state.GetAuthenticationStateAsync();
            string token = currentUserState.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypesExtra.AccessToken)?.Value;
            string dateExpired = currentUserState.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypesExtra.Expired)?.Value;

            if (!string.IsNullOrEmpty(token) && !string.IsNullOrWhiteSpace(dateExpired)) {
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                authenticationExpired = DateTime.Parse(dateExpired);
                return true;
            }

            return ForceReload();
        }
    }
}
