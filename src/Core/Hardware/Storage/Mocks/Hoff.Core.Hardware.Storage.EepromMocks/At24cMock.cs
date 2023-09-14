using System;
using System.Collections;

namespace Hoff.Core.Hardware.Storage.EepromMocks
{
    /// <summary>
    ///  This class mocks the At24c256
    /// </summary>
    public class At24cMock : IDisposable
    {
        #region Fields

        public const byte DefaultI2cAddress = 80;
        public static bool LoadDefaults = true;

        private static readonly ArrayList mockDevice = new ArrayList();
        private static int currentPosition = 0;

        #endregion Fields

        #region Properties

        public int PageCount => 512;

        public int PageSize => 64;

        public int Size => this.PageCount * this.PageSize;

        #endregion Properties

        #region Public Methods

        public void Dispose()
        {
            // throw new NotImplementedException();
        }

        public byte[] Read(int address, int length)
        {
            byte[] toRet = new byte[length];

            for (int i = 0; i < length; i++)
            {
                toRet[address + i] = (byte)mockDevice[address + i];
            }

            return toRet;
        }

        public byte ReadByte()
        {
            if (currentPosition == this.Size)
            {
                currentPosition = 0;
            }

            return (byte)mockDevice[currentPosition];
        }

        public byte ReadByte(int address)
        {
            return (byte)mockDevice[address];
        }

        public uint Write(int address, byte[] data)
        {
            int index = 0;
            for (int i = 0; i < data.Length; i++)
            {
                mockDevice[address + index] = data[address + index];
            }

            return (uint)data.Length;
        }

        public uint WriteByte(int address, byte value)
        {
            mockDevice[address] = value;
            return 1;
        }

        #endregion Public Methods
    }
}
