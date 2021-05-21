using System;
using System.Threading.Tasks;
using Website.Shared.Models.Authentication;

namespace Website.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> Login(string username, string password);
        Task Logout();
        Task<LoginResponse> VerifyTwoFactor(string token, Guid? session);
    }
}
