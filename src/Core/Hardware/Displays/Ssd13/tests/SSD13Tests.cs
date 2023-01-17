using System;
using System.Threading;

using Hoff.Hardware.Displays.Common.Interfaces;
using Hoff.Hardware.Displays.Common.Structs;
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

            Assert.IsNotNull(display);
        }


        [TestMethod]
        public void DrawDirectLineTest()
        {
            // Arrange
            SSD13Display display = new();
            DirectLine line = default;

            // Act
            display.DrawDirectLine(
                line);

            // Assert
            Assert.IsNotNull(display);
        }

        [TestMethod]
        public void ClearDirectLineTest()
        {
            // Arrange
            SSD13Display display = new();
            DirectLine line = default;
            display.DrawDirectLine(line);
            Thread.Sleep(TimeSpan.FromSeconds(1));

            // Act
            display.ClearDirectLine(
                line);

            // Assert
            Assert.IsNotNull(display);
        }

        [TestMethod]
        public void ClearScreenTest()
        {
            // Arrange
            SSD13Display display = new();

            // Act
            display.ClearScreen();

            // Assert
            Assert.IsNotNull(display);
        }

        [TestMethod]
        public void HorizontalLineTest()
        {
            // Arrange
            SSD13Display display = new();
            Line line = default;
            bool draw = false;

            // Act
            display.HorizontalLine(
                line,
                draw);

            // Assert
            Assert.IsNotNull(display);
        }

        [TestMethod]
        public void VerticalLineTest()
        {
            // Arrange
            SSD13Display display = new();
            Line line = default;
            bool draw = false;

            // Act
            display.VerticalLine(
                line,
                draw);

            // Assert
            Assert.IsNotNull(display);
        }

        [TestMethod]
        public void WriteLineTest()
        {
            // Arrange
            SSD13Display display = new();
            int x = 2;
            int y = 2;
            string text = "８９ＡＢ功夫＄";
            byte fontSize = 2;
            bool center = false;
            object font = null;

            // Act
            display.WriteLine(
                x,
                y,
                text,
                fontSize,
                center,
                font);


            // Assert
            Assert.IsNotNull(display);
        }
    }
}
