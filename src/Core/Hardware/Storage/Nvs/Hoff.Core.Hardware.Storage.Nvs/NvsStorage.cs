// Ignore Spelling: Nvs

using System;
using System.IO;
using System.Text;

using Hoff.Core.Common.Interfaces;
using Hoff.Hardware.Common.Interfaces.Storage;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging.Debug;

namespace Hoff.Core.Hardware.Storage.Nvs
{
    public class NvsStorage : ISettingsStorageDriver
    {
        public NvsStorage(ILoggerCore loggerCore) => Logger = loggerCore.GetDebugLogger(this.GetType().ToString());

        public string Read(string storageName)
        {
            bool hasStored = HasStored(storageName);
            string temp = !hasStored ? string.Empty : GetString(ToUri(storageName));

            if (!hasStored)
            {
                WriteData(ToUri(storageName), string.Empty);
            }

            return temp;
        }

        public void Write(string storageName, string data)
        {

            WriteData(ToUri(storageName), data);
        }

        public void Clear(string storageName)
        {
            string uri = ToUri(storageName);
            if (File.Exists(uri))
            {
                File.Delete(uri);
            }

            WriteData(uri, string.Empty);
        }

        private static DebugLogger Logger;
        private static bool HasStored(string storageName)
        {
            bool temp = File.Exists(ToUri(storageName));
            return temp;
        }

        private static string ToUri(string storageName)
        {
            return $"I:\\{storageName}";
        }

        private static bool WriteData(string uri, string data)
        {
            try
            {
                FileStream file = new FileStream(uri, FileMode.Create);

                byte[] buffer = Encoding.UTF8.GetBytes(data);
                file.Write(buffer, 0, buffer.Length);
                file.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex.Message, ex);
                throw;
            }
        }

        private static string GetString(string uri)
        {
            try
            {
                FileStream stream = new FileStream(uri, FileMode.Open);
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int)stream.Length);

                string raw = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                return raw;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex.Message, ex);
                throw;
            }
        }
    }
}