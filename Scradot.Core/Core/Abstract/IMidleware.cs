using Scradot.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Scradot.Core.Abstract
{
    public interface IMidleware
    {
        public abstract void StartSpider();
        public abstract void SendRequest(Request request);
        public abstract void ErrorRequest(Request request, HttpResponseMessage httpResponseMessage);
        public abstract void ReceivedResponse(Request request, Response response);
        public abstract void SendItem(Response response, object item);
        public abstract void CloseSpider();
    }
}
