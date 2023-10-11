using System;
using System.Threading;

using Hoff.Core.Common.Interfaces;
using Hoff.Core.Hardware.Common.Interfaces.Displays;
using Hoff.Core.Hardware.Common.Structs;
using Hoff.Hardware.Displays.Ssd13.Fonts;
using Hoff.Hardware.Displays.Ssd13.Interfaces;
using Hoff.Hardware.Displays.Ssd13.Tests.Helpers;
using Hoff.Hardware.SoC.SoCEsp32.Interfaces;

using Microsoft.Extensions.Logging;

using nanoFramework.DependencyInjection;
using nanoFramework.TestFramework;

namespace Hoff.Hardware.Displays.Ssd13.Tests
{
    [TestClass]
    public class SSD13Tests
    {
        #region Fields

        private static ISsd13 display;
        private static ServiceProvider services;

        #endregion Fields

        #region Public Methods

        public static ISsd13 Setup()
        {
            if (display is null)
            {
                services = DiSetup.ConfigureServices();

                const string loggerName = "TestLogger";
                const LogLevel minLogLevel = LogLevel.Trace;
                ILoggerCore loggerCore = (ILoggerCore)services.GetRequiredService(typeof(ILoggerCore));
                loggerCore.SetDefaultLoggingLevel(minLogLevel);
                _ = loggerCore.GetDebugLogger(loggerName);

                IEspConfig espConfig = (IEspConfig)services.GetRequiredService(typeof(IEspConfig));
                espConfig.SetI2C1Pins();

                //// Arrange
                display = (ISsd13)services.GetRequiredService(typeof(ISsd13));
                _ = display.DefaultInit();
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
            ISsd13 display = Setup();
            const bool draw = true;

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
            ISsd13 display = Setup();

            Line line = new() { X = 10, Y = 0, Length = 64 };
            const bool draw = true;

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
            ISsd13 display = Setup();

            const int x = 2;
            const int y = 2;
            const string text = "Test";
            const byte fontSize = 2;
            const bool center = false;
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
            ISsd13 display = Setup();

            const int x = 2;
            const int y = 2;
            const string text = "Test with font";
            const byte fontSize = 2;
            const bool center = false;
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

        #endregion Public Methods

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
