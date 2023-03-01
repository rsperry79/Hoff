
using nanoFramework.DependencyInjection;


namespace Sensors.Environmental.HTU21D.Tests.Helpers
{
    internal class DiSetup
    {
        internal ServiceProvider ConfigureServices()
        {
            ServiceProvider services = new ServiceCollection()
                .BuildServiceProvider();

            return services;
        }
    }
}
