// Ignore Spelling: Nvs

using System;
using System.IO;
using System.Text;

using Hoff.Core.Common.Interfaces;
using Hoff.Core.Hardware.Common.Interfaces.Storage;
using Hoff.Hardware.Common.Interfaces.Storage;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging.Debug;

namespace Hoff.Core.Hardware.Storage.Nvs
{
    public class NvsStorage : INvsSettingsStorageDriver, ISettingsStorageDriver
    {
        private static DebugLogger Logger;

        public string FiePath { get; set; } = "I:\\";

        public NvsStorage(ILoggerCore loggerCore)
        {
            Logger = loggerCore.GetDebugLogger(this.GetType().ToString());
        }



        public string Read(string storageName)
        {
            bool hasStored = this.HasStored(storageName);
            string temp = !hasStored ? string.Empty : GetString(this.ToUri(storageName));

            if (!hasStored)
            {
                WriteData(this.ToUri(storageName), string.Empty);
            }

            return temp;
        }

        public void Write(string storageName, string data)
        {

            WriteData(this.ToUri(storageName), data);
        }

        public void Clear(string storageName)
        {
            string uri = this.ToUri(storageName);
            if (File.Exists(uri))
            {
                File.Delete(uri);
            }

            WriteData(uri, string.Empty);
        }


        internal bool HasStored(string storageName)
        {
            bool temp = File.Exists(this.ToUri(storageName));
            return temp;
        }

        private static bool WriteData(string uri, string data)
        {
            bool temp = false;
            using (FileStream file = new FileStream(uri, FileMode.Create))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                file.Write(buffer, 0, buffer.Length);
                file.Dispose();
                temp = true;
            }

            return temp;
        }

        private static string GetString(string uri)
        {
            try
            {
                using (FileStream stream = new FileStream(uri, FileMode.Open))
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, (int)stream.Length);

                    string raw = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    return raw;
                }

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex.Message, ex);
                throw;
            }
        }

        internal string ToUri(string storageName)
        {
            return $"{this.FiePath}{storageName}";
        }
    }
}