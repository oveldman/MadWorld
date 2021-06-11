using System;
using System.Collections.Generic;
using Datalayer.Database.Tables.Identity;

namespace Datalayer.Database.Queries.Interfaces
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
