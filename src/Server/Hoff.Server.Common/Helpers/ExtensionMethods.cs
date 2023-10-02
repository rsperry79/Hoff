using System.Diagnostics;

using Hoff.Server.Common.Models;

using nanoFramework.Json;

namespace Hoff.Server.Common.Helpers
{
    public static class ExtensionMethods
    {
        public static WsBaseMessage ToWsEncodedBaseMessage(this WifiSettings settings)
        {
            string json = JsonConvert.SerializeObject(settings);
            return new WsBaseMessage(settings.GetType().ToString(), json);
        }

        public static string ToEncodedMessage(this WsBaseMessage settings)
        {
            Debug.WriteLine(settings.Message);
            Debug.WriteLine(settings.MessageType);
            string toRet = JsonConvert.SerializeObject(settings);
            return toRet;
        }
    }
}
