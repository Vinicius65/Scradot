using Scradot.Core.Abstract;
using Scradot.Core.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Scradot.Core.Midleware
{
    class SpiderStatisticMiddleware<TItem> : IMiddleware<TItem>
    {
        private bool _stop = false;
        public int NumberOfRequests { get; private set; }
        public int RequestsLastMinute { get; private set; }
        public int NumberOfErrorRequests { get; private set; }
        public int NumberOfResponses { get; private set; }
        public int ResponsesLastMinute { get; private set; }
        public int NumberOfItems { get; private set; }
        public int ItemsLastMinute { get; private set; }

        public void StartSpider()
        {
            Console.WriteLine("Start Statistics Logging Midleware");
            Logging();
        }
        public void SendRequest(Request<TItem> request) => RequestsLastMinute++;
        public void ErrorRequest(Request<TItem> request, HttpResponseMessage httpResponseMessage) => NumberOfErrorRequests++;
        public void ReceivedResponse(Request<TItem> request, Response response) => ResponsesLastMinute++;
        public void SendItem(Response response, TItem item) => ItemsLastMinute++;

        public void CloseSpider()
        {
            _stop = true;
            Print();
        }

        private async void Logging()
        {
            while (!_stop)
            {
                await Task.Delay(TimeSpan.FromSeconds(60));
                Print();
            }
        }

        private void SetCounter()
        {
            NumberOfRequests += RequestsLastMinute;
            NumberOfResponses += ResponsesLastMinute;
            NumberOfItems += ItemsLastMinute;
        }

        private void Print()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            SetCounter();
            Console.WriteLine($"" +
                $"Requests [{NumberOfRequests}] (last minute {RequestsLastMinute}). " +
                $"Responses [{NumberOfResponses}] (last minute {ResponsesLastMinute}). " +
                $"Items [{NumberOfItems}] (last minute {ItemsLastMinute}). " +
                $"Request errors {NumberOfErrorRequests}");
            ResetCounterLastMinute();
            Console.ResetColor();
        }

        private void ResetCounterLastMinute()
        {
            RequestsLastMinute = 0;
            NumberOfResponses = 0;
            NumberOfItems = 0;
        }
    }
}
