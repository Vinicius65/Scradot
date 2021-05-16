using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Scradot.Core
{
    public abstract class AbstractMidleware
    {
        public abstract void StartSpider();
        public abstract void SendRequest(Request request);
        public abstract void ErrorRequest(Request request, HttpResponseMessage httpResponseMessage);
        public abstract void ReceivedResponse(Request request, Response response);
        public abstract void SendItem(Response response, AbstractItem item);
        public abstract void CloseSpider();
    }
}
