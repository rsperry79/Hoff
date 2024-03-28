using System;

using Hoff.Core.Services.Common.Interfaces.Services;
using Hoff.Core.Services.WirelessConfig.Tests;
using Hoff.Tests.App.Helpers;
using Hoff.Tests.Common;
using Hoff.Tests.Common.Interfaces;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using nanoFramework.TestFramework;

namespace Hoff.Tests.App
{
    public class Program
    {
        private static IServiceProvider Services;

        private static ILogger Logger;
        private static IApConfig ApHelper;
        private static ISettingsService SettingsService;
        private static ApConfigTests tests = new();
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
            }
        }

        private static void Setup()
        {
            ProgramHelpers.ConfigureServices();
            tests.Setup();

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


            // Arrange
            SettingsService.FactoryReset(true);
            SettingsService.Add(typeof(ISettingsTestModel));

            // Act
            ISettingsTestModel result = (ISettingsTestModel)SettingsService.Get(typeof(ISettingsTestModel));

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(SettingsService.SettingsCount(), 1);


            //SettingsService.FactoryReset(true);
            //tests.SetConfigurationTest();
            Logger.LogInformation("Test Complete");
        }

    }
}
