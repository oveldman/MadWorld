using System;
using Database.Tables;

namespace Database.Queries.Interfaces
{
    public interface ILoggerQueries
    {
        bool AddLog(Log log);
    }
}
