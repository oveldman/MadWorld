using System;
namespace Datalayer.Database.Queries.Interfaces
{
    public interface IGeneralQueries
    {
        bool IsAuthenticationOnline();
        bool IsMadWorldOnline();
    }
}
