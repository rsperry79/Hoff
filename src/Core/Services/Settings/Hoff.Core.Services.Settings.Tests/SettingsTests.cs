using System;

using Hoff.Core.Services.Common.Interfaces.Services;
using Hoff.Core.Services.Settings.Tests.Helpers;
using Hoff.Tests.Common;
using Hoff.Tests.Common.Interfaces;
using Hoff.Tests.Common.Models;

using Microsoft.Extensions.Logging;

using nanoFramework.TestFramework;

namespace Hoff.Core.Services.Settings.Tests
{
    [TestClass]
    public class SettingsTests
    {
        #region Fields
        private static ILogger Logger;
        private static IServiceProvider Services;
        private static ISettingsService SettingsService;
        #endregion Fields

        #region Tests 

        #region Get
        [TestMethod]
        public void GetSettingsByTypeTest()
        {
            // Arrange
            SettingsService.FactoryReset(true);
            SettingsService.Add(typeof(ISettingsTestModel));

            // Act
            ISettingsTestModel result = (ISettingsTestModel)SettingsService.Get(typeof(ISettingsTestModel));


            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(SettingsService.SettingsCount(), 1);
        }

        [TestMethod]
        public void GetSettingsNullByTypeTest()
        {
            // Arrange
            SettingsService.FactoryReset(true);

            // Act
            ISettingsTestModel result = (ISettingsTestModel)SettingsService.Get(typeof(ISettingsTestModel));


            // Assert
            Assert.IsNull(result);
            Assert.AreEqual(SettingsService.SettingsCount(), 0);
        }

        [TestMethod]
        public void GetDefaultSettingsByTypeTest()
        {
            // Arrange
            SettingsService.FactoryReset(true);

            // Act
            ISettingsTestModel result = (ISettingsTestModel)SettingsService.GetOrDefault(typeof(ISettingsTestModel));


            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(SettingsService.SettingsCount(), 1);
        }

        [TestMethod]
        public void GetExistingSettingsByTypeTest()
        {
            // Arrange
            SettingsService.FactoryReset(true);
            SettingsService.Add(typeof(ISettingsTestModel));

            // Act
            ISettingsTestModel result = (ISettingsTestModel)SettingsService.Get(typeof(ISettingsTestModel));

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(SettingsService.SettingsCount(), 1);
        }
        #endregion Get

        #region Add
        [TestMethod]
        public void AddObjectTest()
        {
            // Arrange
            SettingsService.FactoryReset(true);
            SettingsTestModel settingsTest = new()
            {
                MachineName = "Test"
            };

            // Act
            SettingsService.Add(settingsTest);

            // Assert
            Assert.IsNotNull(settingsTest);
            Assert.AreEqual(SettingsService.SettingsCount(), 1);
        }

        [TestMethod]
        public void AddTypeTest()
        {
            // Arrange
            SettingsService.FactoryReset(true);

            // Act
            ISettingsTestModel result = (ISettingsTestModel)SettingsService.Add(typeof(ISettingsTestModel));
            result.MachineName = "Test";

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(SettingsService.SettingsCount(), 1);
        }

        [TestMethod]
        public void AddConcreteObjectTest()
        {
            // Arrange
            SettingsService.FactoryReset(true);

            // Act
            SettingsTestModel result = (SettingsTestModel)SettingsService.Add(new SettingsTestModel());
            result.MachineName = "Test";

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(SettingsService.SettingsCount(), 1);
        }
        #endregion Add

        #region AddOrUpdate
        [TestMethod]
        public void AddOrUpdateObjectTest()
        {

            SettingsService.FactoryReset(true);
            SettingsTestModel settingsTest = new()
            {
                MachineName = "Test"
            };
            SettingsService.Add(settingsTest);

            SettingsTestModel result1 = (SettingsTestModel)SettingsService.Get(typeof(SettingsTestModel));
            Assert.AreEqual(settingsTest.MachineName, result1.MachineName);

            SettingsTestModel settingsTest2 = new()
            {
                MachineName = "Test2"
            };

            SettingsService.AddOrUpdate(settingsTest2);
            SettingsTestModel result2 = (SettingsTestModel)SettingsService.Get(typeof(SettingsTestModel));
            Assert.AreEqual(settingsTest2.MachineName, result2.MachineName);
            Assert.AreEqual(SettingsService.SettingsCount(), 1);
        }

