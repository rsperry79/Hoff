using System;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging;

namespace Hoff.Core.Services.Logging.Tests.Helpers
{
    internal class TestComponent
    {
        #region Fields

        private readonly ILogger _logger;

        #endregion Fields

        #region Public Constructors

        public TestComponent() => this._logger = this.GetCurrentClassLogger();

        #endregion Public Constructors

        #region Public Methods

        public void DoSomeTestLogging()
        {
            this._logger.LogInformation("An informative message");
            this._logger.LogError("An error situation");
            this._logger.LogWarning(new Exception("Something is not supported"), "With exception context");
        }

        #endregion Public Methods
    }
}
