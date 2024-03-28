using System;
using System.Threading;

using Hoff.Core.Hardware.Storage.Nvs;
using Hoff.Core.Services.Common.Interfaces.Services;
using Hoff.Core.Services.Logging;
using Hoff.Core.Services.Settings;
using Hoff.Services.Common.Interfaces.Storage;
using Hoff.Tests.Common.Interfaces;
using Hoff.Tests.Common.Models;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hoff.Tests.Common
{
    public static class TestHelpers

    {
        private static ILoggerCore core;
        private static IServiceCollection sCollection;
        private static IServiceProvider Services;


        public static IServiceProvider GetServices()
        {
            if (Services == null)
            {
                Services = sCollection.BuildServiceProvider();
            }

            return Services;
        }

        public static IServiceCollection GetServiceCollection()
        {
            if (sCollection == null)
            {
                sCollection = new ServiceCollection();
                LoadTransients();
                LoadSingleTons();
            }


            return sCollection;
        }

        public static ILogger GetLogger(string name, LogLevel logLevel = LogLevel.Trace)
        {

            if (core == null)
            {
                Console.WriteLine("Configuring Logger");
                core = (ILoggerCore)Services.GetService(typeof(ILoggerCore));
                core.SetDefaultLoggingLevel(LogLevel.Trace);
            }

            return core.GetDebugLogger(name);
        }


        private static void LoadTransients()
        {
            _ = sCollection
                .AddTransient(typeof(ISettingsTestModel), typeof(SettingsTestModel));

            Console.WriteLine("Load transients");
        }

        private static void LoadSingleTons()
        {
            _ = sCollection
                .AddSingleton(typeof(ILoggerCore), typeof(LoggerCore))
                .AddSingleton(typeof(ISettingsStorageDriver), typeof(NvsStorage))
                .AddSingleton(typeof(ISettingsService), typeof(SettingsService));

        }

        public static void InfiniteLoop()
        {

            Thread.Sleep(Timeout.Infinite);
        }

    }
}
