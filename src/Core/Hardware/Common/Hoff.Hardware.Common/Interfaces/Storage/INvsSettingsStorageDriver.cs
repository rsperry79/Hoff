namespace Hoff.Core.Hardware.Common.Interfaces.Storage
{
    public interface INvsSettingsStorageDriver
    {
        string[] ReadLines(string storageName);
    }
}