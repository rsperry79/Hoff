using nanoFramework.DependencyInjection;

namespace Hoff.Hardware.Sensors.Environmental.Tests
{
    internal class DiSetup
    {
        internal static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
            //.AddSingleton(typeof(IPinConfig), typeof(PinConfig))
            //.AddSingleton(typeof(IEspConfig), typeof(EspConfig))
            .BuildServiceProvider();
        }
    }
}
