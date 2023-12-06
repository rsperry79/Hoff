using Hoff.Core.DependencyInjection.Tests.Helpers;

using Microsoft.Extensions.DependencyInjection;

using nanoFramework.TestFramework;

namespace Hoff.Core.IntegrationTests.DependencyInjection.Tests
{
    [TestClass]
    public class DependencyInjectionTests
    {
        [TestMethod]
        public void LoggingDependencyInjectionTest()
        {
            // Setup
            ServiceProvider services = DiSetup.ConfigureLoggingServices(); // by ext static class as this is a common set up

            // Arrange
            DiLoggingTestClass loggingTest = (DiLoggingTestClass)services.GetRequiredService(typeof(DiLoggingTestClass));

            // Act
            loggingTest.RunLogTests();
        }
    }
}
