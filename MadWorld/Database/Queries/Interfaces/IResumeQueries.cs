using System;
using Database.Tables;

namespace Database.Queries.Interfaces
{
    public interface IResumeQueries
    {
        Resume GetLastResume();
    }
}
