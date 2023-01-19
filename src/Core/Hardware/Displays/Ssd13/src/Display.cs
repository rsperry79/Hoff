using System.Device.I2c;

using Hoff.Hardware.Displays.Common.Interfaces;
using Hoff.Hardware.Displays.Common.Structs;
using Hoff.Hardware.Displays.Ssd13.Fonts;

using Iot.Device.Ssd13xx;

namespace Hoff.Hardware.Displays.Ssd13
{
    public class Display : IDisplay
    {
        private static I2cDevice i2CDevice;
        private static Ssd1306 ssdDisplay;


        public Display() : this(Ssd13xx.DisplayResolution.OLED128x64)
        {

        }

        public Display(Ssd13xx.DisplayResolution resolution = Ssd13xx.DisplayResolution.OLED128x64)
        {
            if (i2CDevice == null)
            {
                int bussId = 1;
                byte address = Ssd1306.DefaultI2cAddress;
                I2cBusSpeed busSpeed = I2cBusSpeed.FastMode;
                i2CDevice = I2cDevice.Create(new I2cConnectionSettings(bussId, address, busSpeed));
            }


            ssdDisplay = new Ssd1306(i2CDevice, resolution);

        }

        public void DrawDirectLine(DirectLine line)
        {
            ssdDisplay.DrawDirectAligned(line.X, line.Y, line.Width, line.Height, line.Data);
        }

        public void ClearDirectLine(DirectLine line)
        {
            ssdDisplay.ClearDirectAligned(line.X, line.Y, line.Width, line.Height);
        }

        public void ClearScreen()
        {
            ssdDisplay.ClearScreen();
        }


        public void HorizontalLine(Line line, bool draw = true)
        {

            ssdDisplay.DrawHorizontalLine(line.X, line.Y, line.Length, draw);
        }


        public void VerticalLine(Line line, bool draw = true)
        {

            ssdDisplay.DrawVerticalLine(line.X, line.Y, line.Length, draw);
        }

        public void WriteLine(int x, int y, string text, byte fontSize = 1, bool center = false, object font = null)
        {
            ssdDisplay.Font = font is null ? new BasicFont() : (IFont)font;

            ssdDisplay.DrawString(x, y, text, fontSize, center);
            ssdDisplay.Display();
        }
    }
}
