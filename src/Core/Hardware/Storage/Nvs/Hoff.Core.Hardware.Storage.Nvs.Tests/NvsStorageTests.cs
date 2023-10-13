
using Hoff.Core.Common.Interfaces;
using Hoff.Core.Services.Logging;
using Hoff.Core.Hardware.Common.Models;

using nanoFramework.TestFramework;
using Hoff.Core.Hardware.Common.Interfaces.Services;

namespace Hoff.Core.Hardware.Storage.Nvs.Tests
{

    [TestClass]
    public class NvsStorageTests
    {
        

        [TestMethod]
        public void BaseTest()
        {
             ILoggerCore LoggerCore = new LoggerCore();
            // Arrange
            Settings settings = new();
            NvsStorage<Settings> nvsStorage = new NvsStorage<Settings>(LoggerCore, settings);

            // Act
            Assert.IsNotNull(nvsStorage.Settings);

            // Assert
        }

        //[TestMethod]
        //public void Dispose_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var nvsStorage = new NvsStorage(TODO, TODO, TODO);

        //    // Act
        //    nvsStorage.Dispose();

        //    // Assert
        //    Assert.Fail();
        //}
    }
}
