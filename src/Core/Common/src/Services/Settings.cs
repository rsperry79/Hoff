using System;

using Hoff.Hardware.Common.Interfaces.Storage;

using nanoFramework.Json;

namespace Hoff.Core.Common.Services
{
    public class Settings<T> :IDisposable
    {
        protected static IEeprom Eeprom;
        protected static byte StartLocation;

        protected static T  settings;
        private bool disposedValue;

        public Settings(IEeprom eeprom, byte startLocation = 0x00)
        {
            Eeprom = eeprom;
            StartLocation = startLocation;

            if (settings is null)
            {
                this.GetSettings();
            }
        }

        protected  void GetSettings()
        {
            try
            {
                string data = Eeprom.ReadString(StartLocation);
                T temp = (T)JsonConvert.DeserializeObject(data, typeof(T));
            }
            catch (Exception)
            {

                throw;
            }



        }

        public bool WriteSettings()
        {
            string result = JsonConvert.SerializeObject(this);
            bool write = Eeprom.WriteString(StartLocation, result);
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