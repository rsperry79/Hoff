using System;

using Hoff.Core.Services.Common.Interfaces.Services;
using Hoff.Core.Services.Common.Interfaces.Wireless;
using Hoff.Core.Services.Settings.Tests.Helpers;

using Microsoft.Extensions.Logging;

using nanoFramework.TestFramework;

namespace Hoff.Core.Services.WirelessConfig.Tests
{

    [TestClass]
    public class ApConfigTests
    {
        private static WifiDefaults secrets = new();
        private static ILogger Logger;
        private static IServiceProvider Services;

        private static ISettingsService SettingsService;

        #region Tests
        [TestMethod]
        public void SetConfigurationTest()
        {
            //Arrange
            IWifiSettings Settings = SetConfig();

            IApConfig ApConfig = (IApConfig)Services.GetService(typeof(IApConfig));

            // Act
            bool result = ApConfig.SetConfiguration(Settings);

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
                Services = DiSetup.ConfigureServices();
                Logger = DiSetup.ConfigureLogger("TestLogger");
                SettingsService = (ISettingsService)Services.GetService(typeof(ISettingsService));
            }
        }

        private static IWifiSettings SetConfig()
        {
            IWifiSettings Settings = (IWifiSettings)SettingsService.Get(typeof(IWifiSettings));
            Settings.SSID = secrets.SSID;
            Settings.Password = secrets.Password;
            Settings.AuthenticationType = secrets.AuthenticationType;
            Settings.EncryptionType = secrets.EncryptionType;
            Settings.IsStaticIP = false;
            Settings.IsAdHoc = false;
            return Settings;
        }
        #endregion Helpers
    }
}
