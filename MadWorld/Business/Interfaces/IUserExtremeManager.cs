using System;
using System.Collections.Generic;
using Database.Tables.Identity;

namespace Business.Interfaces
{
    public interface IUserExtremeManager
    {
        User FindUserByTwoFactorSession(Guid? session);
        List<User> GetAllUsers();
        bool SetTwoFactorEnabled(User user, bool enabled);
        bool UpdateNewSecret(string username, string secret);
        Guid? UpdateTwoFactorSession(string username);
    }
}
