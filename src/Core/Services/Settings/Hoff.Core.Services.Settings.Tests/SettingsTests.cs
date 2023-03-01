
using Hoff.Core.Common.Services.Settings.Tests.Helpers;

using nanoFramework.DependencyInjection;
using nanoFramework.TestFramework;

using NFUnitTest.Models;


namespace Hoff.Core.Common.Services.Settings.Tests
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
