using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Managers.Interfaces;
using Database.Tables.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Website.Shared.Models.Authentication;

namespace API.Managers
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly string _issuer;
        private readonly SignInManager<User> _signInManager;
        private readonly SymmetricSecurityKey _securityKey;

        public AuthenticationManager(string issuer, string key, SignInManager<User> signInManager)
        {
            _issuer = issuer;
            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            _signInManager = signInManager;
        }

        public async Task<LoginResponse> AuthenticateAsync(string username, string password)
        {
            var loginModel = new LoginResponse
            {
                Succeed = false,
                Roles = new List<string>()
            };

            var result = await _signInManager.PasswordSignInAsync(username, password, true, true);

            if (result.Succeeded)
            {
                var signinCredentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);

                DateTime validUntil = DateTime.Now.AddMinutes(30);

                var tokenOptions = new JwtSecurityToken(
                    issuer: _issuer,
                    audience: _issuer,
                    claims: new List<Claim>() {
                        new Claim(ClaimTypes.Name, username),
                    },
                    expires: validUntil,
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                loginModel.Succeed = true;
                loginModel.AccessToken = tokenString;
                loginModel.Type = "Bearer";
                loginModel.Username = username;
                loginModel.Issued = DateTime.Now.ToString();
                loginModel.Expires = validUntil.ToString();
                loginModel.Roles.Add("Admin");

                // Ticks  divide by 10000000 makes time in seconds. 
                loginModel.ExpiresIn = ((validUntil.Ticks - DateTime.Now.Ticks) / 10000000).ToString();
            }
            else
            {
                loginModel.ErrorMessage = "Username and/or password isn't correct.";
            }

            return loginModel;
        }

        public string GetUsername(string bearerToken)
        {
            bearerToken = bearerToken.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);

            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _securityKey,
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var claims = handler.ValidateToken(bearerToken, validations, out var tokenSecure);
            return claims.FindFirstValue(ClaimTypes.Name);
        }
    }
}
