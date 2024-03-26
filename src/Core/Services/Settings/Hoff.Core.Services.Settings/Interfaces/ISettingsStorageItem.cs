namespace Hoff.Core.Services.Settings.Interfaces
{
    public interface ISettingsStorageItem
    {
        string ConfigType { get; }

        object Payload { get; set; }

    }
}