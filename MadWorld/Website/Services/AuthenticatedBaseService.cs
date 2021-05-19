using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Website.Settings;

namespace Website.Services
{
    public abstract class AuthenticatedBaseService
    {
        private AuthenticationStateProvider _state;
        protected HttpClient _client;

        public AuthenticatedBaseService(IHttpClientFactory clientFactory, AuthenticationStateProvider state)
        {
            _state = state;
            _client = clientFactory.CreateClient(ApiUrls.MadWorldApi);
        }

        protected async Task<T> SendGetRequest<T>(string url)
        {
            await SetBearerTokenIfEmpty();
            return await _client.GetFromJsonAsync<T>(url);
        }

        protected async Task<T> SendPostRequest<T, Y>(string url, Y request)
        {
            await SetBearerTokenIfEmpty();
            HttpResponseMessage response = await _client.PostAsJsonAsync(url, request);
            return await response.Content.ReadFromJsonAsync<T>();
        }

        private async Task SetBearerTokenIfEmpty()
        {
            if (_client.DefaultRequestHeaders.Any(h => h.Key
                        .Equals("Authorization", StringComparison.OrdinalIgnoreCase))) return;

            var currentUserState = await _state.GetAuthenticationStateAsync();
            string token = currentUserState.User?.Claims?.FirstOrDefault(c => c.Type == "access_token")?.Value;

            if (!string.IsNullOrEmpty(token)) {
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }
        }
    }
}
