using Hoff.Hardware.Displays.Ssd13.Tests;

using Hoff.Hardware.Displays.Common.Interfaces;
using Hoff.Hardware.Displays.Ssd13.Tests.Helpers;

using nanoFramework.DependencyInjection;
using nanoFramework.TestFramework;
using Moq;

using System;
using Hoff.Hardware.Displays.Common.Structs;

namespace Hoff.Hardware.Displays.Ssd13.Tests
{
    [TestClass]
    public class SSD13DisplayTests
    {
        private MockRepository mockRepository;



        [Setup]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        private SSD13Display CreateSSD13Display()
        {
            return new SSD13Display();
        }

        [TestMethod]
        public void DrawDirectLine_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var sSD13Display = this.CreateSSD13Display();
            DirectLine line = default(global::Hoff.Hardware.Displays.Common.Structs.DirectLine);

            // Act
            sSD13Display.DrawDirectLine(
                line);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        public void ClearDirectLine_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var sSD13Display = this.CreateSSD13Display();
            DirectLine line = default(global::Hoff.Hardware.Displays.Common.Structs.DirectLine);

            // Act
            sSD13Display.ClearDirectLine(
                line);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        public void ClearScreen_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var sSD13Display = this.CreateSSD13Display();

            // Act
            sSD13Display.ClearScreen();

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        public void HorizontalLine_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var sSD13Display = this.CreateSSD13Display();
            Line line = default(global::Hoff.Hardware.Displays.Common.Structs.Line);
            bool draw = false;

            // Act
            sSD13Display.HorizontalLine(
                line,
                draw);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        public void VerticalLine_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var sSD13Display = this.CreateSSD13Display();
            Line line = default(global::Hoff.Hardware.Displays.Common.Structs.Line);
            bool draw = false;

            // Act
            sSD13Display.VerticalLine(
                line,
                draw);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        public void WriteLine_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var sSD13Display = this.CreateSSD13Display();
            int x = 0;
            int y = 0;
            string text = null;
            byte fontSize = 0;
            bool center = false;
            object font = null;

            // Act
            sSD13Display.WriteLine(
                x,
                y,
                text,
                fontSize,
                center,
                font);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }
    }
}
