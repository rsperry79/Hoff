using System;

using Hoff.Core.Common.Interfaces;

using Microsoft.Extensions.Logging;

namespace Hoff.Hardware.Displays.Ssd13.Tests.Helpers
{
    public class DiDisplayTestClass
    {
        #region Fields

        private readonly ILogger _logger;

        #endregion Fields

        #region Public Constructors

        public DiDisplayTestClass(ILoggerCore loggerCore)
        {
            const string loggerName = "DiTestClassLogger";
            const LogLevel minLogLevel = LogLevel.Trace;
            this._logger = loggerCore.GetDebugLogger(loggerName, minLogLevel);
            this._logger.LogInformation("Initializing application...");
        }

        #endregion Public Constructors

        #region Public Methods

        public void RunLogTests()
        {
            this._logger.LogTrace("TRACE {0} {1}", new object[] { "param 1", 42 });
            this._logger.LogDebug("DEBUG {0} {1}", new object[] { "param 1", 42 });
            this._logger.LogInformation("INFORMATION and nothing else");
            this._logger.LogWarning("WARNING {0} {1}", new object[] { "param 1", 42 });
            this._logger.LogError(new Exception("Big problem"), "ERROR {0} {1}", new object[] { "param 1", 42 });
            this._logger.LogCritical(42, new Exception("Insane problem"), "CRITICAL {0} {1}", new object[] { "param 1", 42 });
        }

        #endregion Public Methods
    }
}
