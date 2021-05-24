using System;
using System.Collections.Generic;
using Database.Queries.Interfaces;
using Database.Tables.Identity;

namespace Tests.Fake.Business.UserExtremeManager
{
    public class AccountQueriesUserNotFound : IAccountQueries
    {
        public User FindUserBySession(Guid? session)
        {
            return null;
        }

        public List<User> GetUsers()
        {
            throw new NotImplementedException();
        }

        public bool SetSecretToken(string username, string twofactorSecret)
        {
            throw new NotImplementedException();
        }

        public bool SetTwoFactorEnabled(string username, bool enabled)
        {
            throw new NotImplementedException();
        }

        public bool SetTwoFactorSession(string username, Guid? twoFactorSession)
        {
            throw new NotImplementedException();
        }
    }
}
