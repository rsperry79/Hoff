
using System;
using System.Device.I2c;

using static Hoff.Hardware.Common.Interfaces.Storage.IEeprom;

namespace Hoff.Hardware.Common.Interfaces.Storage
{
    public interface IEeprom : IDisposable

    {
        bool DefaultInit();
        int GetPageCount();
        int GetPageSize();
        int GetSize();
        bool Init(int bussId, byte deviceAddr, I2cBusSpeed busSpeed);
        byte ReadByte(byte address);
        byte[] ReadByteArray(byte address);
        string ReadString(byte address);
        bool WriteByte(byte address, byte message);
        bool WriteByteArray(byte address, byte[] list);
        bool WriteString(byte address, string message);

        // Event Handlers
        delegate void DataChangedEventHandler();
        event DataChangedEventHandler EepromDataChanged;
    }

}
