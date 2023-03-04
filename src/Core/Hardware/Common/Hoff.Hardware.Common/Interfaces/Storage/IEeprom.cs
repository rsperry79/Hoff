
using System;
using System.Device.I2c;


namespace Hoff.Hardware.Common.Interfaces.Storage
{
    public interface IEeprom : IDisposable
    {
        bool DefaultInit(int size);
        int GetPageCount();
        int GetPageSize();
        int GetSize();
        bool Init(int bussId, byte deviceAddr, I2cBusSpeed busSpeed, int size);
        byte ReadByte(byte address);
        byte[] ReadByteArray(byte address);
        string ReadString(byte address);
        bool WriteByte(byte address, byte message);
        bool WriteByteArray(byte address, byte[] list);
        bool WriteString(byte address, string message);

        void EraseProm(bool confirm);

        // Event Handlers
        event EventHandler<bool> DataChanged;
        internal delegate void EepromChangedEventHandler(object sender, bool dataChanged);
    }
}
