using System;
using System.Threading;

using Hoff.Core.Common.Interfaces;
using Hoff.Core.Hardware.Common.Interfaces.Services;
using Hoff.Server.Core.Helpers;
using Hoff.Server.Web;

using Microsoft.Extensions.Logging;

using nanoFramework.DependencyInjection;

namespace Hoff.Server.Core
{
    public class Program
    {
        private static ServiceProvider services;

        private static ILogger Logger;
        private static IApConfig ApHelper;
        private static IWifiSettings wifiSettings;

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
            wifiSettings = (IWifiSettings)services.GetRequiredService(typeof(IWifiSettings));
            ApHelper = (IApConfig)services.GetRequiredService(typeof(IApConfig));
            bool ap = ApHelper.StartAndWaitForConfig();
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
