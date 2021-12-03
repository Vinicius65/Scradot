using Scradot.Core.Abstract;
using Scradot.Core.Midleware;
using Scradot.Core.Models;
using System;
using System.Net.Http;

namespace Scradot.Test.Midlewares
{
    public class MyFirstMidleware<TItem> : IMiddleware<TItem>
    {
        public void StartSpider()
        {
            throw new NotImplementedException();
        }

        public void ErrorRequest(Request<TItem> request, HttpResponseMessage httpResponseMessage)
        {
            throw new NotImplementedException();
        }

        public void ReceivedResponse(Request<TItem> request, Response response)
        {
            throw new NotImplementedException();
        }

        public void SendItem(Response response, TItem item) {}

        public void SendRequest(Request<TItem> request)
        {
            throw new NotImplementedException();
        }

        
        public void CloseSpider()
        {
            throw new NotImplementedException();
        }
    }
}
