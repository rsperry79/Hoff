﻿using System;
using System.Collections;
using System.Device.I2c;
using System.Text;

using Hoff.Hardware.Common.Helpers;
using Hoff.Hardware.Common.Interfaces.Services;
using Hoff.Hardware.Common.Interfaces.Storage;

using Iot.Device.At24cxx;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging;

using static Hoff.Hardware.Common.Interfaces.Storage.IEeprom;

namespace Hoff.Core.Hardware.Storage.At24
{
    public class At24c256Eeprom : IEeprom, IDisposable
    {

        //IMPLEMENT EVENT
        //IMPLEMENT Erase



        #region Implementation
        private bool init = false;
        private readonly ILogger logger;

        /// <summary>
        /// The underlying I2C device
        /// </summary>
        private static I2cDevice i2CDevice = null;

        /// <summary>
        /// The buss detector
        /// </summary>
        private static II2cBussControllerService deviceScan;


        private At24c256 eeprom;

        private bool disposedValue;
        private const char EOL = '\0';

        public event DataChangedEventHandler EepromDataChanged;
        #endregion

        public At24c256Eeprom(II2cBussControllerService scanner)
        {
            this.logger = this.GetCurrentClassLogger();
            deviceScan = scanner;
        }

        public bool DefaultInit()
        {
            int bussId = 0;
            byte deviceAddr = 0;
            I2cBusSpeed speed = I2cBusSpeed.FastMode;

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

            this.logger.LogDebug($"At24c256 Autodetect");
            this.logger.LogDebug($"At24c256 Buss ID: {bussId}");
            this.logger.LogDebug($"At24c256 Device Address: {deviceAddr}");

            bool complete = this.Init(bussId, deviceAddr, speed);
            return complete;
        }

        public bool Init(int bussId, byte deviceAddr, I2cBusSpeed busSpeed)
        {
            if (this.init == false)
            {
                i2CDevice = I2cDevice.Create(new I2cConnectionSettings(bussId, deviceAddr, busSpeed));
                this.eeprom = new At24c256(i2CDevice);
                this.init = true;
            }

            return this.init;
        }

        public int GetSize()
        {
            if (!this.init)
            {
                throw new Exception("Not Initialized");
            }
            return this.eeprom != null ? this.eeprom.Size : 0;
        }

        public int GetPageSize()
        {
            return this.eeprom != null ? this.eeprom.PageSize : 0;
        }

        public int GetPageCount()
        {
            return this.eeprom != null ? this.eeprom.PageCount : 0;
        }

        public bool WriteByte(byte address, byte message)
        {
            uint writeResult = this.eeprom.WriteByte(address, message);
            return writeResult == 1;
        }

        public byte ReadByte(byte address)
        {
            return this.eeprom.ReadByte(address);
        }

        public byte[] ReadByteArray(byte address)
        {
            ArrayList receivedData = new ArrayList();
            bool hasEol = false;

            byte receivedCharacter = this.eeprom.ReadByte(address);

            if (receivedCharacter != EOL)
            {
                receivedData.Add(receivedCharacter);

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
            return writeResult == list.Length + 1;
        }

        public bool WriteString(byte address, string message)
        {
            try
            {
                byte[] encodedMessage = message.ToBytes();
                return this.WriteByteArray(address, encodedMessage);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public string ReadString(byte address)
        {
            byte[] data = this.ReadByteArray(address);

            string decodedMessage = Encoding.UTF8.GetString(data, 0, data.Length);
            return decodedMessage;
        }

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

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
