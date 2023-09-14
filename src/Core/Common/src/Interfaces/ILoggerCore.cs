using Microsoft.Extensions.Logging;

using nanoFramework.Logging.Debug;

namespace Hoff.Core.Common.Interfaces
{
    public interface ILoggerCore
    {
        #region Public Methods

        DebugLogger GetDebugLogger(string loggerName, LogLevel logLevel);

        void GetMemoryStreamLogger();

        void GetSerialLogger(string port);

        #endregion Public Methods
    }
}
