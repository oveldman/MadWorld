using System;
using Common;
using Datalayer.Database.Queries.Interfaces;
using Datalayer.Database.Tables;
using Microsoft.Extensions.Logging;

namespace Datalayer.Database.Logging
{
    public class MadWorldDbLogger : ILogger
    {
        protected Type loggerType;

        private readonly MadWorldLoggerConfiguration _config;

        private readonly ILoggerQueries _loggerQueries;

        public MadWorldDbLogger(ILoggerQueries loggerQueries, MadWorldLoggerConfiguration config)
        {
            _loggerQueries = loggerQueries;
            _config = config;

            if (loggerType == null)
            {
                loggerType = typeof(MadWorldSystem);
            }
        }

        public IDisposable BeginScope<TState>(TState state) => default;

        public bool IsEnabled(LogLevel logLevel)
        {
            return _config.MinimumLogLevel <= logLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            Log newLog = new()
            {
                Application = "Mad-World",
                Logger = loggerType.Name,
                Level = (int)logLevel,
                Text = formatter(state, exception),
                Message = exception?.Message,
                Exception = exception?.InnerException?.ToString(),
                StackTrace = exception?.StackTrace,
                Created = SystemTime.Now()
            };

            _loggerQueries.AddLog(newLog);
        }
    }

    public class MadWorldDbLogger<T> : MadWorldDbLogger, ILogger<T>
    {
        public MadWorldDbLogger(ILoggerQueriesSingleton loggerQueries, MadWorldLoggerConfiguration config) : base(loggerQueries, config)
        {
            loggerType = typeof(T);
        }
    }
}
