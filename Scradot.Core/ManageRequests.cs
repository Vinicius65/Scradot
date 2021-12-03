using Scradot.Core.Abstract;
using Scradot.Core.Exceptions;
using Scradot.Core.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Scradot.Core
{
    public class ManageRequests<TItem> : IManageRequests
    {
        public AbstractSpider<TItem> SpiderAbstract { get; private set; }
        public SemaphoreSlim ConcurrencyRequests { get; private set; }
        public TimeSpan DownloadDelay { get; private set; }
        public int CountRetryRequests { get; private set; }
        private readonly IManageMiddlewares<TItem> _manageMiddlewares;

        public ManageRequests(AbstractSpider<TItem> spiderAbstract, IManageMiddlewares<TItem> midlewares)
        {
            _manageMiddlewares = midlewares;
            SpiderAbstract = spiderAbstract;

            CountRetryRequests = spiderAbstract.SpiderConfig.RetryRequests;
            ConcurrencyRequests = new SemaphoreSlim(spiderAbstract.SpiderConfig.ConcurrencyRequests);
            DownloadDelay = spiderAbstract.SpiderConfig.DownloadDelay;
        }

        public async Task StartRequests()
        {
            _manageMiddlewares.ExecuteStartSpider();
            foreach (var (item, request) in SpiderAbstract.BeginRequests())
            {
                await HandleRequests(item, request, 1);
            }
            _manageMiddlewares.ExecuteCloseSpider();
        }

        private async Task HandleRequests(TItem item, Request<TItem> request, int depth)
        {
            await ConcurrencyRequests.WaitAsync();
            await Task.Delay(DownloadDelay);

            Response response = null;
            var success = true;
            try
            {
                _manageMiddlewares.ExecuteSendRequest(request);
                response = await RetryRequests(request, depth);
            }
            catch(RequestException exception)
            {
                success = false;
                _manageMiddlewares.ExecuteErrorRequest(request, exception.ResponseMessge);
            }
            ConcurrencyRequests.Release();

            if (success)
            {
                _manageMiddlewares.ExecuteReceivedResponse(request, response);

                var taskList = new List<Task>();
                foreach (var (newItem, newRequest) in request.Callback.Invoke(response))
                {
                    if (newRequest != null)
                        taskList.Add(HandleRequests(item, newRequest, depth + 1));
                    else if (newItem != null)
                        _manageMiddlewares.ExecuteSendItem(response, newItem);
                }
                Task.WaitAll(taskList.ToArray());
            }
        }

        private async Task<Response> RetryRequests(Request<TItem> request, int depth)
        {
            HttpResponseMessage responseMessage = null;
            for (int index = 0; index < CountRetryRequests; index++)
            {
                try
                {
                    using var httpClient = new HttpClient();
                    var httpRequest = request.CreateHttpRequestMessage();
                    var timePrev = DateTime.Now;
                    var httpResponse = await httpClient.SendAsync(httpRequest);
                    var timeDownloadDelay = DateTime.Now - timePrev;
                    responseMessage = httpResponse;
                    httpResponse.EnsureSuccessStatusCode();
                    return new Response(
                        httpResponse: httpResponse, 
                        content: await httpResponse.Content.ReadAsStringAsync(),
                        meta: new Meta(depth, timeDownloadDelay, index),
                        args: request.Args
                   );
                }
                catch { }
            }
            throw new RequestException($"Não foi possível fazer a requição para a url {request.Url} após {CountRetryRequests} tentativas.", responseMessage: responseMessage);
        }
    }
}
