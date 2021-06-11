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

        public List<LogItem> GetLogging(DateTime? startDate, DateTime? endDate)
        {
            List<Log> dbLogs = _loggerQueries.GetLogs(startDate, endDate);

            if (dbLogs == null) return new List<LogItem>();

            return dbLogs.Select(l =>
                            new LogItem {
                                Application = l.Application,
                                StackTrace = l.StackTrace,
                                Created = l.Created,
                                ID = l.ID,
                                Exception = l.Exception,
                                Level = Enum.GetName(typeof(LogLevel), l.Level),
                                Logger = l.Logger,
                                Message = l.Message,
                                Text = l.Text
                            }).ToList();
        }
    }
}
