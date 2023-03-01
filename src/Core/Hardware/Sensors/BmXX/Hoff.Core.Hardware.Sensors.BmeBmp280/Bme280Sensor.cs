using System;
using System.Device.I2c;
using System.Threading;

using Hoff.Hardware.Common.Abstract;
using Hoff.Hardware.Common.Helpers;
using Hoff.Hardware.Common.Interfaces.Base;
using Hoff.Hardware.Common.Interfaces.Sensors;
using Hoff.Hardware.Common.Interfaces.Services;

using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.ReadResult;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging;

namespace Hoff.Core.Hardware.Sensors.BmXX
{
  
    public class Bme280Sensor : SensorBase, IBme280Sensor, IHumidityTempatureSensor, ITempatureSensor, IHumiditySensor, ISensorBase, IDisposable
    {
        #region Implementation
        /// <summary>
        /// The underlying I2C device
        /// </summary>
        private I2cDevice i2CDevice = null;
        /// <summary>
        /// How many decimal places to account in temperature and humidity measurements
        /// </summary>
        private uint _scale = 2;


        private bool init;
        private readonly ILogger _logger;
        private static II2cBussControllerService deviceScan;
        private Bme280 sensor;
        #endregion


        #region Properties
        /// <summary>
        /// Accessor/Mutator for relative humidity %
        /// </summary>

        private double relativeHumidity;

        public double Humidity
        {
            get => this.relativeHumidity;
            set
            {
                if (this.relativeHumidity != value)
                {
                    this.relativeHumidity = value;
                    _ = HumiditySensorChanged();
                }
            }
        }

        private double temperature;

        /// <summary>
        /// Accessor/Mutator for temperature in celcius
        /// </summary>

        public double Temperature
        {
            get => this.temperature;
            set
            {
                if (this.temperature != value)
                {
                    this.temperature = value;
                    TemperatureSensorChanged();
                }

            }
        }

        #endregion

        #region Constants
        /// <summary>
        /// Command to soft reset the HTU21D sensor
        /// </summary>
        private readonly byte[] SOFT_RESET = { 0xFE };
        /// <summary>
        /// Command to trigger a humidity measurement and hold the value
        /// </summary>
        private readonly byte[] TRIGGER_HUMD_MEASURE_HOLD = { 0xE5 };
        /// <summary>
        /// Command to trigger a temperature measurement and hold the value
        /// </summary>
        private readonly byte[] TRIGGER_TEMP_MEASURE_HOLD = { 0xE3 };
        /// <summary>
        /// Command to read user register
        /// </summary>
        private readonly byte[] READ_USER_REGISTER = { 0xE7 };
        /// <summary>
        /// Command to write user register
        /// </summary>
        private readonly byte[] WRITE_USER_REGISTER = { 0xE6 };
        /// <summary>
        /// For CRC check
        /// </summary>
        private const int SHIFTED_DIVISOR = 0x988000;
        /// <summary>
        /// Error value of humidity
        /// </summary>
        private const float ERROR_HUMIDITY = -999.99F;
        /// <summary>
        /// Error value of temperature
        /// </summary>
        private const float ERROR_TEMPERATURE = -999.99F;

        public event ITempatureSensor.TempatureChangedEventHandler TemperatureSensorChanged;
        public event IHumiditySensor.HumidityChangedEventHandler HumiditySensorChanged;


        #endregion

        #region Constructor
     
        public Bme280Sensor(II2cBussControllerService scanner)
        {
            this._logger = this.GetCurrentClassLogger();
            deviceScan = scanner;
        }

        public bool DefaultInit()
        {
            int bussId = 0;
            byte deviceAddr = 0;
            I2cBusSpeed speed = I2cBusSpeed.FastMode;

            if (deviceScan.I2C1.Contains(Bme280.DefaultI2cAddress))
            {
                bussId = 1;
                deviceAddr = Bme280.DefaultI2cAddress;
                speed = deviceScan.I2C1BusSpeed;
            }

            if (deviceScan.I2C1.Contains(Bme280.SecondaryI2cAddress))
            {
                bussId = 1;
                deviceAddr = Bme280.SecondaryI2cAddress;
                speed = deviceScan.I2C1BusSpeed;
            }

            if (deviceScan.I2C2.Contains(Bme280.DefaultI2cAddress))
            {
                bussId = 2;
                deviceAddr = Bme280.DefaultI2cAddress;
                speed = deviceScan.I2C2BusSpeed;
            }

            if (deviceScan.I2C2.Contains(Bme280.SecondaryI2cAddress))
            {
                bussId = 2;
                deviceAddr = Bme280.SecondaryI2cAddress;
                speed = deviceScan.I2C2BusSpeed;
            }

            this._logger.LogDebug($"Bme280 Autodetect");
            this._logger.LogDebug($"Bme280 Buss ID: {bussId}");
            this._logger.LogDebug($"Bme280 Device Address: {deviceAddr}");

            bool complete = this.Init(bussId, deviceAddr, speed, 2);
            return complete;
        }

