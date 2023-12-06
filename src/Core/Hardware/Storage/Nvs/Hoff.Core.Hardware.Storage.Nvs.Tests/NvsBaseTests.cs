using Hoff.Core.Hardware.Storage.Nvs.Tests.Helpers;

using Microsoft.Extensions.Logging;

using Microsoft.Extensions.DependencyInjection;
using nanoFramework.TestFramework;

namespace Hoff.Core.Hardware.Storage.Nvs.Tests
{
    [TestClass]
    public class NvsBaseTests
    {
        private static ServiceProvider services;

        private static ILogger Logger;

        [TestMethod]
        public void NvsClearTest()
        {
            // Arrange
            string storageName = "Test";

            // Act
            NvsStorage nvsStorage = (NvsStorage)services.GetRequiredService(typeof(NvsStorage));

            nvsStorage.Clear(storageName);
            // Assert
            Assert.AreEqual(string.Empty, nvsStorage.Read(storageName));
        }

        [TestMethod]
        public void NvsWriteTest()
        {
            // Arrange
            string storageName = "WriteTest";
            string expected = "Hello World";

            // Act
            NvsStorage nvsStorage = (NvsStorage)services.GetRequiredService(typeof(NvsStorage));
            nvsStorage.Write(storageName, expected);

            // Act
            Assert.AreEqual(expected, nvsStorage.Read(storageName));
        }

        [Setup]
        public void Setup()
        {
            services = DiService.ConfigureServices();
            Logger = DiService.ConfigureLogging(typeof(NvsBaseTests));
        }
    }
}
