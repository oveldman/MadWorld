using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Managers.Interfaces;
using Business.Interfaces;
using Datalayer.Database.Tables.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Website.Shared.Models;
using Website.Shared.Models.Admin;

namespace API.Controllers.Admin
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;

        private readonly IAccountManager _accountManager;
        private readonly IUserExtremeManager _userExtremeManager;

        public AdminController(ILogger<AdminController> logger, IUserExtremeManager userExtremeManager, IAccountManager accountManager)
        {
            _logger = logger;
            _accountManager = accountManager;
            _userExtremeManager = userExtremeManager;
        }

        [HttpGet]
        public AdminModel Index()
        {
            return new AdminModel()
            {
                Succeed = true,
                WelcomeMessage = "Welcome dude!"
            };
        }

        [HttpDelete]
        [Route("DeleteAccount")]
        public async Task<BaseModel> DeleteAccount(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                return await _accountManager.DeleteAccount(id);
            }

            return new BaseModel
            {
                ErrorMessage = "ID is required"
            };
        }

        [HttpGet]
        [Route("GetAllAccounts")]
        public IEnumerable<UserModel> GetAllAccounts()
        {
            List<User> users = _userExtremeManager.GetAllUsers();

            return users.Select(u => new UserModel {
                Id = u.Id,
                Username = u.UserName,
                TwoFactorEnabled = u.TwoFactorOn
            });
        }

        [HttpGet]
        [Route("GetAccount")]
        public UserModel GetAccount(Guid? id)
        {
            if (id.HasValue)
            {
                return _userExtremeManager.GetUser(id.Value);
            }

            return new UserModel();
        }

        [HttpPost]
        [Route("SaveAccount")]
        public async Task<BaseModel> SaveAccount(UserModel userModel)
        {
            if (userModel is not null)
            {
                return await _accountManager.SaveAccount(userModel);
            }

            return new BaseModel
            {
                Succeed = false
            };
        }
    }
}
