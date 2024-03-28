using System;

using Hoff.Core.Services.Common.Interfaces.Services;
using Hoff.Services.Common.Interfaces.Storage;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging;

namespace Hoff.Core.Services.Settings
{
    public class SettingsService : ISettingsService, IDisposable
    {
        #region Fields
        private static bool disposed;

        private static SettingsStorage SettingsStorage;
        private static IServiceProvider ServiceProvider;
        private static ILogger Logger;
        #endregion Fields

        #region Public Methods
        public SettingsService(IServiceProvider serviceProvider, ILoggerCore loggerCore)
        {
            Logger = loggerCore.GetCurrentClassLogger();
            ServiceProvider = serviceProvider;

            ISettingsStorageDriver settingsStorageDriver = (ISettingsStorageDriver)serviceProvider.GetService(typeof(ISettingsStorageDriver));
            SettingsStorage = new SettingsStorage(settingsStorageDriver, ServiceProvider, loggerCore);
        }

        public int SettingsCount()
        {
            return SettingsStorage.Count;
        }
        #endregion Public Methods

        #region Add
        public object Add(object payload)
        {
            Type type = payload.GetType();
            bool added = SettingsStorage.Add(payload);
            return added ? payload : throw new Exception($"Failed to add {type.Name}");
        }

        public object Add(Type type)
        {
            object temp = ServiceProvider.GetService(type);

            temp ??= Activator.CreateInstance(type);
            _ = SettingsStorage.Add(temp);
            return temp;
        }

        public object AddOrUpdate(object payload)
        {
            object temp = SettingsStorage.AddOrUpdate(payload);
            return temp;
        }
        public object AddOrUpdate(Type type)
        {
            object temp = ServiceProvider.GetService(type);
            return SettingsStorage.AddOrUpdate(temp);
        }
        #endregion Add

        #region Get
        public object Get(Type type)

        {
            object result = SettingsStorage.FindByType(type);
            return result ?? null;

        }

        public object GetOrDefault(Type type)
        {
            object temp = SettingsStorage.FindByType(type);
            temp ??= this.Add(type);

            return temp;
        }
        #endregion Get

        #region Remove
        public object Reset(Type type)
        {
            object temp = ServiceProvider.GetService(type);
            object added = this.AddOrUpdate(temp);
            return added;
        }

        public bool Remove(Type type)
        {
            bool removed = SettingsStorage.Remove(type);
            return removed;
        }

        public void FactoryReset(bool reset)
        {
            if (reset)
            {
                Logger.LogInformation("Factory Resetting");
                SettingsStorage.FactoryReset(reset);
            }
        }
        #endregion Remove

        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {

                    SettingsStorage = null;
                    ServiceProvider = null;
                    Logger = null;
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion Dispose
    }
}
