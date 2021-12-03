using Scradot.Core.Abstract;
using Scradot.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Scradot.Core.Midleware
{
    public class SpiderDepthMiddleware<TItem> : IMiddleware<TItem>
    {
        private bool _stop = false;
        public Dictionary<int, int> DepthRequests { get; private set; } = new() { { 1, 0} };
   
        public async void Logging()
        {
            while (!_stop)
            {
                await Task.Delay(TimeSpan.FromSeconds(60));
                Print();
            }
        }

        private void Print()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"DepthRequests: [1][{DepthRequests[1]}] ");
            foreach (var depth in DepthRequests.Keys.ToList().Skip(1))
            {
                Console.Write($"--> [{depth}][{DepthRequests[depth]}] ");
            }
            Console.ResetColor();
        }

        public void StartSpider()
        {
            Console.WriteLine("Start Depth Logging Middleware");
            Logging();
        }

        public void SendRequest(Request<TItem> request){}

        public void ErrorRequest(Request<TItem> request, HttpResponseMessage httpResponseMessage){}

        public void ReceivedResponse(Request<TItem> request, Response response)
        {
            if (DepthRequests.TryGetValue(response.Meta.Depth, out _))
                DepthRequests[response.Meta.Depth]++;
            else
                DepthRequests.Add(response.Meta.Depth, 1);
        }

        public void SendItem(Response response, TItem item){ }

        public void CloseSpider()
        {
            _stop = true;
            Print();
        }
    }
}
