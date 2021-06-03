using System;
using Microsoft.Extensions.Logging;

namespace Database.Logging
{
    public class MadWorldLoggerConfiguration
    {
        public int EventId { get; set; }
        public LogLevel MinimumLogLevel { get; set; } = LogLevel.None;

        public static MadWorldLoggerConfiguration GetConfig()
        {
            return new MadWorldLoggerConfiguration
            {
                EventId = 1,
                MinimumLogLevel = LogLevel.Information
            };
        }
    }
}
