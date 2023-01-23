
using Hoff.Hardware.Common.Interfaces.Config;
using Hoff.Hardware.Common.Interfaces.Services;
using Hoff.Hardware.Common.Models;
using Hoff.Hardware.SoC.SoCEsp32;

using nanoFramework.DependencyInjection;

namespace Hoff.Hardware.Soc.SoCEsp32.Tests.Helpers
{
    internal class DiSetup
    {
        internal static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(typeof(IPinConfig), typeof(PinConfig))
                .AddSingleton(typeof(IEspConfig), typeof(EspConfig))
                .BuildServiceProvider();
        }
    }
}
