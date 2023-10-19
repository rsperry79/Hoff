

using System;
using System.Collections;

using Hoff.Core.Hardware.Common.Interfaces.Storage;
using Hoff.Hardware.Common.Interfaces.Storage;

namespace Hoff.Core.Hardware.Common.Models
{
    public class SettingsStorageItem : ISettingsStorageItem, IEqualityComparer  
    {
        #region Properties

        public Type ConfigType { get; set; }

        public ISettingsStorageDriver StorageDriver { get; set; }
        public string StorageLocation { get; set; }

        public new bool Equals(object x, object y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion Properties
    }
}
