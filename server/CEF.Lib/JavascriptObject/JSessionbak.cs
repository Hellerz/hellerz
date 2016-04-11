using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using CEF.Lib.Helper;
using Fiddler;

namespace CEF.Lib.JavascriptObject
{
    public class JSessionbak
    {
        public Session InnerSession { get; set; }

        public JSessionbak()
        {

        }

      
        public void Tests(string a,bool b,int c)
        {
            
        }


        public JSessionbak(Session session)
        {
            InnerSession = session;
        }

        public JSessionbak(string sessionIs)
        {
        }




       

        [CodeDescription("Gets Should response be buffered for tampering")]
        public bool GetIsBufferResponse()
        {
            return this.InnerSession.bBufferResponse;
        }
        
        [CodeDescription("Sets Should response be buffered for tampering")]
        public void SetIsBufferResponse(bool isBuffer)
        {
            this.InnerSession.bBufferResponse = isBuffer;
        }

        
        [CodeDescription("Bitflags of commonly-queried session attributes")]
        public string GetBitFlags()
        {
            return this.InnerSession.BitFlags.ToString();
        }

        
        [CodeDescription("Returns True if this is a HTTP CONNECT tunnel.")]
        public bool GetIsTunnel()
        {
            return this.InnerSession.isTunnel;
        }

        
        [CodeDescription("Gets A common use for the Tag property is to store data that is closely associated with the Session. It is NOT marshalled during drag/drop and is NOT serialized to a SAZ file.")]
        public string GetTag()
        {
            return this.InnerSession.Tag.ToString();
        }

        
        [CodeDescription("Sets A common use for the Tag property is to store data that is closely associated with the Session. It is NOT marshalled during drag/drop and is NOT serialized to a SAZ file.")]
        public void SetTag(string tag)
        {
            this.InnerSession.Tag = tag;
        }

        
        [CodeDescription("If this session is a Tunnel, and the tunnel's IsOpen property is TRUE, returns TRUE. Otherwise returns FALSE.")]
        public bool GetTunnelIsOpen()
        {
            return this.InnerSession.TunnelIsOpen;
        }

        
        [CodeDescription("If this session is a Tunnel, returns number of bytes sent from the Server to the Client")]
        public long GetTunnelIngressByteCount()
        {
            return this.InnerSession.TunnelIngressByteCount;
        }

        
        [CodeDescription("If this session is a Tunnel, returns number of bytes sent from the Client to the Server")]
        public long GetTunnelEgressByteCount()
        {
            return this.InnerSession.TunnelEgressByteCount;
        }



        
        [CodeDescription("Gets the request's Method (e.g. GET, POST, etc).")]
        public string GetRequestMethod()
        {
            return this.InnerSession.RequestMethod;
        }

        
        [CodeDescription("Sets the request's Method (e.g. GET, POST, etc).")]
        public void SetRequestMethod(string requestMethod)
        {
            this.InnerSession.RequestMethod = requestMethod;
        }




        
        [CodeDescription("When true, this session was conducted using the HTTPS protocol.")]
        public bool GetIsHTTPS()
        {
            return this.InnerSession.isHTTPS;
        }

        
        [CodeDescription("When true, this session was conducted using the FTPS protocol.")]
        public bool GetIsFTP()
        {
            return this.InnerSession.isFTP;
        }

        
        [CodeDescription("Get the process ID of the application which made this request, or 0 if it cannot be determined.")]
        public int GetLocalProcessID()
        {
            return this.InnerSession.LocalProcessID;
        }

        
        [CodeDescription("Get the Process Info the application which made this request, or String.Empty if it cannot be determined.")]
        public string GetLocalProcess()
        {
            return this.InnerSession.LocalProcess;
        }

        
        [CodeDescription("Gets a path-less filename suitable for saving the Response entity. Uses Content-Disposition if available.")]
        public string GetSuggestedFilename()
        {
            return this.InnerSession.SuggestedFilename;
        }

        
        [CodeDescription("Get if this request should bypass the gateway")]
        public bool GetbypassGateway()
        {
            return this.InnerSession.bypassGateway;
        }

        
        [CodeDescription("Set to true in OnBeforeRequest if this request should bypass the gateway")]
        public void SetBypassGateway(bool bypassGateway)
        {
            this.InnerSession.bypassGateway = bypassGateway;
        }

        
        [CodeDescription("Returns the port used by the client to communicate to Fiddler.")]
        public int GetClientPort()
        {
            return this.InnerSession.clientPort;
        }



        
        [CodeDescription("Returns the path and query part of the URL. (For a CONNECT request, returns the host:port to be connected.)")]
        public string GetPathAndQuery()
        {
            return this.InnerSession.PathAndQuery;
        }

        
        [CodeDescription("Sets the path and query part of the URL. (For a CONNECT request, returns the host:port to be connected.)")]
        public void SetPathAndQuery(string pathAndQuery)
        {
            this.InnerSession.PathAndQuery = pathAndQuery;
        }

        
        [CodeDescription("Retrieves the complete URI, including protocol/scheme, in the form http://www.host.com/filepath?query.")]
        public string GetfullUrl()
        {
            return this.InnerSession.fullUrl;
        }

        
        [CodeDescription("Sets the complete URI, including protocol/scheme, in the form http://www.host.com/filepath?query.")]
        public void SetfullUrl(string fullUrl)
        {
            this.InnerSession.fullUrl = fullUrl;
        }

        
        [CodeDescription("Gets the URL (without protocol) being requested from the server, in the form www.host.com/filepath?query.")]
        public string Geturl()
        {
            return this.InnerSession.url;
        }

        
        [CodeDescription("Sets the URL (without protocol) being requested from the server, in the form www.host.com/filepath?query.")]
        public void Seturl(string url)
        {
            this.InnerSession.url = url;
        }

        
        [CodeDescription("Gets the host to which this request is targeted. MAY include IPv6 literal brackets. MAY include a trailing port#.")]
        public string Gethost()
        {
            return this.InnerSession.host;
        }

        
        [CodeDescription("Sets the host to which this request is targeted. MAY include IPv6 literal brackets. MAY include a trailing port#.")]
        public void Sethost(string host)
        {
            this.InnerSession.host = host;
        }

        
        [CodeDescription("Gets the hostname to which this request is targeted; does NOT include any port# but will include IPv6-literal brackets for IPv6 literals.")]
        public string Gethostname()
        {
            return this.InnerSession.hostname;
        }

        
        [CodeDescription("Sets the hostname to which this request is targeted; does NOT include any port# but will include IPv6-literal brackets for IPv6 literals.")]
        public void Sethostname(string hostname)
        {
            this.InnerSession.hostname = hostname;
        }

        
        [CodeDescription("Returns the server port to which this request is targeted.")]
        public int Getport()
        {
            return this.InnerSession.port;
        }

        
        [CodeDescription("Sets the server port to which this request is targeted.")]
        public void Setport(int port)
        {
            this.InnerSession.port = port;
        }

        
        [CodeDescription("Returns the sequential number of this request.")]
        public int Getid()
        {
            return this.InnerSession.id;
        }

        
        [CodeDescription("Returns the Address used by the client to communicate to Fiddler.")]
        public string GetClientIP()
        {
            return this.InnerSession.clientIP;
        }

        
        [CodeDescription("Gets the HTTP Status code of the server's response")]
        public int GetresponseCode()
        {
            return this.InnerSession.responseCode;
        }

        
        [CodeDescription("Sets the HTTP Status code of the server's response")]
        public void SetresponseCode(int responseCode)
        {
            this.InnerSession.responseCode = responseCode;
        }

        
        [CodeDescription("Checks whether this is a WebSocket, and if so, whether it has logged any parsed messages.")]
        public bool GetbHasWebSocketMessages()
        {
            return this.InnerSession.bHasWebSocketMessages;
        }

        
        [CodeDescription("Returns TRUE if this session state>ReadingResponse and oResponse not null.")]
        public bool GetbHasResponse()
        {
            return this.InnerSession.bHasResponse;
        }

        
        [CodeDescription("Replace HTTP request headers and body using the specified file.")]
        public bool LoadRequestBodyFromFile(string sFilename)
        {
            return this.InnerSession.LoadRequestBodyFromFile(sFilename);
        }

        
        [CodeDescription("Replace HTTP response headers and body using the specified file.")]
        public bool LoadResponseFromFile(string sFilename)
        {
            return this.InnerSession.LoadResponseFromFile(sFilename);
        }

        
        [CodeDescription("Return a string generated from the request body, decoding it and converting from a codepage if needed. Possibly expensive due to decompression and will throw on malformed content.")]
        public string GetRequestBodyAsString()
        {
            return this.InnerSession.GetRequestBodyAsString();
        }

        
        [CodeDescription("Return a string generated from the response body, decoding it and converting from a codepage if needed. Possibly expensive due to decompression and will throw on malformed content.")]
        public string GetResponseBodyAsString()
        {
            return this.InnerSession.GetResponseBodyAsString();
        }

        
        [CodeDescription("Return a string md5, sha1, or sha256 of an unchunked and decompressed copy of the response body.")]
        public string GetResponseBodyHash(string sHashAlg)
        {
            return this.InnerSession.GetResponseBodyHash(sHashAlg);
        }

        
        [CodeDescription("Returns the Encoding of the requestBodyBytes")]
        public string GetRequestBodyEncoding()
        {
            return this.InnerSession.GetRequestBodyEncoding().ToString();
        }

        
        [CodeDescription("Returns the Encoding of the responseBodyBytes")]
        public string GetResponseBodyEncoding()
        {
            return this.InnerSession.GetResponseBodyEncoding().ToString();
        }

        
        [CodeDescription("Returns true if request URI contains the specified string. Case-insensitive.")]
        public bool UriContains(string sLookfor)
        {
            return this.InnerSession.uriContains(sLookfor);
        }

        
        [CodeDescription("Removes chunking and HTTP Compression from the response. Adds or updates Content-Length header.")]
        public bool UtilDecodeResponse()
        {
            return this.InnerSession.utilDecodeResponse();
        }

        
        [CodeDescription("Removes chunking and HTTP Compression from the response. Adds or updates Content-Length header.\r\nbSilent is TRUE if error messages should be suppressed. False otherwise.")]
        public bool UtilDecodeResponse(bool bSilent)
        {
            return this.InnerSession.utilDecodeResponse(bSilent);
        }

        
        [CodeDescription("Removes chunking and HTTP Compression from the Request. Adds or updates Content-Length header.")]
        public bool UtilDecodeRequest()
        {
            return this.InnerSession.utilDecodeRequest();
        }

        
        [CodeDescription("Removes chunking and HTTP Compression from the Request. Adds or updates Content-Length header.\r\nbSilent is TRUE if error messages should be suppressed. False otherwise.")]
        public bool UtilDecodeRequest(bool bSilent)
        {
            return this.InnerSession.utilDecodeRequest(bSilent);
        }

        
        [CodeDescription("Use GZIP to compress the request body. Throws exceptions to caller.")]
        public bool UtilGZIPRequest()
        {
            return this.InnerSession.utilGZIPRequest();
        }

        
        [CodeDescription("Use GZIP to compress the response body. Throws exceptions to caller.")]
        public bool UtilGZIPResponse()
        {
            return this.InnerSession.utilGZIPResponse();
        }

        
        [CodeDescription("Use DEFLATE to compress the response body. Throws exceptions to caller.")]
        public bool UtilDeflateResponse()
        {
            return this.InnerSession.utilDeflateResponse();
        }

        
        [CodeDescription("Use BZIP2 to compress the response body. Throws exceptions to caller.")]
        public bool UtilBZIP2Response()
        {
            return this.InnerSession.utilBZIP2Response();
        }

        
        [CodeDescription("Apply Transfer-Encoding: chunked to the response, if possible.\r\niSuggestedChunkCount :The number of chunks to try to create")]
        public bool UtilChunkResponse(int iSuggestedChunkCount)
        {
            return this.InnerSession.utilChunkResponse(iSuggestedChunkCount);
        }

        
        [CodeDescription("Perform a case-sensitive string replacement on the request body (not URL!). Updates Content-Length header. Returns TRUE if replacements occur.")]
        public bool UtilReplaceInRequest(string sSearchFor, string sReplaceWith)
        {
            return this.InnerSession.utilReplaceInRequest(sSearchFor, sReplaceWith);
        }

        
        [CodeDescription("Call inside OnBeforeRequest to create a Response object and bypass the server.")]
        public void UtilCreateResponseAndBypassServer()
        {
            this.InnerSession.utilCreateResponseAndBypassServer();
        }

        
        [CodeDescription("Perform a regex-based replacement on the response body. Specify RegEx Options via leading Inline Flags, e.g. (?im) for case-Insensitive Multi-line. Updates Content-Length header. Note, you should call utilDecodeResponse first!  Returns TRUE if replacements occur.")]
        public bool UtilReplaceRegexInResponse(string sSearchForRegEx, string sReplaceWithExpression)
        {
            return this.InnerSession.utilReplaceRegexInResponse(sSearchForRegEx, sReplaceWithExpression);
        }

        
        [CodeDescription("Perform a case-sensitive string replacement on the response body. Updates Content-Length header. Note, you should call utilDecodeResponse first!  Returns TRUE if replacements occur.\r\nsSearchFor:String to find (case-sensitive)\r\nsReplaceWith:String to use to replace")]
        public bool UtilReplaceInResponse(string sSearchFor, string sReplaceWith)
        {
            return this.InnerSession.utilReplaceInResponse(sSearchFor, sReplaceWith);
        }

        
        [CodeDescription("Perform a single case-sensitive string replacement on the response body. Updates Content-Length header. Note, you should call utilDecodeResponse first! Returns TRUE if replacements occur.\r\nsSearchFor:String to find (case-sensitive)\r\nsReplaceWith:String to use to replace\r\nbCaseSensitive:TRUE for Case-Sensitive")]
        public bool UtilReplaceOnceInResponse(string sSearchFor, string sReplaceWith,
            bool bCaseSensitive)
        {
            return this.InnerSession.utilReplaceOnceInResponse(sSearchFor, sReplaceWith, bCaseSensitive);
        }

        
        [CodeDescription("Replaces the request body with sString as UTF8. Sets Content-Length header & removes Transfer-Encoding/Content-Encoding.\r\nsString:The desired request Body as a string")]
        public void UtilSetRequestBody(string sString)
        {
            this.InnerSession.utilSetRequestBody(sString);
        }

        
        [CodeDescription("Replaces the response body with sString. Sets Content-Length header & removes Transfer-Encoding/Content-Encoding")]
        public void UtilSetResponseBody(string sString)
        {
            this.InnerSession.utilSetResponseBody(sString);
        }

        
        [CodeDescription("Copy an existing response to this Session, bypassing the server if not already contacted")]
        public void UtilAssignResponse(JResponseHeaders oRh, string body)
        {
            this.InnerSession.utilAssignResponse(oRh.InnerHeaders, this.InnerSession.GetResponseBodyEncoding().GetBytes(body));
        }

        
        [CodeDescription("Prepend a string to the response body. Updates Content-Length header. Note, you should call utilDecodeResponse first!")]
        public void UtilPrependToResponseBody(string sString)
        {
            this.InnerSession.utilPrependToResponseBody(sString);
        }

        
        [CodeDescription("Find a string in the request body. Return its index or -1.")]
        public int UtilFindInRequest(string sSearchFor, bool bCaseSensitive)
        {
            return this.InnerSession.utilFindInRequest(sSearchFor, bCaseSensitive);
        }

        
        [CodeDescription("Find a string in the response body. Return its index or -1. Note, you should call utilDecodeResponse first!")]
        public int UtilFindInResponse(string sSearchFor, bool bCaseSensitive)
        {
            return this.InnerSession.utilFindInResponse(sSearchFor, bCaseSensitive);
        }

        
        [CodeDescription("Returns the fully-qualified URL to which this Session's response points, or null. This method is needed because many servers (illegally) return a relative url in HTTP/3xx Location response headers.")]
        public string GetRedirectTargetURL()
        {
            return this.InnerSession.GetRedirectTargetURL();
        }

        
        [CodeDescription("Returns TRUE if the Session's HTTP Method is available and matches the target method.")]
        public bool HTTPMethodIs(string sTestFor)
        {
            return this.InnerSession.HTTPMethodIs(sTestFor);
        }

        
        [CodeDescription("Returns TRUE if the Session's target hostname (no port) matches sTestHost (case-insensitively).")]
        public bool HostnameIs(string sTestHost)
        {
            return this.InnerSession.HostnameIs(sTestHost);
        }

        
        [CodeDescription("Returns HTML representing the Session. Call Utilities.StringToCF_HTML on the result of this function before placing it on the clipboard.")]
        public string ToHTMLFragment(bool HeadersOnly)
        {
            return this.InnerSession.ToHTMLFragment(HeadersOnly);
        }

        
        [CodeDescription("Store this session's request and response to a string.")]
        public string ToString(bool headersOnly)
        {
            return this.InnerSession.ToString(headersOnly);
        }

        
        [CodeDescription("Store this session's request and response to a string.")]
        public override string ToString()
        {
            return this.InnerSession.ToString();
        }


