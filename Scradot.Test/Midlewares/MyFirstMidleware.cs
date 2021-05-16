using Scradot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Scradot.Test.Midlewares
{
    public class MyFirstMidleware : AbstractMidleware
    {
        public override void StartSpider()
        {
            Console.WriteLine("Inicio spider pelo midleware");
        }

        public override void ErrorRequest(Request request, HttpResponseMessage httpResponseMessage)
        {
            throw new NotImplementedException();
        }

        public override void ReceivedResponse(Request request, Response response)
        {
            throw new NotImplementedException();
        }

        public override void SendItem(Response response, AbstractItem item)
        {
            throw new NotImplementedException();
        }

        public override void SendRequest(Request request)
        {
            throw new NotImplementedException();
        }

        
        public override void CloseSpider()
        {
            throw new NotImplementedException();
        }
    }
}
