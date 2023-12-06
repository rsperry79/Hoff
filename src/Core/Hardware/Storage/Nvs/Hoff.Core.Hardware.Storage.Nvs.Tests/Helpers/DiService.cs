
using System;

using Hoff.Core.Common.Interfaces;
using Hoff.Core.Services.Logging;

using Microsoft.Extensions.Logging;

using Microsoft.Extensions.DependencyInjection;

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

        internal static ILogger ConfigureLogging(Type type)
        {
            string loggerName = type.ToString();
            const LogLevel minLogLevel = LogLevel.Trace;

            ILoggerCore loggerCore = (ILoggerCore)services.GetRequiredService(typeof(ILoggerCore));
            loggerCore.SetDefaultLoggingLevel(minLogLevel);
            ILogger Logger = loggerCore.GetDebugLogger(loggerName);
            return Logger;
        }
    }
}
