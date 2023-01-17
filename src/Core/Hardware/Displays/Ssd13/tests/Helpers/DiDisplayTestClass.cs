using System;

using Hoff.Core.Interfaces;

using Microsoft.Extensions.Logging;

namespace Hoff.Hardware.Displays.Ssd13.Tests.Helpers
{
    public class DiDisplayTestClass
    {
        private readonly ILogger _logger;

        public DiDisplayTestClass(ILoggerCore loggerCore)
        {
            string loggerName = "DiTestClassLogger";
            LogLevel minLogLevel = LogLevel.Trace;

            this._logger = loggerCore.GetDebugLogger(loggerName, minLogLevel);
            this._logger.LogInformation("Initializing application...");
        }

        public void RunLogTests()
        {
            this._logger.LogTrace("TRACE {0} {1}", new object[] { "param 1", 42 });
            this._logger.LogDebug("DEBUG {0} {1}", new object[] { "param 1", 42 });
            this._logger.LogInformation("INFORMATION and nothing else");
            this._logger.LogWarning("WARNING {0} {1}", new object[] { "param 1", 42 });
            this._logger.LogError(new Exception("Big problem"), "ERROR {0} {1}", new object[] { "param 1", 42 });
            this._logger.LogCritical(42, new Exception("Insane problem"), "CRITICAL {0} {1}", new object[] { "param 1", 42 }); ;
        }
    }
}
