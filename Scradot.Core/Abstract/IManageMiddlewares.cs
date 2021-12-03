using Scradot.Core.Abstract;
using Scradot.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Scradot.Core.Abstract
{
    public interface IManageMiddlewares<TItem>
    {
        void ExecuteStartSpider();
        void ExecuteSendRequest(Request<TItem> request);
        void ExecuteErrorRequest(Request<TItem> request, HttpResponseMessage httpResponseMessage);
        void ExecuteReceivedResponse(Request<TItem> request, Response response);
        void ExecuteSendItem(Response response, TItem item);
        void ExecuteCloseSpider();
        void AddMiddleware(IMiddleware<TItem> middleware);
    }
}
