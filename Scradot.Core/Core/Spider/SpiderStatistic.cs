using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scradot.Core
{
    class SpiderStatistic
    {
        public int NumberOfRequests { get; private set; }
        public int NumberOfErrorRequests { get; private set; }

        public void AddRequest() => NumberOfRequests++;
        public void AddErrorRequest() => NumberOfErrorRequests++;
    }
}
