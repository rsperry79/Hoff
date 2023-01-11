using Hoff.Core.DependencyInjection.Tests.Helpers;

using nanoFramework.DependencyInjection;
using nanoFramework.TestFramework;

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
