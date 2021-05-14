using System;
using System.Threading.Tasks;
using Website.Shared.Models.Authentication;

namespace Website.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> Login(string username, string password);
        void Logout();
    }
}
