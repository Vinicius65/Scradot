using Scradot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Base
{
    public abstract class BaseSpider : AbstractSpider<Item>
    {
        public BaseSpider(SpiderConfig spiderConfig = null) : base(spiderConfig)
        {
        }

        public override void HandleItem(Item item)
        {
            //Console.WriteLine($"{item.Autor} - {item.Url}");
        }
    }
}
