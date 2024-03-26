using System;
using System.Collections;
using System.Text;

using Hoff.Core.Services.Common.Interfaces.Services;
using Hoff.Core.Services.Settings.Interfaces;
using Hoff.Core.Services.Settings.Models;
using Hoff.Services.Common.Interfaces.Storage;

using Microsoft.Extensions.Logging;

using nanoFramework.Json;
using nanoFramework.Logging;

namespace Hoff.Core.Services.Settings
{
    internal class SettingsStorage
    {
        #region Properties
        private static readonly string storageLocation = "settings.json";
        private static ISettingsStorageDriver StorageDriver { get; set; }
        private static ILogger Logger;
        private static ArrayList Items { get; set; }
        public int Count => Items.Count;
        #endregion Properties

        #region Ctor
        internal SettingsStorage(ISettingsStorageDriver storageDriver, ILoggerCore loggerCore)
        {
            if (StorageDriver is null)
            {
                StorageDriver = storageDriver;
                Logger = loggerCore.GetCurrentClassLogger();
                SettingsStorageItem.Logger = loggerCore.GetCurrentClassLogger();
            }

            if (Items is null)
            {
                this.GetStoredData();
            }
        }
        #endregion Ctor 

        #region Add
        internal object AddOrUpdate(object payload)
        {
            object existing = this.Update(payload);
            if (existing == null)
            {
                this.Add(payload);
            }

            return payload;
        }

        internal object Update(object payload)
        {
            ISettingsStorageItem existing = this.FindExistingByType(payload.GetType());
            if (existing != null)
            {
                existing.Payload = payload;
                this.Save();
            }
            else
            {
                throw new Exception("Item not found");
            }
            return payload;
        }

        internal bool Add(object payload)
        {
            ISettingsStorageItem existing = this.FindExistingByType(payload.GetType());
            if (existing != null)
            {
                throw new Exception("Existing item found");

            }
            SettingsStorageItem newItem = new(payload);

            _ = Items.Add(newItem);
            bool res = this.Save();
            return res;
        }
        #endregion Add

        #region Get

        internal object FindByType(Type type)
        {
            SettingsStorageItem temp = this.FindExistingByType(type);
            if (temp != null)
            {
                object result = temp.Payload;
                return result;
            }
            else
            {
                return null;
            }
        }

        private SettingsStorageItem FindExistingByType(Type type)
        {
            SettingsStorageItem temp = null;

            foreach (SettingsStorageItem item in Items)
            {
                bool isMatch = item.IsTypeOf(type);

                if (isMatch)
                {
                    temp = item;
                    break;
                }
            }


            return temp;
        }

        public void GetStoredData()
        {
            string DebugLine = "";
            try
            {

                Items = new ArrayList();
                string raw = StorageDriver.Read(storageLocation);
                if (!string.IsNullOrEmpty(raw) && raw != "null")
                {
                    string[] lines = raw.Split('\t');
                    foreach (string line in lines)
                    {
                        DebugLine = line;
                        SettingsStorageItem deser = JsonConvert.DeserializeObject(line, typeof(ISettingsStorageItem)) as SettingsStorageItem;
                        Items.Add(deser);
                        Logger.LogInformation($"SettingsStorageItem Added: {deser.ConfigType}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"{DebugLine}");
                Logger.LogError(ex.Message, ex);
                throw;
            }
        }
        #endregion Get

        #region Remove
        internal static void FactoryReset(bool reset)
        {
            if (reset)
            {
                Items = new ArrayList();
                StorageDriver.Clear(storageLocation);

            }
        }
        internal bool Remove(Type type)
        {
            bool removed = false;
            SettingsStorageItem temp = this.FindExistingByType(type);

            if (temp != null)
            {
                Items.Remove(temp);
                this.Save();
                removed = true;
            }

            return removed;
        }
        #endregion Remove

        #region Save
        internal bool Save()
        {
            SettingsStorageItem[] settingsStorageItems = Items.ToArray(typeof(SettingsStorageItem)) as SettingsStorageItem[];
            StringBuilder sb = new StringBuilder();

            foreach (SettingsStorageItem item in Items)
            {
                string json = JsonConvert.SerializeObject(item);
                sb.Append(json + '\t');
            }

            string results = sb.ToString();
            Logger.LogInformation($"Settings Save Payload: {results}");
            bool saved = StorageDriver.Write(storageLocation, results);

            return saved;
        }


        #endregion Save
    }
}
