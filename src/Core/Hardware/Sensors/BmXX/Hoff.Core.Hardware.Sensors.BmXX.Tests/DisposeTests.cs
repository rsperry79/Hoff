﻿using Hoff.Core.Hardware.Sensors.BmXX.Interfaces;
using Hoff.Core.Hardware.Sensors.BmXX.Tests.Helpers;

using nanoFramework.TestFramework;

namespace Hoff.Core.Hardware.Sensors.BmXX.Tests
{
    [TestClass]
    public class DisposeTests
    {
        #region Public Methods

        [TestMethod]
        public void DisposeTest()
        {
            // Arrange

            using (IBme280Sensor bme280Sensor = SetupHelper.Setup())
            { }
        }

        #endregion Public Methods
    }
}
