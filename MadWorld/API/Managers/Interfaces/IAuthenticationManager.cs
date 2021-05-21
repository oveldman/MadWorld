using System;
using System.Threading.Tasks;
using Website.Shared.Models;
using Website.Shared.Models.Authentication;

namespace API.Managers.Interfaces
{
    public interface IAuthenticationManager
    {
        Task<LoginResponse> AuthenticateAsync(string username, string password);
        LoginResponse VerifyTwoFactor(Guid? session, string token);
    }
}
