using System;
using System.Collections;
using System.Text;

namespace Hoff.Core.Hardware.Common.Helpers
{
    public static class EepromExtentionHelpers
    {
        #region Public Methods

        public static byte[] ToByteArray(this Array list)
        {
            byte[] toRet = new byte[list.Length];

            for (int i = 0; i < list.Length; i++)
            {
                toRet[i] = (byte)list.GetValue(i);
                ;
            }

            return toRet;
        }

        public static byte[] ToBytes(this string message)
        {
            return Encoding.UTF8.GetBytes(message);
        }

        public static char[] ToCharArray(this ArrayList list)
        {
            char[] toRet = new char[list.Count];
            object[] src = list.ToArray();

            for (int i = 0; i < src.Length; i++)
            {
                toRet[i] = (char)src[i];
            }

            return toRet;
        }

        public static string ToString(this Array list)
        {
            string decodedMessage = Encoding.UTF8.GetString(list.ToByteArray(), 0, list.Length);
            return decodedMessage;
        }

        #endregion Public Methods
    }
}
