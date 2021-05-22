using System;
using System.Collections.Generic;
using System.Text;

namespace Utilties.Exceptions
{
    public class Exception : System.Exception
    {
        public Exception()
        {
        }

        public Exception(string message)
            : base(message)
        {
        }

        public Exception(string message, System.Exception inner)
            : base(message, inner)
        {
        }
    }
}
