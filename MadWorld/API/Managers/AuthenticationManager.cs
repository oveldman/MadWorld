using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Managers.Interfaces;
using Business.Interfaces;
using Database.Tables.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TwoFactorAuthNet;
using Website.Shared.Models;
using Website.Shared.Models.Authentication;

namespace API.Managers
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly string _issuer;
        private readonly IUserExtremeManager _userExtremeManager;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly SymmetricSecurityKey _securityKey;
        private readonly TwoFactorAuth _twoFactorAuth;

        public AuthenticationManager(string issuer, TwoFactorAuth twoFactorAuth, string key, IUserExtremeManager userExtremeManager, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _issuer = issuer;
            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            _twoFactorAuth = twoFactorAuth;
            _signInManager = signInManager;
            _userManager = userManager;
            _userExtremeManager = userExtremeManager;
        }

        public async Task<LoginResponse> AuthenticateAsync(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, true, true);

            if (result.Succeeded)
            {
                User user = await _userManager.FindByNameAsync(username);

                if (user.TwoFactorOn)
                {
                    return GenerateTwoFactorSession(username);
                }
                else
                {
                    return GenerateBearerToken(username);
                }
            }

            return new LoginResponse
            {
                ErrorMessage = "Username and/or password isn't correct"
            };
        }

        public LoginResponse VerifyTwoFactor(Guid? session, string token)
        {
            User user = _userExtremeManager.FindUserByTwoFactorSession(session);

            if (user != null && !user.TwoFactorEnabled)
            {
                if (_twoFactorAuth.VerifyCode(user.TwoFactorSecret, token))
                {
                    return GenerateBearerToken(user.UserName);
                }
            }

            return new LoginResponse
            {
                ErrorMessage = "Token is not valid or session is expired"
            };
        }

        private LoginResponse GenerateBearerToken(string username)
        {
            LoginResponse loginModel = new LoginResponse
            {
                Roles = new List<string>()
            };

            var signinCredentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);

            DateTime validUntil = DateTime.Now.AddMinutes(30);

            var tokenOptions = new JwtSecurityToken(
                issuer: _issuer,
                audience: _issuer,
                claims: new List<Claim>() {
                        new Claim(ClaimTypes.Name, username)
                },
                expires: validUntil,
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            loginModel.Succeed = true;
            loginModel.AccessToken = tokenString;
            loginModel.Type = "Bearer";
            loginModel.Username = username;
            loginModel.Issued = DateTime.Now;
            loginModel.Expires = validUntil;
            loginModel.Roles.Add("Admin");

            // Ticks  divide by 10000000 makes time in seconds. 
            loginModel.ExpiresIn = ((validUntil.Ticks - DateTime.Now.Ticks) / 10000000).ToString();

            return loginModel;
        }

        private LoginResponse GenerateTwoFactorSession(string username)
        {
            Guid? newTwoFactorSession = _userExtremeManager.UpdateTwoFactorSession(username);

            return new LoginResponse
            {
                Succeed = true,
                Username = username,
                RequiresTwoFactor = true,
                TwoFactorSession = newTwoFactorSession
            };
        }
    }
}
