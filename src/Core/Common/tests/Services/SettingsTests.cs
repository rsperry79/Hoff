using System;
using System.Diagnostics;

using nanoFramework.TestFramework;

using Hoff.Core.Common.Services;
using NFUnitTest.Models;

namespace Hoff.Core.Common.Services
{
    [TestClass]
    public class SettingsTests
    {
        [TestMethod]
        public void GetSettings_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Settings<SettingsTestModel> settings = new Settings<SettingsTestModel>(null, 0x00);

            // Act


            // Assert
            Assert.IsNotNull(settings);
        }

        [TestMethod]
        public void WriteSettings_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Settings<SettingsTestModel> settings = new Settings<SettingsTestModel>(null, 0x00);

            // Act
            var result = settings.WriteSettings();

            // Assert
            Assert.IsTrue(result);
        }
    }
}
