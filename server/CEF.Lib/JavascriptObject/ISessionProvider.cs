using Fiddler;

namespace CEF.Lib.JavascriptObject
{
    public interface ISessionProvider
    {
        Session SessionProvider(int sessionId);
    }
}
