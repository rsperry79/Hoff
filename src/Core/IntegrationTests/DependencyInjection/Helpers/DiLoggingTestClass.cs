using Hoff.Core.Interfaces;

using Microsoft.Extensions.Logging;

using System;

namespace Hoff.Core.DependencyInjection.Tests.Helpers
{
    public class DiLoggingTestClass
    {
        private readonly ILogger _logger;

        public DiLoggingTestClass(ILoggerCore loggerCore)
        {
            string loggerName = "DiTestClassLogger";
            LogLevel minLogLevel = LogLevel.Trace;

            _logger = loggerCore.GetDebugLogger(loggerName, minLogLevel);
            _logger.LogInformation("Initializing application...");
        }

        public void RunLogTests()
        {
            _logger.LogTrace("TRACE {0} {1}", new object[] { "param 1", 42 });
            _logger.LogDebug("DEBUG {0} {1}", new object[] { "param 1", 42 });
            _logger.LogInformation("INFORMATION and nothing else");
            _logger.LogWarning("WARNING {0} {1}", new object[] { "param 1", 42 });
            _logger.LogError(new Exception("Big problem"), "ERROR {0} {1}", new object[] { "param 1", 42 });
            _logger.LogCritical(42, new Exception("Insane problem"), "CRITICAL {0} {1}", new object[] { "param 1", 42 }); ;
        }
    }
}
