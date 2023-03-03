using System;

using nanoFramework.Json;

namespace Hoff.Core.Services.Settings
{
    public class Settings<T> : IDisposable
    {
        protected IEeprom Eeprom;
        private byte startLocation = 0x00;

        protected static T settings;
        private bool disposedValue;

        public Settings(IEeprom eeprom)
        {
            this.Eeprom = eeprom;

            if (settings is null)
            {
                this.GetSettings();
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

        public bool WriteSettings()
        {
            string result = JsonConvert.SerializeObject(this);
            bool write = this.Eeprom.WriteString(this.startLocation, result);
            return write;
        }

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

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}