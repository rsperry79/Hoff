using System;

using Hoff.Core.Hardware.Common.Interfaces.Services;
using Hoff.Core.Services.Common.Interfaces.Services;
using Hoff.Core.Services.Logging;
using Hoff.Core.Services.Settings.Tests.Helpers;
using Hoff.Core.Services.WirelessConfig.Models;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging.Debug;
using nanoFramework.TestFramework;

namespace Hoff.Core.Services.WirelessConfig.Tests
{

    [TestClass]
    public class ApConfigTests
    {
        private static Secrets secrets = new();
        private static DebugLogger Logger;
        private static LoggerCore LoggerCore;
        private static IServiceProvider Services;

        private static IApConfig ApConfig;


        #region Tests
        [TestMethod]
        public void GetWifiSettingsTest()
        {

            // Act
            IWifiSettings result = ApConfig.GetWifiSettings();

            // Assert
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void SetConfigurationTest()
        {
            // Arrange

            IWifiSettings settings = new WifiSettings(LoggerCore);
            settings.SSID = secrets.SSID;
            settings.Password = secrets.Password;

            ApConfig.StartAndWaitForConfig();

            // Act
            bool result = ApConfig.SetConfiguration(
                settings);

            // Assert
            Assert.IsTrue(result);
        }

        #endregion Tests

        #region Helpers

        [Setup]
        public static void Setup()
        {
            if (Services is null)
            {
                LoggerCore = new();
                LoggerCore.SetDefaultLoggingLevel(LogLevel.Trace);

                Logger = LoggerCore.GetDebugLogger("TestLogger");
                Services = DiSetup.ConfigureServices();

                ApConfig = (IApConfig)Services.GetService(typeof(IApConfig));
                ApConfig.StartAndWaitForConfig();

            }
        }
        #endregion Helpers
    }
}
