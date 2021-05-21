using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Website.Settings;
using Website.Shared.Models.Authentication;

namespace Website.Services.States
{
    public class ApiAuthenticationProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        private bool Initialized;

        private ILocalStorageService _localStorage;

        public ApiAuthenticationProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            await Task.FromResult(0);
            await CheckAccountInLocalStorage();
            Initialized = true;
            return new AuthenticationState(claimsPrincipal);
        }

        public void LoginNotify(LoginResponse loginResponse)
        {
            var identity = new ClaimsIdentity(new[]
                                {
                                    new Claim(ClaimTypes.Name, loginResponse.Username),
                                    new Claim(ClaimTypes.Email,loginResponse.Username),
                                    new Claim("access_token", loginResponse.AccessToken)
                                }, "MadWorld");

            foreach (string role in loginResponse.Roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            claimsPrincipal = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void LogoutNotify()
        {
            var anonymous = new ClaimsIdentity();

            claimsPrincipal = new ClaimsPrincipal(anonymous);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private async Task CheckAccountInLocalStorage()
        {
            if (Initialized || claimsPrincipal.Identity.IsAuthenticated) return;

            var loginResponse = await _localStorage.GetItemAsync<LoginResponse>(LocalStorageNames.Login);

            if (loginResponse != null)
            {
                LoginNotify(loginResponse);
            }
        }
    }
}