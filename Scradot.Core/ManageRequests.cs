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
    public class ManageRequests : IManageRequests
    {
        public AbstractSpider SpiderAbstract { get; private set; }
        public SemaphoreSlim ConcurrencyRequests { get; private set; }
        public TimeSpan DownloadDelay { get; private set; }
        public int CountRetryRequests { get; private set; }
        private readonly IManageMiddlewares _manageMiddlewares;

        public ManageRequests(AbstractSpider spiderAbstract, IManageMiddlewares midlewares)
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
            foreach (var request in SpiderAbstract.BeginRequests())
            {
                await HandleRequests(request, 1);
            }
            _manageMiddlewares.ExecuteCloseSpider();
        }

        private async Task HandleRequests(Request request, int depth)
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
                foreach (var generator in request.Callback.Invoke(response))
                {
                    if (generator is Request)
                        taskList.Add(HandleRequests(generator as Request, depth + 1));
                    else
                        _manageMiddlewares.ExecuteSendItem(response, generator);
                }
                Task.WaitAll(taskList.ToArray());
            }
        }

        private async Task<Response> RetryRequests(Request request, int depth)
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
