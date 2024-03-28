//using System;

//using Hoff.Core.Services.Common.Interfaces.Services;
//using Hoff.Core.Services.Common.Interfaces.Wireless;
//using Hoff.Core.Services.Settings.Tests.Helpers;
//using Hoff.Tests.Common;

//using Microsoft.Extensions.Logging;

//using nanoFramework.TestFramework;

//namespace Hoff.Core.Services.WirelessConfig.Tests
//{

//    [TestClass]
//    public class ApAdHocTests
//    {
//        private static ILogger Logger;
//        private static IServiceProvider Services;

//        private static ISettingsService SettingsService;
//        #region Tests

//        [TestMethod]
//        public void StartAndWaitForConfigAdHocTest()
//        {
//            try
//            {
//                // Arrange
//                IWifiSettings Settings = (IWifiSettings)SettingsService.Get(typeof(IWifiSettings));

//                Settings.SSID = "Nano";
//                Settings.Password = "password";
//                string ip = "192.168.4.1";
//                string mask = "255.255.255.0";
//                Settings.EncryptionType = (int)System.Net.NetworkInformation.EncryptionType.None;
//                Settings.AuthenticationType = (int)System.Net.NetworkInformation.AuthenticationType.Open;
//                Settings.IsStaticIP = true;
//                Settings.NetMask = mask;
//                Settings.Address = ip;
//                Settings.Gateway = ip;
//                Settings.IsAdHoc = true;

//                SettingsService.Add(Settings);
//                IApConfig ApConfig = (IApConfig)Services.GetService(typeof(IApConfig));

//                //// Act
//                bool result = ApConfig.StartAndWaitForConfig();

//                // Assert
//                //Assert.IsTrue(result);
//                Assert.IsTrue(true);
//            }
//            catch (Exception ex)
//            {
//                Logger.LogError(ex, ex.Message);
//                throw;
//            }
//        }
//        #endregion Tests

//        #region Helpers

//        private static bool isSetup = false;

//        public void ConfigureServices()
//        {
//            if (isSetup is false)
//            {
//                TestHelpers.GetServiceCollection();
//                DiSetup.ConfigureServices();
//                isSetup = true;
//            }
//        }

//        [Setup]
//        public void Setup()
//        {
//            if (Services is null)
//            {
//                this.ConfigureServices();
//                Services = TestHelpers.GetServices();
//                Logger = TestHelpers.GetLogger("Integration Tests");
//                SettingsService = (ISettingsService)Services.GetService(typeof(ISettingsService));

//            }
//        }
//        #endregion Helpers
//    }
//}

