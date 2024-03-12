using Hoff.Core.Hardware.Common.Interfaces.Storage;
using Hoff.Hardware.Common.Interfaces.Storage;

namespace Hoff.Core.Hardware.Storage.Nvs.Mocks
{
    public class NvsMock : INvsSettingsStorageDriver, ISettingsStorageDriver
    {
        // Browse our samples repository: https://github.com/nanoframework/samples
        // Check our documentation online: https://docs.nanoframework.net/
        // Join our lively Discord community: https://discord.gg/gCyBu8T
        public void Clear(string storageName)
        {
            throw new System.NotImplementedException();
        }

        public string Read(string storageName)
        {
            throw new System.NotImplementedException();
        }

        public void Write(string storageName, string data)
        {
            throw new System.NotImplementedException();
        }



    }
}
