using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calibur.Model;
using Calibur.Model.IMessage;
using CEF.Lib;
using CEF.Lib.Helper;
using CEF.Lib.JavascriptObject;
using Fiddler;
using Fleck;
using Newtonsoft.Json;

namespace Calibur.Business
{
    public class SessionCore
    {
        private static SessionInfo GenerateSessionInfo(Session isession)
        {
            var session = new JSession(isession);
            var timers = new SessionInfoTimers
            {
                ClientConnected = isession.Timers.ClientConnected,
                ClientBeginRequest = isession.Timers.ClientBeginRequest,
                FiddlerGotRequestHeaders = isession.Timers.FiddlerGotRequestHeaders,
                ClientDoneRequest = isession.Timers.ClientDoneRequest,
                ServerConnected = isession.Timers.ServerConnected,
                FiddlerBeginRequest = isession.Timers.FiddlerBeginRequest,
                ServerGotRequest = isession.Timers.ServerGotRequest,
                ServerBeginResponse = isession.Timers.ServerBeginResponse,
                FiddlerGotResponseHeaders = isession.Timers.FiddlerGotResponseHeaders,
                ServerDoneResponse = isession.Timers.ServerDoneResponse,
                ClientBeginResponse = isession.Timers.ClientBeginResponse,
                ClientDoneResponse = isession.Timers.ClientDoneResponse,
                GatewayDeterminationTime = isession.Timers.GatewayDeterminationTime,
                DNSTime = isession.Timers.DNSTime,
                TCPConnectTime = isession.Timers.TCPConnectTime,
                HTTPSHandshakeTime = isession.Timers.HTTPSHandshakeTime
            };
            return new SessionInfo
            {
                Id = session.Id,
                Url = session.FullUrl,
                Timers = timers,
                IsBufferResponse = session.IsBufferResponse,
                Tag = session.Tag,
                Method = session.Method,
                IsHTTPS = session.IsHTTPS,
                LocalProcessID = session.LocalProcessID,
                LocalProcess = session.LocalProcess,
                SuggestedFilename = session.SuggestedFilename,
                BypassGateway = session.BypassGateway,
                ClientPort = session.ClientPort,
                PathAndQuery = session.PathAndQuery,
                FullUrl = session.FullUrl,
                Host = session.Host,
                Hostname = session.Hostname,
                Port = session.Port,
                ClientIP = session.ClientIP,
                ResponseCode = session.ResponseCode,
                HasWebSocketMessages = session.HasWebSocketMessages,
                HasResponse = session.HasResponse,
                BitFlags = session.BitFlags,
                IsTunnel = session.IsTunnel,
                TunnelIsOpen = session.TunnelIsOpen,
                TunnelIngressByteCount = session.TunnelIngressByteCount,
                TunnelEgressByteCount = session.TunnelEgressByteCount,
                IsFTP = session.IsFTP,
                RequestBodyEncoding = session.RequestBodyEncoding,
                ResponseBodyEncoding = session.ResponseBodyEncoding,
                State = session.State,
                UriScheme = session.UriScheme,
                Return = session.Return,
                UriUserInfo = session.UriUserInfo,
                RequestPath = session.RequestPath,
                GetHttpResponseStatus = session.GetHttpResponseStatus,
                GetStatusDescription = session.GetStatusDescription,
                UrlParam=session.UrlParam,
                RequestHeaders = session.RequestHeaders,
                ResponseHeaders = session.ResponseHeaders
            };
        }

        

        public static SessionHandlerResponse SessionHandler(SessionHandlerRequest request)
        {
            var socket = request.Head.Socket;
            var socketid = socket.ConnectionInfo.Id;
            var name = request.EventName.ToString();
            var id = name + "_" + socketid;
            var hasHandlerEvent = FiddlerHelper.EventHandlers.Keys.ToList().Exists(key => key.StartsWith(name));
            if (request.EventType == EventType.Add && !hasHandlerEvent)
            {
                SessionStateHandler handler = session => SendSessionStateHandler(name, request.EventName, socket, session);
                switch (request.EventName)
                {
                    case EventName.BeforeRequest:
                        FiddlerApplication.BeforeRequest += handler;
                        break;
                    case EventName.BeforeResponse:
                        FiddlerApplication.BeforeResponse += handler;
                        break;
                    case EventName.AfterSessionComplete:
                        FiddlerApplication.AfterSessionComplete += handler;
                        break;
                    case EventName.BeforeReturningError:
                        FiddlerApplication.BeforeReturningError += handler;
                        break;
                    case EventName.RequestHeadersAvailable:
                        FiddlerApplication.RequestHeadersAvailable += handler;
                        break;
                    case EventName.ResponseHeadersAvailable:
                        FiddlerApplication.ResponseHeadersAvailable += handler;
                        break;
                    default:
                        return null;
                }
                FiddlerHelper.EventHandlers.Add(id, handler);
            }
            else if (request.EventType == EventType.Remove && FiddlerHelper.EventHandlers.ContainsKey(id))
            {
                var handler = FiddlerHelper.EventHandlers[id];
                switch (request.EventName)
                {
                    case EventName.BeforeRequest:
                        FiddlerApplication.BeforeRequest -= handler;
                        break;
                    case EventName.BeforeResponse:
                        FiddlerApplication.BeforeResponse -= handler;
                        break;
                    case EventName.AfterSessionComplete:
                        FiddlerApplication.AfterSessionComplete -= handler;
                        break;
                    case EventName.BeforeReturningError:
                        FiddlerApplication.BeforeReturningError -= handler;
                        break;
                    case EventName.RequestHeadersAvailable:
                        FiddlerApplication.RequestHeadersAvailable -= handler;
                        break;
                    case EventName.ResponseHeadersAvailable:
                        FiddlerApplication.ResponseHeadersAvailable -= handler;
                        break;
                    default:
                        return null;
                }
                FiddlerHelper.EventHandlers.Remove(id);
            }
            return null;
        }

        public static void SendSessionStateHandler(string id, EventName eventName,IWebSocketConnection socket, Session session)
        {
            if (session.port == WebSocketHelper.WebSocketPort) return;
            var message = new MessageInfo
            {
                ID = id,
                Type = MessageMode.Event,
                Body = new InstanceResponse
                {
                    Result = GenerateSessionInfo(session)
                }
            };

            socket.Send(JsonConvert.SerializeObject(message, Common.TimeFormat));
            if (FiddlerHelper.IsPauseSession() && eventName != EventName.AfterSessionComplete)
            {
                session.Pause();
            }
        }
    }
}
