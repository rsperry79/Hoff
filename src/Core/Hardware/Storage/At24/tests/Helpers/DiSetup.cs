using Hoff.Core.Interfaces;
using Hoff.Core.Logging;
using Hoff.Hardware.Common.Interfaces.Config;
using Hoff.Hardware.Common.Models;
using Hoff.Hardware.SoC.SoCEsp32;
using Hoff.Hardware.SoC.SoCEsp32.Interfaces;

using nanoFramework.DependencyInjection;

namespace Hoff.Core.Hardware.Storage.At24.Tests.Helpers
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
