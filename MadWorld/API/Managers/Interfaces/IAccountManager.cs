using System;
using System.Threading.Tasks;
using Website.Shared.Models;

namespace API.Managers.Interfaces
{
    public interface IAccountManager
    {
        Task<BaseModel> ChangePassword(string username, string oldPassword, string newPassword);
    }
}