        [TestMethod]
        public void AddOrUpdateTypeTest()
        {
            SettingsService.FactoryReset(true);
            ISettingsTestModel result = (ISettingsTestModel)SettingsService.Add(typeof(ISettingsTestModel));

            ISettingsTestModel result2 = (ISettingsTestModel)SettingsService.AddOrUpdate(typeof(ISettingsTestModel));
            Assert.AreEqual(SettingsService.SettingsCount(), 1, "Count is correct");

        }
        #endregion AddOrUpdate

        #region Remove

        [TestMethod]
        public void RemoveTypeTest()
        {
            // Arrange
            SettingsService.FactoryReset(true);
            ISettingsTestModel temp = (ISettingsTestModel)SettingsService.Add(typeof(ISettingsTestModel));

            // Act
            bool result = SettingsService.Remove(typeof(ISettingsTestModel));

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RemoveConcreteTest()
        {
            // Arrange
            SettingsService.FactoryReset(true);
            SettingsTestModel temp = (SettingsTestModel)SettingsService.Add(typeof(SettingsTestModel));

            // Act
            bool result = SettingsService.Remove(typeof(SettingsTestModel));

            // Assert
            Assert.IsTrue(result);
        }
        #endregion Remove

        #region Reset
        [TestMethod]
        public void ResetTypeTest()
        {
            // Arrange
            SettingsService.FactoryReset(true);
            ISettingsTestModel toAdd = new SettingsTestModel
            {
                MachineName = "Test"
            };
            ISettingsTestModel temp = (ISettingsTestModel)SettingsService.Add(toAdd);
            int pre = SettingsService.SettingsCount();

            // Act
            SettingsService.Reset(typeof(ISettingsTestModel));
            ISettingsTestModel reset = (ISettingsTestModel)SettingsService.Get(typeof(ISettingsTestModel));
            int post = SettingsService.SettingsCount();

            // Assert
            Assert.AreEqual(pre, post);
            Assert.IsNotNull(temp);
            Assert.IsTrue(string.IsNullOrEmpty(reset.MachineName));
            Assert.AreNotEqual(temp.MachineName, reset.MachineName);
        }

        [TestMethod]
        public void FactoryResetTrueTest()
        {
            // Act
            SettingsService.FactoryReset(true);

            // Assert
            Assert.IsTrue(SettingsService.SettingsCount() == 0);
        }

        [TestMethod]
        public void FactoryResetFalseTest()
        {
            // Arrange
            SettingsService.FactoryReset(true);
            ISettingsTestModel temp = (ISettingsTestModel)SettingsService.Add(typeof(ISettingsTestModel));

            // Act
            int pre = SettingsService.SettingsCount();
            SettingsService.FactoryReset(false);
            int post = SettingsService.SettingsCount();

            // Assert
            Assert.AreEqual(pre, post);
        }
        #endregion Reset

        #endregion Tests

        #region Helpers

        private static bool isSetup = false;

        public void ConfigureServices()
        {
            if (isSetup is false)
            {
                TestHelpers.GetServiceCollection();
                DiSetup.ConfigureServices();
                isSetup = true;
            }
        }

        [Setup]
        public void Setup()
        {
            if (Services is null)
            {
                this.ConfigureServices();
                Services = TestHelpers.GetServices();
                Logger = TestHelpers.GetLogger("Integration Tests");
                SettingsService = (ISettingsService)Services.GetService(typeof(ISettingsService));

            }
        }
    }
    #endregion Helpers
}

