using System;
using System.Threading;

using Hoff.Core.Services.Common.Interfaces.Services;
using Hoff.Server.Core.Helpers;
using Hoff.Server.Web;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hoff.Server.Core
{
    public class Program
    {
        private static ServiceProvider services;

        private static ILogger Logger;
        private static IApConfig ApHelper;

        public static void Main()
        {
            try
            {
                services = DiSetup.ConfigureServices();
                ConfigureLogging();
                LoadWireless();

                _ = services.GetRequiredService(typeof(UiServer));

                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
                throw;
            }
        }

        private static void LoadWireless()
        {
            ApHelper = (IApConfig)services.GetRequiredService(typeof(IApConfig));
            _ = ApHelper.StartAndWaitForConfig();
        }

        private static void ConfigureLogging()
        {
            string loggerName = typeof(Program).Name.ToString();
            const LogLevel minLogLevel = LogLevel.Trace;

            ILoggerCore loggerCore = (ILoggerCore)services.GetRequiredService(typeof(ILoggerCore));
            loggerCore.SetDefaultLoggingLevel(minLogLevel);
            Logger = loggerCore.GetDebugLogger(loggerName);
        }
    }
}
