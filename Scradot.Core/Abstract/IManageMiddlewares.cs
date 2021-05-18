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
    public interface IManageMiddlewares
    {
        void ExecuteStartSpider();
        void ExecuteSendRequest(Request request);
        void ExecuteErrorRequest(Request request, HttpResponseMessage httpResponseMessage);
        void ExecuteReceivedResponse(Request request, Response response);
        void ExecuteSendItem(Response response, object item);
        void ExecuteCloseSpider();
        void AddMiddleware(IMiddleware middleware);
    }
}
