using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Scradot.Core.Exceptions
{
    public class RequestException : Exception
    {
        public RequestException() { }
        public RequestException(string message) : base(message) { }
        public RequestException(string message, Exception inner) : base(message, inner) { }
        public RequestException(string message, Exception inner = null, HttpResponseMessage responseMessage = null) : base(message, inner)
        {
            ResponseMessge = responseMessage;
        }
        public HttpResponseMessage ResponseMessge { get; }
    }
}
