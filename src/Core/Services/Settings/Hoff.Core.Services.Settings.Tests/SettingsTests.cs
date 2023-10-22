using System;

using Hoff.Core.Hardware.Common.Interfaces.Services;
using Hoff.Core.Hardware.Common.Interfaces.Storage;
using Hoff.Core.Services.Logging;
using Hoff.Core.Services.Settings.Tests.Helpers;

using Microsoft.Extensions.Logging;

using nanoFramework.DependencyInjection;
using nanoFramework.Logging.Debug;
using nanoFramework.TestFramework;

namespace Hoff.Core.Services.Settings.Tests
{
    [TestClass]
    public class SettingsTests
    {
        private static DebugLogger Logger;
        private static IServiceProvider Services;
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
            ISettingsService settings = (ISettingsService)Services.GetService(typeof(ISettingsService));
            // Arrange
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
