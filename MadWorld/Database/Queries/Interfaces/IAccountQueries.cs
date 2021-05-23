using System;
using System.Collections.Generic;
using Database.Tables.Identity;

namespace Database.Queries.Interfaces
{
    public interface IAccountQueries
    {
        User FindUserBySession(Guid? session);
        List<User> GetUsers();
        bool SetTwoFactorSession(string username, Guid? twoFactorSession);
        bool SetSecretToken(string username, string twofactorSecret);
        bool SetTwoFactorEnabled(string username, bool enabled);
    }
}
