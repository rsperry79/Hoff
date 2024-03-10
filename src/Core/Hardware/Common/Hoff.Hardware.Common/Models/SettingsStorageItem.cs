
//using System;
//using System.Collections;

//using Hoff.Hardware.Common.Interfaces.Storage;

//using Microsoft.Extensions.Logging;

//using nanoFramework.Json;
//using nanoFramework.Logging;

//namespace Hoff.Core.Hardware.Common.Models
//{
//    public class SettingsStorageItem : SettingsBase, ISettingsStorageItem, IChangeNotification, IEqualityComparer
//    {

//        protected static new ILogger Logger;

//        public SettingsStorageItem(ISettingsStorageDriver storageDriver, string storageLocation, object payload)
//            : this(storageDriver, storageLocation) => this.Payload = payload;

//        public SettingsStorageItem(ISettingsStorageDriver storageDriver, string storageLocation) : this()
//        {
//            this.StorageDriver = storageDriver;
//            this.StorageLocation = storageLocation;
//        }

//        public SettingsStorageItem() => Logger = this.GetCurrentClassLogger();

//        #region Properties
//        public Type ConfigType { get; set; }

//        public ISettingsStorageDriver StorageDriver { get; set; }

//        public string StorageLocation { get; set; }

//        public object Payload
//        {
//            get
//            {
//                string temp = this.StorageDriver.Read(this.StorageLocation);
//                return JsonConvert.DeserializeObject(temp, this.ConfigType);
//            }
//            set
//            {
//                string temp = JsonConvert.SerializeObject(value);
//                this.StorageDriver.Write(this.StorageLocation, temp);
//                this.SendEvent();
//            }
//        }

//        #endregion Properties

//        #region Equality    
//        public new bool Equals(object x, object y)
//        {
//            bool loc = ((SettingsStorageItem)x).StorageLocation.Equals(((SettingsStorageItem)y).StorageLocation);
//            bool conf = ((SettingsStorageItem)x).ConfigType.Equals(((SettingsStorageItem)y).ConfigType);
//            return loc && conf;
//        }

//        public int GetHashCode(object obj)
//        {
//            return ((SettingsStorageItem)obj).StorageLocation.GetHashCode() + ((SettingsStorageItem)obj).ConfigType.GetHashCode();
//        }
//        #endregion Equality
//        protected override void Initialize() { }
//    }
//}
