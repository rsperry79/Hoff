using System;

using Hoff.Hardware.Common.Interfaces.Storage;

using nanoFramework.Json;

namespace Hoff.Core.Services.Settings
{
    public class Settings<T> : IDisposable
    {
        #region Fields

        protected static T settings;
        protected IEeprom Eeprom;

        private bool disposedValue;
        private byte startLocation = 0x00;

        #endregion Fields

        #region Public Constructors

        public Settings(IEeprom eeprom)
        {
            this.Eeprom = eeprom;

            if (settings is null)
            {
                this.GetSettings();
            }
        }

        #endregion Public Constructors

        #region Properties

        public byte StartLocation
        {
            get => this.startLocation;
            set
            {
                if (this.startLocation != value)
                {
                    this.startLocation = value;
                    this.GetSettings();
                }
            }
        }

        #endregion Properties

        #region Public Methods

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public bool WriteSettings()
        {
            string result = JsonConvert.SerializeObject(this);
            bool write = this.Eeprom.WriteString(this.startLocation, result);
            return write;
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.WriteSettings();
                }

                this.disposedValue = true;
            }
        }

        protected void GetSettings()
        {
            try
            {
                string data = this.Eeprom.ReadString(this.startLocation);
                T temp = (T)JsonConvert.DeserializeObject(data, typeof(T));
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Protected Methods
    }
}
