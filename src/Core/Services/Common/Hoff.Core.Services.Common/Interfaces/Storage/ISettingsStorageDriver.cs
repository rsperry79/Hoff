namespace Hoff.Services.Common.Interfaces.Storage
{
    public interface ISettingsStorageDriver
    {
        void Clear(string storageName);
        string Read(string storageName);
        bool Write(string storageName, string data);
    }
}