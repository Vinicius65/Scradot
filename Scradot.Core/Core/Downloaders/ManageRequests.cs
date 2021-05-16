using Scradot.Core.Exceptions;
using Scradot.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scradot.Core
{
    public class ManageRequests<T> where T : AbstractItem
    {
        public AbstractSpider<T> SpiderAbstract { get; private set; }
        private readonly SpiderStatistic spiderStatistic;
        private readonly SpiderDepthLog spiderDepthLog;
        public SemaphoreSlim ConcurrencyRequests { get; private set; }
        public TimeSpan DownloadDelay { get; private set; }
        public int CountRetryRequests { get; private set; }
        public ManageMidlewares Midlewares { get; private set; }

        public ManageRequests(AbstractSpider<T> spiderAbstract, ManageMidlewares midlewares = null)
        {
            Midlewares = midlewares ?? new ManageMidlewares();
            SpiderAbstract = spiderAbstract;
            spiderStatistic = new SpiderStatistic();
            spiderDepthLog = new SpiderDepthLog();

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
            SpiderAbstract.Close();
        }

        private async Task HandleRequests(Request request, int depth)
        {
            await ConcurrencyRequests.WaitAsync();
            await Task.Delay(DownloadDelay);

            Response response = null;
            var success = true;
            try
            {
                spiderStatistic.AddRequest();
                spiderDepthLog.AddDepthRequest(depth);

                Midlewares.ExecuteSendRequest(request);

                response = await RetryRequests(request);
            }
            catch(RequestException exception)
            {
                success = false;
                spiderStatistic.AddErrorRequest();

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
                    {
                        Midlewares.ExecuteSendItem(response, generator as T);
                        SpiderAbstract.HandleItem(generator as T);
                    }
                }
                Task.WaitAll(taskList.ToArray());
            }
        }

        public async Task<Response> RetryRequests(Request request)
        {
            HttpResponseMessage responseMessage = null;
            for (int index = 0; index < CountRetryRequests; index++)
            {
                try
                {
                    using var httpClient = new HttpClient();
                    var httpRequest = request.CreateHttpRequestMessage();
                    var httpResponse = await httpClient.SendAsync(httpRequest);
                    responseMessage = httpResponse;
                    httpResponse.EnsureSuccessStatusCode();
                    return new Response(httpResponse, await httpResponse.Content.ReadAsStringAsync(), request.Args);
                }
                catch { }
            }
            throw new RequestException($"Não foi possível fazer a requição para a url {request.Url} após {CountRetryRequests} tentativas.", responseMessage: responseMessage);
        }
    }
}
