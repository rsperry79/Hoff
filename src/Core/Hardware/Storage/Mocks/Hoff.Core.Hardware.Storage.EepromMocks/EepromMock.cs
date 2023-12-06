using System;
using System.Collections;
using System.Device.I2c;

using Hoff.Core.Hardware.Common.Helpers;
using Hoff.Hardware.Common.Interfaces.Storage;

namespace Hoff.Core.Hardware.Storage.EepromMocks
{
    public class EepromMock : IEeprom, IDisposable
    {
        #region Fields

        private const char EOL = '\0';
        private static At24cMock eeprom;
        private bool disposedValue;

        #endregion Fields

        #region Public Constructors

        public EepromMock(At24cMock device) => eeprom = device;

        #endregion Public Constructors

        #region Events

        public event EventHandler<bool> DataChanged;

        #endregion Events

        #region Public Methods

        public bool DefaultInit(int size)
        {
            size = 256;
            return this.Init(1, 0x00, I2cBusSpeed.FastMode, size);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void EraseProm(bool confirm)
        {
            throw new NotImplementedException();
        }

        public int GetPageCount()
        {
            return eeprom != null ? eeprom.PageCount : 0;
        }

        public int GetPageSize()
        {
            return eeprom != null ? eeprom.PageSize : 0;
        }

        public int GetSize()
        {
            return eeprom != null ? eeprom.Size : 0;
        }

        public bool Init(int bussId, byte deviceAddr, I2cBusSpeed busSpeed, int size)
        {
            return true;
        }

        public byte ReadByte(byte address)
        {
            return eeprom.ReadByte(address);
        }

        public byte[] ReadByteArray(byte address)
        {
            ArrayList receivedData = new();
            bool hasEol = false;

            byte received = eeprom.ReadByte(address);

            if (received != EOL)
            {
                _ = receivedData.Add(received);
                do
                {
                    byte[] receivedCharacter = new byte[1];
                    receivedCharacter[0] = eeprom.ReadByte();

                    if (receivedCharacter[0] != EOL)
                    {
                        _ = receivedData.Add(received);
                    }
                    else
                    {
                        hasEol = true;
                    }
                }
                while (hasEol);

                byte[] toRet = new byte[receivedData.Count];
                receivedData.CopyTo(toRet, 0);
                return toRet;
            }
            else
            {
                return null;
            }
        }

        public string ReadString(byte address)
        {
            Array data = this.ReadByteArray(address);
            return data.ToString();
        }

        public bool Write(byte address, byte[] message)
        {
            uint writeResult = eeprom.Write(address, message);
            writeResult += eeprom.WriteByte(address + message.Length, (byte)EOL);

            return writeResult == message.Length;
        }

        public bool WriteByte(byte address, byte message)
        {
            uint writeResult = eeprom.WriteByte(address, message);
            writeResult += eeprom.WriteByte(address + 1, (byte)EOL);

            return writeResult == 2;
        }

        public bool WriteByteArray(byte address, byte[] list)
        {
            byte[] toStore = list;
            uint writeResult = eeprom.Write(address, toStore);
            writeResult += eeprom.WriteByte(address + toStore.Length, (byte)EOL);

            return writeResult == toStore.Length + 1;
        }

        public bool WriteString(byte address, string message)
        {
            byte[] encodedMessage = message.ToBytes();
            return this.Write(address, encodedMessage);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    eeprom.Dispose();
                }

                this.disposedValue = true;
            }
        }

        #endregion Protected Methods
    }
}
