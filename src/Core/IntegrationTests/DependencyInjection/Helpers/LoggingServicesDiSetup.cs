using Hoff.Core.Interfaces;
using Hoff.Core.Logging;

using nanoFramework.DependencyInjection;

namespace Hoff.Core.DependencyInjection.Tests.Helpers
{
    internal class LoggingServicesDiSetup
    {
        internal static ServiceProvider ConfigureLoggingServices()
        {
            return new ServiceCollection()
                .AddSingleton(typeof(ILoggerCore), typeof(LoggerCore))
                 .AddSingleton(typeof(DiLoggingTestClass))

                .BuildServiceProvider();
        }
    }
}
