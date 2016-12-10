using System;
using System.Collections.Generic;
using Calibur.Model.IMessage;

namespace Calibur.Model
{
    public class SessionInfo
    {
        public int Id { get; set; }
        public SessionInfoTimers Timers { get; set; }
        public string Url { get; set; }
        public bool IsBufferResponse { get; set; }
        public string BitFlags { get; set; }
        public bool IsTunnel { get; set; }
        public string Tag { get; set; }
        public long TunnelEgressByteCount { get; set; }
        public long TunnelIngressByteCount { get; set; }
        public bool TunnelIsOpen { get; set; }
        public string State { get; set; }
        public string RequestBodyEncoding { get; set; }
        public string ResponseBodyEncoding { get; set; }
        public bool IsFTP { get; set; }
        public string Method { get; set; }
        public bool IsHTTPS { get; set; }
        public int LocalProcessID { get; set; }
        public string LocalProcess { get; set; }
        public string SuggestedFilename { get; set; }
        public bool BypassGateway { get; set; }
        public int ClientPort { get; set; }
        public string PathAndQuery { get; set; }
        public string FullUrl { get; set; }
        public string Host { get; set; }
        public string Hostname { get; set; }
        public int Port { get; set; }
        public string ClientIP { get; set; }
        public int ResponseCode { get; set; }
        public bool HasWebSocketMessages { get; set; }
        public bool HasResponse { get; set; }
        public object Return { get; set; }
        public string UriScheme { get; set; }
        public string UriUserInfo { get; set; }
        public string RequestPath { get; set; }
        public string GetHttpResponseStatus { get; set; }
        public string GetStatusDescription { get; set; }
        public string RequesetBody { get; set; }
        public Dictionary<string, string> UrlParam { get; set; }
        public Dictionary<string, object> ResponseHeaders { get; set; }
        public Dictionary<string,string> RequestHeaders { get; set; }
    }

    public class SessionInfoTimers
    {
        /// <summary>
        /// The time at which the client's HTTP connection to Fiddler was established
        /// 
        /// </summary>
        public DateTime ClientConnected;

        /// <summary>
        /// The time at which the request's first Send() to Fiddler completes
        /// 
        /// </summary>
        public DateTime ClientBeginRequest;

        /// <summary>
        /// The time at which the request headers were received
        /// 
        /// </summary>
        public DateTime FiddlerGotRequestHeaders;

        /// <summary>
        /// The time at which the request to Fiddler completes (aka RequestLastWrite)
        /// 
        /// </summary>
        public DateTime ClientDoneRequest;

        /// <summary>
        /// The time at which the server connection has been established
        /// 
        /// </summary>
        public DateTime ServerConnected;

        /// <summary>
        /// The time at which Fiddler begins sending the HTTP request to the server (FiddlerRequestFirstSend)
        /// 
        /// </summary>
        public DateTime FiddlerBeginRequest;

        /// <summary>
        /// The time at which Fiddler has completed sending the HTTP request to the server (FiddlerRequestLastSend).
        ///             BUG: Should be named "FiddlerEndRequest".
        ///             NOTE: Value here is often misleading due to buffering inside WinSock's send() call.
        /// 
        /// </summary>
        public DateTime ServerGotRequest;

        /// <summary>
        /// The time at which Fiddler receives the first byte of the server's response (ServerResponseFirstRead)
        /// 
        /// </summary>
        public DateTime ServerBeginResponse;

        /// <summary>
        /// The time at which Fiddler received the server's headers
        /// 
        /// </summary>
        public DateTime FiddlerGotResponseHeaders;

        /// <summary>
        /// The time at which Fiddler has completed receipt of the server's response (ServerResponseLastRead)
        /// 
        /// </summary>
        public DateTime ServerDoneResponse;

        /// <summary>
        /// The time at which Fiddler has begun sending the Response to the client (ClientResponseFirstSend)
        /// 
        /// </summary>
        public DateTime ClientBeginResponse;

        /// <summary>
        /// The time at which Fiddler has completed sending the Response to the client (ClientResponseLastSend)
        /// 
        /// </summary>
        public DateTime ClientDoneResponse;

        /// <summary>
        /// The number of milliseconds spent determining which gateway should be used to handle this request
        ///             (Should be mutually exclusive to DNSTime!=0)
        /// 
        /// </summary>
        public int GatewayDeterminationTime;

        /// <summary>
        /// The number of milliseconds spent waiting for DNS
        /// 
        /// </summary>
        public int DNSTime;

        /// <summary>
        /// The number of milliseconds spent waiting for the server TCP/IP connection establishment
        /// 
        /// </summary>
        public int TCPConnectTime;

        /// <summary>
        /// The number of milliseconds elapsed while performing the HTTPS handshake with the server
        /// 
        /// </summary>
        public int HTTPSHandshakeTime;

        public double Elapsed
        {
            get
            {
                var elaped = (ServerDoneResponse - ClientBeginRequest).TotalMilliseconds;
                return elaped > 0 ? elaped : TCPConnectTime;
            }
        }
    }
}
