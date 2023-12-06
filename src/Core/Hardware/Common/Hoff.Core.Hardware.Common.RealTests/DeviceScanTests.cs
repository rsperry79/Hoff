using Hoff.Core.Hardware.Common.Services;
using Hoff.Core.Services.Logging;
using Hoff.Hardware.SoC.SoCEsp32.Interfaces;

using Microsoft.Extensions.DependencyInjection;

using nanoFramework.TestFramework;

namespace Hoff.Core.Hardware.Common.RealTests
{
    [TestClass]
    internal class DeviceScanTests
    {
        #region Public Methods

        [TestMethod]
        public void I2cScanner()
        {
            ServiceProvider services = DiSetup.ConfigureServices();
            LoggerCore loggerCore = new();
            _ = loggerCore.GetDebugLogger("TestLogger");

            IEspConfig espConfig = (IEspConfig)services.GetRequiredService(typeof(IEspConfig));
            espConfig.SetI2C1Pins();
            espConfig.SetI2C2Pins();

            I2cBussControllerService scanner = new();
            Assert.IsTrue(scanner.I2C1.Count > 0);
            Assert.IsTrue(scanner.I2C2.Count == 0);
        }

        #endregion Public Methods
    }
}
