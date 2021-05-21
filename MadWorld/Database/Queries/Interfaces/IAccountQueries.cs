using System;
using Database.Tables.Identity;

namespace Database.Queries.Interfaces
{
    public interface IAccountQueries
    {
        User FindUserBySession(Guid? session);
        bool SetTwoFactorSession(string username, Guid? twoFactorSession);
        bool SetSecretToken(string username, string twofactorSecret);
        bool SetTwoFactorEnabled(string username, bool enabled);
    }
}
