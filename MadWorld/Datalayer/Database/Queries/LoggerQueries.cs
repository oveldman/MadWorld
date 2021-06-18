using System;
using System.Collections.Generic;
using System.Linq;
using Datalayer.Database.Queries.Interfaces;
using Datalayer.Database.Tables;

namespace Datalayer.Database.Queries
{
    public class LoggerQueries : ILoggerQueries, ILoggerQueriesSingleton
    {
        private MadWorldContext _context;

        public LoggerQueries(MadWorldContext context)
        {
            _context = context;
        }

        public bool AddLog(Log log)
        {
            try
            {
                _context.Logs.Add(log);
                _context.SaveChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public Log GetLog(Guid logID)
        {
            return _context.Logs.FirstOrDefault(l => l.ID.Equals(logID));
        }

        public List<Log> GetLogs(DateTime? startDate, DateTime? endDate)
        {
            //End date gets a day extra because that day needs to be included

            if (startDate.HasValue || endDate.HasValue) {
                return _context.Logs.Where(l =>
                            (startDate == null || l.Created > startDate.Value)
                                && (endDate == null || l.Created < endDate.Value.AddDays(1)))
                            .OrderByDescending(l => l.Created)
                            .ToList();
            }

            return _context.Logs
                            .OrderByDescending(l => l.Created)
                            .Take(100)
                            .ToList();
        }
    }
}
