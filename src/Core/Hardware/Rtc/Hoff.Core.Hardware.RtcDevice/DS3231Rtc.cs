using System;
using System.Device.I2c;

using Hoff.Core.Hardware.Common.Interfaces.Rtc;
using Hoff.Core.Hardware.Common.Interfaces.Services;
using Hoff.Core.Hardware.Rtc.RtcDevice.Interfaces;

using Iot.Device.Rtc;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging;

using UnitsNet;

namespace Hoff.Core.Hardware.Rtc.RtcDevice
{
    public class DS3231Rtc : IDS3231Rtc, IRtcExtended, IRtc, IDisposable
    {
        #region Fields

        protected bool _disposed = false;
        protected ILogger _logger;
        private static II2cBussControllerService deviceScan;
        private bool init = false;
        private Ds3231 rtc;

        #endregion Fields

        // To detect redundant calls

        #region Public Constructors

        public DS3231Rtc(II2cBussControllerService scanner)
        {
            this._logger = this.GetCurrentClassLogger();
            deviceScan = scanner;
        }

        #endregion Public Constructors

        #region Public Methods

        public bool DefaultInit()
        {
            int bussId = 1;
            byte deviceAddr = 0x68;
            I2cBusSpeed speed = I2cBusSpeed.FastMode;

            try
            {
                if (deviceScan.I2C1.Contains(Ds3231.DefaultI2cAddress))
                {
                    bussId = 1;
                    deviceAddr = Ds3231.DefaultI2cAddress;
                    speed = deviceScan.I2C1BusSpeed;
                }

                if (deviceScan.I2C2.Contains(Ds3231.DefaultI2cAddress))
                {
                    bussId = 2;
                    deviceAddr = Ds3231.DefaultI2cAddress;
                    speed = deviceScan.I2C2BusSpeed;
                }

                this._logger.LogDebug("Ds3231 Auto-detect");
                this._logger.LogDebug($"Ds3231 Buss ID: {bussId}");
                this._logger.LogDebug($"Ds3231 Device Address: {deviceAddr:X}");
                this._logger.LogDebug($"Ds3231 Device Speed: {speed}");

                return this.Init(bussId, deviceAddr, speed);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.StackTrace);
                throw;
            }
        }

        public void Dispose()
        {
            if (!this._disposed)
            {
                this.DisposeSensor();
                this._disposed = true;
            }
        }

        public Temperature GetRtcTemperature()
        {
            return this.rtc.Temperature;
        }

        public bool Init(int bussId, byte deviceAddr, I2cBusSpeed busSpeed)
        {
            try
            {
                if (this.rtc == null)
                {
                    this.rtc = new Ds3231(I2cDevice.Create(new I2cConnectionSettings(bussId, deviceAddr, busSpeed)));

                    this.init = true;
                    this._logger.LogDebug("Ds3231 Sensor Init Complete");
                }

                return this.init;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                this._logger.LogError(ex.StackTrace);
                throw;
            }
        }

        public DateTime ReadDateTime()
        {
            return this.rtc.DateTime;
        }

        public void SetDateTime(DateTime time)
        {
            this.rtc.DateTime = time;
        }

        #endregion Public Methods

        #region Protected Methods

        protected void DisposeSensor()
        {
            this.rtc.Dispose();
        }

        #endregion Protected Methods
    }
}
