using Scradot.Core;
using Scradot.Test.Midlewares;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        public static async Task Main()
        {
            var manager = new ManageRequests<Item>(
                new QuotesSpider(),
                new ManageMidlewares(
                    new List<AbstractMidleware>{
                        new MyFirstMidleware() 
                    }
                )
            );
            await manager.StartSpider();
            Console.WriteLine("Finish");
        }
    }
}
