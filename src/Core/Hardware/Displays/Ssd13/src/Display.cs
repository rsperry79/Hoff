﻿using System.Device.I2c;

using Hoff.Core.Hardware.Common.Interfaces.Displays;
using Hoff.Core.Hardware.Common.Interfaces.Services;
using Hoff.Core.Hardware.Common.Structs;
using Hoff.Hardware.Displays.Ssd13.Fonts;
using Hoff.Hardware.Displays.Ssd13.Interfaces;

using Iot.Device.Ssd13xx;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging;

using static Iot.Device.Ssd13xx.Ssd13xx;

namespace Hoff.Hardware.Displays.Ssd13
{
    public class Display : ISsd13, IDisplay
    {
        #region Fields

        private readonly ILogger _logger;
        private readonly II2cBussControllerService deviceScan;
        private I2cDevice i2CDevice;
        private bool init;
        private Ssd1306 ssdDisplay;

        #endregion Fields

        #region Public Constructors

        public Display(II2cBussControllerService scanner)
        {
            this._logger = this.GetCurrentClassLogger();

            try
            {
                this.deviceScan = scanner;
            }
            catch (System.Exception ex)
            {
                this._logger.LogError(ex.StackTrace);
                throw;
            }
        }

        #endregion Public Constructors

        #region Public Methods

        public void ClearDirectLine(DirectLine line)
        {
            this.ssdDisplay.ClearDirectAligned(line.X, line.Y, line.Width, line.Height);
        }

        public void ClearScreen()
        {
            this.ssdDisplay.ClearScreen();
            this.ssdDisplay.Display();
        }

        public bool DefaultInit()
        {
            int bussId = 0;
            byte deviceAddr = 0;
            I2cBusSpeed speed = I2cBusSpeed.FastMode;

            if (this.deviceScan.I2C1.Contains(Ssd1306.DefaultI2cAddress))
            {
                bussId = 1;
                deviceAddr = Ssd1306.DefaultI2cAddress;
                speed = this.deviceScan.I2C1BusSpeed;
            }

            if (this.deviceScan.I2C1.Contains(Ssd1306.SecondaryI2cAddress))
            {
                bussId = 1;
                deviceAddr = Ssd1306.SecondaryI2cAddress;
                speed = this.deviceScan.I2C1BusSpeed;
            }

            if (this.deviceScan.I2C2.Contains(Ssd1306.DefaultI2cAddress))
            {
                bussId = 2;
                deviceAddr = Ssd1306.DefaultI2cAddress;
                speed = this.deviceScan.I2C2BusSpeed;
            }

            if (this.deviceScan.I2C2.Contains(Ssd1306.SecondaryI2cAddress))
            {
                bussId = 2;
                deviceAddr = Ssd1306.SecondaryI2cAddress;
                speed = this.deviceScan.I2C2BusSpeed;
            }

            this._logger.LogDebug($"SSD1306 Autodetect");
            this._logger.LogDebug($"SSD1306 Buss ID: {bussId}");
            this._logger.LogDebug($"SSD1306 Device Address: {deviceAddr}");

            bool complete = this.Init(bussId, deviceAddr, speed, DisplayResolution.OLED128x64);
            return complete;
        }

        public void DrawDirectLine(DirectLine line)
        {
            this.ssdDisplay.DrawDirectAligned(line.X, line.Y, line.Width, line.Height, line.Data);
        }

        public void HorizontalLine(Line line, bool draw = true)
        {
            this.ssdDisplay.DrawHorizontalLine(line.X, line.Y, line.Length, draw);
        }

        public bool Init(int bussId, byte deviceAddr, I2cBusSpeed busSpeed, DisplayResolution resolution)
        {
            if (!this.init)
            {
                this.i2CDevice = I2cDevice.Create(new I2cConnectionSettings(bussId, deviceAddr, busSpeed));
                this.ssdDisplay = new Ssd1306(this.i2CDevice, resolution);
                this.ClearScreen();
                this.init = true;
            }

            return this.init;
        }

        public void UpdateDisplay()
        {
            this.ssdDisplay.Display();
        }

        public void VerticalLine(Line line, bool draw = true)
        {
            this.ssdDisplay.DrawVerticalLine(line.X, line.Y, line.Length, draw);
        }

        public void WriteLine(int x, int y, string text, byte fontSize = 1, bool center = false, object font = null)
        {
            this.ssdDisplay.Font = font is null ? new BasicFont() : (IFont)font;
            this.ssdDisplay.DrawString(x, y, text, fontSize, center);
        }

        #endregion Public Methods
    }
}
