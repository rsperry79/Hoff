using System;

using Hoff.Core.Hardware.Common.Interfaces.Storage;
using Hoff.Hardware.Common.Interfaces.Storage;

namespace Hoff.Core.Hardware.Common.Interfaces.Services
{
    public interface ISettingsService
    {
        void Add(Type type, ISettingsStorageDriver driver, string storageLocation, object payload);
        void Dispose();
        ISettingsStorageItem GetFirst(Type type);
        ISettingsStorageItem GetFirstOrDefault(Type type);
    }
}