using System;
namespace Database.Queries.Interfaces
{
    public interface IGeneralQueries
    {
        bool IsAuthenticationOnline();
        bool IsMadWorldOnline();
    }
}
