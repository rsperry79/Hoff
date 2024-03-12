using Hoff.Core.Services.Common.Interfaces;
using Hoff.Core.Services.Logging;

using Microsoft.Extensions.DependencyInjection;

namespace Hoff.Core.DependencyInjection.Tests.Helpers
{
    internal class DiSetup
    {
        internal static ServiceProvider ConfigureLoggingServices()
        {
            return new ServiceCollection()
                .AddSingleton(typeof(ILoggerCore), typeof(LoggerCore))
                 .AddSingleton(typeof(DiLoggingTestClass))

                .BuildServiceProvider();
        }
    }
}
