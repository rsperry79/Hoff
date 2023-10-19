using System;

using Hoff.Hardware.Common.Interfaces.Storage;

namespace Hoff.Core.Hardware.Common.Interfaces.Storage
{
    public interface ISettingsStorageItem
    {
        Type ConfigType { get; set; }
        ISettingsStorageDriver StorageDriver { get; set; }
        string StorageLocation { get; set; }
    }
}