using Hoff.Hardware.Common.Interfaces.Services;
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
        private static ServiceProvider services;

        [Setup]
        public void Setup()
        {

            services = DiSetup.ConfigureLoggingServices(); // by ext static class as this is a common set up
            IEspConfig espConfig = (IEspConfig)services.GetRequiredService(typeof(IEspConfig));

            // Act
            espConfig.SetSpi1Pins();
        }


        [TestMethod]
        public void BaseTest()
        {
            // Setup
            DiDisplayTestClass loggingTest = (DiDisplayTestClass)services.GetRequiredService(typeof(DiDisplayTestClass));

            // Arrange
            loggingTest.RunLogTests();

            // Act

            IDisplay display = (IDisplay)services.GetRequiredService(typeof(IDisplay));

            Assert.IsNotNull(display);
        }


        //[TestMethod]
        //public void DrawDirectLineTest()
        //{
        //    // Arrange
        //    IDisplay display = (IDisplay)services.GetRequiredService(typeof(IDisplay));
        //    DirectLine line = new DirectLine { X = 1, Y = 1, Width = 2, Height = 2 };

        //    // Act
        //    display.DrawDirectLine(
        //        line);

        //    // Assert
        //    Assert.IsNotNull(display);
        //}

        //[TestMethod]
        //public void ClearDirectLineTest()
        //{
        //    // Arrange
        //    IDisplay display = (IDisplay)services.GetRequiredService(typeof(IDisplay));
        //    DirectLine line = new DirectLine { X = 1, Y = 1, Width = 2, Height =2  };
        //    display.DrawDirectLine(line);
        //    Thread.Sleep(TimeSpan.FromSeconds(1));

        //    // Act
        //    display.ClearDirectLine(
        //        line);

        //    // Assert
        //    Assert.IsNotNull(display);
        //}

        [TestMethod]
        public void ClearScreenTest()
        {
            // Arrange
            IDisplay display = (IDisplay)services.GetRequiredService(typeof(IDisplay));

            // Act
            display.ClearScreen();

            // Assert
            Assert.IsNotNull(display);
        }

        [TestMethod]
        public void HorizontalLineTest()
        {
            // Arrange
            IDisplay display = (IDisplay)services.GetRequiredService(typeof(IDisplay));
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
            IDisplay display = (IDisplay)services.GetRequiredService(typeof(IDisplay));
            Line line = new Line { X = 10, Y = 10, Length = 10 };
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
            IDisplay display = (IDisplay)services.GetRequiredService(typeof(IDisplay));
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
