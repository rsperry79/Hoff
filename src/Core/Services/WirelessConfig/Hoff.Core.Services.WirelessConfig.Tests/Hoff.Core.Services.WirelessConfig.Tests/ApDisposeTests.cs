using System;

using Hoff.Core.Services.Common.Interfaces.Services;
using Hoff.Core.Services.Settings.Tests.Helpers;

using Microsoft.Extensions.Logging;

using nanoFramework.TestFramework;

namespace Hoff.Core.Services.WirelessConfig.Tests
{

    [TestClass]
    public class ApDisposeTests
    {
        private static ILogger Logger;

        #region Tests
        [TestMethod]
        public void DisposeTest()
        {

            using (IApConfig ApConfig = (IApConfig)DiSetup.Services.GetService(typeof(IApConfig)))
            {

            }
            GC.WaitForPendingFinalizers();

            Assert.IsTrue(true);
        }

        #endregion Tests

        #region Helpers

        [Setup]
        public static void Setup()
        {

            if (Logger is null)
            {
                Logger = DiSetup.ConfigureLogger("TestLogger");
                DiSetup.ConfigureServices();
            }

        }
        #endregion Helpers
    }
}
