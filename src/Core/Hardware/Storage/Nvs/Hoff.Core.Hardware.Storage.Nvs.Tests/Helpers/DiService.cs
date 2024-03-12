
using System;

using Hoff.Core.Services.Common.Interfaces;
using Hoff.Core.Services.Logging;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hoff.Core.Hardware.Storage.Nvs.Tests.Helpers
{
    internal static class DiService
    {
        private static ServiceProvider services;

        internal static ServiceProvider ConfigureServices()
        {
            services = new ServiceCollection()
                .AddSingleton(typeof(ILoggerCore), typeof(LoggerCore))
                .AddSingleton(typeof(NvsStorage))

                .BuildServiceProvider();

            return services;
        }

        internal static ILoggerCore ConfigureLogging(Type type)
        {
            string loggerName = type.ToString();
            const LogLevel minLogLevel = LogLevel.Trace;

            ILoggerCore loggerCore = (ILoggerCore)services.GetRequiredService(typeof(ILoggerCore));
            loggerCore.SetDefaultLoggingLevel(minLogLevel);

            return loggerCore;
        }
    }
}
