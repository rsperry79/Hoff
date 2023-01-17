using Hoff.Core.Interfaces;
using Hoff.Core.Logging;
using Hoff.Hardware.Displays.Common.Interfaces;

using nanoFramework.DependencyInjection;

namespace Hoff.Hardware.Displays.Ssd13.Tests.Helpers
{
    internal class DiSetup
    {
        internal static ServiceProvider ConfigureLoggingServices()
        {
            return new ServiceCollection()
                .AddSingleton(typeof(ILoggerCore), typeof(LoggerCore))
                .AddSingleton(typeof(IDisplay), typeof(Display))
                .AddSingleton(typeof(DiDisplayTestClass))
                .BuildServiceProvider();
        }
    }
}
