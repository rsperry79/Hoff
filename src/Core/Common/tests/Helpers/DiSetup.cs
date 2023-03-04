using nanoFramework.DependencyInjection;

namespace Hoff.Core.Common.Tests.Helpers
{
    internal  static class DiSetup
    {
        internal static ServiceProvider ConfigureServices()
        {
            ServiceProvider services = new ServiceCollection()

                .BuildServiceProvider();

            return services;
        }
    }
}
