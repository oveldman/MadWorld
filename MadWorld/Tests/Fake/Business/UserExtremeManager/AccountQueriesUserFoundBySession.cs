using System;
using System.Collections.Generic;
using Database.Queries.Interfaces;
using Database.Tables.Identity;

namespace Tests.Fake.Business.UserExtremeManager
{
    public class AccountQueriesUserFoundBySession : IAccountQueries
    {
        public User FindUserBySession(Guid? session)
        {
            return new User
            {
                UserName = "test@test.nl",
                TwoFactorEnabled = true,
                TwoFactorSessionExpire = DateTime.Parse("05/24/2021 10:00:00"),
                TwoFactorSession = Guid.Parse("6b976751-e0c9-43c6-9aef-57bfba4ecfa9"),
                TwoFactorOn = true,
            };
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
