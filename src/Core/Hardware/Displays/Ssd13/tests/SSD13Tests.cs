using System;
using System.Threading;

using Hoff.Core.Services.Logging;
using Hoff.Hardware.Common.Interfaces.Displays;
using Hoff.Hardware.Common.Structs;
using Hoff.Hardware.Displays.Ssd13.Fonts;
using Hoff.Hardware.Displays.Ssd13.Tests.Helpers;
using Hoff.Hardware.SoC.SoCEsp32.Interfaces;

using Microsoft.Extensions.Logging;

using nanoFramework.DependencyInjection;
using nanoFramework.Logging.Debug;
using nanoFramework.TestFramework;

namespace Hoff.Hardware.Displays.Ssd13.Tests
{
    [TestClass]
    public class SSD13Tests
    {
        private static ServiceProvider services;
        private static IDisplay display;
        public static IDisplay Setup()
        {
            if (display is null)
            {
                // Arrange
                LoggerCore loggerCore = new();
                string loggerName = "TestLogger";

                // Setup
                LogLevel minLogLevel = LogLevel.Trace;
                DebugLogger logger = loggerCore.GetDebugLogger(loggerName, minLogLevel);

                services = DiSetup.ConfigureServices();
                IEspConfig espConfig = (IEspConfig)services.GetRequiredService(typeof(IEspConfig));
                espConfig.SetI2C1Pins();

                //// Arrange
                display = (IDisplay)services.GetRequiredService(typeof(IDisplay));
                display.DefaultInit();
            }

            return display;
        }

        [TestMethod]
        public void ClearScreenTest()
        {
            // Arrange
            IDisplay display = Setup();

            // Act
            display.ClearScreen();

            // Assert
            Assert.IsNotNull(display);
        }

        [TestMethod]
        public void HorizontalLineTest()
        {
            // Arrange
            IDisplay display = Setup();
            bool draw = true;

            // Act
            Line line = new() { X = 0, Y = 10, Length = 128 };
            display.HorizontalLine(
                line,
                draw);
            display.UpdateDisplay();

            Thread.Sleep(TimeSpan.FromSeconds(1));

            Line line2 = new() { X = 0, Y = 20, Length = 128 };
            display.HorizontalLine(
              line2,
              draw);
            display.UpdateDisplay();

            // Assert
            Assert.IsNotNull(display);
            display.ClearScreen();
        }

        [TestMethod]
        public void VerticalLineTest()
        {
            // Arrange
            IDisplay display = Setup();

            Line line = new() { X = 10, Y = 0, Length = 64 };
            bool draw = true;

            // Act
            display.VerticalLine(
                line,
                draw);

            display.UpdateDisplay();

            Thread.Sleep(TimeSpan.FromSeconds(10));
            // Assert
            Assert.IsNotNull(display);
            display.ClearScreen();
        }

        [TestMethod]
        public void WriteLineTest()
        {
            // Arrange
            IDisplay display = Setup();

            int x = 2;
            int y = 2;
            string text = "Test";
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

            display.UpdateDisplay();
            Thread.Sleep(TimeSpan.FromSeconds(1));

            // Assert
            Assert.IsNotNull(display);
        }

        [TestMethod]
        public void WriteLineWithFontTest()
        {
            // Arrange
            IDisplay display = Setup();

            int x = 2;
            int y = 2;
            string text = "Test with font";
            byte fontSize = 2;
            bool center = false;
            object font = new BasicFont();

            // Act
            display.WriteLine(
                x,
                y,
                text,
                fontSize,
                center,
                font);

            display.UpdateDisplay();
            Thread.Sleep(TimeSpan.FromSeconds(3));

            // Assert
            Assert.IsNotNull(display);
            display.ClearScreen();
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
    }
}
