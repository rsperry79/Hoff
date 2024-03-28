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
        public Type ConfigType { get; set; }
        public object Payload { get; set; }

        #endregion Properties

        #region Public Methods

        public SettingsStorageItem(object payload)
        {
            this.Payload = payload;

            this.ConfigType = this.GetTypeName(payload.GetType());

        }

        public void SetPayload(object payload)
        {
            if (payload is null) { throw new ArgumentNullException(nameof(payload)); }

            this.Payload = payload;
        }

        public bool IsTypeOf(Type type)
        {
            // Check if the type is the same as the stored type
            if (this.ConfigType.FullName == type.FullName)
            {
                return true;
            }

            // Check if the type is an interface of the argument type
            foreach (Type t in type.GetInterfaces())
            {
                if (this.ConfigType.FullName == t.FullName)
                {
                    return true;
                }
            }

            // Check if the type is a subclass of the stored type
            foreach (Type iface in this.GetInterfacesOf())
            {
                if (iface.FullName == type.FullName)
                {
                    return true;
                }
            }

            // if no match return false
            return false;
        }

        #endregion Public Methods

        #region Private Methods
        private Type[] GetInterfacesOf()
        {
            ArrayList interfaces = new();

            Type[] types = this.ConfigType.GetInterfaces();
            int idx = 0;
            foreach (Type type in types)
            {
                _ = interfaces.Add(type.FullName);
                idx++;
            }

            return (Type[])interfaces.ToArray(typeof(Type));
        }

        private Type GetTypeName(Type type)
        {
            Type temp = type;

            bool startsWithI = type.Name.StartsWith("I");
            if (!startsWithI)
            {
                Type[] ifaces = type.GetInterfaces();
                if (ifaces.Length > 0)
                {
                    temp = ifaces[0];
                }
            }

            return temp;
        }
        #endregion Private Methods
    }
}
