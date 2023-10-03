using System.Net;
using System.Text;

using nanoFramework.WebServer;

namespace Hoff.Server.Web.Helpers
{
    internal static class ExtentionMethods
    {



        internal static bool Send(this HttpListenerResponse response, string content, string type)
        {
            try
            {
                byte[] encoded = Encoding.UTF8.GetBytes(content);
                Send(response, encoded, type);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }


        internal static bool Send(this HttpListenerResponse response, byte[] bytes, string type)
        {
            try
            {
                response.ContentType = type;
                response.ContentLength64 = bytes.Length;
                response.OutputStream.Write(bytes, 0, bytes.Length);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }

        }

        internal static bool Send(this HttpListenerResponse response, int code, string text)
        {
            try
            {
                response.StatusCode = code;
                WebServer.OutPutStream(response, text);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }

        }


        internal static string Inject(this Resources.StringResources template, string content, string tag = "{content}")
        {
            // get the real string from the resources
            string realTemplate = Resources.GetString(template);

            // Get the place to inject
            int index = realTemplate.IndexOf(tag);

            // Split the template without the tag included.
            string start = realTemplate.Substring(0, index); // The template before the tag.
            string end = realTemplate.Substring(index + tag.Length); // The template after the tag.

            //inject 
            string injected = start + content + end;

            // Return the new string with the injection added.
            return injected;
        }
    }
}