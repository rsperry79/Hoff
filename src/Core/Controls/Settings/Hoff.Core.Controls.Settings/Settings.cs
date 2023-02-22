using System;

using Hoff.Hardware.Common.Interfaces.Storage;

using nanoFramework.Json;

namespace Hoff.Core.Controls.Settings
{
    public class Settings <T>
    {
        private static IEeprom Eeprom;
        private static byte StartLocation;

        public Settings(IEeprom eeprom, byte startLocation = 0x00)
        {
            Eeprom = eeprom;
            StartLocation = startLocation;
        }

        public T GetSettings()
        {
            string data = Eeprom.ReadString(StartLocation);
            T settings = (T)JsonConvert.DeserializeObject(data, typeof(T));
            return settings;
        }

        public bool WriteSettings(T settings)
        {
            string result = JsonConvert.SerializeObject(settings);
            bool write = Eeprom.WriteString(StartLocation, result);
            return write;
        }
    }
}
