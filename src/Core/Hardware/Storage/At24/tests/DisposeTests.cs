using System.Device.I2c;

using Hoff.Core.Hardware.Storage.At24.Tests.Helpers;
using Hoff.Hardware.Common.Interfaces.Storage;

using nanoFramework.DependencyInjection;
using nanoFramework.TestFramework;

namespace Hoff.Core.Hardware.Storage.At24.Tests
{
    [TestClass]
    public class DisposeTests
    {
        #region Public Methods

        [TestMethod]
        public void DisposeTest()
        {
            // Arrange
            using (IEeprom at24cEeprom = SetupHelper.Setup())
            {
            }
        }

        [TestMethod]
        public void InitTest()
        {
            // Arrange
            SetupHelper.ConfigureServices();
            IEeprom at24c256Eeprom = (IEeprom)SetupHelper.Services.GetRequiredService(typeof(IEeprom));
            const int bussId = 1;
            const byte deviceAddr = 0x32;
            const I2cBusSpeed busSpeed = I2cBusSpeed.FastMode;
            const int size = 256;
            // Act
            bool result = at24c256Eeprom.Init(bussId, deviceAddr, busSpeed, size);

            // Assert
            Assert.IsTrue(result);
        }

        #endregion Public Methods
    }
}
