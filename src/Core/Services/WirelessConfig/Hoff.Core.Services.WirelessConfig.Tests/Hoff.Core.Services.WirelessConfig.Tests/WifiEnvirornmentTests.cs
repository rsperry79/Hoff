
using System;

using Hoff.Core.Services.Common.Interfaces.Wireless;
using Hoff.Core.Services.Settings.Tests.Helpers;

using Microsoft.Extensions.Logging;

using nanoFramework.TestFramework;
namespace Hoff.Core.Services.WirelessConfig.Tests
{

    [TestClass]
    public class WifiEnvironmentTests
    {
        private static ILogger Logger;
        private static IServiceProvider Services;

        #region Tests
        [TestMethod]
        public void GetAvailableAPsTest()
        {
            // Arrange
            IWifiEnvironment wifiEnvirornment = (IWifiEnvironment)Services.GetService(typeof(IWifiEnvironment));

            // Act
            Array result = wifiEnvirornment.GetAvailableAPs();

            // Assert
            Assert.IsNotNull(result);
            Logger.LogInformation(Equals(result, null) ? "No APs found" : $"GetAvailableAPs APs found: {result.Length}");
            Assert.IsTrue(result.Length > 0);
        }

        [TestMethod]
        public void GetAvailableAPsForceFalseTest()
        {
            // Arrange
            IWifiEnvironment wifiEnvirornment = (IWifiEnvironment)Services.GetService(typeof(IWifiEnvironment));
            bool forceUpdate = false;

            // Act
            Array result = wifiEnvirornment.GetAvailableAPs(forceUpdate);

            // Assert
            Assert.IsNotNull(result);
            Logger.LogInformation(Equals(result, null) ? "No APs found" : $"ForceFalse1 APs found: {result.Length}");
            Assert.IsTrue(result.Length > 0);

            Array result2 = wifiEnvirornment.GetAvailableAPs(forceUpdate, 10);
            Assert.IsNotNull(result);
            Logger.LogInformation(Equals(result2, null) ? "No APs found" : $"ForceFalse2 APs found: {result2.Length}");
            Assert.IsTrue(result.Length == result2.Length);

        }

        [TestMethod]
        public void GetAvailableAPsForceTrueTest()
        {
            // Arrange
            IWifiEnvironment wifiEnvirornment = (IWifiEnvironment)Services.GetService(typeof(IWifiEnvironment));
            bool forceUpdate = true;

            // Act
            Array result = wifiEnvirornment.GetAvailableAPs();

            // Assert
            Assert.IsNotNull(result);
            Logger.LogInformation(Equals(result, null) ? "No APs found" : $"ForceTrue1 APs found: {result.Length}");
            Assert.IsTrue(result.Length > 0);

            Array result2 = wifiEnvirornment.GetAvailableAPs(forceUpdate);
            Assert.IsNotNull(result);
            Logger.LogInformation(Equals(result2, null) ? "No APs found" : $"ForceTrue2 APs found: {result2.Length}");
            Assert.IsTrue(result2.Length > 0);

        }

        [TestMethod]
        public void GetAvailableAPsTimeTest()
        {
            // Arrange
            IWifiEnvironment wifiEnvirornment = (IWifiEnvironment)Services.GetService(typeof(IWifiEnvironment));
            bool forceUpdate = false;

            // Act
            Array result = wifiEnvirornment.GetAvailableAPs(forceUpdate, 10);

            // Assert
            Assert.IsNotNull(result);
            Logger.LogInformation(Equals(result, null) ? "No APs found" : $"Time APs found: {result.Length}");
            Assert.IsTrue(result.Length > 0);
        }

        #endregion Tests

        #region Helpers
        [Setup]
        public static void Setup()
        {
            if (Services is null)
            {
                Services = DiSetup.ConfigureServices();
                Logger = DiSetup.ConfigureLogger("WifiEnvironmentTests");

            }
        }
        #endregion
    }
}
