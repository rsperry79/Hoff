using System;
using System.Collections;

using Hoff.Core.Hardware.Common.Interfaces.Services;
using Hoff.Core.Hardware.Common.Interfaces.Storage;
using Hoff.Core.Hardware.Common.Models;
using Hoff.Core.Services.Common.Interfaces;
using Hoff.Hardware.Common.Interfaces.Storage;

using Microsoft.Extensions.Logging;

namespace Hoff.Core.Services.Settings
{
    public class SettingsService : ISettingsService, IDisposable
    {
        private bool disposedValue;
        private readonly ISettingsStorage SettingsStorage;
        private readonly IServiceProvider ServiceProvider;
        private readonly ILogger Logger;

        private readonly ISettingsStorageDriver defaultDriver;
        public SettingsService(IServiceProvider serviceProvider, ISettingsStorage settingsStorage, ILoggerCore loggerCore)
        {
            this.ServiceProvider = serviceProvider;
            this.SettingsStorage = settingsStorage;
            this.defaultDriver = (ISettingsStorageDriver)this.ServiceProvider.GetService(typeof(INvsSettingsStorageDriver));

            this.Logger = loggerCore.GetDebugLogger(this.GetType().ToString());
        }

        public void Add(ISettingsStorageDriver driver, string storageLocation, object payload)
        {
            SettingsStorageItem temp = new
                (driver, storageLocation, payload);
            _ = this.SettingsStorage.Add(temp);
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
                SettingsStorageItem ssi = new(this.defaultDriver, tempType.GetType().Name, tempType);
                _ = this.SettingsStorage.Add(ssi);
                return ssi;
            }
        }

        public ISettingsStorageItem GetFirst(Type type)
        {
            ArrayList temp = this.SettingsStorage.FindByType(type);
            return temp.Count > 0 ? (ISettingsStorageItem)temp[0] : null;
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
