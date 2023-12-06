using System;

using Hoff.Core.Common.Interfaces;

using Microsoft.Extensions.Logging;

namespace Hoff.Core.DependencyInjection.Tests.Helpers
{
    public class DiLoggingTestClass
    {
        private readonly ILogger _logger;

        public DiLoggingTestClass(ILoggerCore loggerCore)
        {
            string loggerName = "DiTestClassLogger";
            LogLevel minLogLevel = LogLevel.Trace;
            loggerCore.SetDefaultLoggingLevel(minLogLevel);
            this._logger = loggerCore.GetDebugLogger(loggerName);
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
