
using Hoff.Core.Hardware.Storage.At24.Tests.Helpers;
using Hoff.Hardware.Common.Interfaces.Storage;

using nanoFramework.TestFramework;

namespace Hoff.Core.Hardware.Storage.At24.Tests
{
    [TestClass]
    public class StringTests
    {


        //[TestMethod]
        //public void WriteTest()
        //{
        //    // Arrange
        //    IEeprom at24cEeprom = SetupHelper.Setup();
        //    byte address = 0;
        //    byte[] message = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

        //    // Act
        //    bool result = at24cEeprom.Write(address, message);

        //    // Assert
        //    Assert.IsTrue(result);
        //}

        private const byte stringWriteAddr = 0x90;
        private const string stringWriteMesage = "This is a test";

        [TestMethod]
        public void WriteStringTest()
        {
            // Arrange
            IEeprom at24cEeprom = SetupHelper.Setup();

            // Act
            bool result = at24cEeprom.WriteString(stringWriteAddr, stringWriteMesage);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ReadStringTest()
        {
            // Arrange
            IEeprom at24cEeprom = SetupHelper.Setup();


            // Act
            string result = at24cEeprom.ReadString(stringWriteAddr);

            // Assert
            Assert.AreEqual(stringWriteMesage, result);
        }



    }
}
