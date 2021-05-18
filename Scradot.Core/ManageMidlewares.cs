using Scradot.Core.Abstract;
using Scradot.Core.Midleware;
using Scradot.Core.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Scradot.Core
{
    public class ManageMidlewares : IManageMiddlewares
    {
        public List<IMiddleware> Midlewares { get; private set; }
        public ManageMidlewares(List<IMiddleware> midlewares = null)
        {
            Midlewares = midlewares ?? new();
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

        public void AddMiddleware(IMiddleware middleware)
        {
            Midlewares.Add(middleware);
        }
    }
}
