using System;

namespace Hoff.Core.Services.Settings.Interfaces
{
    public interface ISettingsStorageItem
    {
        Type ConfigType { get; }

        object Payload { get; set; }
    }
}