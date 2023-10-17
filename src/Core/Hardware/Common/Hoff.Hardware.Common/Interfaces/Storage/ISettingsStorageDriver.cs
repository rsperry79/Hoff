namespace Hoff.Hardware.Common.Interfaces.Storage
{
    public interface ISettingsStorageDriver
    {
        void Clear(string storageName);
        string Read(string storageName);
        void Write(string storageName, string data);
    }
}