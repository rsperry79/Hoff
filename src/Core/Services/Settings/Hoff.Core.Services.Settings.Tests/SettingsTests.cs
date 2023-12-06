using System;

using Hoff.Core.Hardware.Common.Interfaces.Services;
using Hoff.Core.Hardware.Common.Interfaces.Storage;
using Hoff.Core.Services.Logging;
using Hoff.Core.Services.Settings.Tests.Helpers;
using Hoff.Hardware.Common.Interfaces.Storage;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using nanoFramework.Logging.Debug;
using nanoFramework.TestFramework;

namespace Hoff.Core.Services.Settings.Tests
{
    [TestClass]
    public class SettingsTests
    {
        private static DebugLogger Logger;
        private static readonly IServiceProvider Services;
        private static ISettingsService SettingsService = (ISettingsService)Services.GetService(typeof(ISettingsService));
        #region Tests 

        [TestMethod]
        public void GetSettingsTest()
        {
            // Assert
            Assert.IsNotNull(SettingsService);
        }

        [TestMethod]
        public void WriteSettingsTest()
        {
            // Arrange
            ISettingsService settings = (ISettingsService)Services.GetService(typeof(ISettingsService));
            ISettingsStorageDriver Driver = (ISettingsStorageDriver)Services.GetService(typeof(ISettingsStorageDriver));
            IWifiSettings wifiSettings = (IWifiSettings)Services.GetService(typeof(IWifiSettings));

            settings.Add(Driver, "Wifi", wifiSettings);

            // Act
            ISettingsStorageItem result = settings.GetFirstOrDefault(typeof(IWifiSettings));

            // Assert
            Assert.IsNotNull(result.Payload);
        }

        #endregion Tests

        #region Helpers

        [Setup]
        public static void Setup()
        {
            if (SettingsService is null)
            {         // Arrange
                LoggerCore loggerCore = new();
                const string loggerName = "TestLogger";

                // Setup
                const LogLevel minLogLevel = LogLevel.Trace;
                loggerCore.SetDefaultLoggingLevel(minLogLevel);

                Logger = loggerCore.GetDebugLogger(loggerName);
                ServiceProvider Services = DiSetup.ConfigureServices();

                SettingsService = (ISettingsService)Services.GetService(typeof(ISettingsService));
            }
        }
        #endregion Helpers
    }
}
