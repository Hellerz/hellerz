using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Calibur.Model;
using Calibur.Model.IMessage;
using CEF.Lib.Attributes;
using Fiddler;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;

namespace CEF.Lib.Helper
{
    public static class FiddlerHelper
    {
        public readonly static Dictionary<string, SessionStateHandler> EventHandlers = new Dictionary<string, SessionStateHandler>();
        private static int _port = Common.ConvertToStruct(StorageHelper.AchiveValue("fiddlerport", "5389"),5389);
        private const string AppName = "Calibur";
        private readonly static MethodInfo ThreadPauseMethod = typeof(Session).GetMethod("ThreadPause", BindingFlags.Instance | BindingFlags.NonPublic);
        private readonly static Regex SplitLine = new Regex(@"[\r\n]+", RegexOptions.Compiled);
        private readonly static Regex HasPrtl = new Regex(@"^http(s)?://", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly static Regex SplitHdBd = new Regex(@"\r\n\r\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        static FiddlerHelper()
        {
            FiddlerApplication.SetAppDisplayName(AppName);
            CONFIG.IgnoreServerCertErrors = false;
            FiddlerApplication.RequestHeadersAvailable += session =>
            {
                SessionHelper.OpSession((mapping,iSession) =>
                {
                   mapping.Add(iSession.id, iSession);
                },session);
            };
            FiddlerApplication.BeforeRequest += session =>
            {
                session.bBufferResponse = true;
            };
        }

        [JSchema]
        public static void ClearAllSession()
        {
            SessionHelper.OpSession(mapping => mapping.Clear());
        }

        private static bool _isPauseSession = Common.ConvertToStruct(StorageHelper.AchiveValue("isautoresponser", "false"), false);

        private static bool _isHttps = Common.ConvertToStruct(StorageHelper.AchiveValue("ishttps", "true"), true);

        [JSchema]
        public static bool IsPauseSession()
        {
            return _isPauseSession;
        }
        [JSchema]
        public static void ClearAllPause()
        {
             EventHandlers.Clear();
        }

        [JSchema]
        public static void SetPauseSession(bool isPauseSession)
        {
            StorageHelper.SetValue("isautoresponser", isPauseSession.ToString().ToLower());
            _isPauseSession = isPauseSession;
        }

        [JSchema]
        public static bool IsAttached()
        {
            if (FiddlerApplication.oProxy != null)
            {
                return FiddlerApplication.oProxy.IsAttached;
            }
            return false;
        }

        [JSchema]
        public static void SetPort(int port)
        {
            StorageHelper.SetValue("fiddlerport", port.ToString());
            _port = port;
        }
        [JSchema]
        public static int GetPort()
        {
            return _port;
        }

        [JSchema]
        public static void SetHttps(bool isHttps)
        {
            StorageHelper.SetValue("ishttps", isHttps.ToString().ToLower());
            _isHttps = isHttps;
        }
        [JSchema]
        public static bool GetHttps()
        {
            return _isHttps;
        }

        public static void Pause(this Session session)
        {
            if (ThreadPauseMethod != null)
            {
                ThreadPauseMethod.Invoke(session, null);
            }
        }

        public static void Resume(this Session session)
        {
            session.ThreadResume();
        }
        [JSchema]
        public static void ReStart()
        {
            Stop();
            Start();
        }
        [JSchema]
        public static void Start()
        {
            var flag = FiddlerCoreStartupFlags.Default;
            if (!GetHttps())
            {
                flag = flag & ~FiddlerCoreStartupFlags.DecryptSSL;
            }
            FiddlerApplication.Startup(_port, flag);
            if (FiddlerApplication.oProxy != null)
            {
                FiddlerApplication.oProxy.DetachedUnexpectedly += NotifyProxyChanged;
            }

        }

         [JSchema]
         public static string InjectRaw(string rawRequest)
         {
             var guid = Guid.NewGuid().ToString();
             var hdbd = SplitHdBd.Split(rawRequest,2);
             
             if (hdbd.Length == 2)
             {
                 var httpheaders = new HTTPRequestHeaders();
                 httpheaders.AssignFromString(hdbd[0]);
                 httpheaders["Calibur-Composer"] = guid;
                 if (!string.IsNullOrWhiteSpace(hdbd[1]))
                 {
                     httpheaders["Content-Length"] = Encoding.UTF8.GetBytes(hdbd[1]).Length.ToString();
                 }
                 var headerstr = httpheaders.ToString(true, true, false);
                 FiddlerApplication.oProxy.InjectCustomRequest(headerstr + hdbd[1]);
                 return guid;
             }
             else
             {
                 throw new ArgumentException("Raw request is invalid.");
             }
             
         }
        
         [JSchema]
         public static string Inject(string url, string method, string contenttype, string headers, string body)
         {
             if (!string.IsNullOrWhiteSpace(url) && !string.IsNullOrWhiteSpace(method))
             {
                 var guid = Guid.NewGuid().ToString();
                 if (!HasPrtl.IsMatch(url))
                 {
                     url = "http://" + url;
                 }
                 var uri = new Uri(url);
                 var headerList = SplitLine.Split(headers).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
                 var httpheaders = new HTTPRequestHeaders(uri.AbsoluteUri, headerList) { HTTPMethod = method };
                 httpheaders["Connection"] = "keep-alive";
                 httpheaders["Calibur-Composer"] = guid;
                 httpheaders["Host"] = uri.Host;
                 if (!string.IsNullOrWhiteSpace(contenttype))
                 {
                     httpheaders["Content-Type"] = contenttype;
                 }
                 if (!string.IsNullOrWhiteSpace(body))
                 {
                     httpheaders["Content-Length"] = Encoding.UTF8.GetBytes(body).Length.ToString();
                 }
                 else
                 {
                     body = string.Empty;
                 }
                 var headerstr = httpheaders.ToString(true, true, false);
                 FiddlerApplication.oProxy.InjectCustomRequest(headerstr + body);
                 return guid;
             }
             else
             {
                 throw new ArgumentException("url or method is empty.");
             }
             
         }

        [JSchema]
        public static string AcheiveHeader(string url)
        {
            var uri = new Uri(url);
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "HEAD";
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response != null)
                {
                    return JsonConvert.SerializeObject(response.Headers);
                }
            }
            return string.Empty;
        }

