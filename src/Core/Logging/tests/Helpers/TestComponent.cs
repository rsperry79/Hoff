using Microsoft.Extensions.Logging;

using nanoFramework.Logging;

using System;

namespace Hoff.Core.Logging.Tests.Helpers
{
    internal class TestComponent
    {
        private readonly ILogger _logger;

        public TestComponent()
        {
            _logger = this.GetCurrentClassLogger();
        }

        public void DoSomeTestLogging()
        {
            _logger.LogInformation("An informative message");
            _logger.LogError("An error situation");
            _logger.LogWarning(new Exception("Something is not supported"), "With exception context");
        }
    }
}
