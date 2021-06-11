using System;
using Datalayer.Database.Tables;

namespace Datalayer.Database.Queries.Interfaces
{
    public interface IResumeQueries
    {
        Resume GetLastResume();
    }
}
