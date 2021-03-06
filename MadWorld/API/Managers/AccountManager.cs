using System;
using System.Collections.Generic;
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
        RoleManager<IdentityRole> _roleManager;
        UserManager<User> _userManager;

        public AccountManager(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IUserExtremeManager userExtremeManager, TwoFactorAuth twoFactorAuth)
        {
            _twoFactorAuth = twoFactorAuth;
            _userExtremeManager = userExtremeManager;
            _roleManager = roleManager;
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
                BaseModel result = await EditAccount(userModel, user);

                if (!result.Succeed) return result;
            }

            return await SaveRoles(userModel, user);
        }

        public async Task<BaseModel> DeleteAccount(string id)
        {
            User user = await _userManager.FindByIdAsync(id);

            if (user is not null) {
                await _userManager.DeleteAsync(user);
            }

            return new BaseModel
            {
                Succeed = true
            };
        }

        public async Task<UserModel> GetRoles(UserModel user)
        {
            User currentUser = await _userManager.FindByIdAsync(user.Id);

            List<IdentityRole> allRolesInManager = _roleManager.Roles.ToList();

            if (allRolesInManager is null)
            {
                user.Roles = new();
                return user;
            }

            user.Roles = allRolesInManager.Select(r => new RoleModel { Name = r.Name }).ToList();

            IList<string> allUserRoles = await _userManager.GetRolesAsync(currentUser);
            user.Roles.ForEach(r => r.HasAccess = allUserRoles.Contains(r.Name));

            return user;
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

        private async Task<BaseModel> SaveRoles(UserModel userModel, User user)
        {
            List<string> allRolesInManager = _roleManager.Roles.Select(r => r.Name).ToList();
            if (allRolesInManager is null) allRolesInManager = new();

            IList<string> oldUserRoles = await _userManager.GetRolesAsync(user);
            if (oldUserRoles is null) oldUserRoles = new List<string>();

            List<string> userHasRoles = userModel?.Roles?
                                                    .Where(r => r.HasAccess && allRolesInManager.Contains(r.Name))
                                                    .Select(r => r.Name)
                                                    .ToList();
            if (userHasRoles is null) userHasRoles = new();

            await _userManager.RemoveFromRolesAsync(user, oldUserRoles);
            IdentityResult result = await _userManager.AddToRolesAsync(user, userHasRoles);

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
