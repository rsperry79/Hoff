

using System;
using System.Collections;
using System.Text;

namespace Hoff.Hardware.Common.Helpers
{
    public static class EepromExtentionHelpers
    {
        public static byte[] ToByteArray(this ArrayList list)
        {
            byte[] toRet = new byte[list.Count];
            object[] src = list.ToArray();

            for (int i = 0; i < src.Length; i++)
            {
                toRet[i] = (byte)src[i];
            }

            return toRet;
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

        public static string ToString(this ArrayList list)
        {
            string decodedMessage = Encoding.UTF8.GetString(list.ToByteArray(), 0, list.Count);
            return decodedMessage;
        }

        public static byte[] ToBytes(this string message)
        {
            return Encoding.UTF8.GetBytes(message);
        }

    }
}
