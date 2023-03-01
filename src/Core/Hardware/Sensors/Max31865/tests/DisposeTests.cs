using Hoff.Core.Hardware.Sensors.Max31865Sensor.Interfaces;
using Hoff.Core.Hardware.Sensors.Max31865Sensor.Tests.Helpers;

using nanoFramework.TestFramework;

namespace Hoff.Core.Hardware.Sensors.Max31865Sensor.Tests
{
    [TestClass]
    public class DisposeTests
    {

        [TestMethod]
        public void DisposeTest()
        {
            // Arrange

            using (IMax31865Senor bme280Sensor = SetupHelpers.Setup())
            { }


        }
    }
}
