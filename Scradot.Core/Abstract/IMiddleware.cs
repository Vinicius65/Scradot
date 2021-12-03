using Scradot.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Scradot.Core.Abstract
{
    public interface IMiddleware<TItem>
    {
        void StartSpider();
        void SendRequest(Request<TItem> request);
        void ErrorRequest(Request<TItem> request, HttpResponseMessage httpResponseMessage);
        void ReceivedResponse(Request<TItem> request, Response response);
        void SendItem(Response response, TItem item);
        void CloseSpider();
    }
}
