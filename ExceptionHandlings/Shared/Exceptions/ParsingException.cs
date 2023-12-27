using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionHandlings.Shared.Exceptions
{
    public class ParsingException : Exception
    {
        public ParsingException(string Message)
            : base(Message)
        {

        }
        public ParsingException(string message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
