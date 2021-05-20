using System;
namespace Business.Interfaces
{
    public interface IUserExtremeManager
    {
        bool UpdateNewSecret(string username, string secret);
    }
}
