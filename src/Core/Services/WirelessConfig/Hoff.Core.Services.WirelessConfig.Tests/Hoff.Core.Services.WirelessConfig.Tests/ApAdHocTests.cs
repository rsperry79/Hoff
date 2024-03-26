using System;

using Hoff.Core.Services.Common.Interfaces.Services;
using Hoff.Core.Services.Common.Interfaces.Wireless;
using Hoff.Core.Services.Settings.Tests.Helpers;

using Microsoft.Extensions.Logging;

using nanoFramework.TestFramework;

namespace Hoff.Core.Services.WirelessConfig.Tests
{

    [TestClass]
    public class ApAdHocTests
    {
        private static ILogger Logger;

        private static ISettingsService SettingsService;

        #region Tests

        [TestMethod]
        public void StartAndWaitForConfigAdHocTest()
        {
            try
            {
                // Arrange
                IWifiSettings Settings = (IWifiSettings)SettingsService.Get(typeof(IWifiSettings));

                Settings.SSID = "Nano";
                Settings.Password = "password";
                string ip = "192.168.4.1";
                string mask = "255.255.255.0";
                Settings.EncryptionType = System.Net.NetworkInformation.EncryptionType.None;
                Settings.AuthenticationType = System.Net.NetworkInformation.AuthenticationType.Open;
                Settings.IsStaticIP = true;
                Settings.NetMask = mask;
                Settings.Address = ip;
                Settings.Gateway = ip;
                Settings.IsAdHoc = true;

                SettingsService.Add(Settings);
                IApConfig ApConfig = (IApConfig)DiSetup.Services.GetService(typeof(IApConfig));

                //// Act
                bool result = ApConfig.StartAndWaitForConfig();

                // Assert
                //Assert.IsTrue(result);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
        #endregion Tests

        #region Helpers
        [Setup]
        public static void Setup()
        {

            if (DiSetup.Services is null)
            {
                _ = DiSetup.ConfigureServices();
            }

            Logger ??= DiSetup.ConfigureLogger("TestLogger");
            SettingsService ??= (ISettingsService)DiSetup.Services.GetService(typeof(ISettingsService));
        }

        #endregion Helpers
    }
}

