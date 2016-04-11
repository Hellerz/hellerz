using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using CEF.Lib.Attributes;
using Fiddler;
using Newtonsoft.Json;

namespace CEF.Lib.Helper
{
    public static class FiddlerHelper
    {
        public readonly static Dictionary<string, SessionStateHandler> EventHandlers = new Dictionary<string, SessionStateHandler>();
        private static int _port = Common.ConvertToStruct(StorageHelper.AchiveValue("fiddlerport", "5389"),5389);
        private const string AppName = "Calibur";
        private readonly static MethodInfo ThreadPauseMethod = typeof(Session).GetMethod("ThreadPause", BindingFlags.Instance | BindingFlags.NonPublic);
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
            StorageHelper.SetValue("fiddlerport", isHttps.ToString().ToLower());
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
                Debug.WriteLine("BeforePause —— ID:{0}, Time:{1},Thread:{2} Url:{3}", session.id, DateTime.Now,Thread.CurrentThread.ManagedThreadId,session.fullUrl);
                ThreadPauseMethod.Invoke(session, null);
                Debug.WriteLine("AfterPause —— ID:{0}, Time:{1},Thread:{2} Url:{3}", session.id, DateTime.Now, Thread.CurrentThread.ManagedThreadId, session.fullUrl);
            }
        }

        public static void Resume(this Session session)
        {
            Debug.WriteLine("BeforeResume —— ID:{0}, Time:{1},Thread:{2} Url:{3}", session.id, DateTime.Now, Thread.CurrentThread.ManagedThreadId, session.fullUrl);
            session.ThreadResume();
            Debug.WriteLine("AfterResume —— ID:{0}, Time:{1},Thread:{2} Url:{3}", session.id, DateTime.Now, Thread.CurrentThread.ManagedThreadId, session.fullUrl);
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
        }

         [JSchema]
         public static void Inject(string rawRequest)
         {
             FiddlerApplication.oProxy.InjectCustomRequest(rawRequest);
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
            Utilities.RunExecutable("certmgr.msc","");
        }
        

        [JSchema]
        public static void Stop()
        {
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
