using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Managers.Interfaces;
using Datalayer.Database.Tables.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Website.Shared.Models;
using Website.Shared.Models.Account;
using Website.Shared.Models.Authentication;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationManager _authenticationManager;

        public AuthenticationController(ILogger<AuthenticationController> logger, RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IAuthenticationManager authenticationManager)
        {
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
            _authenticationManager = authenticationManager;
        }


        [HttpPost]
        [Route("Login")]
        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            return await _authenticationManager.AuthenticateAsync(loginRequest.Username, loginRequest.Password);
        }

        [HttpPost]
        [Route("TwoFactor")]
        public LoginResponse TwoFactor(TwoFactorRequest twoFactorRequest)
        {
            return _authenticationManager.VerifyTwoFactor(twoFactorRequest.Session, twoFactorRequest.Token);
        }

        /*
        [HttpGet]
        [Route("FirstTime")]
        public async Task<BaseModel> FirstTime()
        {
            User user = new()
            {
                Email = "test@test.nl",
                UserName = "test@test.nl",
                EmailConfirmed = true,
            };

            IdentityRole role = new()
            {
                Name = "Admin"
            };

            await _roleManager.CreateAsync(role);

            string password = "{-1NotMyRealPassword1-}";
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                user = await _userManager.FindByEmailAsync(user.Email);
                await _userManager.AddToRoleAsync(user, role.Name);
            }

            return new BaseModel()
            {
                Succeed = true,
                Message = $"Password: '{password}'",
                ErrorMessage = string.Join(", ", result.Errors.Select(e => e.Description).ToList())
            };
        }
        */
    }
}
