using System.Net;
using System.Net.WebSockets;
using System.Net.WebSockets.Server;
using System.Net.WebSockets.WebSocketFrame;
using System.Text;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging.Debug;
using nanoFramework.WebServer;

namespace Hoff.Server.Web
{
    public class Server
    {
        private static DebugLogger Logger;
        private static WebSocketServer socketServer;
        private static WebServer webServer;

        public Server(DebugLogger logger)
        {
            Logger = logger;

            //Initialize WebsocketServer with Webserver intergration
            socketServer = new WebSocketServer(new WebSocketServerOptions()
            {
                MaxClients = 10,
                IsStandAlone = false
            });

            socketServer.MessageReceived += WsServer_MessageReceived;
            socketServer.Start();

            //WebServer
            webServer = new WebServer(80, HttpProtocol.Http);
            webServer.CommandReceived += WebServer_CommandReceived;
            _ = webServer.Start();

        }

        //WebServer receive message
        private static void WebServer_CommandReceived(object obj, WebServerEventArgs e)
        {

            HttpListenerRequest request = e.Context.Request;
            Logger.LogInformation($"Request method: {request.HttpMethod}");

            HttpListenerResponse response = e.Context.Response;
            Logger.LogInformation($"response url: {request.RawUrl}");

            string[] url = request?.RawUrl.Split('?');

            //check the path of the request
            if (request.RawUrl == "/")
            {
                //check if this is a WebSocket request or a page request 
                if (request.Headers["Upgrade"] == "websocket")
                {
                    //Upgrade to a WebSocket
                    _ = socketServer.AddWebSocket(e.Context);
                }
                else
                {
                    //Return the WebApp
                    response.ContentType = "text/html";
                    string responseString = Resources.GetString(Resources.StringResources.index);
                    OutPutResponse(response, responseString);
                }
            }
            else if (url[0] == "/favicon.ico")
            {
                response.ContentType = "image/png";
                byte[] responseBytes = Resources.GetBytes(Resources.BinaryResources.favicon);
                OutPutByteResponse(response, responseBytes);
            }

            else if (url[0] == "/core.js")
            {
                response.ContentType = "application/javascript";
                string responseString = Resources.GetString(Resources.StringResources.core);
                OutPutResponse(response, responseString);
            }
            else if (url[0] == "/core.js.map")
            {
                response.ContentType = "application/javascript";
                string responseString = Resources.GetString(Resources.StringResources.core_js);
                OutPutResponse(response, responseString);
            }

            else if (url[0] == "/core.css")
            {
                response.ContentType = "text/css";
                string responseString = Resources.GetString(Resources.StringResources.css_core);
                OutPutResponse(response, responseString);
            }
            else if (url[0] == "/core.css.map")
            {
                response.ContentType = "text/css";
                string responseString = Resources.GetString(Resources.StringResources.css_core_min);
                OutPutResponse(response, responseString);
            }
            else
            {
                //Send Page not Found
                response.StatusCode = 404;
                WebServer.OutPutStream(response, "Page not Found!");
            }
        }

        //WebSocket Server Receive message
        private static void WsServer_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            WebSocketServer wsServer = (WebSocketServer)sender;
            if (e.Frame.MessageType == WebSocketMessageType.Binary && e.Frame.MessageLength == 3)
            {
                wsServer.BroadCast(e.Frame.Buffer);
            }
        }

        private static void OutPutResponse(HttpListenerResponse response, string responseString)
        {
            byte[] encoded = Encoding.UTF8.GetBytes(responseString);
            OutPutByteResponse(response, encoded);
        }

        private static void OutPutByteResponse(HttpListenerResponse response, byte[] responseBytes)
        {
            response.ContentLength64 = responseBytes.Length;
            response.OutputStream.Write(responseBytes, 0, responseBytes.Length);

        }
    }
}
