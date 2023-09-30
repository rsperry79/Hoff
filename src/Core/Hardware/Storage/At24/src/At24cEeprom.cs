using System;
using System.Collections;
using System.Device.I2c;
using System.Text;

using Hoff.Core.Hardware.Common.Helpers;
using Hoff.Core.Hardware.Common.Interfaces.Services;
using Hoff.Hardware.Common.Interfaces.Storage;

using Iot.Device.At24cxx;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging;

namespace Hoff.Core.Hardware.Storage.At24
{
    public class At24cEeprom : IEeprom, IDisposable
    {
        #region Fields

        private const char EOL = '\0';

        /// <summary>
        /// The buss detector
        /// </summary>
        private static II2cBussControllerService deviceScan;

        /// <summary>
        /// The underlying I2C device
        /// </summary>
        private static I2cDevice i2CDevice = null;

        private readonly ILogger logger;
        private bool disposedValue;
        private At24Base eeprom;
        private bool init = false;

        #endregion Fields

        #region Public Constructors

        public At24cEeprom(II2cBussControllerService scanner)
        {
            this.logger = this.GetCurrentClassLogger();
            deviceScan = scanner;
        }

        public At24cEeprom(II2cBussControllerService scanner, bool auto = true, int size = 32, int buss = 1, byte address = 0x00)
        {
            this.logger = this.GetCurrentClassLogger();
            deviceScan = scanner;

            if (auto)
            {
                _ = this.DefaultInit(size);
            }
            else
            {
                I2cBusSpeed speed = buss == 1 ? deviceScan.I2C1BusSpeed : deviceScan.I2C2BusSpeed;
                _ = this.Init(buss, address, speed, size);
            }
        }

        #endregion Public Constructors

        #region Delegates

        public delegate void EepromChangedEventHandler(object sender, bool dataChanged);

        #endregion Delegates

        #region Events

        // Event Handlers
        public event EventHandler<bool> DataChanged;

        #endregion Events

        #region Public Methods

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void EraseProm(bool confirm)
        {
            if (confirm)
            {
                for (byte i = 0; i < this.GetSize(); i++)
                {
                    _ = this.WriteByte(i, 0x00);
                }
            }
        }

        public int GetPageCount()
        {
            return this.eeprom != null ? this.eeprom.PageCount : 0;
        }

        public int GetPageSize()
        {
            return this.eeprom != null ? this.eeprom.PageSize : 0;
        }

        public int GetSize()
        {
            return !this.init ? throw new Exception("Not Initialized") : this.eeprom != null ? this.eeprom.Size : 0;
        }

        public byte ReadByte(byte address)
        {
            return this.eeprom.ReadByte(address);
        }

        public byte[] ReadByteArray(byte address)
        {
            ArrayList receivedData = new();
            bool hasEol = false;

            byte receivedCharacter = this.eeprom.ReadByte(address);

            if (receivedCharacter != EOL)
            {
                _ = receivedData.Add(receivedCharacter);

                do
                {
                    address++;
                    receivedCharacter = this.eeprom.ReadByte(address);

                    if (receivedCharacter == EOL)
                    {
                        hasEol = true;
                    }
                    else
                    {
                        _ = receivedData.Add(receivedCharacter);
                    }
                }
                while (!hasEol);

                byte[] toRet = new byte[receivedData.Count];

                for (int i = 0; i < receivedData.Count; i++)
                {
                    toRet[i] = (byte)receivedData[i];
                }

                return toRet;
            }
            else
            {
                return null;
            }
        }

        public string ReadString(byte address)
        {
            byte[] data = this.ReadByteArray(address);

            string decodedMessage = Encoding.UTF8.GetString(data, 0, data.Length);
            return decodedMessage;
        }

        public bool WriteByte(byte address, byte message)
        {
            uint writeResult = this.eeprom.WriteByte(address, message);
            this.SendEvent();
            return writeResult == 1;
        }

        public bool WriteByteArray(byte address, byte[] list)
        {
            uint writeResult = 0;

            try
            {
                for (int i = 0; i < list.Length; i++)
                {
                    try
                    {
                        byte currentByte = list[i];
                        _ = this.eeprom.WriteByte(address, currentByte);
                        writeResult++;
                        address++;
                    }
                    catch (Exception ex)
                    {
                        this.logger.LogError(ex.Message, ex);
                        throw;
                    }
                }

                _ = this.eeprom.WriteByte(address, (byte)EOL);
                writeResult++;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, ex);
                throw;
            }

            this.SendEvent();
            return writeResult == list.Length + 1;
        }

        public bool WriteString(byte address, string message)
        {
            try
            {
                byte[] encodedMessage = message.ToBytes();
                this.SendEvent();

                return this.WriteByteArray(address, encodedMessage);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, ex);
                throw;
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.eeprom.Dispose();
                }

                this.disposedValue = true;
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private bool DefaultInit(int size)
        {
            int bussId;
            byte deviceAddr;
            I2cBusSpeed speed;
            if (deviceScan.I2C1.Contains(At24c256.DefaultI2cAddress))
            {
                bussId = 1;
                deviceAddr = At24c256.DefaultI2cAddress;
                speed = deviceScan.I2C1BusSpeed;
            }
            else if (deviceScan.I2C1.Contains(0x32))
            {
                bussId = 1;
                deviceAddr = 0x32;
                speed = deviceScan.I2C1BusSpeed;
            }
            else if (deviceScan.I2C2.Contains(At24c256.DefaultI2cAddress))
            {
                bussId = 2;
                deviceAddr = At24c256.DefaultI2cAddress;
                speed = deviceScan.I2C2BusSpeed;
            }
            else if (deviceScan.I2C2.Contains(0x32))
            {
                bussId = 2;
                deviceAddr = 0x32;
                speed = deviceScan.I2C1BusSpeed;
            }
            else
            {
                throw new Exception("No Common Address found");
            }

            this.logger.LogDebug($"{size} Auto-detect");
            this.logger.LogDebug($"{size} Buss ID: {bussId}");
            this.logger.LogDebug($"{size} Device Address: {deviceAddr}");

            bool complete = this.Init(bussId, deviceAddr, speed, size);
            return complete;
        }

        private bool Init(int bussId, byte deviceAddr, I2cBusSpeed busSpeed, int size)
        {
            if (!this.init)
            {
                i2CDevice = I2cDevice.Create(new I2cConnectionSettings(bussId, deviceAddr, busSpeed));
                //this.eeprom = new At24c256(i2CDevice);

                this.eeprom = size switch
                {
                    32 => new At24c32(i2CDevice),
                    64 => new At24c64(i2CDevice),
                    128 => new At24c128(i2CDevice),
                    256 => new At24c256(i2CDevice),
                    _ => throw new ArgumentException(nameof(size)),
                };
                this.init = true;
            }

            return this.init;
        }

        private void SendEvent()
        {
            EventHandler<bool> tempEvent = DataChanged;
            tempEvent(this, true);
        }

        #endregion Private Methods
    }
}
