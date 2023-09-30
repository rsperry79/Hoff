using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;

using Hoff.Server.ApHelper.Ap;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging.Debug;
using nanoFramework.Runtime.Native;

namespace Hoff.Server.ApHelper
{
    internal class SoftApWebServer
    {
        private HttpListener listener;
        private readonly DebugLogger Logger;
        private Thread serverThread;
        private static bool wasConfigured;
        public SoftApWebServer(DebugLogger logger) => this.Logger = logger;
        public void Start()
        {

            if (this.listener == null)
            {
                this.listener = new HttpListener("http");
                this.serverThread = new Thread(this.RunServer);
                this.serverThread.Start();
            }
        }

        public void Stop()
        {
            this.listener?.Stop();
        }

        private void RunServer()
        {
            try
            {
                this.listener.Start();

                while (this.listener.IsListening)
                {
                    HttpListenerContext context = this.listener.GetContext();

                    if (context != null)
                    {
                        this.ProcessRequest(context);
                    }
                }

                this.listener.Close();

                this.listener = null;
            }
            catch (Exception ex)
            {
                this.Logger.LogCritical(ex, ex.Message);

            }
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            try
            {
                if (context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }

                HttpListenerRequest request = context.Request;
                this.Logger.LogInformation($"reqest methode: {request.HttpMethod}");

                HttpListenerResponse response = context.Response;

                this.Logger.LogInformation($"reqest url: {request.RawUrl}");

                string responseString;
                string ssid = null;
                string password = null;
                bool isApSet = false;

                string[] url = request?.RawUrl.Split('?');
                switch (request.HttpMethod)
                {
                    case "GET":
                        try
                        {
                            if (url[0] == "/favicon.ico")
                            {
                                response.ContentType = "image/png";
                                byte[] responseBytes = Resources.GetBytes(Resources.BinaryResources.favicon);
                                OutPutByteResponse(response, responseBytes);
                            }
                            else
                            {
                                response.ContentType = "text/html";
                                responseString = ReplaceMessage(Resources.GetString(Resources.StringResources.main), "", "");
                                OutPutResponse(response, responseString);
                            }
                        }
                        catch (Exception ex)
                        {
                            this.Logger.LogCritical(ex, ex.Message);
                            throw;
                        }

                        break;

                    case "POST":
                        try
                        {
                            if (url[0] == "/")
                            {
                                // Pick up POST parameters from Input Stream
                                Hashtable hashPars = ParseParamsFromStream(request.InputStream);

                                ssid = (string)hashPars["ssid"];
                                password = (string)hashPars["password"];

                                Debug.WriteLine($"Wireless parameters SSID:{ssid} PASSWORD:{password}");

                                string message = "<p>SSID can not be empty</p>";
                                if (ssid != null)
                                {
                                    if (ssid.Length >= 1)
                                    {
                                        message = "<p>New settings saved.</p><p>Rebooting device to put into normal mode</p>";

                                        //responseString = CreateMainPage(message);

                                        isApSet = true;
                                    }
                                }

                                responseString = ReplaceMessage(Resources.GetString(Resources.StringResources.main), message, ssid);
                            }
                            else
                            {
                                responseString = ReplaceMessage(Resources.GetString(Resources.StringResources.main), $"{DateTime.UtcNow.AddHours(8)}", "");
                            }

                            OutPutResponse(response, responseString);
                        }
                        catch (Exception ex)
                        {
                            this.Logger.LogCritical(ex, ex.Message);
                            throw;
                        }

                        break;
                }

                response.Close();

                if (isApSet && (!string.IsNullOrEmpty(ssid)) && (!string.IsNullOrEmpty(password)))
                {
                    wasConfigured = true;
                    // Enable the Wireless station interface
                    _ = Wireless80211.Configure(ssid, password);

                    // Disable the Soft AP
                    WirelessAP.Disable();
                    Thread.Sleep(200);
                    Power.RebootDevice();
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogCritical(ex, ex.Message);
                throw;
            }
        }
        public static class MyTags
        {
            public static string ssid = "{ssid}";
            public static string message = "{message}";
        }

        private static string ReplaceMessage(string page, string message, string ssid)
        {
            string retpage;
            int index = page.IndexOf(MyTags.ssid);
            retpage = index >= 0 ? page.Substring(0, index) + ssid + page.Substring(index + MyTags.ssid.Length) : page;

            index = retpage.IndexOf(MyTags.message);
            return index >= 0 ? retpage.Substring(0, index) + message + retpage.Substring(index + MyTags.message.Length) : retpage;
        }

        private static void OutPutResponse(HttpListenerResponse response, string responseString)
        {
            _ = System.Text.Encoding.UTF8.GetBytes(responseString);
            OutPutByteResponse(response, System.Text.Encoding.UTF8.GetBytes(responseString));
        }

        private static void OutPutByteResponse(HttpListenerResponse response, byte[] responseBytes)
        {
            response.ContentLength64 = responseBytes.Length;
            response.OutputStream.Write(responseBytes, 0, responseBytes.Length);

        }

        private static Hashtable ParseParamsFromStream(Stream inputStream)
        {
            byte[] buffer = new byte[inputStream.Length];
            _ = inputStream.Read(buffer, 0, (int)inputStream.Length);

            return ParseParams(System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length));
        }

        private static Hashtable ParseParams(string rawParams)
        {
            Hashtable hash = new();

            string[] parPairs = rawParams.Split('&');
            foreach (string pair in parPairs)
            {
                string[] nameValue = pair.Split('=');
                hash.Add(nameValue[0], nameValue[1]);
            }

            return hash;
        }
    }
}