        [JSchema]
        public static bool IsStarted()
        {
            return FiddlerApplication.IsStarted();
        }
        [JSchema]
        public static void OpenWinNet()
        {
            Utilities.RunExecutable("rundll32.exe", "shell32.dll,Control_RunDLL inetcpl.cpl,,4");
        }
        [JSchema]
        public static void OpenCertManager()
        {
            Utilities.RunExecutable("certmgr.msc", "");
        }
        [JSchema]
        public static void RunExecutable(string executable,string arg)
        {
            Utilities.RunExecutable(executable, arg);
        }
        

        [JSchema]
        public static void Stop()
        {
            if (FiddlerApplication.oProxy != null)
            {
                FiddlerApplication.oProxy.DetachedUnexpectedly -= NotifyProxyChanged;
            }
            if (FiddlerApplication.IsStarted())
            {
                FiddlerApplication.Shutdown();
            }
            Thread.Sleep(500);
        }
        [JSchema]
        public static void RemoveFiddlerCertificates()
        {
            CertMaker.removeFiddlerGeneratedCerts();
        }


        [JSchema]
        public static string GetBase64RootCertificate() 
        {
            return Convert.ToBase64String(CertMaker.GetRootCertificate().Export(X509ContentType.Cert));
        }

        private static void NotifyProxyChanged(object sender, EventArgs e)
        {
            var message = new MessageInfo
            {
                ID = "DetachedUnexpectedly",
                Type = MessageMode.Event
            };
            WebSocketHelper.Broadcast(JsonConvert.SerializeObject(message, Common.TimeFormat));
        }


        #region ResetCertificate
        [JSchema]
        public static void ResetCertificate()
        {
            DecryptHttps();
            RemoveAllCertificates();
            DecryptHttps();
        }
        private static void DecryptHttps()
        {
            CertMaker.EnsureReady();
            if ((CertMaker.rootCertExists() || CertMaker.createRootCert()) && !CertMaker.rootCertIsTrusted())
            {
                var flag = CertMaker.trustRootCert();
                if (!flag)
                {
                    //FiddlerApplication.DoNotifyUser(this, "Unable to configure Windows to Trust the Fiddler Root certificate.\n\nThe LOG tab may contain more information.", "Certificate Trust", MessageBoxIcon.Exclamation);
                }
                if (flag && FiddlerApplication.Prefs.GetBoolPref("fiddler.CertMaker.OfferMachineTrust", ((Environment.OSVersion.Version.Major > 6) || ((Environment.OSVersion.Version.Major == 6) && (Environment.OSVersion.Version.Minor > 1)))))
                {
                    var subject = CertMaker.GetRootCertificate().Subject;
                    Utilities.RunExecutable(CONFIG.GetPath("App") + "TrustCert.exe", String.Format("\"{0}\"", subject));
                }
            }

        }
        private static void RemoveAllCertificates()
        {

            try
            {
                bool flag = false;
                if (CertMaker.rootCertIsMachineTrusted())
                {
                    var rootCertificate = CertMaker.GetRootCertificate();
                    if (rootCertificate != null)
                    {
                        var subject = rootCertificate.Subject;
                        if (!String.IsNullOrEmpty(subject))
                        {
                            flag = Utilities.RunExecutableAndWait(CONFIG.GetPath("App") + "TrustCert.exe", String.Format("-u \"{0}\"", subject));
                        }
                    }
                }
                if (CertMaker.removeFiddlerGeneratedCerts() || flag)
                {
                    //FiddlerApplication.DoNotifyUser(this, string.Format("Fiddler-generated certificates have been removed from {0}", flag ? "both User and Machine Root storage." : "the Current User storage."), "Success", MessageBoxIcon.Asterisk);
                }
            }
            catch (Exception exception)
            {
                FiddlerApplication.ReportException(exception, "");
            }
        } 
        #endregion
    }
}
