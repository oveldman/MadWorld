using System;
namespace Database.Queries.Interfaces
{
    public interface IAccountQueries
    {
        bool SetSecretToken(string username, string twofactorSecret);
    }
}
