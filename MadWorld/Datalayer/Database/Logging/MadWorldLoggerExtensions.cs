using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace Datalayer.Database.Logging
{
    public static class MadWorldLoggerExtensions
    {
        public static ILoggingBuilder AddMadWorldLogger(
            this ILoggingBuilder builder, DbContextOptions<MadWorldContext> options, MadWorldLoggerConfiguration config = null)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<ILoggerProvider, MadWorldLoggerProvider>(_ => new MadWorldLoggerProvider(config, options)));

            LoggerProviderOptions.RegisterProviderOptions
                <MadWorldLoggerConfiguration, MadWorldLoggerProvider>(builder.Services);

            return builder;
        }

        public static ILoggingBuilder AddMadWorldLogger(
            this ILoggingBuilder builder,
            Action<MadWorldLoggerConfiguration> configure, DbContextOptions<MadWorldContext> options)
        {
            builder.AddMadWorldLogger(options);
            builder.Services.Configure(configure);

            return builder;
        }
    }
}
