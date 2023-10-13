// Ignore Spelling: Nvs

using System;

using Hoff.Core.Common.Interfaces;
using Hoff.Core.Hardware.Common.Interfaces.Services;
using Hoff.Hardware.Common.Interfaces.Storage;

using System.IO;
using System.Text;
using nanoFramework.Json;

using nanoFramework.Logging.Debug;
using Microsoft.Extensions.Logging;

namespace Hoff.Core.Hardware.Storage.Nvs
{
    public class NvsStorage<T> : ISettingsStorage, IDisposable
    {
        #region Fields
        private bool disposedValue;

        private string ConfigFile { get; set; }

        private DebugLogger Logger;
        private bool HasStored => File.Exists(this.ConfigFile);

        private T settings;

        public T Settings {
            get
            {
               return this.settings;
            }
            set
            {
                this.settings = value;
                this.WriteConfig();
                this.SendEvent();
            }
        }

        #endregion Fields

        #region Public Constructors

        public NvsStorage(ILoggerCore loggerCore, T defaults, string configFile = "I:\\configuration.json")
        {
            this.ConfigFile = configFile;
            this.Logger = loggerCore.GetDebugLogger(this.GetType().ToString());

            if (!this.HasStored)
            {
                this.Settings = defaults;
            }
            else
            {
                this.Settings = this.GetSettings();
            }
        }

        #endregion Public Constructors

        #region Delegates

        public delegate void SettingsStorageChangedEventHandler(object sender, bool dataChanged);

        #endregion Delegates

        #region Events

        // Event Handlers
        public event EventHandler<bool> DataChanged;

        #endregion Events

        #region Public Methods

        public void HardResetSettings(bool confirm)
        {
            if (File.Exists(this.ConfigFile))
            {
                File.Delete(this.ConfigFile);
            }
        }


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

        private bool WriteConfig()
        {
            try
            {
                string configJson = JsonConvert.SerializeObject(this.settings);
                FileStream json = new FileStream(this.ConfigFile, FileMode.Create);

                byte[] buffer = Encoding.UTF8.GetBytes(configJson);
                json.Write(buffer, 0, buffer.Length);
                json.Dispose();

                return true;
            }
            catch(Exception ex)
            {
                this.Logger.LogCritical(ex.Message, ex);
                return false;
            }
        }

        private T GetSettings()
        {
            FileStream json = new FileStream(this.ConfigFile, FileMode.Open);
            T config = (T)JsonConvert.DeserializeObject(json, typeof(T));
            return config;
        }

        #endregion Private Methods
    }
}
