using System;
using Business.Interfaces;
using Database.Queries.Interfaces;
using Database.Tables.Identity;

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

            if (user.TwoFactorSessionExpire > DateTime.Now)
            {
                return user;
            }

            return null;
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
