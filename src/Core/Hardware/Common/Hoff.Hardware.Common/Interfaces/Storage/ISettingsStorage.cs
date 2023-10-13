using System;

namespace Hoff.Hardware.Common.Interfaces.Storage
{
    public interface ISettingsStorage : IDisposable
    {
        #region Delegates

        internal delegate void EepromChangedEventHandler(object sender, bool dataChanged);

        #endregion Delegates

        #region Events

        // Event Handlers
        event EventHandler<bool> DataChanged;

        #endregion Events

        #region Public Methods

        void HardResetSettings(bool confirm);









        #endregion Public Methods
    }
}