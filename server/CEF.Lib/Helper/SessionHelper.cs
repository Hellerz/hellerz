using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fiddler;

namespace CEF.Lib.Helper
{
    public static class SessionHelper
    {
        private static readonly Dictionary<int, Session> Sessions = new Dictionary<int, Session>();
        private static readonly Dictionary<string, HTTPRequestHeaders> JRequestHeaderss = new Dictionary<string, HTTPRequestHeaders>();
        private static readonly Dictionary<string, HTTPResponseHeaders> JResponseHeaders = new Dictionary<string, HTTPResponseHeaders>();
        public static void OpSession<T>(Action<Dictionary<int, Session>, T> handler, T data)
        {
            Monitor.Enter(Sessions);
            handler(Sessions, data);
            Monitor.Exit(Sessions);
        }
        public static void OpSession<T>(Action<Dictionary<int, Session>> handler)
        {
            Monitor.Enter(Sessions);
            handler(Sessions);
            Monitor.Exit(Sessions);
        }
        public static void OpRequestHeaderss<T>(Action<Dictionary<string, HTTPRequestHeaders>, T> handler, T data)
        {
            Monitor.Enter(JRequestHeaderss);
            handler(JRequestHeaderss, data);
            Monitor.Exit(JRequestHeaderss);
        }
        public static void OpResponseHeaders<T>(Action<Dictionary<string, HTTPResponseHeaders>, T> handler, T data)
        {
            Monitor.Enter(JResponseHeaders);
            handler(JResponseHeaders, data);
            Monitor.Exit(JResponseHeaders);
        }
        public static void OpSession(Action<Dictionary<int, Session>> handler)
        {
            Monitor.Enter(Sessions);
            handler(Sessions);
            Monitor.Exit(Sessions);
        }
        public static void OpRequestHeaderss(Action<Dictionary<string, HTTPRequestHeaders>> handler)
        {
            Monitor.Enter(JRequestHeaderss);
            handler(JRequestHeaderss);
            Monitor.Exit(JRequestHeaderss);
        }
        public static void OpResponseHeaders(Action<Dictionary<string, HTTPResponseHeaders>> handler)
        {
            Monitor.Enter(JResponseHeaders);
            handler(JResponseHeaders);
            Monitor.Exit(JResponseHeaders);
        }

    }
}
