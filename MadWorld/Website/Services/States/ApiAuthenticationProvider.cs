using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Website.Shared.Models.Authentication;

namespace Website.Services.States
{
    public class ApiAuthenticationProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            await Task.FromResult(0);
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
    }
}