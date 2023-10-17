using System;

using Hoff.Core.Common.Interfaces;
using Hoff.Core.Hardware.Common.Interfaces.Services;
using Hoff.Hardware.Common.Interfaces.Storage;

namespace Hoff.Core.Services.Settings
{
    public class Settings : IChangeNotifcation, IDisposable
    {
        #region Fields
        private ISettingsStorageDriver storage;
        private bool disposedValue;
        private IWifiSettings WifiSettings;


        #endregion Fields


        #region Public Constructors

        public Settings(ISettingsStorageDriver settingsStorage, IWifiSettings wifiSettings)
        {
            this.storage = settingsStorage;
            this.WifiSettings = wifiSettings;

            this.GetSettings();


        }

        #endregion Public Constructors

        #region Delegates

        public delegate void SettingsChangedEventHandler(object sender, bool dataChanged);

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

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    _ = this.WriteSettings();
                }

                this.disposedValue = true;
            }
        }



        #endregion Protected Methods

        #region Private Methods
        private void SendEvent()
        {
            EventHandler<bool> tempEvent = DataChanged;
            tempEvent(this, true);
        }
        private void GetSettings()
        {
            this.storage.GetSettings();
        }

        private object WriteSettings()
        {
            throw new NotImplementedException();
        }




        #endregion Private Methods
    }
}
