using System;

namespace Hoff.Hardware.Common.Interfaces.Storage
{
    public interface IEeprom : IDisposable
    {
        #region Delegates

        internal delegate void EepromChangedEventHandler(object sender, bool dataChanged);

        #endregion Delegates

        #region Events

        // Event Handlers
        event EventHandler<bool> DataChanged;

        #endregion Events

        #region Public Methods

        void EraseProm(bool confirm);

        int GetPageCount();

        int GetPageSize();

        int GetSize();

        byte ReadByte(byte address);

        byte[] ReadByteArray(byte address);

        string ReadString(byte address);

        bool WriteByte(byte address, byte message);

        bool WriteByteArray(byte address, byte[] list);

        bool WriteString(byte address, string message);

        #endregion Public Methods
    }
}