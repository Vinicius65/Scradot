using Scradot.Core.Abstract;
using Scradot.Core.Exceptions;
using Scradot.Core.Midleware;
using Scradot.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scradot.Core
{
    public class ManageSpider
    {
        public AbstractSpider SpiderAbstract { get; private set; }
        public SemaphoreSlim ConcurrencyRequests { get; private set; }
        public TimeSpan DownloadDelay { get; private set; }
        public int CountRetryRequests { get; private set; }
        public ManageMidlewares Midlewares { get; private set; }

        public ManageSpider(AbstractSpider spiderAbstract, ManageMidlewares midlewares = null)
        {
            Midlewares = midlewares ?? new ManageMidlewares();
            SpiderAbstract = spiderAbstract;

            CountRetryRequests = spiderAbstract.SpiderConfig.RetryRequests;
            ConcurrencyRequests = new SemaphoreSlim(spiderAbstract.SpiderConfig.ConcurrencyRequests);
            DownloadDelay = spiderAbstract.SpiderConfig.DownloadDelay;
        }

        public async Task StartSpider()
        {
            Midlewares.ExecuteStartSpider();
            foreach (var request in SpiderAbstract.BeginRequests())
            {
                await HandleRequests(request, 1);
            }
            Midlewares.ExecuteCloseSpider();
        }

        private async Task HandleRequests(Request request, int depth)
        {
            await ConcurrencyRequests.WaitAsync();
            await Task.Delay(DownloadDelay);

            Response response = null;
            var success = true;
            try
            {
                Midlewares.ExecuteSendRequest(request);
                response = await RetryRequests(request, depth);
            }
            catch(RequestException exception)
            {
                success = false;
                Midlewares.ExecuteErrorRequest(request, exception.ResponseMessge);
            }
            ConcurrencyRequests.Release();

            if (success)
            {
                Midlewares.ExecuteReceivedResponse(request, response);

                var taskList = new List<Task>();
                foreach (var generator in request.Callback.Invoke(response))
                {
                    if (generator is Request)
                        taskList.Add(HandleRequests(generator as Request, depth + 1));
                    else
                        Midlewares.ExecuteSendItem(response, generator);
                }
                Task.WaitAll(taskList.ToArray());
            }
        }

        public async Task<Response> RetryRequests(Request request, int depth)
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
