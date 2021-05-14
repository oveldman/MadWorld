using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Managers.Interfaces;
using Database.Tables.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Website.Shared.Models;
using Website.Shared.Models.Authentication;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationManager _authenticationManager;

        public AuthenticationController(ILogger<AuthenticationController> logger, UserManager<User> userManager, IAuthenticationManager authenticationManager)
        {
            _logger = logger;
            _userManager = userManager;
            _authenticationManager = authenticationManager;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            return await _authenticationManager.AuthenticateAsync(loginRequest.Username, loginRequest.Password);
        }

        /*
        [HttpGet]
        [AllowAnonymous]
        [Route("FirstTime")]
        public async Task<BaseModel> FirstTime()
        {
            var user = new User()
            {
                Email = "test@test.nl",
                UserName = "test@test.nl",
                EmailConfirmed = true,
            };

            string password = "{-1NotMyRealPassword1-}";
            var result = await _userManager.CreateAsync(user, password);
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
