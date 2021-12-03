using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scradot.Core.Abstract
{
    public interface IManageRequests<TItem>
    {
        IAsyncEnumerable<TItem> StartRequests();
    }
}
