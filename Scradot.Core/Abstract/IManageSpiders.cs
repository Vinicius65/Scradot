using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scradot.Core.Abstract
{
    public interface IManageSpiders<TItem>
    {
        IAsyncEnumerable<TItem> StartSpiders();
        IManageSpiders<TItem> AddMiddleware(IMiddleware<TItem> middleware);
        IManageSpiders<TItem> AddSpider(AbstractSpider<TItem> abstractSpider);
        IManageSpiders<TItem> AddDepthMiddleware();
        IManageSpiders<TItem> AddStatisticMiddleware();
        IManageSpiders<TItem> AddScradotMiddlewares();
    }
}
