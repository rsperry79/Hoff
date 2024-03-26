// Ignore Spelling: Nvs

using System;
using System.IO;

using Hoff.Core.Hardware.Common.Interfaces.Storage;
using Hoff.Core.Services.Common.Interfaces.Services;

using Hoff.Services.Common.Interfaces.Storage;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging.Debug;

namespace Hoff.Core.Hardware.Storage.Nvs
{
    public class NvsStorage : INvsSettingsStorageDriver, ISettingsStorageDriver
    {
        private static DebugLogger Logger;

        private string FilePath { get; set; } = "I:\\";

        public NvsStorage(ILoggerCore loggerCore) => Logger = loggerCore.GetDebugLogger(this.GetType().ToString());

        public string Read(string storageName)
        {
            bool hasStored = this.HasStored(storageName);
            string temp = !hasStored ? string.Empty : GetString(this.ToUri(storageName));

            if (!hasStored)
            {
                _ = this.Write(this.ToUri(storageName), "");
            }

            return temp;
        }

        public bool Write(string storageName, string data)
        {
            File.WriteAllText(this.ToUri(storageName), data);
            return true;
        }

        public void Clear(string storageName)
        {
            string uri = this.ToUri(storageName);
            if (File.Exists(uri))
            {
                File.Delete(uri);
            }

            _ = _ = this.Write(uri, string.Empty);
        }

        internal bool HasStored(string storageName)
        {
            bool temp = File.Exists(this.ToUri(storageName));
            return temp;
        }

        //private static bool WriteData(string uri, string data)
        //{
        //    //using (FileStream file = new(uri, FileMode.Create))
        //    //{
        //    //    byte[] buffer = Encoding.UTF8.GetBytes(data);
        //    //    file.Write(buffer, 0, buffer.Length);
        //    //    file.Dispose();
        //    //    temp = true;
        //    //}
        //}

        private static string GetString(string uri)
        {
            try
            {
                return File.ReadAllText(uri);
                //using (FileStream stream = new(uri, FileMode.Open))
                //{
                //    byte[] buffer = new byte[stream.Length];
                //    _ = stream.Read(buffer, 0, (int)stream.Length);

                //    string raw = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                //    return raw;
                //}
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex.Message, ex);
                throw;
            }
        }

        internal string ToUri(string storageName)
        {
            return $"{this.FilePath}{storageName}";
        }
    }
}