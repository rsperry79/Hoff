


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
//    public class ApClientTests
//    {
//        private static WifiDefaults secrets = new();
//        private static ILogger Logger;
//        private static IServiceProvider Services;

//        private static ISettingsService SettingsService;

//        #region Tests

//        //[TestMethod]
//        //public void StartAndWaitForConfigTest()
//        //{
//        //    // Arrange
//        //    IWifiSettings Settings = (IWifiSettings)SettingsService.Get(typeof(IWifiSettings));
//        //    Settings.SSID = secrets.SSID;
//        //    Settings.Password = secrets.Password;
//        //    Settings.AuthenticationType = secrets.AuthenticationType;
//        //    Settings.EncryptionType = secrets.EncryptionType;
//        //    Settings.IsStaticIP = false;
//        //    Settings.IsAdHoc = false;
//        //    SettingsService.Add(Settings);
//        //    IApConfig ApConfig = (IApConfig)Services.GetService(typeof(IApConfig));

//        //    // Act
//        //    bool result = ApConfig.StartAndWaitForConfig();

//        //    // Assert
//        //    Assert.IsTrue(result);
//        //}
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

//            }
//        }

//        private static IWifiSettings SetConfig()
//        {
//            IWifiSettings Settings = (IWifiSettings)Services.GetService(typeof(IWifiSettings));
//            Settings.SSID = secrets.SSID;
//            Settings.Password = secrets.Password;
//            Settings.AuthenticationType = secrets.AuthenticationType;
//            Settings.EncryptionType = secrets.EncryptionType;
//            Settings.IsStaticIP = false;
//            Settings.IsAdHoc = false;
//            return Settings;
//        }
//        #endregion Helpers
//    }
//}
