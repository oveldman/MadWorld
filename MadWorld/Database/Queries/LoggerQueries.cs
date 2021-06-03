using System;
using System.Collections.Generic;
using System.Linq;
using Database.Queries.Interfaces;
using Database.Tables;

namespace Database.Queries
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

        public List<Log> GetLogs(DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue || endDate.HasValue) {
                return _context.Logs.Where(l =>
                            (startDate == null || l.Created > startDate)
                                && (endDate == null || l.Created < endDate))
                            .OrderBy(l => l.Created)
                            .ToList();
            }

            return _context.Logs
                            .OrderByDescending(l => l.Created)
                            .Take(100)
                            .ToList();
        }
    }
}
