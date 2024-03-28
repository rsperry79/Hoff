//using System;

//using Hoff.Core.Services.Common.Interfaces.Services;
//using Hoff.Core.Services.Settings.Tests.Helpers;
//using Hoff.Tests.Common;

//using Microsoft.Extensions.Logging;

//using nanoFramework.TestFramework;

//namespace Hoff.Core.Services.WirelessConfig.Tests
//{

//    [TestClass]
//    public class ApDisposeTests
//    {
//        private static ILogger Logger;
//        private static IServiceProvider Services;
//        private static ISettingsService SettingsService;

//        #region Tests
//        [TestMethod]
//        public void DisposeTest()
//        {

//            using (IApConfig ApConfig = (IApConfig)Services.GetService(typeof(IApConfig)))
//            {

//            }
//            GC.WaitForPendingFinalizers();

//            Assert.IsTrue(true);
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
