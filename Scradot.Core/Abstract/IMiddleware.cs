using Scradot.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Scradot.Core.Abstract
{
    public interface IMiddleware
    {
        void StartSpider();
        void SendRequest(Request request);
        void ErrorRequest(Request request, HttpResponseMessage httpResponseMessage);
        void ReceivedResponse(Request request, Response response);
        void SendItem(Response response, object item);
        void CloseSpider();
    }
}
