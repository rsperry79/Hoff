using System;
using System.Collections;

using Hoff.Core.Common.Interfaces;
using Hoff.Core.Hardware.Common.Interfaces.Services;
using Hoff.Core.Hardware.Common.Interfaces.Storage;
using Hoff.Core.Hardware.Common.Models;
using Hoff.Hardware.Common.Interfaces.Storage;

using nanoFramework.Logging.Debug;

namespace Hoff.Core.Services.Settings
{
    public class SettingsService : ISettingsService, IDisposable
    {
        private bool disposedValue;
        private ISettingsStorage SettingsStorage;
        private IServiceProvider ServiceProvider;
        private ILoggerCore LoggerCore;
        private DebugLogger Logger;


        private ISettingsStorageDriver defaultDriver;
        public SettingsService(IServiceProvider serviceProvider, ISettingsStorage settingsStorage, ILoggerCore loggerCore)
        {
            this.ServiceProvider = serviceProvider;
            this.SettingsStorage = settingsStorage;
            this.defaultDriver = (ISettingsStorageDriver)this.ServiceProvider.GetService(typeof(INvsSettingsStorageDriver));

            this.LoggerCore = loggerCore;

            this.Logger = loggerCore.GetDebugLogger(this.GetType().ToString());
        }


        public void Add(Type type, ISettingsStorageDriver driver, string storageLocation, object payload)
        {
            SettingsStorageItem temp = new
                (driver, storageLocation, payload, this.LoggerCore);
            this.SettingsStorage.Add(temp);
        }

        public ISettingsStorageItem GetFirstOrDefault(Type type)
        {
            ArrayList temp = this.SettingsStorage.FindByType(type);
            if (temp.Count > 0)
            {
                return (ISettingsStorageItem)temp[0];
            }
            else
            {
                object tempType = this.ServiceProvider.GetService(type);
                SettingsStorageItem ssi = new SettingsStorageItem(this.defaultDriver, (tempType).GetType().Name, tempType, this.LoggerCore);
                int arrLoc = this.SettingsStorage.Add(ssi);
                return ssi;
            }
        }


        public ISettingsStorageItem GetFirst(Type type)
        {
            ArrayList temp = this.SettingsStorage.FindByType(type);
            if (temp.Count > 0)
            {
                return (ISettingsStorageItem)temp[0];
            }
            else
            {
                return null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {

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
