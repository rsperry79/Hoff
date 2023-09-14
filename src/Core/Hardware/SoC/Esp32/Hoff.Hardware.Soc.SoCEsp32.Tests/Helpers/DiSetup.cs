using Hoff.Core.Hardware.Common.Interfaces.Config;
using Hoff.Hardware.SoC.SoCEsp32;
using Hoff.Hardware.SoC.SoCEsp32.Interfaces;
using Hoff.Hardware.SoC.SoCEsp32.Models;

using nanoFramework.DependencyInjection;

namespace Hoff.Hardware.Soc.SoCEsp32.Tests.Helpers
{
    internal static class DiSetup
    {
        #region Internal Methods

        internal static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(typeof(IPinConfig), typeof(PinConfig))
                .AddSingleton(typeof(IEspConfig), typeof(EspConfig))
                .BuildServiceProvider();
        }

        #endregion Internal Methods
    }
}
