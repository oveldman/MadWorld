using System;
namespace Website.Services.Interfaces
{
    public interface IAuthenticationService
    {
        void Login(string username, string password);
        void Logout();
    }
}
