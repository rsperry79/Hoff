using System;
using System.Device.Gpio;
using System.Threading;

using nanoFramework.Hardware.Esp32;
using nanoFramework.TestFramework;

namespace Hoff.Hardware.Soc.SoCEsp32.Tests
{
    [TestClass]
    public class BaseTests
    {
        #region Fields

        private static GpioPin button;
        private static GpioController gpioController;
        private static GpioPin led;
        private static bool wasPressed = false;

        #endregion Fields

        #region Public Methods

        [TestMethod]
        public void BlinkLedTest()
        {
            int index = 0;
            int sleep = 256;
            led.Write(PinValue.Low);

            do
            {
                led.Toggle();
                Thread.Sleep(TimeSpan.FromMilliseconds(sleep));
                led.Toggle();
                Thread.Sleep(TimeSpan.FromMilliseconds(sleep * 2));
                led.Toggle();
                Thread.Sleep(TimeSpan.FromMilliseconds(sleep * 3));
                led.Toggle();
                Thread.Sleep(TimeSpan.FromMilliseconds(sleep * 4));
                index++;
            }
            while (index < 2);

            Assert.AreEqual(2, index);
        }

        [TestMethod]
        public void ButtonEventTest()
        {
            button.ValueChanged += this.Button_ValueChanged;
            led.Write(PinValue.Low);
            int index = 0;

            do
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(100));
                index++;
            }
            while (!wasPressed && index < 30);
            Assert.IsTrue(index < 31);
        }

        [Setup]
        public void Setup()
        {
            gpioController = new GpioController();
            led = gpioController.OpenPin(Gpio.IO02, PinMode.Output);
            button = gpioController.OpenPin(Gpio.IO03, PinMode.Input);
        }

        #endregion Public Methods

        #region Private Methods

        private void Button_ValueChanged(object sender, PinValueChangedEventArgs e)
        {
            led.Toggle();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            led.Toggle();
            wasPressed = true;
        }

        #endregion Private Methods
    }
}
