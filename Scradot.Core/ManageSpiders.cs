using Scradot.Core.Abstract;
using Scradot.Core.Midleware;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scradot.Core
{
    public class ManageSpiders<TItem> : IManageSpiders<TItem>
    {
        public readonly List<IManageRequests<TItem>> _manageRequestsList;
        private readonly IManageMiddlewares<TItem> _manageMiddlewares;
        public ManageSpiders(IManageMiddlewares<TItem> manageMiddlewares, IEnumerable<IManageRequests<TItem>> manageRequestsList)
        {
            _manageMiddlewares = manageMiddlewares;
            _manageRequestsList = manageRequestsList.ToList();
        }

        public static IManageSpiders<TItem> NewManager() => new ManageSpiders<TItem>(new ManageMidlewares<TItem>(), new List<ManageRequests<TItem>>());

        public async IAsyncEnumerable<TItem> StartSpiders()
        {
            foreach (var spiders in _manageRequestsList.Select(manageRequest => manageRequest.StartRequests()))
            {
                await foreach (var item in spiders)
                {
                    yield return item;
                }
            }
        }
        public IManageSpiders<TItem> AddMiddleware(IMiddleware<TItem> midleware)
        {
            _manageMiddlewares.AddMiddleware(midleware);
            return this;
        }

        public IManageSpiders<TItem> AddSpider(AbstractSpider<TItem> spider)
        {
            _manageRequestsList.Add(new ManageRequests<TItem>(spider, _manageMiddlewares));
            return this;
        }

        public IManageSpiders<TItem> AddDepthMiddleware()
        {
            _manageMiddlewares.AddMiddleware(new SpiderDepthMiddleware<TItem>());
            return this;
        }

        public IManageSpiders<TItem> AddStatisticMiddleware()
        {
            _manageMiddlewares.AddMiddleware(new SpiderStatisticMiddleware<TItem>());
            return this;
        }

        public IManageSpiders<TItem> AddScradotMiddlewares()
        {
            _manageMiddlewares.AddMiddleware(new SpiderStatisticMiddleware<TItem>());
            _manageMiddlewares.AddMiddleware(new SpiderDepthMiddleware<TItem>());
            return this;
        }
    }
}
