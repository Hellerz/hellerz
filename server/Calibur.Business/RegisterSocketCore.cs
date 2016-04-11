using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calibur.Model;
using CEF.Lib.Helper;

namespace Calibur.Business
{
    public class RegisterSocketCore
    {
        public static RegisterSocketResponse RegisterSocket(RegisterSocketRequest request)
        {
            WebSocketHelper.OpSocket((sockets, req) =>
            {
                if (req != null && !string.IsNullOrWhiteSpace(req.BrowserId) && !sockets.ContainsKey(req.BrowserId))
                {
                    sockets.Add(req.BrowserId, req.Head.Socket);
                    WebSocketHelper.OpSocketId((insockets, inreq) =>
                    {
                        insockets.Add(req.Head.Socket,req.BrowserId);
                    }, req);
                }
            }, request);
            return new RegisterSocketResponse();
        }
    }
}
