using Hoff.Core.Interfaces;
using Hoff.Core.Services.Logging;
using Hoff.Hardware.Common.Interfaces.Config;
using Hoff.Hardware.SoC.SoCEsp32;
using Hoff.Hardware.SoC.SoCEsp32.Interfaces;
using Hoff.Hardware.SoC.SoCEsp32.Models;

using nanoFramework.DependencyInjection;

namespace Sensors.Environmental.Max31865Sensor.Tests.Helpers
{
    internal class DiSetup
    {
        internal ServiceProvider ConfigureServices()
        {
            ServiceProvider services = new ServiceCollection()
             .AddSingleton(typeof(ILoggerCore), typeof(LoggerCore))
             .AddSingleton(typeof(IPinConfig), typeof(PinConfig))
             .AddSingleton(typeof(IEspConfig), typeof(EspConfig))
             .BuildServiceProvider();

            return services;
        }
    }
}
