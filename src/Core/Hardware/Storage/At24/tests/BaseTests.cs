﻿
using Hoff.Core.Hardware.Storage.At24.Tests.Helpers;

using nanoFramework.TestFramework;

namespace Hoff.Core.Hardware.Storage.At24.Tests
{
    [TestClass]
    public class BaseTests
    {

        [TestMethod]
        public void GetSizeTest()
        {
            // Arrange
            IEeprom at24c256Eeprom = SetupHelper.Setup();
            // Act
            int result = at24c256Eeprom.GetSize();

            // Assert
            Assert.AreEqual(32768, result);
        }

        [TestMethod]
        public void GetPageSizeTest()
        {
            // Arrange
            IEeprom at24c256Eeprom = SetupHelper.Setup();
            // Act
            int result = at24c256Eeprom.GetPageSize();

            // Assert
            Assert.AreEqual(64, result);
        }

        [TestMethod]
        public void GetPageCountTest()
        {
            // Arrange
            IEeprom at24c256Eeprom = SetupHelper.Setup();
            // Act
            int result = at24c256Eeprom.GetPageCount();

            // Assert
            Assert.AreEqual(512, result);
        }
    }
}
