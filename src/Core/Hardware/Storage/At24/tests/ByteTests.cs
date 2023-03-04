
using Hoff.Core.Hardware.Storage.At24.Tests.Helpers;
using Hoff.Hardware.Common.Interfaces.Storage;

using Iot.Device.At24cxx;

using nanoFramework.TestFramework;

namespace Hoff.Core.Hardware.Storage.At24.Tests
{
    [TestClass]
    public class ByteTests
    {
        [TestMethod]
        public void ByteTest()
        {
            byte byteAddress = 0x10;
            byte byteMessage = 1;

            // Arrange
            IEeprom at24cEeprom = SetupHelper.Setup();

            // write
            bool write = at24cEeprom.WriteByte(byteAddress, byteMessage);
            Assert.IsTrue(write);

            // read
            byte result = at24cEeprom.ReadByte(byteAddress);
            Assert.AreEqual(byteMessage, result);
        }

        [TestMethod]
        public void ByteArrayTest()
        {
            byte byteArrayAddress = 0x80;
            byte[] byteArrayMessage = new byte[] { 1, 2, 3, 4 };

            // Arrange
            IEeprom at24cEeprom = SetupHelper.Setup();

            // write
            bool write = at24cEeprom.WriteByteArray(byteArrayAddress, byteArrayMessage);
            Assert.IsTrue(write);

            byte[] read = at24cEeprom.ReadByteArray(byteArrayAddress);

            Assert.AreEqual(byteArrayMessage.Length, read.Length, "Sizes");

            // Assert
            for (int i = 0; i < byteArrayMessage.Length; i++)
            {
                Assert.AreEqual(byteArrayMessage[i], read[i], $"index: {i}");
            }
        }
    }
}
