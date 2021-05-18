using Scradot.Core.Abstract;
using Scradot.Core.Midleware;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scradot.Core
{
    public class ManageSpiders : IManageSpiders
    {
        public readonly List<IManageRequests> _manageRequestsList;
        private readonly IManageMiddlewares _manageMiddlewares;
        public ManageSpiders(IManageMiddlewares manageMiddlewares, IEnumerable<IManageRequests> manageRequestsList)
        {
            _manageMiddlewares = manageMiddlewares;
            _manageRequestsList = manageRequestsList.ToList();
        }

        public static IManageSpiders NewManage() => new ManageSpiders(new ManageMidlewares(), new List<ManageRequests>());

        public void StartSpiders()
        {
            var taskList = _manageRequestsList.Select(manageRequest => manageRequest.StartRequests());
            Task.WaitAll(taskList.ToArray());
        }
        public IManageSpiders AddMiddleware(IMiddleware midleware)
        {
            _manageMiddlewares.AddMiddleware(midleware);
            return this;
        }

        public IManageSpiders AddSpider(AbstractSpider spider)
        {
            _manageRequestsList.Add(new ManageRequests(spider, _manageMiddlewares));
            return this;
        }

        public IManageSpiders AddDepthMiddleware()
        {
            _manageMiddlewares.AddMiddleware(new SpiderDepthMiddleware());
            return this;
        }

        public IManageSpiders AddStatisticMiddleware()
        {
            _manageMiddlewares.AddMiddleware(new SpiderStatisticMiddleware());
            return this;
        }

        public IManageSpiders AddScradotMiddlewares()
        {
            _manageMiddlewares.AddMiddleware(new SpiderStatisticMiddleware());
            _manageMiddlewares.AddMiddleware(new SpiderDepthMiddleware());
            return this;
        }
    }
}
