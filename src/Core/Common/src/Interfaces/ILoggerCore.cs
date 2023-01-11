using Microsoft.Extensions.Logging;
using nanoFramework.Logging.Debug;

namespace Hoff.Core.Interfaces
{
    public interface ILoggerCore
    {
        DebugLogger GetDebugLogger(string loggerName, LogLevel logLevel);
        void GetMemoryStreamLogger();
        void GetSerialLogger(string port);
    }
}
