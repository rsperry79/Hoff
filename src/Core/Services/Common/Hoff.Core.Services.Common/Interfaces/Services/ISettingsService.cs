using System;

namespace Hoff.Core.Services.Common.Interfaces.Services
{
    public interface ISettingsService
    {
        object Add(object payload);
        object Add(Type type);
        object AddOrUpdate(object payload);
        object AddOrUpdate(Type type);
        void Dispose();
        void FactoryReset(bool reset);
        object Get(Type type);
        object GetOrDefault(Type type);
        bool Remove(Type type);
        object Reset(Type type);
        int SettingsCount();
    }
}