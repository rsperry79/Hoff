﻿using Hoff.Core.Hardware.Common.Interfaces.Config;
using Hoff.Hardware.SoC.SoCEsp32.Interfaces;

using Microsoft.Extensions.Logging;

using nanoFramework.Hardware.Esp32;
using nanoFramework.Logging;

namespace Hoff.Hardware.SoC.SoCEsp32

{
    public class EspConfig : IEspConfig
    {
        #region Fields

        private readonly IPinConfig Config;

        private readonly ILogger logger;

        #endregion Fields

        #region Public Constructors

        /// <summary>
        /// Instantiates the class and sets the config for the other functions.
        /// The intent is to allow config loaded from Dependency Injection.
        /// </summary>
        /// <param name="config">The pin config file. <see cref="IPinConfig"/></param>
        public EspConfig(IPinConfig config)
        {
            this.Config = config;
            this.logger = this.GetCurrentClassLogger();
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Gets the pin number for the specified function.
        /// </summary>
        /// <param name="deviceFunction">The function wanted.</param>
        /// <returns>an int for the pin set.<see cref="int"/></returns>
        public int GetPinFunction(DeviceFunction deviceFunction)
        {
            int funct = Configuration.GetFunctionPin(deviceFunction);
            return funct;
        }

        /// <summary>
        /// Sets the I2C1 pins from the IPinConfig loaded in the ctor.
        /// </summary>
        public void SetI2C1Pins()
        {
            // I2C 1
            Configuration.SetPinFunction(this.Config.I2C1_DATA, DeviceFunction.I2C1_DATA);
            Configuration.SetPinFunction(this.Config.I2C1_CLOCK, DeviceFunction.I2C1_CLOCK);
        }

        /// <summary>
        /// Sets the I2C2 pins from the IPinConfig loaded in the ctor.
        /// </summary>
        public void SetI2C2Pins()
        {
            // I2C 2
            Configuration.SetPinFunction(this.Config.I2C2_DATA, DeviceFunction.I2C2_DATA);
            Configuration.SetPinFunction(this.Config.I2C2_CLOCK, DeviceFunction.I2C2_CLOCK);
        }

        /// <summary>
        /// Sets the SPI Pins from the IPinConfig loaded in the ctor
        /// </summary>
        public void SetSpi1Pins()
        {
            Configuration.SetPinFunction(this.Config.Spi1_Mosi, DeviceFunction.SPI1_MOSI);
            Configuration.SetPinFunction(this.Config.Spi1_Miso, DeviceFunction.SPI1_MISO);
            Configuration.SetPinFunction(this.Config.Spi1_Clock, DeviceFunction.SPI1_CLOCK);
        }

        #endregion Public Methods
    }
}
