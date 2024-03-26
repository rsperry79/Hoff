using System;
using System.Threading;

using Hoff.Core.Services.Common.Interfaces.Services;
using Hoff.Core.Services.Logging;
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

        //public static void AddTransient(Type iface, Type concrete)
        //{
        //    if (!sCollection.Contains(new ServiceDescriptor(iface, concrete)))
        //    {
        //        sCollection.AddTransient(iface, concrete);
        //    }
        //}

        //public static void AddSingleton(Type iface, Type concrete)
        //{
        //    if (!sCollection.Contains(new ServiceDescriptor(iface, concrete)))
        //    {
        //        sCollection.AddSingleton(iface, concrete);
        //    }
        //}

        private static void LoadTransients()
        {
            _ = sCollection
                .AddTransient(typeof(ISettingsTestModel), typeof(SettingsTestModel));
        }

        private static void LoadSingleTons()
        {
            _ = sCollection
                .AddSingleton(typeof(ILoggerCore), typeof(LoggerCore));

        }

        public static void InfiniteLoop()
        {

            Thread.Sleep(Timeout.Infinite);
        }

    }
}
