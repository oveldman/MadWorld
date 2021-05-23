using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Database.Tables.Identity;
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

        private readonly IUserExtremeManager _userExtremeManager;

        public AdminController(ILogger<AdminController> logger, IUserExtremeManager userExtremeManager)
        {
            _logger = logger;
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
    }
}
