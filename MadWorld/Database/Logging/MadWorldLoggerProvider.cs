using System;
using System.Collections.Concurrent;
using Database.Queries;
using Database.Queries.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Database.Logging
{
    public class MadWorldLoggerProvider : ILoggerProvider
    {
        private readonly IDisposable _onChangeToken;
        private MadWorldLoggerConfiguration _currentConfig;
        private readonly ConcurrentDictionary<string, MadWorldDbLogger> _loggers = new();
        private readonly ILoggerQueries _loggerQueries;

        public MadWorldLoggerProvider(MadWorldLoggerConfiguration config, DbContextOptions<MadWorldContext> options)
        {
            _currentConfig = config;
            _loggerQueries = new LoggerQueries(new MadWorldContext(options));
        }

        public MadWorldLoggerProvider(IOptionsMonitor<MadWorldLoggerConfiguration> config, DbContextOptions<MadWorldContext> options)
        {
            _currentConfig = config.CurrentValue;
            _onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
            _loggerQueries = new LoggerQueries(new MadWorldContext(options));
        }

        public ILogger CreateLogger(string categoryName)
            => _loggers.GetOrAdd(categoryName, name => new MadWorldDbLogger(_loggerQueries, _currentConfig));

        public void Dispose()
        {
            _loggers.Clear();

            if (_onChangeToken != null) {
                _onChangeToken.Dispose();
            }
        }
    }
}
