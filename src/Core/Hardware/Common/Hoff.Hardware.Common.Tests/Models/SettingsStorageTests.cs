using System;

using Hoff.Core.Hardware.Common.Interfaces.Config;
using Hoff.Core.Hardware.Common.Interfaces.Services;
using Hoff.Core.Hardware.Common.Models;
using Hoff.Hardware.SoC.SoCEsp32.Models;

using nanoFramework.TestFramework;

namespace Hoff.Core.Hardware.Common.Tests.Models
{
    [TestClass]
    public class SettingsStorageTests
    {
        [TestMethod]
        public void FindByType_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            SettingsStorage settingsStorage = new SettingsStorage();
            Type type = typeof(IWifiSettings);

            SettingsStorageItem settingsStorageItem =   new SettingsStorageItem();
            settingsStorageItem.StorageDriver = 

            // Act
            var result = settingsStorage.FindByType(
                type);

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        public void Add_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var settingsStorage = new SettingsStorage();
            SettingsStorageItem value = null;

            // Act
            var result = settingsStorage.Add(
                value);

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        public void Contains_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var settingsStorage = new SettingsStorage();
            Type toFind = null;

            // Act
            var result = settingsStorage.Contains(
                toFind);

            // Assert
            Assert.Fail();
        }
    }
}
