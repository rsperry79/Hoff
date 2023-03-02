using nanoFramework.DependencyInjection;

namespace Hoff.Core.Common.Tests.Helpers
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
