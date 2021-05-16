using System;

namespace Scradot.Core
{
    public class SpiderConfig
    {
        public TimeSpan DownloadDelay { get; private set; }
        public int ConcurrencyRequests { get; private set; }
        public int RetryRequests { get; private set; }

        public SpiderConfig(TimeSpan downloadDelay, int concurrencyRequests, int retryRequests)
        {
            DownloadDelay = downloadDelay;
            ConcurrencyRequests = concurrencyRequests;
            RetryRequests = retryRequests;
        }
    }
}