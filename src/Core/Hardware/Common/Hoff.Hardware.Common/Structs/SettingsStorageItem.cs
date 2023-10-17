

using System;

namespace Hoff.Core.Hardware.Common.Structs
{
    public class SettingsStorageItem
    {
        #region Properties

        public Type ConfigType { get; set; }

        public Type StorageDriver { get; set; }
        public string StorageLocation { get; set; }

        #endregion Properties
    }
}
