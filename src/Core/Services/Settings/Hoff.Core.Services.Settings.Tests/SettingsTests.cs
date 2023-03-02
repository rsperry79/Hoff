using Hoff.Core.Services.Settings.Tests.Helpers;
using Hoff.Core.Services.Settings.Tests.Models;

using nanoFramework.TestFramework;

namespace Hoff.Core.Services.Settings.Tests
{
    [TestClass]
    public class SettingsTests
    {
        [TestMethod]

        public void GetSettingsTest()
        {
            // Arrange
            Settings<SettingsTestModel> settings = TestHelpers.Setup();

            // Act

            // Assert
            Assert.IsNotNull(settings);
        }

        [TestMethod]
        public void WriteSettingsTest()
        {
            // Arrange
            Settings<SettingsTestModel> settings = TestHelpers.Setup();
            // Act
            bool result = settings.WriteSettings();

            // Assert
            Assert.IsTrue(result);
        }
    }
}
