using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLinker.Exceptions
{
    public class LinkerException : Exception
    {
        public LinkerException(string errorMessage, Exception tryCatchException) : base(errorMessage, tryCatchException) { }
        public LinkerException(string errorMessage) : base(errorMessage) { }
    }
}
