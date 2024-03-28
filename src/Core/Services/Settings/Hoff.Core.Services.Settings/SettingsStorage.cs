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
    internal class SettingsStorage : IDisposable
    {
        #region Properties
        private static readonly string storageLocation = "settings.json";
        private static ISettingsStorageDriver StorageDriver { get; set; }
        private static IServiceProvider ServiceProvider;
        private static ILogger Logger;
        private static ArrayList Items { get; set; }
        public int Count => Items.Count;
        #endregion Properties

        #region Ctor
        internal SettingsStorage(ISettingsStorageDriver storageDriver, IServiceProvider serviceProvider, ILoggerCore loggerCore)
        {
            if (StorageDriver is null)
            {
                StorageDriver = storageDriver;
                Logger = loggerCore.GetCurrentClassLogger();
                SettingsStorageItem.Logger = loggerCore.GetCurrentClassLogger();
                ServiceProvider = serviceProvider;
            }

            if (Items is null)
            {
#if DEBUG
                Items = new ArrayList();
#else
                this.GetStoredData();
#endif
            }
        }
        #endregion Ctor

        #region Add
        internal object AddOrUpdate(object payload)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }
            int existing = this.FindExistingByType(payload.GetType());


            if (existing == -1)
            {
                _ = this.Add(payload);
            }
            else
            {
                _ = this.Update(payload);
            }

            return payload;
        }

        private object Update(object payload)
        {

            int existing = this.FindExistingByType(payload.GetType());
            if (existing != -1)
            {
                ((ISettingsStorageItem)Items[existing]).Payload = payload;
                _ = this.Save();
            }
            else
            {
                throw new Exception("Item not found");
            }

            return payload;
        }

        internal bool Add(object payload)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            int existing = this.FindExistingByType(payload.GetType());
            if (existing != -1) // second check cleans storage errors if they exist w/o throwing an exception
            {
                throw new Exception("Existing item found");
            }

            SettingsStorageItem newItem = new(payload);
            _ = Items.Add(newItem);
            bool saved = this.Save();
            return saved;
        }
        #endregion Add

        #region Get

        internal object FindByType(Type type)
        {
            int temp = this.FindExistingByType(type);
            if (temp != -1)
            {
                object result = ((ISettingsStorageItem)Items[temp]).Payload;
                return result;
            }
            else
            {
                return null;
            }
        }

        private int FindExistingByType(Type type)
        {
            int temp = -1;

            foreach (SettingsStorageItem item in Items)
            {
                bool isMatch = item.IsTypeOf(type);

                if (isMatch)
                {
                    temp = Items.IndexOf(item);
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
                string[] lines = StorageDriver.ReadLines(storageLocation);
                if (lines != null)
                {
                    Logger.LogInformation($"SettingsStorageItem entries: {lines.Length}");
                    foreach (string line in lines)
                    {
                        DebugLine = line;
                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }
                        try
                        {

                            string[] data = line.Split('\t');

                            // Takes the stored IFace and converts it to the concrete type specified in the DI config for deserialization
                            Type concreteType = ServiceProvider.GetService(Type.GetType(data[0])).GetType();


                            if (concreteType == null)
                            {
                                throw new Exception("Error getting concrete type for deserialization.");
                            }

                            // check for existing item and skip if found
                            int existing = this.FindExistingByType(concreteType);
                            if (existing == -1)
                            {
                                string rawjson = data[1].Trim();
                                // Deserializes the stored object

                                object toStore = JsonConvert.DeserializeObject(rawjson, concreteType);

                                // Creates a new SettingsStorageItem with the deserialized object
                                if (toStore != null)
                                {
                                    ISettingsStorageItem settingsStorageItem = new SettingsStorageItem(toStore);
                                    Items.Add(settingsStorageItem); // ensures that the item is added to the list and saved to storage and is not duplicated
                                    Logger.LogInformation($"SettingsStorageItem Added: {settingsStorageItem.ConfigType}");
                                }
                            }
                            else
                            {
                                Logger.LogInformation($"SettingsStorageItem already exists: {concreteType}");
                            }
                        }
                        catch (Exception ex)
                        {
#if DEBUG
                            Logger.LogError($"{DebugLine}");
                            Logger.LogError(ex.Message, ex);
                            //FactoryReset(true);
                            //throw;
#else
           throw;
#endif

                        }
                    }
                }
            }
            catch (Exception)
            {

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
            int existing = this.FindExistingByType(type);

            if (existing != -1)
            {
                Items.RemoveAt(existing);
                _ = this.Save();
                removed = true;
            }

            return removed;
        }
        #endregion Remove

        #region Save
        internal bool Save()
        {
            _ = Items.ToArray(typeof(SettingsStorageItem)) as SettingsStorageItem[];
            StringBuilder sb = new();

            foreach (SettingsStorageItem item in Items)
            {
                _ = sb.Append(item.ConfigType.FullName + '\t');

                string json = JsonConvert.SerializeObject(item.Payload);
                _ = sb.AppendLine(json);
            }

            string results = sb.ToString();
            bool saved = StorageDriver.Write(storageLocation, results);

            return saved;
        }

        public void Dispose()
        {
            this.Save();
        }

        #endregion Save
    }
}
