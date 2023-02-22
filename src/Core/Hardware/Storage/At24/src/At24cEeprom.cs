using System;
using System.Collections;
using System.Device.I2c;

using Hoff.Hardware.Common.Helpers;
using Hoff.Hardware.Common.Interfaces.Storage;

using Iot.Device.At24cxx;

namespace Hoff.Core.Hardware.Storage.At24
{
    public class At24cEeprom : IEeprom, IDisposable
    {
        private static At24Base eeprom;
        private bool disposedValue;
        private const char EOL = '\0';

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

        public At24cEeprom(I2cDevice device)
        {
            eeprom = new At24c256(device);
        }

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

        public bool WriteArrayList(byte address, ArrayList list)
        {
            byte[] toStore = list.ToByteArray();
            uint writeResult = eeprom.Write(address, toStore);
            writeResult += eeprom.WriteByte(address + toStore.Length, (byte)EOL);

            return writeResult == toStore.Length + 1;
        }

        public bool WriteString(byte address, string message)
        {
            byte[] encodedMessage = message.ToBytes();
            return this.Write(address, encodedMessage);
        }

        public ArrayList ReadArrayList(byte address)
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

                return receivedData;
            }
            else
            {
                return null;
            }
        }

        public string ReadString(byte address)
        {
            ArrayList data = this.ReadArrayList(address);
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
    }
}
