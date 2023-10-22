using Hoff.Core.Common.Interfaces;

using nanoFramework.Logging.Debug;

namespace Hoff.Core.Hardware.Common.Models
{
    public abstract class SettingsBase : ChangeNotifcation
    {
        protected DebugLogger Logger;

        protected abstract void Initialize();
    }
}
