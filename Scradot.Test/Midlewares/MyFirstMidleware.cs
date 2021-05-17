using Scradot.Core.Abstract;
using Scradot.Core.Models;
using System;
using System.Net.Http;

namespace Scradot.Test.Midlewares
{
    public class MyFirstMidleware : IMidleware
    {
        public void StartSpider()
        {
            throw new NotImplementedException();
        }

        public void ErrorRequest(Request request, HttpResponseMessage httpResponseMessage)
        {
            throw new NotImplementedException();
        }

        public void ReceivedResponse(Request request, Response response)
        {
            throw new NotImplementedException();
        }

        public void SendItem(Response response, AbstractItem item)
        {
            throw new NotImplementedException();
        }

        public void SendRequest(Request request)
        {
            throw new NotImplementedException();
        }

        
        public void CloseSpider()
        {
            throw new NotImplementedException();
        }
    }
}
