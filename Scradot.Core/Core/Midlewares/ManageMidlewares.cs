using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Scradot.Core
{
    public class ManageMidlewares
    {
        public List<AbstractMidleware> Midlewares { get; private set; }
        public ManageMidlewares(List<AbstractMidleware> midlewares = null)
        {
            Midlewares = midlewares ?? new();
        }
        
        public void ExecuteStartSpider() => Execute(() => Midlewares.ForEach(midleware => midleware.StartSpider()));
        public void ExecuteSendRequest(Request request) => Execute(() => Midlewares.ForEach(midleware => midleware.SendRequest(request)));
        public void ExecuteErrorRequest(Request request, HttpResponseMessage httpResponseMessage) => Execute(() => Midlewares.ForEach(midleware => midleware.ErrorRequest(request, httpResponseMessage)));
        public void ExecuteReceivedResponse(Request request, Response response) => Execute(() => Midlewares.ForEach(midleware => midleware.ReceivedResponse(request, response)));
        public void ExecuteSendItem(Response response, AbstractItem item) => Execute(() => Midlewares.ForEach(midleware => midleware.SendItem(response, item)));
        public void ExecuteCloseSpider() => Execute(() => Midlewares.ForEach(midleware => midleware.CloseSpider()));

        private void Execute(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch { }
        }
    }
}
