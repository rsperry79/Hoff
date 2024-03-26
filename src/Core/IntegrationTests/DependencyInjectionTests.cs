using System;

using Hoff.Core.IntegrationTests.Integration.Tests.Helpers;
using Hoff.Core.Services.Common.Interfaces.Services;
using Hoff.Core.Services.Common.Interfaces.Wireless;
using Hoff.Tests.Common;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using nanoFramework.TestFramework;

namespace Hoff.Core.IntegrationTests.Integration.Tests
{
    [TestClass]
    public class DependencyInjectionTests
    {
        #region Fields
        private static ILogger Logger;
        private static IServiceProvider Services;

        #endregion Fields

        #region Tests
        [TestMethod]
        public void LoggingDependencyInjectionTest()
        {
            // Setup


            // Arrange
            DiLoggingTestClass loggingTest = (DiLoggingTestClass)Services.GetRequiredService(typeof(DiLoggingTestClass));

            // Act
            loggingTest.RunLogTests();
            Assert.IsNotNull(loggingTest);
        }

        [TestMethod]
        public void SettingsDependencyInjectionTest()
        {
            // Setup
            ISettingsService SettingsService = (ISettingsService)Services.GetService(typeof(ISettingsService));

            // Arrange

            // Act
            IWifiSettings result = (IWifiSettings)SettingsService.Add(typeof(IWifiSettings));
            result.SSID = "Test";

            // Act
            Assert.IsNotNull(result);
        }
        #endregion Tests

        #region Helpers
        private static bool isSetup = false;

        public void ConfigureServices()
        {
            if (isSetup is false)
            {
                DiSetup.ConfigureServices();
                isSetup = true;
            }
        }

        [Setup]
        public void Setup()
        {
            if (Services is null)
            {
                this.ConfigureServices();
                Services = TestHelpers.GetServices();
                Logger = TestHelpers.GetLogger("Integration Tests");

            }
        }
        #endregion Helpers
    }
}
