using System;

using Hoff.Core.Services.Common.Interfaces;
using Hoff.Core.Services.Logging.Tests.Helpers;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging.Debug;
using nanoFramework.TestFramework;

namespace Hoff.Core.Services.Logging.Tests
{
    [TestClass]
    public class LoggerCoreTests
    {
        #region Public Methods

        [TestMethod]
        public void CreateNewLoggerTest()
        {
            // Arrange
            LoggerCore loggerCore = new();
            const string loggerName = "TestLogger";
            const LogLevel minLogLevel = LogLevel.Trace;
            loggerCore.SetDefaultLoggingLevel(minLogLevel);
            DebugLogger logger = loggerCore.GetDebugLogger(loggerName);

            // Act
            Assert.IsNotNull(logger);
            Assert.AreEqual(loggerName, logger.LoggerName);
        }

        [TestMethod]
        public void GetMemoryStreamLoggerTest()
        {
            // Arrange
            LoggerCore loggerCore = new();
            const string loggerName = "SerialLogger";
            const LogLevel minLogLevel = LogLevel.Trace;
            loggerCore.SetDefaultLoggingLevel(minLogLevel);
            DebugLogger logger = loggerCore.GetDebugLogger(loggerName);
            loggerCore.GetMemoryStreamLogger();
            TestComponent testComponent = new();

            // Act
            Assert.IsNotNull(logger);
            testComponent.DoSomeTestLogging();
        }

        [TestMethod]
        public void GetSerialLoggerTest()
        {
            //Arrange
            ILoggerCore loggerCore = new LoggerCore();
            const string loggerName = "SerialLogger";
            const LogLevel minLogLevel = LogLevel.Trace;
            loggerCore.SetDefaultLoggingLevel(minLogLevel);
            DebugLogger logger = loggerCore.GetDebugLogger(loggerName);
            //loggerCore.GetSerialLogger();
            TestComponent testComponent = new();

            // Act
            Assert.IsNotNull(logger);
            testComponent.DoSomeTestLogging();
        }

        [TestMethod]
        public void LoggerDoesNotThrowTest()
        {
            // Arrange
            ILoggerCore loggerCore = new LoggerCore();
            const string loggerName = "SerialLogger";
            const LogLevel minLogLevel = LogLevel.Trace;
            loggerCore.SetDefaultLoggingLevel(minLogLevel);
            DebugLogger _logger = loggerCore.GetDebugLogger(loggerName);

            // Act
            Assert.IsNotNull(_logger);
            _logger.LogInformation("Hello from nanoFramework!");
            _logger.LogTrace("Trace: the Debug Logger is initialized");
            _logger.LogInformation($"Logger name is: {_logger.LoggerName}, you can use that to trace which component is used");
            _logger.LogInformation("The next call to the class will log as well");
            _logger.LogInformation("For this component, we're using the Logger Factory pattern. It will use the logger as well");

            _logger.LogInformation("Your responsibility is to make sure you set the right level as well as formatting the strings");
            _logger.LogInformation("More examples below. All will display as the log level is Trace.");
            _logger.LogTrace("TRACE {0} {1}", new object[] { "param 1", 42 });
            _logger.LogDebug("DEBUG {0} {1}", new object[] { "param 1", 42 });
            _logger.LogInformation("INFORMATION and nothing else");
            _logger.LogWarning("WARNING {0} {1}", new object[] { "param 1", 42 });
            _logger.LogError(new Exception("Big problem"), "ERROR {0} {1}", new object[] { "param 1", 42 });
            _logger.LogCritical(42, new Exception("Insane problem"), "CRITICAL {0} {1}", new object[] { "param 1", 42 });
            _logger.LogInformation("Now we will adjust the level to Critical, only the Critical message will appear");
            _logger.MinLogLevel = LogLevel.Critical;
            _logger.LogTrace("TRACE {0} {1}", new object[] { "param 1", 42 });
            _logger.LogDebug("DEBUG {0} {1}", new object[] { "param 1", 42 });
            _logger.LogInformation("INFORMATION and nothing else");
            _logger.LogWarning("WARNING {0} {1}", new object[] { "param 1", 42 });
            _logger.LogError(new Exception("Big problem"), "ERROR {0} {1}", new object[] { "param 1", 42 });
            _logger.LogCritical(42, new Exception("Insane problem"), "CRITICAL {0} {1}", new object[] { "param 1", 42 });
        }

        #endregion Public Methods
    }
}
