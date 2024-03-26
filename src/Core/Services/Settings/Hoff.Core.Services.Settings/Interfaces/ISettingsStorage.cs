using System;
using System.Collections;

namespace Hoff.Core.Services.Settings.Interfaces
{
    public interface ISettingsStorage
    {

        int Count { get; }

        bool Add(object payload, Type type);
        ISettingsStorageItem AddOrUpdate(object value, Type type);

        bool Contains(Type toFind);
        ArrayList FindByType(Type type);
        void Remove(ISettingsStorageItem value);
        bool Save();
    }
}