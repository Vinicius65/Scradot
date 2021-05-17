using Scradot.Core.Abstract;
using Scradot.Core.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Scradot.Core.Midleware
{
    public class ManageMidlewares
    {
        public List<IMidleware> Midlewares { get; private set; }
        public ManageMidlewares(List<IMidleware> midlewares = null)
        {
            Midlewares = midlewares ?? new();
            Midlewares.Add(new SpiderStatisticMidleware());
            Midlewares.Add(new SpiderDepthMidleware());
        }

        public void ExecuteStartSpider() => Midlewares.ForEach(midleware => Execute(() => midleware.StartSpider()));
        public void ExecuteSendRequest(Request request) => Midlewares.ForEach(midleware => Execute(() => midleware.SendRequest(request)));
        public void ExecuteErrorRequest(Request request, HttpResponseMessage httpResponseMessage) => Midlewares.ForEach(midleware => Execute(() => midleware.ErrorRequest(request, httpResponseMessage)));
        public void ExecuteReceivedResponse(Request request, Response response) => Midlewares.ForEach(midleware => Execute(() => midleware.ReceivedResponse(request, response)));
        public void ExecuteSendItem(Response response, object item) => Midlewares.ForEach(midleware => Execute(() => midleware.SendItem(response, item)));
        public void ExecuteCloseSpider() => Midlewares.ForEach(midleware => Execute(() => midleware.CloseSpider()));

        private void Execute(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch {}
        }
    }
}
