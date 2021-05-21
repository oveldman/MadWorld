using System;
using Database.Tables.Identity;

namespace Business.Interfaces
{
    public interface IUserExtremeManager
    {
        bool UpdateNewSecret(string username, string secret);
        Guid? UpdateTwoFactorSession(string username);
        User FindUserByTwoFactorSession(Guid? session);
        bool SetTwoFactorEnabled(User user, bool enabled);
    }
}
