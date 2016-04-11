using CEF.Lib.JavascriptObject;
using Fiddler;

namespace CEF.Lib.JavascriptObject
{
    public class JResponseHeaders 
    {
        public HTTPResponseHeaders InnerHeaders { get; set; }

        public JResponseHeaders(JSession session)
        {
            this.InnerHeaders = session.InnerSession.ResponseHeaders;
        }

        public string Get(string name)
        {
            return InnerHeaders[name];
        }
        
        public void Set(string name, string value)
        {
            InnerHeaders[name] = value;
        }
        
        public void Remove(string name)
        {
            InnerHeaders.Remove(name);
        }

        //
        //public byte[] ToByteArray(bool prependStatusLine, bool appendEmptyLine)
        //{
        //    return InnerHeaders.ToByteArray(prependStatusLine, appendEmptyLine);
        //}
        
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

        
        public JResponseHeaders Clone()
        {
            return (JResponseHeaders)InnerHeaders.Clone();
        }


        
        public string GetHTTPResponseStatus()
        {
            return this.InnerHeaders.HTTPResponseStatus;
        }

        
        public void SetHTTPResponseStatus(string status)
        {
            this.InnerHeaders.HTTPResponseStatus = status;
        }

        
        public int GetHTTPResponseCode()
        {
            return this.InnerHeaders.HTTPResponseCode;
        }

        
        public void SetHTTPResponseCode(int code)
        {
            this.InnerHeaders.HTTPResponseCode = code;
        }

        
        public string GetStatusDescription()
        {
            return this.InnerHeaders.StatusDescription;
        }

        
        public void SetStatusDescription(string description)
        {
            this.InnerHeaders.StatusDescription = description;
        }

        
        public void SetStatus(int iCode, string sDescription)
        {
            this.InnerHeaders.SetStatus (iCode, sDescription);
        }


    }
}
