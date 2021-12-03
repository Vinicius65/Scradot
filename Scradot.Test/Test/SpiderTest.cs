using Scradot.Core;
using Scradot.Test.Midlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Scradot.Test.Test
{
    public class SpiderTest
    {
        [Fact]
        public async void SpiderOneTest()
        {
            var man = ManageSpiders<Item>.NewManager()
             .AddScradotMiddlewares()
             .AddMiddleware(new MyFirstMidleware<Item>())
             .AddSpider(new QuotesSpiderOne());
            await foreach (var item in man.StartSpiders())
            {
                Console.WriteLine(item);
            }
        }
    }
}
