using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Managers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Website.Shared.Models;
using Website.Shared.Models.Account;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountManager _accountManager;

        public AccountController(ILogger<AccountController> logger, IAccountManager accountManager)
        {
            _logger = logger;
            _accountManager = accountManager;
        }

        [HttpPut]
        [Route("ChangePassword")]
        public async Task<BaseModel> ChangePassword(PasswordRequest loginRequest)
        {
            var username = User.FindFirst(ClaimTypes.Name).Value;

            if (loginRequest.NewPassword.Equals(loginRequest.PasswordConfirm)) {
                return await _accountManager.ChangePassword(username, loginRequest.OldPassword, loginRequest.NewPassword);
            }

            return new BaseModel
            {
                Succeed = false,
                ErrorMessage = "New password and password confirm doesn't much"
            };
        }

        [HttpGet]
        [Route("GetNewTwoFactorAuthentication")]
        public async Task<NewTwoFactorResponse> GetNewTwoFactorAuthentication()
        {
            var username = User.FindFirst(ClaimTypes.Name).Value;
            return await _accountManager.GetNewTwoFactorAuthentication(username, true);
        }

        [HttpPost]
        [Route("SetTwoFactorOn")]
        public async Task<NewTwoFactorResponse> SetTwoFactorOn(TwoFactorRequest twofactorRequest)
        {
            var username = User.FindFirst(ClaimTypes.Name).Value;
            return await _accountManager.GetNewTwoFactorTurnOn(username, twofactorRequest.Token);
        }

        [HttpGet]
        [Route("SetTwoFactorOff")]
        public async Task<NewTwoFactorResponse> SetTwoFactorOff()
        {
            var username = User.FindFirst(ClaimTypes.Name).Value;
            return await _accountManager.GetNewTwoFactorTurnOff(username);
        }
    }
}
