using System.Net;
using System.Net.WebSockets;
using System.Net.WebSockets.Server;
using System.Net.WebSockets.WebSocketFrame;
using System.Text;

using Hoff.Server.ApHelper;
using Hoff.Server.Common.Helpers;
using Hoff.Server.Common.Models;
using Hoff.Server.Web.Helpers;

using Microsoft.Extensions.Logging;

using nanoFramework.Logging.Debug;
using nanoFramework.WebServer;

using static Hoff.Server.Web.Resources;

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

            //Initialize WebsocketServer with WebServer integration
            socketServer = new WebSocketServer(new WebSocketServerOptions()
            {
                MaxClients = 10,
                IsStandAlone = false,
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
            string[] url = request?.RawUrl.Split('?');

            HttpListenerResponse response = e.Context.Response;
            Logger.LogInformation($"response url: {request.RawUrl}");


            //check if this is a WebSocket request or a page request 
            if (request.Headers["Upgrade"] == "websocket")
            {
                //Upgrade to a WebSocket
                _ = socketServer.AddWebSocket(e.Context);
            }
            else
            {
                bool resultText = url[0] switch
                {
                    "/" => response.Send(StringResources.template.Inject(GetString(StringResources.main)), "text/html"),
                    "/settings" => response.Send(StringResources.template.Inject(GetString(StringResources.settings)), "text/html"),
                    "/favicon.ico" => response.Send(Resources.GetBytes(BinaryResources.favicon), "image/png"),
                    "/settings_code" => response.Send(Resources.GetString(StringResources.settings_code), "application/javascript"),
                    "/socket.js" => response.Send(Resources.GetString(StringResources.sockets), "application/javascript"),
                    "/core.css" => response.Send(Resources.GetString(StringResources.Core_css), "text/css"),
                    _ => response.Send(404, "Resource Not Found")
                };
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
            else if (e.Frame.MessageType == WebSocketMessageType.Text)
            {
                string raw = Encoding.UTF8.GetString(e.Frame.Buffer, 0, e.Frame.MessageLength);

                bool matched = GetByCommand(raw, wsServer, e.Frame.EndPoint);

                if (!matched)
                {
                    GetByEncoded(raw, wsServer);
                }
            }
        }

        private static bool GetByCommand(string raw, WebSocketServer ws, IPEndPoint endPoint)
        {
            bool matched = false;

            if (raw == "GetWifiSettings")
            {
                WifiSettings settings = ApConfig.WifiSettings;
                Logger.LogInformation(settings.IsConfigured.ToString());

                WsBaseMessage baseMessage = settings.ToWsEncodedBaseMessage();
                Logger.LogInformation($"{baseMessage.MessageType}");
                Logger.LogInformation($"{baseMessage.Message}");
                ws.SendText(endPoint.ToString(), baseMessage.ToEncodedMessage());
                matched = true;

                Logger.LogInformation(baseMessage.ToString());
            }

            return matched;
        }

        private static void GetByEncoded(string raw, WebSocketServer ws)
        {
            WsMessage EncodedMessage = new(raw);
            if (EncodedMessage.GetType() == typeof(WifiSettings))
            {
                WifiSettings settings = (WifiSettings)EncodedMessage.Decode();
                ApConfig.SetConfiguration(settings.SSID, settings.Password);
            }
        }
    }
}
