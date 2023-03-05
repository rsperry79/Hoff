using nanoFramework.DependencyInjection;

namespace Hoff.Core.Common.Tests.Helpers
{
    internal static class DiSetup
    {
        #region Internal Methods

        internal static ServiceProvider ConfigureServices()
        {
            ServiceProvider services = new ServiceCollection()

                .BuildServiceProvider();

            return services;
        }

        #endregion Internal Methods
    }
}
