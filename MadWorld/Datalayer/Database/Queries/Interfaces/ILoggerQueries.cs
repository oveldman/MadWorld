using System;
using System.Collections.Generic;
using Datalayer.Database.Tables;

namespace Datalayer.Database.Queries.Interfaces
{
    public interface ILoggerQueries
    {
        bool AddLog(Log log);
        List<Log> GetLogs(DateTime? startDate, DateTime? endDate);
        Log GetLog(Guid logID);
    }
}