        [CodeDescription("This method resumes the Session's thread in response to 'Continue' commands from the UI")]
        public void ThreadResume()
        {
            this.InnerSession.ThreadResume();
        }

        [CodeDescription("This method pauses the Session's thread in response to 'Pause' commands from the UI")]
        public void ThreadPause()
        {
            var threadPause = typeof(Session).GetMethod("ThreadPause", BindingFlags.Instance | BindingFlags.NonPublic);
            if (threadPause != null)
            {
                threadPause.Invoke(this.InnerSession, null);
            }
        }

        [CodeDescription("Set the SessionFlags.Ignore bit for this Session, also configuring it to stream, drop read data, and bypass event handlers. For a CONNECT Tunnel, traffic will be blindly shuffled back and forth. Session will be hidden.")]
        public void Ignore()
        {
            this.InnerSession.Ignore();
        }

        
        [CodeDescription("Call this function while in the 'reading response' state to update the responseBodyBytes array with the partially read response.")]
        public bool COMETPeek()
        {
            return this.InnerSession.COMETPeek();
        }

        
        [CodeDescription("Prevents the server pipe from this session from being pooled for reuse")]
        public void PoisonServerPipe()
        {
            this.InnerSession.PoisonServerPipe();
        }

        
        [CodeDescription("Ensures that, after the response is complete, the client socket is closed and not reused. Does NOT (and must not) close the pipe.")]
        public void PoisonClientPipe()
        {
            this.InnerSession.PoisonClientPipe();
        }

        
        [CodeDescription("Closes both client and server pipes and moves state to Aborted; unpauses thread if paused.")]
        public void Abort()
        {
            this.InnerSession.Abort();
        }
        
