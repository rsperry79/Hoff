using Hoff.Core.Interfaces;
using Hoff.Core.Services.Logging;
using Hoff.Hardware.Common.Interfaces.Config;
using Hoff.Hardware.Common.Interfaces.Displays;
using Hoff.Hardware.Common.Interfaces.Services;
using Hoff.Hardware.Common.Services;
using Hoff.Hardware.SoC.SoCEsp32;
using Hoff.Hardware.SoC.SoCEsp32.Interfaces;
using Hoff.Hardware.SoC.SoCEsp32.Models;

using nanoFramework.DependencyInjection;

namespace Hoff.Hardware.Displays.Ssd13.Tests.Helpers
{
    internal class DiSetup
    {
        internal static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(typeof(IPinConfig), typeof(PinConfig))
                .AddSingleton(typeof(II2cBussControllerService), typeof(I2cBussControllerService))
                .AddSingleton(typeof(IEspConfig), typeof(EspConfig))
                .AddSingleton(typeof(ILoggerCore), typeof(LoggerCore))
                .AddTransient(typeof(DiDisplayTestClass))
                .AddSingleton(typeof(IDisplay), typeof(Display))

                .BuildServiceProvider();
        }
    }
}
