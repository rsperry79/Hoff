

using System;
using System.Device.I2c;

using Hoff.Core.Hardware.Rtc.RtcDevice.Interfaces;
using Hoff.Hardware.Common.Interfaces.Rtc;
using Hoff.Hardware.Common.Interfaces.Services;

using Iot.Device.Rtc;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging;

using UnitsNet;

namespace Hoff.Core.Hardware.Rtc.RtcDevice
{
    public class DS3231Rtc : IDS3231Rtc, IRtcExtended, IRtc, IDisposable
    {
        #region Implementation
        private static II2cBussControllerService deviceScan;
        protected ILogger _logger;
        private Ds3231 rtc;
        private bool init = false;
        protected bool _disposed = false; // To detect redundant calls
        #endregion

        #region Constructor
        public DS3231Rtc(II2cBussControllerService scanner)
        {
            this._logger = this.GetCurrentClassLogger();
            deviceScan = scanner;
        }

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
        #endregion

        #region Core Methods
        public DateTime ReadDateTime()
        {
            return this.rtc.DateTime;
        }

        public void SetDateTime(DateTime time)
        {
            this.rtc.DateTime = time;
        }
        #endregion

        #region RtcExtended
        public Temperature GetRtcTemperature()
        {
            return this.rtc.Temperature;
        }
        #endregion

        #region IDisposable Support
        protected void DisposeSensor()
        {
            this.rtc.Dispose();
        }

        public void Dispose()
        {
            if (!this._disposed)
            {
                this.DisposeSensor();
                this._disposed = true;
            }
        }


        #endregion
    }
}