using System;
using System.Collections;
using System.Device.I2c;

using Hoff.Core.Hardware.Common.Interfaces.Services;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging;

namespace Hoff.Core.Hardware.Common.Services
{
    public class I2cBussControllerService : II2cBussControllerService
    {
        #region Fields

        private static readonly ArrayList i2C1 = new();
        private static readonly ArrayList i2C2 = new();
        private static I2cBusSpeed i2C1BusSpeed = I2cBusSpeed.FastMode;
        private static I2cBusSpeed i2C2BusSpeed = I2cBusSpeed.FastMode;
        private static bool init = false;
        private static ILogger logger;

        #endregion Fields

        #region Public Constructors

        public I2cBussControllerService()
        {
            logger = this.GetCurrentClassLogger();
            if (!init)
            {
                _ = this.ScanForI2cDevices();
                init = true;
                logger.LogDebug($"Exit ScanForI2cDevices");
            }
        }

        #endregion Public Constructors

        #region Properties

        public ArrayList I2C1 => i2C1;
        public I2cBusSpeed I2C1BusSpeed => i2C1BusSpeed;
        public ArrayList I2C2 => i2C2;
        public I2cBusSpeed I2C2BusSpeed => i2C2BusSpeed;

        #endregion Properties

        #region Public Methods

        public void SetIC2BussSpeed(int bussID, I2cBusSpeed speed)
        {
            if (bussID == 1)
            {
                if (i2C1BusSpeed != speed)
                {
                    i2C1BusSpeed = speed;
                    _ = this.ScanForI2cDevices();
                }
            }
            else if (bussID == 2)
            {
                if (i2C2BusSpeed != speed)
                {
                    i2C2BusSpeed = speed;
                    _ = this.ScanForI2cDevices();
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(bussID));
            }
        }

        #endregion Public Methods

        #region Private Methods

        private ushort ScanForI2cDevices()
        {
            try
            {
                ushort totalDevices = 0;
                for (int b = 1; b < 3; b++)
                {
                    ushort numberOfDevices = 0;

                    I2cBusSpeed speed = I2cBusSpeed.FastMode;

                    speed = b == 1 ? i2C1BusSpeed : i2C2BusSpeed;

                    for (byte i = 0; i < 127; i++)
                    {
                        using (I2cDevice i2cDevice = new(new I2cConnectionSettings(b, i, speed)))
                        {
                            try
                            {
                                I2cTransferResult write = i2cDevice.WriteByte(0);
                                logger.LogTrace($"write: {write.Status}");

                                if (write.Status == I2cTransferStatus.FullTransfer)
                                {
                                    _ = b == 1 ? i2C1.Add(i) : i2C2.Add(i);

                                    logger.LogDebug($"Found I2C device at bus {b}: {i:X}");
                                    numberOfDevices++;
                                }
                                else
                                {
                                    logger.LogTrace($"Did not find an I2C device at bus {b}: {i:X}. Returned code {write.Status}");
                                }
                            }
                            catch (System.Exception ex)
                            {
                                logger.LogError($"Error on I2C device at bus {b}: {i:X}", ex);
                            }
                        }
                    }

                    logger.LogDebug($"Found {numberOfDevices} on IC2 Buss {b}");

                    totalDevices += numberOfDevices;
                }

                return totalDevices;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.StackTrace);

                throw;
            }
        }

        #endregion Private Methods
    }
}
