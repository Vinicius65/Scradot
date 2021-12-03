using Scradot.ConsoleApp;
using Scradot.ConsoleApp.Midlewares;
using Scradot.ConsoleApp.Spiders;
using Scradot.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Scradot.Test.Test
{
    public class SpiderTest
    {
        private readonly ITestOutputHelper output;
        public SpiderTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public async void SpiderOneTest()
        {
            var man = ManageSpiders<Item>.NewManager()
                .AddScradotMiddlewares()
                .AddMiddleware(new MyFirstMidleware<Item>())
                .AddSpider(new QuotesSpiderOne());
            await foreach (var item in man.StartSpiders())
            {
                output.WriteLine(item.ToString());
            }
        }
    }
}
