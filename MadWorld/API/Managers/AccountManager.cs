using System;
using System.Linq;
using System.Threading.Tasks;
using API.Managers.Interfaces;
using Database.Tables.Identity;
using Microsoft.AspNetCore.Identity;
using Website.Shared.Models;

namespace API.Managers
{
    public class AccountManager : IAccountManager
    {
        UserManager<User> _userManager;

        public AccountManager(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<BaseModel> ChangePassword(string username, string oldPassword, string newPassword)
        {
            var currentUser = await _userManager.FindByNameAsync(username);

            var result = await _userManager.ChangePasswordAsync(currentUser, oldPassword, newPassword);

            return new BaseModel
            {
                Succeed = result.Succeeded,
                ErrorMessages = !result.Succeeded ? result.Errors.Select(e => e.Description).ToList() : null
            };
        }
    }
}
