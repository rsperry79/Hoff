using System;
using System.Collections;
using System.Device.I2c;

using Hoff.Hardware.Common.Helpers;
using Hoff.Hardware.Common.Interfaces.Storage;

namespace Hoff.Core.Hardware.Storage.EepromMocks
{
    public class EepromMock : IEeprom, IDisposable
    {
        private static At24cMock eeprom;
        private bool disposedValue;
        private const char EOL = '\0';

        public event IEeprom.DataChangedEventHandler EepromDataChanged;

        public byte ReadByte(byte address)
        {
           return eeprom.ReadByte(address);
        }

        public bool DefaultInit()
        {
            return this.Init(1, 0x00, I2cBusSpeed.FastMode);
        }

        public bool Init(int bussId, byte deviceAddr, I2cBusSpeed busSpeed)
        {
            return true;
        }

        public int GetSize()
        {
            return eeprom != null ? eeprom.Size : 0;
        }

        public int GetPageSize()
        {
            return eeprom != null ? eeprom.PageSize : 0;
        }

        public int GetPageCount()
        {
            return eeprom != null ? eeprom.PageCount : 0;
        }

        public EepromMock(At24cMock device) => eeprom = device;

        public bool WriteByte(byte address, byte message)
        {
            uint writeResult = eeprom.WriteByte(address, message);
            writeResult += eeprom.WriteByte(address + 1, (byte)EOL);

            return writeResult == 2;
        }

        public bool Write(byte address, byte[] message)
        {
            uint writeResult = eeprom.Write(address, message);
            writeResult += eeprom.WriteByte(address + message.Length, (byte)EOL);

            return writeResult == message.Length;
        }



        public bool WriteString(byte address, string message)
        {
            byte[] encodedMessage = message.ToBytes();
            return this.Write(address, encodedMessage);
        }

        public string ReadString(byte address)
        {
            Array data = this.ReadByteArray(address);
            return data.ToString();
        }

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


        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public byte[] ReadByteArray(byte address)
        {
            ArrayList receivedData = new();
            bool hasEol = false;

            byte received = eeprom.ReadByte(address);

            if (received != EOL)
            {
                receivedData.Add(received);
                do
                {
                    byte[] receivedCharacter = new byte[1];
                    receivedCharacter[0] = eeprom.ReadByte();

                    if (receivedCharacter[0] != EOL)
                    {
                        receivedData.Add(received);
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

        public bool WriteByteArray(byte address, byte[] list)
        {
            byte[] toStore = list;
            uint writeResult = eeprom.Write(address, toStore);
            writeResult += eeprom.WriteByte(address + toStore.Length, (byte)EOL);

            return writeResult == toStore.Length + 1;
        }

   
    }
}