        public bool Init(int bussId, byte deviceAddr, I2cBusSpeed busSpeed, uint scale)
        {
            if (!this.init)
            {
                this.i2CDevice = I2cDevice.Create(new I2cConnectionSettings(bussId, deviceAddr, busSpeed));
                this.sensor = new Bme280(this.i2CDevice);

                if (busSpeed == I2cBusSpeed.FastMode)
                {
                    this.sensor.TemperatureSampling = Sampling.LowPower;
                    this.sensor.PressureSampling = Sampling.UltraHighResolution;
                }
              

                this._scale = scale ;

                this.init = true;

            }

            this.HasSensorValueChanged();
            return this.init;

        }

        #endregion

        #region Core Methods


        /// <summary>
        /// Reset the sensor...this performs a soft reset. To perform a hard reset, the system must be
        /// power cycled
        /// </summary>
        public void Reset()
        {
            this.sensor.Reset();
        }
        #endregion

        #region Change tracking
        /// <summary>
        /// This sensor suports change tracking
        /// </summary>
        /// <returns>bool</returns>
        public override bool CanTrackChanges()
        {
            return true;
        }

        /// <summary>
        /// Let the world know whether the sensor value has changed or not
        /// </summary>
        /// <returns>bool</returns>
        public override void HasSensorValueChanged()
        {
            try
            {
                if (this.ReadTemperature() != 0)
                {
                    this.Temperature = this.ReadTemperature();
                    this.Humidity = this.ReadHumidity();
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.StackTrace);
                throw;
            }
           
        }
        #endregion

        #region IDisposable Support
        protected override void DisposeSensor()
        {
            this.i2CDevice.Dispose();
            this.i2CDevice = null;
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


        #region Helpers
        /// <summary>
        /// Read humidity value from the sensor
        /// </summary>
        /// <returns>humidity as a floating point number</returns>
        protected float ReadHumidity()
        {

            Bme280ReadResult readResult = this.sensor.Read();
            var rh = ((float)readResult.Humidity.Percent).Truncate(this._scale); ;
            return rh;

        }

        /// <summary>
        /// Read the temperature from the sensor
        /// </summary>
        /// <returns>temperature  as a floating point number in celcius</returns>
        protected float ReadTemperature()
        {

            // Perform a synchronous measurement
            Bme280ReadResult readResult = this.sensor.Read();
            var t = ((float)readResult.Temperature.DegreesCelsius).Truncate(this._scale); ;

            return t;

            //this.sensor.TryReadAltitude(out var altValue);

        }

        /// <summary>
        /// Read the user register
        /// </summary>
        /// <returns>byte</returns>
        protected byte ReadUserRegister()
        {
            byte[] result = new byte[1];

            _ = this.i2CDevice.Write(this.READ_USER_REGISTER);
            Thread.Sleep(50);

            _ = this.i2CDevice.Read(result);
            return result[0];
        }

        /// <summary>
        /// Check if CRC returned by the sensor matches our calculation.
        /// This calculation is based on the algorithm as given here
        /// https://github.com/TEConnectivity/HTU21D_Generic_C_Driver/blob/master/htu21d.c
        /// </summary>
        /// <param name="sensorValue">The sensor reading</param>
        /// <param name="crc">The CRC returned by the sensor</param>
        /// <returns>true if our CRC calculation matches the CRC returned by the sensor</returns>
        private static bool IsCRCValid(ushort sensorValue, byte crc)
        {
            uint polynomial = 0x988000;
            uint msb = 0x800000;
            uint mask = 0xFF8000;
            uint result = (uint)sensorValue << 8;//pad with zeros as specified in spec

            while (msb != 0x80)
            {
                //Check if msb of current value is 1 and apply XOR mask
                if ((result & msb) == msb)
                {
                    result = ((result ^ polynomial) & mask) | (result & ~mask);
                }
                //shift by one
                msb >>= 1;
                mask >>= 1;
                polynomial >>= 1;
            }
            return result == crc;
        }

        #endregion
    }
}
