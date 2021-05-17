using Scradot.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Scradot.Core.Models
{
    public class Request : Generator
    {
        public Request(string url, Func<Response, IEnumerable<Generator>> callback, HttpMethod method = null, DictHeaders headers = null, HttpContent content = null, DictArgs args = null)
        {
            Url = url;
            Callback = callback;
            Method = method ?? HttpMethod.Get;
            Headers = headers;
            Content = content;
            Args = args;
        }

        public string Url { get; private set; }
        public Func<Response, IEnumerable<Generator>> Callback { get; set; }
        public HttpMethod Method { get; private set; }
        public DictHeaders Headers { get; private set; }
        public HttpContent Content { get; private set; }
        public DictArgs Args { get; private set; }

        public HttpRequestMessage CreateHttpRequestMessage()
        {
            var httpRequest = new HttpRequestMessage(Method, Url) { Content = Content };
            if (Headers != null)
                foreach (var item in Headers)
                    httpRequest.Headers.Add(item.Key, item.Value);
            return httpRequest;
        }
    }
}
