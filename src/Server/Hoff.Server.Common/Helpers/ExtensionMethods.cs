using System;
using System.Diagnostics;

using Hoff.Server.Common.Interfaces;
using Hoff.Server.Common.Models;

using nanoFramework.Json;

namespace Hoff.Server.Common.Helpers
{
    public static class ExtensionMethods
    {

        public static IWsMessage GetWsMessage(this string encoded)
        {
            try
            {
                Debug.WriteLine(encoded);
                WsMessage temp = (WsMessage)JsonConvert.DeserializeObject(encoded, typeof(WsMessage));
                return temp;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(encoded);
                Debug.WriteLine(ex.StackTrace);
                throw;
            }
        }

        public static object GetWsMessagePayload(this IWsMessage encoded)
        {
            object temp = JsonConvert.DeserializeObject(encoded.Message, Type.GetType(encoded.MessageType));
            return temp;
        }

        public static string ToJson(this WsMessage settings)
        {
            string toRet = JsonConvert.SerializeObject(settings);
            return toRet;
        }
    }
}
