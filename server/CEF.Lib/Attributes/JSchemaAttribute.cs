using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEF.Lib.Attributes
{
    /// <summary>
    /// 通过这个标记，client端可以通过×××Helper直接调用服务端的类的方法，比如File.HttpDownloadFile(www.xxx.com/img.jpg)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method|AttributeTargets.Constructor)]
    public class JSchemaAttribute : Attribute
    {

    }
}
