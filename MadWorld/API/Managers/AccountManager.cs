using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Managers.Interfaces;
using API.Models;
using Business.Interfaces;
using Datalayer.Database.Tables.Identity;
using Microsoft.AspNetCore.Identity;
using QRCoder;
using TwoFactorAuthNet;
using Website.Shared.Models;
using Website.Shared.Models.Account;
using Website.Shared.Models.Admin;

namespace API.Managers
{
    public class AccountManager : IAccountManager
    {
        TwoFactorAuth _twoFactorAuth;
        IUserExtremeManager _userExtremeManager;
        UserManager<User> _userManager;

        public AccountManager(UserManager<User> userManager, IUserExtremeManager userExtremeManager, TwoFactorAuth twoFactorAuth)
        {
            _twoFactorAuth = twoFactorAuth;
            _userExtremeManager = userExtremeManager;
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

        public async Task<NewTwoFactorResponse> GetNewTwoFactorAuthentication(string username, bool refreshToken)
        {
            bool succeed = false;
            string newSecret = string.Empty;

            User user = await _userManager.FindByNameAsync(username);

            if (user != null && !user.TwoFactorOn)
            {
                newSecret = refreshToken ? _twoFactorAuth.CreateSecret(160) : user.TwoFactorSecret;
                succeed = _userExtremeManager.UpdateNewSecret(username, newSecret);
            }

            succeed = user.TwoFactorOn || succeed;

            string applicationName = ApiSettings.Issuer;

            return new NewTwoFactorResponse
            {
                Succeed = succeed,
                ApplicationName = applicationName,
                IsTwoFactorOn = user?.TwoFactorOn ?? false,
                Secret = newSecret,
                QRBase64 = GenerateQrCode(applicationName, newSecret)
            };
        }

        public async Task<NewTwoFactorResponse> GetNewTwoFactorTurnOff(string username)
        {
            User user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                _userExtremeManager.SetTwoFactorEnabled(user, false);
                return await GetNewTwoFactorAuthentication(username, true);
            }

            return new NewTwoFactorResponse
            {
                Succeed = false,
                ErrorMessage = "Account not found"
            };
        }

        public async Task<NewTwoFactorResponse> GetNewTwoFactorTurnOn(string username, string token)
        {
            User user = await _userManager.FindByNameAsync(username);
            bool verified = _twoFactorAuth.VerifyCode(user?.TwoFactorSecret, token);
            bool userFound = user != null;

            if (userFound && verified)
            {
                 _userExtremeManager.SetTwoFactorEnabled(user, true);
            }

            if (!userFound)
            {
                return new NewTwoFactorResponse
                {
                    Succeed = false,
                    ErrorMessage = "Account not found"
                };
            }

            NewTwoFactorResponse response = await GetNewTwoFactorAuthentication(username, true);

            if (!verified)
            {
                response.Succeed = false;
                response.ErrorMessage = "Token not good";
            }

            return response;
        }

        public async Task<BaseModel> SaveAccount(UserModel userModel)
        {
            User user;

            if (userModel.IsNew)
            {
                BaseModel result = await CreateNewAccount(userModel);

                if (!result.Succeed) return result;

                user = await _userManager.FindByEmailAsync(userModel.Email);
            }
            else
            {
                user = await _userManager.FindByIdAsync(userModel.Id);
            }

            if (user is null)
            {
                return new BaseModel
                {
                    ErrorMessage = "User not found"
                };
            }

            if (!string.IsNullOrEmpty(userModel.Password))
            {
                BaseModel result = await SetPassword(user, userModel.Password);

                if (!result.Succeed) return result;
            }

            if (!userModel.IsNew)
            {
                return await EditAccount(userModel, user);
            }

            return new BaseModel
            {
                Succeed = true
            };
        }

        private string GenerateQrCode(string applicationName, string secret)
        {
            if (string.IsNullOrEmpty(secret)) return string.Empty;

            string qrUrl = $"otpauth://totp/{applicationName}?secret={secret}&issuer={ApiSettings.Issuer}";

            using (MemoryStream stream = new MemoryStream())
            {
                QRCodeGenerator generator = new QRCodeGenerator();
                QRCodeData qrData = generator.CreateQrCode(qrUrl, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrData);

                using (Bitmap bitmap = qrCode.GetGraphic(20))
                {
                    bitmap.Save(stream, ImageFormat.Png);
                    return Convert.ToBase64String(stream.ToArray());
                }
            }
        }

        private async Task<BaseModel> CreateNewAccount(UserModel userModel)
        {
            User user = new()
            {
                Email = userModel.Email,
                UserName = userModel.Username,
                EmailConfirmed = true
            };

            IdentityResult result = await _userManager.CreateAsync(user);

            return new BaseModel
            {
                Succeed = result.Succeeded,
                ErrorMessage = result?.Errors?.FirstOrDefault()?.Description
            };
        }

        private async Task<BaseModel> EditAccount(UserModel userModel, User user)
        {
            user.Email = userModel.Email;
            user.UserName = userModel.Username;
            user.TwoFactorOn = !userModel.TwoFactorEnabled ? false : user.TwoFactorOn;

            IdentityResult result = await _userManager.UpdateAsync(user);

            return new BaseModel
            {
                Succeed = result.Succeeded,
                ErrorMessage = result?.Errors?.FirstOrDefault()?.Description
            };
        }

        private async Task<BaseModel> SetPassword(User user, string password)
        {
            bool passwordValid = true;

            foreach (IPasswordValidator<User> validator in _userManager.PasswordValidators)
            {
                IdentityResult validResult = await validator.ValidateAsync(_userManager, user, password);
                passwordValid = validResult.Succeeded;

                if (!passwordValid) break;
            }

            if (passwordValid)
            {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, password);

                return new BaseModel
                {
                    Succeed = true
                };
            }

            return new BaseModel
            {
                ErrorMessage = "Password isn't valid"
            };
        }
    }
}
