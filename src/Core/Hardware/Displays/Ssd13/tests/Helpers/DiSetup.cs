using Hoff.Core.Interfaces;
using Hoff.Core.Logging;
using Hoff.Hardware.Common.Interfaces.Config;
using Hoff.Hardware.Common.Interfaces.Services;
using Hoff.Hardware.Common.Models;
using Hoff.Hardware.Displays.Common.Interfaces;
using Hoff.Hardware.SoC.SoCEsp32;

using nanoFramework.DependencyInjection;

namespace Hoff.Hardware.Displays.Ssd13.Tests.Helpers
{
    internal class DiSetup
    {
        internal static ServiceProvider ConfigureLoggingServices()
        {
            return new ServiceCollection()
                 .AddSingleton(typeof(IPinConfig), typeof(PinConfig))
                .AddSingleton(typeof(IEspConfig), typeof(EspConfig))
                .AddSingleton(typeof(ILoggerCore), typeof(LoggerCore))
                .AddTransient(typeof(DiDisplayTestClass))
                .AddSingleton(typeof(IDisplay), typeof(Display))

                .BuildServiceProvider();


        }
    }
}
