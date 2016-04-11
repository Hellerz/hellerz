using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using CEF.Lib.Attributes;
using CEF.Lib.Helper;
using Fiddler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CEF.Lib.JavascriptObject
{    public partial class JSession
    {
        private Session InnerSession { get; set; }


        public JSession(Session session)
        {
            InnerSession = session;
        }
        [JSchema]
        public JSession(int sessionId)
        {
            SessionHelper.OpSession((maping, id) =>
            {
                InnerSession = maping.FirstOrDefault(session => sessionId == session.Value.id).Value;
            },sessionId);
        }

        [CodeDescription("Sets Should response be buffered for tampering")]        [JSchema]
        public void SetIsBufferResponse(bool isBuffer)
        {
            this.InnerSession.bBufferResponse = isBuffer;
        }

        
        [CodeDescription("Sets A common use for the Tag property is to store data that is closely associated with the Session. It is NOT marshalled during drag/drop and is NOT serialized to a SAZ file.")]        [JSchema]
        public void SetTag(string tag)
        {
            this.InnerSession.Tag = tag;
        }

        
        [CodeDescription("Sets the request's Method (e.g. GET, POST, etc).")]        [JSchema]
        public void SetRequestMethod(string requestMethod)
        {
            this.InnerSession.RequestMethod = requestMethod;
        }

        
        [CodeDescription("Set to true in OnBeforeRequest if this request should bypass the gateway")]        [JSchema]
        public void SetBypassGateway(bool bypassGateway)
        {
            this.InnerSession.bypassGateway = bypassGateway;
        }


        [CodeDescription("Sets the path and query part of the URL. (For a CONNECT request, returns the host:port to be connected.)")]        [JSchema]
        public void SetPathAndQuery(string pathAndQuery)
        {
            this.InnerSession.PathAndQuery = pathAndQuery;
        }

        [CodeDescription("Sets the complete URI, including protocol/scheme, in the form http://www.host.com/filepath?query.")]        [JSchema]
        public void SetfullUrl(string fullUrl)
        {
            this.InnerSession.fullUrl = fullUrl;
        }


        
        [CodeDescription("Sets the URL (without protocol) being requested from the server, in the form www.host.com/filepath?query.")]        [JSchema]
        public void Seturl(string url)
        {
            this.InnerSession.url = url;
        }

        
        [CodeDescription("Sets the host to which this request is targeted. MAY include IPv6 literal brackets. MAY include a trailing port#.")]        [JSchema]
        public void Sethost(string host)
        {
            this.InnerSession.host = host;
        }

        
   
        
        [CodeDescription("Sets the hostname to which this request is targeted; does NOT include any port# but will include IPv6-literal brackets for IPv6 literals.")]        [JSchema]
        public void Sethostname(string hostname)
        {
            this.InnerSession.hostname = hostname;
        }

        
     
        
        [CodeDescription("Sets the server port to which this request is targeted.")]        [JSchema]
        public void Setport(int port)
        {
            this.InnerSession.port = port;
        }

       
        
        [CodeDescription("Sets the HTTP Status code of the server's response")]        [JSchema]
        public void SetresponseCode(int responseCode)
        {
            this.InnerSession.responseCode = responseCode;
        }

        
        [CodeDescription("Replace HTTP request headers and body using the specified file.")]        [JSchema]
        public bool LoadRequestBodyFromFile(string sFilename)
        {
            return this.InnerSession.LoadRequestBodyFromFile(sFilename);
        }

        
        [CodeDescription("Replace HTTP response headers and body using the specified file.")]        [JSchema]
        public bool LoadResponseFromFile(string sFilename)
        {
            return this.InnerSession.LoadResponseFromFile(sFilename);
        }

        
        [CodeDescription("Return a string generated from the request body, decoding it and converting from a codepage if needed. Possibly expensive due to decompression and will throw on malformed content.")]        [JSchema]
        public string GetRequestBodyAsString()
        {
            return this.InnerSession.GetRequestBodyAsString();
        }

        
        [CodeDescription("Return a string generated from the response body, decoding it and converting from a codepage if needed. Possibly expensive due to decompression and will throw on malformed content.")]        [JSchema]
        public string GetResponseBodyAsString()
        {
            return this.InnerSession.GetResponseBodyAsString();
        }

        
        [CodeDescription("Return a string md5, sha1, or sha256 of an unchunked and decompressed copy of the response body.")]        [JSchema]
        public string GetResponseBodyHash(string sHashAlg)
        {
            return this.InnerSession.GetResponseBodyHash(sHashAlg);
        }

        [CodeDescription("Returns true if request URI contains the specified string. Case-insensitive.")]        [JSchema]
        public bool UriContains(string sLookfor)
        {
            return this.InnerSession.uriContains(sLookfor);
        }

        
        [CodeDescription("Removes chunking and HTTP Compression from the response. Adds or updates Content-Length header.")]        [JSchema]
        public bool UtilDecodeResponse()
        {
            return this.InnerSession.utilDecodeResponse();
        }

        
        [CodeDescription("Removes chunking and HTTP Compression from the response. Adds or updates Content-Length header.\r\nbSilent is TRUE if error messages should be suppressed. False otherwise.")]        [JSchema]
        public bool UtilDecodeResponseSilent()
        {
            return this.InnerSession.utilDecodeResponse(true);
        }

        
        [CodeDescription("Removes chunking and HTTP Compression from the Request. Adds or updates Content-Length header.")]        [JSchema]
        public bool UtilDecodeRequest()
        {
            return this.InnerSession.utilDecodeRequest();
        }

        
        [CodeDescription("Removes chunking and HTTP Compression from the Request. Adds or updates Content-Length header.\r\nbSilent is TRUE if error messages should be suppressed. False otherwise.")]        [JSchema]
        public bool UtilDecodeRequestSilent()
        {
            return this.InnerSession.utilDecodeRequest(true);
        }

        
        [CodeDescription("Use GZIP to compress the request body. Throws exceptions to caller.")]        [JSchema]
        public bool UtilGZIPRequest()
        {
            return this.InnerSession.utilGZIPRequest();
        }

        
        [CodeDescription("Use GZIP to compress the response body. Throws exceptions to caller.")]        [JSchema]
        public bool UtilGZIPResponse()
        {
            return this.InnerSession.utilGZIPResponse();
        }

        
        [CodeDescription("Use DEFLATE to compress the response body. Throws exceptions to caller.")]        [JSchema]
        public bool UtilDeflateResponse()
        {
            return this.InnerSession.utilDeflateResponse();
        }

        
        [CodeDescription("Use BZIP2 to compress the response body. Throws exceptions to caller.")]        [JSchema]
        public bool UtilBZIP2Response()
        {
            return this.InnerSession.utilBZIP2Response();
        }

        
        [CodeDescription("Apply Transfer-Encoding: chunked to the response, if possible.\r\niSuggestedChunkCount :The number of chunks to try to create")]        [JSchema]
        public bool UtilChunkResponse(int iSuggestedChunkCount)
        {
            return this.InnerSession.utilChunkResponse(iSuggestedChunkCount);
        }

        
        [CodeDescription("Perform a case-sensitive string replacement on the request body (not URL!). Updates Content-Length header. Returns TRUE if replacements occur.")]        [JSchema]
        public bool UtilReplaceInRequest(string sSearchFor, string sReplaceWith)
        {
            return this.InnerSession.utilReplaceInRequest(sSearchFor, sReplaceWith);
        }

        
        [CodeDescription("Call inside OnBeforeRequest to create a Response object and bypass the server.")]        [JSchema]
        public void UtilCreateResponseAndBypassServer()
        {
            this.InnerSession.utilCreateResponseAndBypassServer();
        }

        
        [CodeDescription("Perform a regex-based replacement on the response body. Specify RegEx Options via leading Inline Flags, e.g. (?im) for case-Insensitive Multi-line. Updates Content-Length header. Note, you should call utilDecodeResponse first!  Returns TRUE if replacements occur.")]        [JSchema]
        public bool UtilReplaceRegexInResponse(string sSearchForRegEx, string sReplaceWithExpression)
        {
            return this.InnerSession.utilReplaceRegexInResponse(sSearchForRegEx, sReplaceWithExpression);
        }

        
        [CodeDescription("Perform a case-sensitive string replacement on the response body. Updates Content-Length header. Note, you should call utilDecodeResponse first!  Returns TRUE if replacements occur.\r\nsSearchFor:String to find (case-sensitive)\r\nsReplaceWith:String to use to replace")]        [JSchema]
        public bool UtilReplaceInResponse(string sSearchFor, string sReplaceWith)
        {
            return this.InnerSession.utilReplaceInResponse(sSearchFor, sReplaceWith);
        }

        
        [CodeDescription("Perform a single case-sensitive string replacement on the response body. Updates Content-Length header. Note, you should call utilDecodeResponse first! Returns TRUE if replacements occur.\r\nsSearchFor:String to find (case-sensitive)\r\nsReplaceWith:String to use to replace\r\nbCaseSensitive:TRUE for Case-Sensitive")]        [JSchema]
        public bool UtilReplaceOnceInResponse(string sSearchFor, string sReplaceWith,
            bool bCaseSensitive)
        {
            return this.InnerSession.utilReplaceOnceInResponse(sSearchFor, sReplaceWith, bCaseSensitive);
        }

        
        [CodeDescription("Replaces the request body with sString as UTF8. Sets Content-Length header & removes Transfer-Encoding/Content-Encoding.\r\nsString:The desired request Body as a string")]        [JSchema]
        public void UtilSetRequestBody(string sString)
        {
            this.InnerSession.utilSetRequestBody(sString);
        }

        
        [CodeDescription("Replaces the response body with sString. Sets Content-Length header & removes Transfer-Encoding/Content-Encoding")]        [JSchema]
        public void UtilSetResponseBody(string sString)
        {
            this.InnerSession.utilSetResponseBody(sString);
        }

        
        [CodeDescription("Copy an existing response to this Session, bypassing the server if not already contacted")]        [JSchema]
        public void UtilAssignResponse(string header, string body)
        {
            var requestHeaders  = new HTTPResponseHeaders();
            requestHeaders.AssignFromString(header);
            this.InnerSession.utilAssignResponse(requestHeaders, this.InnerSession.GetResponseBodyEncoding().GetBytes(body));
        }

        
        [CodeDescription("Prepend a string to the response body. Updates Content-Length header. Note, you should call utilDecodeResponse first!")]        [JSchema]
        public void UtilPrependToResponseBody(string sString)
        {
            this.InnerSession.utilPrependToResponseBody(sString);
        }

        
        [CodeDescription("Find a string in the request body. Return its index or -1.")]        [JSchema]
        public int UtilFindInRequest(string sSearchFor, bool bCaseSensitive)
        {
            return this.InnerSession.utilFindInRequest(sSearchFor, bCaseSensitive);
        }

        
        [CodeDescription("Find a string in the response body. Return its index or -1. Note, you should call utilDecodeResponse first!")]        [JSchema]
        public int UtilFindInResponse(string sSearchFor, bool bCaseSensitive)
        {
            return this.InnerSession.utilFindInResponse(sSearchFor, bCaseSensitive);
        }

        
        [CodeDescription("Returns TRUE if the Session's HTTP Method is available and matches the target method.")]        [JSchema]
        public bool HTTPMethodIs(string sTestFor)
        {
            return this.InnerSession.HTTPMethodIs(sTestFor);
        }

        
        [CodeDescription("Returns TRUE if the Session's target hostname (no port) matches sTestHost (case-insensitively).")]        [JSchema]
        public bool HostnameIs(string sTestHost)
        {
            return this.InnerSession.HostnameIs(sTestHost);
        }

        
        [CodeDescription("Returns HTML representing the Session. Call Utilities.StringToCF_HTML on the result of this function before placing it on the clipboard.")]        [JSchema]
        public string ToHTMLFragment(bool HeadersOnly)
        {
            return this.InnerSession.ToHTMLFragment(HeadersOnly);
        }

        
        [CodeDescription("Store this session's request and response to a string.")]        [JSchema]
        public string ToStringHeadersOnly()
        {
            return this.InnerSession.ToString(true);
        }

        
        [CodeDescription("Store this session's request and response to a string.")]        [JSchema]
        public override string ToString()
        {
            return this.InnerSession.ToString();
        }


        [CodeDescription("This method resumes the Session's thread in response to 'Continue' commands from the UI")]        [JSchema]
        public void Resume()
        {
            this.InnerSession.Resume();
        }

        [CodeDescription("This method pauses the Session's thread in response to 'Pause' commands from the UI")]        [JSchema]
        public void Pause()
        {
            this.InnerSession.Pause();
        }

        [CodeDescription("Set the SessionFlags.Ignore bit for this Session, also configuring it to stream, drop read data, and bypass event handlers. For a CONNECT Tunnel, traffic will be blindly shuffled back and forth. Session will be hidden.")]        [JSchema]
        public void Ignore()
        {
            this.InnerSession.Ignore();
        }

        
        [CodeDescription("Call this function while in the 'reading response' state to update the responseBodyBytes array with the partially read response.")]        [JSchema]
        public bool COMETPeek()
        {
            return this.InnerSession.COMETPeek();
        }

        
        [CodeDescription("Prevents the server pipe from this session from being pooled for reuse")]        [JSchema]
        public void PoisonServerPipe()
        {
            this.InnerSession.PoisonServerPipe();
        }

        
        [CodeDescription("Ensures that, after the response is complete, the client socket is closed and not reused. Does NOT (and must not) close the pipe.")]        [JSchema]
        public void PoisonClientPipe()
        {
            this.InnerSession.PoisonClientPipe();
        }

        
        [CodeDescription("Closes both client and server pipes and moves state to Aborted; unpauses thread if paused.")]        [JSchema]
        public void Abort()
        {
            this.InnerSession.Abort();
        }
        
        [CodeDescription("Save HTTP response body to Fiddler Captures folder.")]        [JSchema]
        public bool SaveResponseBody()
        {
            return this.InnerSession.SaveResponseBody();
        }

        
        [CodeDescription("Save HTTP response body to specified location.")]        [JSchema]
        public bool SaveResponseBodyCustom(string sFilename)
        {
            return this.InnerSession.SaveResponseBody(sFilename);
        }

        
        [CodeDescription("Save HTTP request body to specified location.")]        [JSchema]
        public bool SaveRequestBody(string sFilename)
        {
            return this.InnerSession.SaveRequestBody(sFilename);
        }

        
        [CodeDescription("Save the request and response to a single file.")]        [JSchema]
        public void SaveSession(string sFilename, bool bHeadersOnly)
        {
            this.InnerSession.SaveSession(sFilename, bHeadersOnly);
        }

        
        [CodeDescription("Save the request to a file. The headers' Request Line will not contain the scheme or host, which is probably not what you want.")]
        [JSchema]
        public void SaveRequest(string sFilename, bool bHeadersOnly)
        {
            this.InnerSession.SaveRequest(sFilename, bHeadersOnly);
        }

        [JSchema]
        public string GetRequest()
        {
            return RequestHeadersToString3(true, true, true) + GetRequestBodyAsString();
        }

        [JSchema]
        public string GetResponse()
        {
            return ResponseHeadersToString2(true, true) + GetResponseBodyAsString();
        }

        
        [CodeDescription("Save the request to a file. Throws if file cannot be written.")]        [JSchema]
        public void SaveRequestInclude(string sFilename, bool bHeadersOnly, bool bIncludeSchemeAndHostInPath)
        {
            this.InnerSession.SaveRequest(sFilename, bHeadersOnly, bIncludeSchemeAndHostInPath);
        }

        
        [CodeDescription("null, or Target URL. Note, you may want to call Utilities.TrimAfter(sTarget, '#');")]        [JSchema]
        public string GetRedirectTargetURL(string sBase, string sLocation)
        {
            return Session.GetRedirectTargetURL(sBase, sLocation);
        }

        
        [CodeDescription("Writes this session's metadata to a file.the metadata should be saved in XML format.")]        [JSchema]
        public bool SaveMetadata(string sFilename)
        {
            return this.InnerSession.SaveMetadata(sFilename);
        }

        
        [CodeDescription("Saves the response (headers and body) to a file")]        [JSchema]
        public void SaveResponse(string sFilename, bool bHeadersOnly)
        {
            this.InnerSession.SaveResponse(sFilename, bHeadersOnly);
        }

        [CodeDescription("Enumerated state of the current session.")]        [JSchema]
        public void SetState(string state)
        {
            this.InnerSession.state = (SessionStates)Enum.Parse(typeof(SessionStates), state);
        }

        #region HttpHeaders
        


        [CodeDescription("Bypass this session and get a response by proxy of socksv5 ")]        [JSchema]
        public void ProxySelfSocksV5(string proxyServer, bool bypassOnlocal, string bypasss)
        {
            this.InnerSession.bypassGateway = true;
            this.InnerSession.utilCreateResponseAndBypassServer();
            var request = (HttpWebRequest)WebRequest.Create(this.InnerSession.fullUrl);
            request.Proxy = new WebProxy(proxyServer, bypassOnlocal, bypasss.Split(';'));
            request.Method = this.InnerSession.RequestMethod;
            BuildHeader(request, InnerSession.GetRequestBodyAsString());
            using (var stream = request.GetRequestStream())
            {
                stream.Write(InnerSession.requestBodyBytes, 0, InnerSession.requestBodyBytes.Length);
            }
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response != null)
                {
                    var header = response.Headers.ToString();
                    var firstLine = string.Format("HTTP/{0} {1} {2}\r\n", response.ProtocolVersion, (int)response.StatusCode, response.StatusDescription);
                    InnerSession.ResponseHeaders.AssignFromString(firstLine + header);
                    var stream = response.GetResponseStream();
                    if (stream != null && stream.CanRead)
                    {
                        using (var reader = new StreamReader(stream, this.InnerSession.GetRequestBodyEncoding()??Encoding.UTF8))
                        {
                            this.InnerSession.utilSetResponseBody(reader.ReadToEnd());
                        }
                    }
                }
            }
        }

        
        [CodeDescription("Re-Send a message by proxy of socksv5 ")]        [JSchema]
        public string ProxySocksV5(string proxyServer, bool bypassOnlocal, string bypasss, string headers, string body)
        {
            var request = (HttpWebRequest)WebRequest.Create(this.InnerSession.fullUrl);
            request.Proxy = new WebProxy(proxyServer, bypassOnlocal, bypasss.Split(';'));
            BuildHeader(request, headers);
            var bytes = (this.InnerSession.GetRequestBodyEncoding() ?? Encoding.UTF8).GetBytes(body);
            using (var stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }
            return "";
        }

        private static MethodInfo _setSpecialHeaders;
        private static readonly Regex EachLine = new Regex(@"(?<name>.+?): +(?<value>.*)", RegexOptions.Compiled);
        private static void BuildHeader(HttpWebRequest request, string headers)
        {
            if (_setSpecialHeaders != null)
            {
                _setSpecialHeaders = typeof(HttpWebRequest).GetMethod("SetSpecialHeaders", BindingFlags.Instance | BindingFlags.NonPublic);
            }
            if (!string.IsNullOrWhiteSpace(headers) && _setSpecialHeaders != null)
            {
                var match = EachLine.Match(headers);
                while (match.Success)
                {
                    var name = match.Groups["name"].Value;
                    var value = match.Groups["value"].Value;
                    _setSpecialHeaders.Invoke(request, new object[] { name, value });
                    match = match.NextMatch();
                }
            }
        }
        [JSchema]
        public string GetRequestBodyAsBase64()
        {
            return Convert.ToBase64String(this.InnerSession.RequestBody);
        }        [JSchema]
        public string GetResponseBodyAsBase64()
        {
            return Convert.ToBase64String(this.InnerSession.ResponseBody);
        }

        #region HttpRequsetHeaders
        [JSchema]
        public void SetRequestHeaders(string pairJson)
        {
            var headers = JObject.Parse(pairJson);
            foreach (var header in headers)
            {
                this.InnerSession.RequestHeaders[header.Key] = header.Value.ToString();
            }
        }

        [JSchema]
        public void SetUriScheme(string uriScheme)
        {
            this.InnerSession.RequestHeaders.UriScheme = uriScheme;
        }
        [JSchema]
        public string SetRequestPath(string path)
        {
            return this.InnerSession.RequestHeaders.RequestPath = path;
        }
        [JSchema]
        public string RequestHeadersToString3(bool prependVerbLine, bool appendEmptyLine, bool includeProtocolAndHostInPath)
        {
            return this.InnerSession.RequestHeaders.ToString(prependVerbLine, appendEmptyLine, includeProtocolAndHostInPath);
        }
        [JSchema]
        public string RequestHeadersToString2(bool prependVerbLine, bool appendEmptyLine)
        {
            return this.InnerSession.RequestHeaders.ToString(prependVerbLine, appendEmptyLine);
        }
        [JSchema]
        public string RequestHeadersToString()
        {
            return this.InnerSession.RequestHeaders.ToString();
        }
        [JSchema]
        public bool AssignRequestHeadersFromString(string sHeaders)
        {
            return this.InnerSession.RequestHeaders.AssignFromString(sHeaders);
        }

        private string GetContentType(HTTPHeaders headers)
        {
            if (headers != null&&headers["Content-Type"]!=null)
            {
                if( headers["Content-Type"].ToLower() == "application/json")
                {
                    return "JSON";
                }
                if( headers["Content-Type"].ToLower() == "application/xml")
                {
                    return "XML";
                }
            }
            return string.Empty;
        }
        [JSchema]
        public string RequestXml2Json()
        {
            var body = this.InnerSession.GetRequestBodyAsString();
            if (GetContentType(this.InnerSession.RequestHeaders) == "XML")
            {
                var doc = new XmlDocument();
                doc.LoadXml(body);
                return JsonConvert.SerializeXmlNode(doc);
            }
            return body;
        }

        [JSchema]
        public string ResponseXml2Json()
        {
            var body = this.InnerSession.GetResponseBodyAsString();
            if (GetContentType(this.InnerSession.ResponseHeaders) == "XML")
            {
                var doc = new XmlDocument();
                doc.LoadXml(this.InnerSession.GetResponseBodyAsString());
                return JsonConvert.SerializeXmlNode(doc);
            }
            return body;
        }

        [JSchema]
        public void SetRequestJson2Xml(string json)
        {
            if (GetContentType(this.InnerSession.ResponseHeaders) == "XML")
            {
                var doc = JsonConvert.DeserializeXmlNode(json);
                json = doc.ToString();
            }
            this.UtilSetRequestBody(json);
        }

        [JSchema]
        public void SetResponseJson2Xml(string json)
        {
            if (GetContentType(this.InnerSession.ResponseHeaders) == "XML")
            {
                var doc = JsonConvert.DeserializeXmlNode(json);
                json = doc.ToString();
            }
            this.UtilSetRequestBody(json);
        }

        #endregion

        #region HttpResponseHeaders
        [JSchema]
        public void SetResponseHeaders(string pairJson)
        {
            var headers = JObject.Parse(pairJson);
            foreach (var header in headers)
            {
                this.InnerSession.ResponseHeaders[header.Key] = header.Value.ToString();
            }
        }
        [JSchema]
        public bool AssignResponseHeadersFromString(string sHeaders)
        {
            return this.InnerSession.ResponseHeaders.AssignFromString(sHeaders);
        }
        [JSchema]
        public string ResponseHeadersToString2(bool prependVerbLine, bool appendEmptyLine)
        {
            return this.InnerSession.ResponseHeaders.ToString(prependVerbLine, appendEmptyLine);
        }
        [JSchema]
        public string ResponseHeadersToString()
        {
            return this.InnerSession.ResponseHeaders.ToString();
        }
        [JSchema]
        public void SetHttpResponseStatus(string status)
        {
            this.InnerSession.ResponseHeaders.HTTPResponseStatus = status;
        }
        [JSchema]
        public void SetStatusDescription(string description)
        {
            this.InnerSession.ResponseHeaders.StatusDescription = description;
        }
        [JSchema]
        public void SetStatus(int iCode, string sDescription)
        {
            this.InnerSession.ResponseHeaders.SetStatus(iCode, sDescription);
        }
        #endregion

        //
        //public void SetRequestBody(byte[] requestBody)
        //{
        //    this.InnerSession.RequestBody = requestBody;
        //}
        //
        //
        //public void SetResponseBody(byte[] responseBody)
        //{
        //    this.InnerSession.ResponseBody = responseBody;
        //}

        //#endregion

        //#region Session
        //
        //public void UtilAssignResponse(Session oFromSession)
        //{
        //    this.InnerSession.utilAssignResponse(oFromSession);
        //}
        //
        //public Session BuildFromData(bool bClone, HTTPRequestHeaders headersRequest, byte[] arrRequestBody,
        //    HTTPResponseHeaders headersResponse, byte[] arrResponseBody, SessionFlags oSF)
        //{
        //    return Session.BuildFromData(bClone, headersRequest, arrRequestBody, headersResponse,
        //        arrResponseBody, oSF);
        //}

        #endregion

        //
        //public void add_OnStateChanged(System.EventHandler `1[StateChangeEventArgs] value)
        //{
        //    this.InnerSession.add_OnStateChanged(value);
        //}

        //
        //public void remove_OnStateChanged(System.EventHandler `1[StateChangeEventArgs] value)
        //{
        //    this.InnerSession.remove_OnStateChanged(value);
        //}

        //
        //public void add_OnContinueTransaction(System.EventHandler `1[ContinueTransactionEventArgs] value)
        //{
        //    this.InnerSession.add_OnContinueTransaction(value);
        //}

        //
        //public void remove_OnContinueTransaction(System.EventHandler `1[ContinueTransactionEventArgs] value)
        //{
        //    this.InnerSession.remove_OnContinueTransaction(value);
        //}

        //
        //public void add_OnCompleteTransaction(System.EventHandler `1[System.EventArgs] value)
        //{
        //    this.InnerSession.add_OnCompleteTransaction(value);
        //}

        //
        //public void remove_OnCompleteTransaction(System.EventHandler `1[System.EventArgs] value)
        //{
        //    this.InnerSession.remove_OnCompleteTransaction(value);
        //}



        //
        //public bool LoadMetadata(Stream strmMetadata)
        //{
        //    return this.InnerSession.LoadMetadata(strmMetadata);
        //}
        //
        //public bool WriteRequestToStream(bool bHeadersOnly, bool bIncludeProtocolAndHostWithPath,
        //    bool bEncodeIfBinary, Stream oFs)
        //{
        //    return this.InnerSession.WriteRequestToStream(bHeadersOnly, bIncludeProtocolAndHostWithPath, bEncodeIfBinary,
        //        oFs);
        //}

        //
        //public bool WriteResponseToStream(Stream oFS, bool bHeadersOnly)
        //{
        //    return this.InnerSession.WriteResponseToStream(oFS, bHeadersOnly);
        //}

        //
        //public bool WriteToStream(Stream oFS, bool bHeadersOnly)
        //{
        //    return this.InnerSession.WriteToStream(oFS, bHeadersOnly);
        //}
        //
        //public bool LoadResponseFromStream(Stream strmResponse, string sOptionalContentTypeHint)
        //{
        //    return this.InnerSession.LoadResponseFromStream(strmResponse, sOptionalContentTypeHint);
        //}
        //
        //public void WriteMetadataToStream(Stream strmMetadata)
        //{
        //    this.InnerSession.WriteMetadataToStream(strmMetadata);
        //}

        //
        //public bool WriteRequestToStream(bool bHeadersOnly, bool bIncludeProtocolAndHostWithPath,
        //    Stream oFs)
        //{
        //    return this.InnerSession.WriteRequestToStream(bHeadersOnly, bIncludeProtocolAndHostWithPath, oFs);
        //}

    }

}