using System;
using System.Collections;

using Hoff.Hardware.Common.Interfaces.Storage;

namespace Hoff.Core.Hardware.Common.Interfaces.Storage
{
    public interface ISettingsStorageItem : IEqualityComparer
    {
        Type ConfigType { get; set; }
        ISettingsStorageDriver StorageDriver { get; set; }
        string StorageLocation { get; set; }
        object Payload { get; set; }
    }
}