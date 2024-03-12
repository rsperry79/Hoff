using nanoFramework.Logging.Debug;

namespace Hoff.Core.Hardware.Common.Models
{
    public abstract class SettingsBase
    {
        protected DebugLogger Logger;

        protected abstract void Initialize();
    }
}
