using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

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

        public void LoginNotify(string username)
        {
            var identity = new ClaimsIdentity(new[]
                                {
                                    new Claim(ClaimTypes.Name, username),
                                    new Claim(ClaimTypes.Email, "test@test.com")
                                }, "Fake Authentication");

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