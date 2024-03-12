using Microsoft.Extensions.Logging;

using nanoFramework.Logging.Debug;

namespace Hoff.Core.Services.Common.Interfaces
{
    public interface ILoggerCore
    {
        void SetDefaultLoggingLevel(LogLevel level);
        #region Public Methods

        DebugLogger GetDebugLogger(string loggerName);

        void GetMemoryStreamLogger();
        void GetSerialLogger();
        void GetSerialLogger(string port);

        #endregion Public Methods
    }
}
