using Hoff.Core.Interfaces;
using Hoff.Core.Services.Logging;
using Hoff.Hardware.Common.Interfaces.Sensors;
using Hoff.Hardware.Sensors.Dht;

using nanoFramework.DependencyInjection;

namespace Sensors.Environmental.Dht.Tests.Helpers
{
    internal static class DiSetup
    {
        internal static ServiceProvider ConfigureServices()
        {
            ServiceProvider services = new ServiceCollection()
             .AddSingleton(typeof(ILoggerCore), typeof(LoggerCore))
             .AddSingleton(typeof(IDht11Sensor), typeof(Dht11Sensor))
             .BuildServiceProvider();

            return services;
        }
    }
}
