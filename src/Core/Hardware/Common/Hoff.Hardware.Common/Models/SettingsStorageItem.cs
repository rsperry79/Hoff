

using System;
using System.Collections;

using Hoff.Core.Common.Interfaces;
using Hoff.Core.Hardware.Common.Interfaces.Storage;
using Hoff.Hardware.Common.Interfaces.Storage;

using nanoFramework.Json;
using nanoFramework.Logging.Debug;

namespace Hoff.Core.Hardware.Common.Models
{
    public class SettingsStorageItem : ChangeNotifcation, ISettingsStorageItem, IChangeNotifcation, IEqualityComparer
    {

        private DebugLogger Logger;

        public SettingsStorageItem(ISettingsStorageDriver storageDriver, string storageLocation, object payload, ILoggerCore loggerCore)
            : this(storageDriver, storageLocation, loggerCore)
        {
            this.Payload = payload;

        }

        public SettingsStorageItem(ISettingsStorageDriver storageDriver, string storageLocation, ILoggerCore loggerCore) : this(loggerCore)
        {
            this.StorageDriver = storageDriver;
            this.StorageLocation = storageLocation;



        }

        public SettingsStorageItem(ILoggerCore loggerCore)
        {
            this.Logger = loggerCore.GetDebugLogger(this.GetType().ToString());
        }

        #region Properties
        public Type ConfigType { get; set; }

        public ISettingsStorageDriver StorageDriver { get; set; }

        public string StorageLocation { get; set; }


        public object Payload
        {
            get
            {
                string temp = this.StorageDriver.Read(this.StorageLocation);
                return JsonConvert.DeserializeObject(temp, this.ConfigType);
            }
            set
            {
                string temp = JsonConvert.SerializeObject(value);
                this.StorageDriver.Write(this.StorageLocation, temp);
                this.SendEvent();
            }
        }

        #endregion Properties

        #region Equality    
        public new bool Equals(object x, object y)
        {
            bool loc = ((SettingsStorageItem)x).StorageLocation.Equals(((SettingsStorageItem)y).StorageLocation);
            bool conf = ((SettingsStorageItem)x).ConfigType.Equals(((SettingsStorageItem)y).ConfigType);
            return loc && conf;
        }

        public int GetHashCode(object obj)
        {
            return ((SettingsStorageItem)obj).StorageLocation.GetHashCode() + ((SettingsStorageItem)obj).ConfigType.GetHashCode();
        }
        #endregion Equality
    }
}
