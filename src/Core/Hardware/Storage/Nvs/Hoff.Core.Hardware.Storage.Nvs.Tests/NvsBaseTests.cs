using Hoff.Core.Hardware.Storage.Nvs.Tests.Helpers;
using Hoff.Core.Services.Common.Interfaces;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using nanoFramework.Json;
using nanoFramework.TestFramework;

namespace Hoff.Core.Hardware.Storage.Nvs.Tests
{
    [TestClass]
    public class NvsBaseTests
    {
        private static ServiceProvider services;

        private static string storageName = typeof(Secrets).Name;

        private static Secrets Secrets = new Secrets();
        private static ILoggerCore LoggerCore;

        [TestMethod]
        public void NvsClearTest()
        {
            // Arrange


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
            string json = JsonConvert.SerializeObject(Secrets);

            // Act
            NvsStorage nvsStorage = (NvsStorage)services.GetRequiredService(typeof(NvsStorage));
            nvsStorage.Write(storageName, json);

            string returnedJson = nvsStorage.Read(storageName);
            Secrets result = (Secrets)JsonConvert.DeserializeObject(returnedJson, typeof(Secrets));

            // Act
            Assert.AreEqual(Secrets.TestString, result.TestString);
            Assert.AreEqual(Secrets.TestInt, result.TestInt);
            Assert.AreEqual(Secrets.TestDouble, result.TestDouble);
            Assert.AreEqual(Secrets.TestBool, result.TestBool);
        }




        [Setup]
        public void Setup()
        {
            services = DiService.ConfigureServices();
            LoggerCore = DiService.ConfigureLogging(typeof(NvsBaseTests));
            ILogger logger = LoggerCore.GetDebugLogger(nameof(NvsBaseTests));
            logger.LogTrace($"Storage Name :{storageName}");

        }
    }
}
