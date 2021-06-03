using System;
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
    }
}
