using Scradot.ConsoleApp.Midlewares;
using Scradot.ConsoleApp.Spiders;
using Scradot.Core;
using Scradot.Core.Midleware;
using System;
using System.Threading.Tasks;

namespace Scradot.ConsoleApp
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var man = ManageSpiders<Item>.NewManager()
              .AddScradotMiddlewares()
              .AddMiddleware(new MyFirstMidleware<Item>())
              .AddSpider(new QuotesSpiderOne());
            await foreach (var item in man.StartSpiders())
            {
                Console.WriteLine(item.ToString());
            }
        }
    }
}