        [CodeDescription("Save HTTP response body to Fiddler Captures folder.")]
        public bool SaveResponseBody()
        {
            return this.InnerSession.SaveResponseBody();
        }

        
        [CodeDescription("Save HTTP response body to specified location.")]
        public bool SaveResponseBody(string sFilename)
        {
            return this.InnerSession.SaveResponseBody(sFilename);
        }

        
        [CodeDescription("Save HTTP request body to specified location.")]
        public bool SaveRequestBody(string sFilename)
        {
            return this.InnerSession.SaveRequestBody(sFilename);
        }

        
        [CodeDescription("Save the request and response to a single file.")]
        public void SaveSession(string sFilename, bool bHeadersOnly)
        {
            this.InnerSession.SaveSession(sFilename, bHeadersOnly);
        }

        
        [CodeDescription("Save the request to a file. The headers' Request Line will not contain the scheme or host, which is probably not what you want.")]
        public void SaveRequest(string sFilename, bool bHeadersOnly)
        {
            this.InnerSession.SaveRequest(sFilename, bHeadersOnly);
        }

        
        [CodeDescription("Save the request to a file. Throws if file cannot be written.")]
        public void SaveRequest(string sFilename, bool bHeadersOnly, bool bIncludeSchemeAndHostInPath)
        {
            this.InnerSession.SaveRequest(sFilename, bHeadersOnly, bIncludeSchemeAndHostInPath);
        }

        
        [CodeDescription("null, or Target URL. Note, you may want to call Utilities.TrimAfter(sTarget, '#');")]
        public string GetRedirectTargetURL(string sBase, string sLocation)
        {
            return Session.GetRedirectTargetURL(sBase, sLocation);
        }

        
        [CodeDescription("Writes this session's metadata to a file.the metadata should be saved in XML format.")]
        public bool SaveMetadata(string sFilename)
        {
            return this.InnerSession.SaveMetadata(sFilename);
        }

        
        [CodeDescription("Saves the response (headers and body) to a file")]
        public void SaveResponse(string sFilename, bool bHeadersOnly)
        {
            this.InnerSession.SaveResponse(sFilename, bHeadersOnly);
        }

