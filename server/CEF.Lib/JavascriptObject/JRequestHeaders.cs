using CEF.Lib.JavascriptObject;
using Fiddler;

namespace CEF.Lib.JavascriptObject
{
    

    public class JRequestHeaders
    {
        public HTTPRequestHeaders InnerHeaders { get; set; }

        public JRequestHeaders(JSession session)
        {
            this.InnerHeaders = session.InnerSession.RequestHeaders;
        }

        
        public string Get(string name)
        {
            return InnerHeaders[name];
        }
        
        
        
        public void Remove(string name)
        {
            InnerHeaders.Remove(name);
        }

        //
        //public byte[] ToByteArray(bool prependVerbLine, bool appendEmptyLine, bool includeProtocolInPath)
        //{
        //    return InnerHeaders.ToByteArray(prependVerbLine, appendEmptyLine, includeProtocolInPath);
        //}

        //
        //public byte[] ToByteArray(bool prependVerbLine, bool appendEmptyLine, bool includeProtocolInPath, string sVerbLineHost)
        //{
        //    return InnerHeaders.ToByteArray(prependVerbLine, appendEmptyLine, includeProtocolInPath, sVerbLineHost);
        //}
        
        public string ToString(bool prependVerbLine, bool appendEmptyLine, bool includeProtocolAndHostInPath)
        {
            return InnerHeaders.ToString(prependVerbLine, appendEmptyLine, includeProtocolAndHostInPath);
        }

        
        public string ToString(bool prependVerbLine, bool appendEmptyLine)
        {
            return InnerHeaders.ToString(prependVerbLine, appendEmptyLine);
        }

         
        public override string ToString()
        {
            return InnerHeaders.ToString();
        }


        
        public bool AssignFromString(string sHeaders)
        {
            return InnerHeaders.AssignFromString(sHeaders);
        }

        
        public JRequestHeaders Clone()
        {
            return (JRequestHeaders)InnerHeaders.Clone();
        }


        
        public string GetHTTPMethod()
        {
            return this.InnerHeaders.HTTPMethod;
        }

        
        public void SetHTTPMethod(string method)
        {
            this.InnerHeaders.HTTPMethod = method;
        }

        
        public string GetUriScheme()
        {
            return this.InnerHeaders.UriScheme;
        }

        
        public void SetUriScheme(string uriScheme)
        {
            this.InnerHeaders.UriScheme = uriScheme;
        }


        
        public string GetUriUserInfo()
        {
            return this.InnerHeaders.UriUserInfo;
        }

        
        public string GetRequestPath()
        {
            return this.InnerHeaders.RequestPath;
        }

        
        public void SetRequestPath(string requestPath)
        {
            this.InnerHeaders.RequestPath = requestPath;
        }



    }
}
