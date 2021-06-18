using System;
using System.Collections.Generic;
using System.Linq;
using Business.Interfaces;
using Datalayer.Database.Queries.Interfaces;
using Datalayer.Database.Tables;
using Microsoft.Extensions.Logging;
using Website.Shared.Models.BackendInfo;

namespace Business
{
    public class LoggingManager : ILoggingManager
    {
        ILoggerQueries _loggerQueries;

        public LoggingManager(ILoggerQueries loggerQueries)
        {
            _loggerQueries = loggerQueries;
        }

        public LogItem GetLog(Guid logID)
        {
           Log log = _loggerQueries.GetLog(logID);

            if (log is null) return null;

            return new LogItem
            {
                Application = log.Application,
                StackTrace = log.StackTrace,
                Created = log.Created,
                ID = log.ID,
                Exception = log.Exception,
                Level = Enum.GetName(typeof(LogLevel), log.Level),
                Logger = log.Logger,
                Message = log.Message,
                Text = log.Text
            };
        }

        public List<LogItem> GetLogging(DateTime? startDate, DateTime? endDate)
        {
            List<Log> dbLogs = _loggerQueries.GetLogs(startDate, endDate);

            if (dbLogs is null) return new List<LogItem>();

            return dbLogs.Select(l =>
                            new LogItem {
                                Application = l.Application,
                                Created = l.Created,
                                ID = l.ID,
                                Level = Enum.GetName(typeof(LogLevel), l.Level),
                                Text = l.Text
                            }).ToList();
        }
    }
}
