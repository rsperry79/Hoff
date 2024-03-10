//using System;

//using Hoff.Core.Hardware.Common.Interfaces.Services;
//using Hoff.Core.Hardware.Common.Models;

//using nanoFramework.TestFramework;

//namespace Hoff.Core.Hardware.Common.Tests.Models
//{
//    [TestClass]
//    public class SettingsStorageTests
//    {
//        [TestMethod]
//        public void FindByType_StateUnderTest_ExpectedBehavior()
//        {
//            // Arrange
//            SettingsStorage settingsStorage = new();
//            Type type = typeof(IWifiSettings);
//            _ = new
//            SettingsStorageItem();

//            // Act
//            System.Collections.ArrayList result = settingsStorage.FindByType(
//                type);

//            // Assert
//            Assert.IsNotNull(result);
//        }

//        [TestMethod]
//        public void Add_StateUnderTest_ExpectedBehavior()
//        {
//            // Arrange
//            SettingsStorage settingsStorage = new();
//            SettingsStorageItem value = null;

//            // Act
//            int result = settingsStorage.Add(
//                value);

//            // Assert
//            Assert.IsNotNull(result);
//        }

//        [TestMethod]
//        public void Contains_StateUnderTest_ExpectedBehavior()
//        {
//            // Arrange
//            SettingsStorage settingsStorage = new();
//            Type toFind = null;

//            // Act
//            bool result = settingsStorage.Contains(
//                toFind);

//            // Assert
//            Assert.IsNotNull(result);
//        }
//    }
//}
