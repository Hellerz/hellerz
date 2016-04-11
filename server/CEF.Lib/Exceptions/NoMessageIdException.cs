using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEF.Lib.Exceptions
{
    public class NoMessageIdException:Exception
    {
        public NoMessageIdException(string message):base(message)
        {
        }
    }
}
