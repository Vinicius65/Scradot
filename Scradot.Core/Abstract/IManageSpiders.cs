using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scradot.Core.Abstract
{
    public interface IManageSpiders
    {
        void StartSpiders();
        IManageSpiders AddMiddleware(IMiddleware middleware);
        IManageSpiders AddSpider(AbstractSpider abstractSpider);
        IManageSpiders AddDepthMiddleware();
        IManageSpiders AddStatisticMiddleware();
        IManageSpiders AddScradotMiddlewares();
    }
}
