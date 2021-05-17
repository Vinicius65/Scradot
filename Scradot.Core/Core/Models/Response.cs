using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Scradot.Core.Models
{
    public class Response
    {
        private readonly HtmlDocument _selector;
        public Response(HttpResponseMessage httpResponse, string content, Meta meta = null, DictArgs args = null)
        {
            Url = httpResponse.RequestMessage.RequestUri.AbsoluteUri;
            _selector = new HtmlDocument();
            _selector.LoadHtml(content);
            Content = content;
            Request = httpResponse.RequestMessage;
            Args = args ?? new DictArgs();
            Meta = meta;
        }
        public Meta Meta { get; private set; }
        public DictArgs Args { get; private set; }
        public string Url { get; private set; }
        public string Content { get; private set; }
        public HttpRequestMessage Request { get; private set; }
        public HtmlNodeCollection Xpath(string expression) => _selector.DocumentNode.SelectNodes(expression) ?? new HtmlNodeCollection(null);

    }
}
