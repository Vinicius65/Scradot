using Scradot.Core.Abstract;
using Scradot.Core.Midleware;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scradot.Core
{
    public class ManageSpiders<TItem> : IManageSpiders<TItem>
    {
        public readonly List<IManageRequests> _manageRequestsList;
        private readonly IManageMiddlewares<TItem> _manageMiddlewares;
        public ManageSpiders(IManageMiddlewares<TItem> manageMiddlewares, IEnumerable<IManageRequests> manageRequestsList)
        {
            _manageMiddlewares = manageMiddlewares;
            _manageRequestsList = manageRequestsList.ToList();
        }

        public static IManageSpiders<TItem> NewManager() => new ManageSpiders<TItem>(new ManageMidlewares<TItem>(), new List<ManageRequests<TItem>>());

        public void StartSpiders()
        {
            var taskList = _manageRequestsList.Select(manageRequest => manageRequest.StartRequests());
            Task.WaitAll(taskList.ToArray());
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
