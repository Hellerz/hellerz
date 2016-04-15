using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fiddler;
using Newtonsoft.Json.Linq;

namespace CEF.Lib.JavascriptObject
{
    public partial class JSession : IWapperReturn
    {
        public object Return { get; set; }
        public object WapperReturn(object value)
        {
            Return = value;
            return this;
        }

        public bool IsBufferResponse
        {
            get { return this.InnerSession.bBufferResponse; }
        }

        public string BitFlags
        {
            get { return this.InnerSession.BitFlags.ToString(); }
        }

        public bool IsTunnel
        {
            get { return this.InnerSession.isTunnel; }
        }


        public string Tag
        {
            get { return this.InnerSession.Tag==null ?null: this.InnerSession.Tag.ToString(); }
        }


        public bool TunnelIsOpen
        {
            get { return this.InnerSession.TunnelIsOpen; }
        }

        public long TunnelIngressByteCount
        {
            get { return this.InnerSession.TunnelIngressByteCount; }
        }

        public long TunnelEgressByteCount
        {
            get { return this.InnerSession.TunnelEgressByteCount; }
        }


        public string Method
        {
            get { return this.InnerSession.RequestMethod; }
        }

        public SessionTimers Timers
        {
            get { return this.InnerSession.Timers; }
        }
        public bool IsHTTPS
        {
            get { return this.InnerSession.isHTTPS; }
        }
        public bool IsFTP
        {
            get { return this.InnerSession.isFTP; }
        }


        public int LocalProcessID
        {
            get { return this.InnerSession.LocalProcessID; }
        }

        public string LocalProcess
        {
            get { return this.InnerSession.LocalProcess; }
        }

        public string SuggestedFilename
        {
            get { return this.InnerSession.SuggestedFilename; }
        }

        public bool BypassGateway
        {
            get { return this.InnerSession.bypassGateway; }
        }

        public int ClientPort
        {
            get { return this.InnerSession.clientPort; }
        }

        public string PathAndQuery
        {
            get { return this.InnerSession.PathAndQuery; }
        }

        public string FullUrl
        {
            get { return this.InnerSession.fullUrl; }
        }

        public string Url
        {
            get { return this.InnerSession.url; }
        }

        public string Host
        {
            get { return this.InnerSession.host; }
        }

        public string Hostname
        {
            get { return this.InnerSession.hostname; }
        }
        public int Port
        {
            get { return this.InnerSession.port; }
        }

        public int Id
        {
            get { return this.InnerSession.id; }
        }
        public string ClientIP
        {
            get { return this.InnerSession.clientIP; }
        }

        public int ResponseCode
        {
            get { return this.InnerSession.responseCode; }
        }
        public bool HasWebSocketMessages
        {
            get
            {
                try
                {
                    return this.InnerSession.bHasWebSocketMessages;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool HasResponse
        {
            get { return this.InnerSession.bHasResponse; }
        }

        public string RequestBodyEncoding
        {
            get { return this.InnerSession.GetRequestBodyEncoding().ToString(); }
        }

        public string ResponseBodyEncoding
        {
            get { return this.InnerSession.GetResponseBodyEncoding().ToString(); }
        }
        public string RedirectTargetUrl
        {
            get { return this.InnerSession.GetRedirectTargetURL(); }
        }

        public string State
        {
            get { return this.InnerSession.state.ToString(); }
        }


        public string UriScheme
        {
            get { return this.InnerSession.RequestHeaders.UriScheme; }
        }

        public string UriUserInfo
        {
            get { return this.InnerSession.RequestHeaders.UriUserInfo; }
        }

        public string RequestPath
        {
            get { return this.InnerSession.RequestHeaders.RequestPath; }
        }

        public Dictionary<string, string> RequestHeaders
        {
            get
            {
                return this.InnerSession.RequestHeaders.ToDiffDictionary(header => header.Name, header => header.Value, true);
            }
        }

        public string GetHttpResponseStatus
        {
            get { return this.InnerSession.ResponseHeaders.HTTPResponseStatus; }
        }

        public string GetStatusDescription
        {
            get { return this.InnerSession.ResponseHeaders.StatusDescription; }
        }

        private static readonly Regex UrlParamRegex = new Regex(@"(\?|&)(?<key>[^&?=]+?)=(?<value>[^&=]+)", RegexOptions.Compiled); 
        public Dictionary<string, string> UrlParam
        {
            get
            {
                var mapping = new Dictionary<string,string>();
                var match = UrlParamRegex.Match(PathAndQuery);
                while (match.Success)
                {
                    var key = match.Groups["key"].Value;
                    var value = match.Groups["value"].Value;
                    if (mapping.ContainsKey(key))
                    {
                        mapping[key] = value;
                    }
                    else
                    {
                        mapping.Add(key,value);
                    }
                    match = match.NextMatch();
                }
                return mapping;
            }
        }
        
        public Dictionary<string, object> ResponseHeaders
        {
            get
            {
                return this.InnerSession.ResponseHeaders.ToDiffDictionary(header => header.Name, header => (object)header.Value, true);
            }
        }

    }

   
}
