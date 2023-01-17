using System;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging;

namespace Hoff.Core.Logging.Tests.Helpers
{
    internal class TestComponent
    {
        private readonly ILogger _logger;

        public TestComponent() => this._logger = this.GetCurrentClassLogger();

        public void DoSomeTestLogging()
        {
            this._logger.LogInformation("An informative message");
            this._logger.LogError("An error situation");
            this._logger.LogWarning(new Exception("Something is not supported"), "With exception context");
        }
    }
}
