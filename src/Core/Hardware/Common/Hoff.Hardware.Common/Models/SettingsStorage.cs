﻿//using System;
//using System.Collections;

//using Hoff.Core.Hardware.Common.Interfaces.Storage;

//namespace Hoff.Core.Hardware.Common.Models
//{
//    public class SettingsStorage : ISettingsStorage
//    {
//        #region Properties
//        private readonly ArrayList items;

//        public int Count => this.items.Count;

//        #endregion Properties

//        #region Public Methods
//        public ArrayList FindByType(Type type)
//        {
//            ArrayList matches = new();
//            for (int i = 0; i < this.items.Count; i++)
//            {
//                if ((this.items[i] as SettingsStorageItem).ConfigType == type)
//                {
//                    _ = matches.Add(this.items[i]);
//                }
//            }

//            return matches;
//        }

//        public int Add(SettingsStorageItem value)
//        {
//            ArrayList existing = this.FindByType(value.ConfigType);

//            foreach (SettingsStorageItem item in existing)
//            {
//                if (item == value)
//                {
//                    return this.items.IndexOf(item);
//                }
//            }

//            return this.items.Add(value);
//        }

//        public bool Contains(Type toFind)
//        {
//            return this.FindByType(toFind).Count > 0;
//        }
//        #endregion Methods
//    }
//}
