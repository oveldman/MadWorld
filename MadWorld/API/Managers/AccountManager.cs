﻿using System;
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
    }
}
