using Microsoft.Extensions.Logging;

using nanoFramework.Logging.Debug;

namespace Hoff.Core.Common.Interfaces
{
    public interface ILoggerCore
    {
        DebugLogger GetDebugLogger(string loggerName, LogLevel logLevel);
        void GetMemoryStreamLogger();
        void GetSerialLogger(string port);
    }
}
