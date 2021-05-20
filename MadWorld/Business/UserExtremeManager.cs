using System;
using Business.Interfaces;
using Database.Queries.Interfaces;

namespace Business
{
    public class UserExtremeManager : IUserExtremeManager
    {
        private readonly IAccountQueries _accountQueries;

        public UserExtremeManager(IAccountQueries accountQueries)
        {
            _accountQueries = accountQueries;
        }

        public bool UpdateNewSecret(string username, string secret)
        {
            return _accountQueries.SetSecretToken(username, secret);
        }
    }
}
