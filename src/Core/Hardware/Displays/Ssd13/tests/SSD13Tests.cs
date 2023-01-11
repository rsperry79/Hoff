using Hoff.Hardware.Displays.Common.Interfaces;
using Hoff.Hardware.Displays.Ssd13.Tests.Helpers;

using nanoFramework.DependencyInjection;
using nanoFramework.TestFramework;

namespace Hoff.Hardware.Displays.Ssd13.Tests
{
    [TestClass]
    public class SSD13Tests
    {
        [TestMethod]
        public void BaseTest()
        {     
            // Setup
            ServiceProvider services = DiSetup.ConfigureLoggingServices(); // by ext static class as this is a common set up

            // Arrange
           DiDisplayTestClass loggingTest = (DiDisplayTestClass)services.GetRequiredService(typeof(DiDisplayTestClass));

            // Act
            loggingTest.RunLogTests();

            IDisplay display = (IDisplay)services.GetRequiredService(typeof(IDisplay));
            //display.WriteLine(1, 1, "x");
        }
    }
}
