using nanoFramework.TestFramework;
using nanoFramework.DependencyInjection;
using System;
using Microsoft.Extensions.Logging;
using Hoff.Core.Interfaces;
using Hoff.Core.Logging;
using Hoff.Core.DependencyInjection.Tests.Helpers;

namespace Hoff.Core.IntegrationTests.DependencyInjection.Tests
{
    [TestClass]
    public class DependencyInjectionCoreTests
    {
        [TestMethod]
        public void LoggingDependencyInjectionTest()
        {
            // Setup
            ServiceProvider services = LoggingServicesDiSetup.ConfigureLoggingServices(); // by ext static class as this is a common set up

            // Arrange
            DiLoggingTestClass loggingTest = (DiLoggingTestClass)services.GetRequiredService(typeof(DiLoggingTestClass));

            // Act
            loggingTest.RunLogTests();
        }
    }
}
