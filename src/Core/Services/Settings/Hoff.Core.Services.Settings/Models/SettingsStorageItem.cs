using System;
using System.Collections;

using Hoff.Core.Services.Settings.Interfaces;

using Microsoft.Extensions.Logging;

namespace Hoff.Core.Services.Settings.Models
{
    public class SettingsStorageItem : ISettingsStorageItem
    {
        public static ILogger Logger { get; set; }

        #region Properties
        public string ConfigType { get; set; }
        public object Payload { get; set; }

        #endregion Properties

        #region Public Methods


        public SettingsStorageItem(object payload)
        {
            this.Payload = payload;
            this.ConfigType = payload.GetType().FullName;
        }

        public void SetPayload(object payload)
        {
            if (payload is null) { throw new ArgumentNullException(nameof(payload)); }

            this.Payload = payload;
        }

        public object GetPayload()
        {
            return this.Payload;
        }

        public bool IsTypeOf(Type type)
        {
            //Type.GetType(this.ConfigType
            if (this.ConfigType == type.FullName)
            {
                Logger.LogInformation($"Matched {this.ConfigType}");
                return true;
            }

            foreach (string iface in this.GetInterfacesOf())
            {
                if (iface == type.FullName.ToString())
                {
                    Logger.LogInformation($"Matched {iface} to {type.FullName}");
                    return true;
                }
            }

            return false;
        }

        #endregion Public Methods

        #region Private Methods
        private string[] GetInterfacesOf()
        {
            ArrayList interfaces = new();
            Type[] types = Type.GetType(this.ConfigType).GetInterfaces();

            foreach (Type type in types)
            {
                interfaces.Add(type.FullName);
            }

            Logger.LogInformation($"GetInterfacesOf Added {interfaces.Count}");
            return (string[])interfaces.ToArray(typeof(string));
        }
        #endregion Private Methods
    }
}
