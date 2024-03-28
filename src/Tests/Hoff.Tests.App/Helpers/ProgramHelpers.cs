using System.Threading;

using Hoff.Server.Web;
using Hoff.Tests.Common;
using Hoff.Tests.Common.Interfaces;
using Hoff.Tests.Common.Models;

using Microsoft.Extensions.DependencyInjection;

namespace Hoff.Tests.App.Helpers
{
    public class ProgramHelpers
    {
        public static ServiceProvider Services;

        private static IServiceCollection serviceCollection;

        public static void ConfigureServices()
        {
            serviceCollection = TestHelpers.GetServiceCollection();
            LoadSingleTons();
            LoadTransients();

        }

        private static void LoadTransients()
        {
            _ = serviceCollection
                .AddTransient(typeof(UiServer))
                 .AddTransient(typeof(ISettingsTestModel), typeof(SettingsTestModel));


        }

        private static void LoadSingleTons()
        {

        }

        public static void InfiniteLoop()
        {

            Thread.Sleep(Timeout.Infinite);
        }


    }
}
