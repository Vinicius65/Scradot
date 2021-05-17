using Scradot.Core;
using Scradot.Core.Abstract;
using Scradot.Core.Midleware;
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
            var manager = new ManageSpider(
                new QuotesSpider(),
                new ManageMidlewares(
                    new List<IMidleware>{
                        new MyFirstMidleware() 
                    }
                )
            );
            await manager.StartSpider();
            Console.WriteLine("Finish");
        }
    }
}
