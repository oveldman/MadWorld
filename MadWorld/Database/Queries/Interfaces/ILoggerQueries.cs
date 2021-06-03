using System;
using System.Collections.Generic;
using Database.Tables;

namespace Database.Queries.Interfaces
{
    public interface ILoggerQueries
    {
        bool AddLog(Log log);
        List<Log> GetLogs(DateTime? startDate, DateTime? endDate); 
    }
}
