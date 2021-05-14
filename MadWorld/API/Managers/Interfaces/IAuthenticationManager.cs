using System;
using System.Threading.Tasks;
using Website.Shared.Models.Authentication;

namespace API.Managers.Interfaces
{
    public interface IAuthenticationManager
    {
        Task<LoginResponse> AuthenticateAsync(string username, string password);
    }
}
