using System;

using Hoff.Core.IntegrationTests.Integration.Tests;
using Hoff.Core.Services.Common.Interfaces.Services;
using Hoff.Tests.App.Helpers;
using Hoff.Tests.Common;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hoff.Tests.App
{
    public class Program
    {
        private static IServiceProvider Services;

        private static ILogger Logger;
        private static IApConfig ApHelper;
        private static ISettingsService SettingsService;
        private static DependencyInjectionTests tests = new();
        public static void Main()
        {
            try
            {
                Setup();
                TempTest();
                //LoadWireless();

                return;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
                throw;
            }
        }

        private static void Setup()
        {
            ProgramHelpers.ConfigureServices();
            tests.ConfigureServices();

            Services = TestHelpers.GetServices();
            Logger = TestHelpers.GetLogger("Tests App");

            /// get services
            SettingsService = (ISettingsService)Services.GetService(typeof(ISettingsService));
        }

        private static void LoadWireless()
        {
            ApHelper = (IApConfig)Services.GetRequiredService(typeof(IApConfig));
            _ = ApHelper.StartAndWaitForConfig();
            ProgramHelpers.InfiniteLoop();
        }

        public static void TempTest()
        {


            tests.SettingsDependencyInjectionTest();
            Logger.LogInformation("Test Complete");
        }

    }
}
