using System;
using System.Collections.Generic;
using Business.Interfaces;
using Common;
using Datalayer.Database.Queries.Interfaces;
using Datalayer.Database.Tables.Identity;
using Website.Shared.Models;
using Website.Shared.Models.Admin;

namespace Business
{
    public class UserExtremeManager : IUserExtremeManager
    {
        private readonly IAccountQueries _accountQueries;

        public UserExtremeManager(IAccountQueries accountQueries)
        {
            _accountQueries = accountQueries;
        }

        public User FindUserByTwoFactorSession(Guid? session)
        {
            User user = _accountQueries.FindUserBySession(session);

            if (user?.TwoFactorSessionExpire > SystemTime.Now())
            {
                return user;
            }

            return null;
        }

        public List<User> GetAllUsers()
        {
            return _accountQueries.GetUsers();
        }

        public UserModel GetUser(Guid id)
        {
            User user = _accountQueries.GetUserByID(id);

            if (user is not null)
            {
                return new UserModel
                {
                    Id = user.Id,
                    TwoFactorEnabled = user.TwoFactorOn,
                    Username = user.UserName,
                    Email = user.Email
                };
            }

            return new UserModel();
        }

        public bool SetTwoFactorEnabled(User user, bool enabled)
        {
            return _accountQueries.SetTwoFactorEnabled(user.UserName, enabled);
        }

        public bool UpdateNewSecret(string username, string secret)
        {
            return _accountQueries.SetSecretToken(username, secret);
        }

        public Guid? UpdateTwoFactorSession(string username)
        {
            Guid? newSessionToken = Guid.NewGuid();
            _accountQueries.SetTwoFactorSession(username, newSessionToken);
            return newSessionToken;
        }
    }
}
