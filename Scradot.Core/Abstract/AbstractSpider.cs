using Scradot.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scradot.Core.Abstract
{
    public abstract class AbstractSpider<TItem>
    { 
        public SpiderConfig SpiderConfig { get; private set; }
        public AbstractSpider(SpiderConfig spiderConfig = null)
        {
            SpiderConfig = spiderConfig ?? new SpiderConfig(
                downloadDelay: TimeSpan.FromSeconds(1.5), 
                concurrencyRequests: 10, 
                retryRequests: 3
            );
        }

        public abstract IEnumerable<Request<TItem>> BeginRequests();
        public abstract IEnumerable<(TItem, Request<TItem>)> Parse(Response response);

    }
}