        #region enum
        
        [CodeDescription("Enumerated state of the current session.")]
        public string GetState()
        {
            return this.InnerSession.state.ToString();
        }

        
        [CodeDescription("Enumerated state of the current session.")]
        public void SetState(string state)
        {
            this.InnerSession.state = (SessionStates)Enum.Parse(typeof(SessionStates), state);
        }
        #endregion

        #region HttpHeaders
        
        [CodeDescription("Get RequestHeaders")]
        public int GetRequestHeaders()
        {
            return this.InnerSession.id;
        }

        
        [CodeDescription("Get ResponseHeaders")]
        public int GetResponseHeaders()
        {
            return this.InnerSession.id;
        }

        
        [CodeDescription("Bypass this session and get a response by proxy of socksv5 ")]
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
                        using (var reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            this.InnerSession.utilSetResponseBody(reader.ReadToEnd());
                        }
                    }
                }
            }
        }

        
        [CodeDescription("Re-Send a message by proxy of socksv5 ")]
        public string ProxySocksV5(string proxyServer, bool bypassOnlocal, string bypasss, string headers, string body)
        {
            var request = (HttpWebRequest)WebRequest.Create(this.InnerSession.fullUrl);
            request.Proxy = new WebProxy(proxyServer, bypassOnlocal, bypasss.Split(';'));
            BuildHeader(request, headers);
            var bytes = Encoding.UTF8.GetBytes(body);
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


        //
        //public byte[] GetRequestBody()
        //{
        //    return this.InnerSession.RequestBody;
        //}

        //
        //public void SetRequestBody(byte[] requestBody)
        //{
        //    this.InnerSession.RequestBody = requestBody;
        //}
        //
        //public byte[] GetResponseBody()
        //{
        //    return this.InnerSession.ResponseBody;
        //}

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