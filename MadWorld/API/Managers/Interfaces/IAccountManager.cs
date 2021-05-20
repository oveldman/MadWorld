using System;
using System.Threading.Tasks;
using Website.Shared.Models;
using Website.Shared.Models.Account;

namespace API.Managers.Interfaces
{
    public interface IAccountManager
    {
        Task<BaseModel> ChangePassword(string username, string oldPassword, string newPassword);
        Task<NewTwoFactorResponse> GetNewTwoFactorAuthentication(string username, bool refreshToken);
        Task<NewTwoFactorResponse> GetNewTwoFactorTurnOn(string username, string token);
        Task<NewTwoFactorResponse> GetNewTwoFactorTurnOff(string username);
    }
}
