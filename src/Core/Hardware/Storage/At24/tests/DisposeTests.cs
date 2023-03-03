using System.Device.I2c;

using Hoff.Core.Hardware.Storage.At24.Tests.Helpers;

using nanoFramework.DependencyInjection;
using nanoFramework.TestFramework;

namespace Hoff.Core.Hardware.Storage.At24.Tests
{
    [TestClass]
    public class DisposeTests
    {
        [TestMethod]
        public void InitTest()
        {
            // Arrange
            SetupHelper.ConfigureServices();
            IEeprom at24c256Eeprom = (IEeprom)SetupHelper.Services.GetRequiredService(typeof(IEeprom));
            int bussId = 1;
            byte deviceAddr = 0x32;
            I2cBusSpeed busSpeed = I2cBusSpeed.FastMode;

            // Act
            bool result = at24c256Eeprom.Init(bussId, deviceAddr, busSpeed);

            // Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void DisposeTest()
        {
            // Arrange
            using (IEeprom at24cEeprom = SetupHelper.Setup())
            {
            }

        }
    }
}
