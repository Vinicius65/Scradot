using Scradot.Core;
using Scradot.Core.Midleware;
using Scradot.Test.Midlewares;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scradot.Test
{
    class Program
    {
        public static void Main()
        {
            ManageSpiders<Item>.NewManager()
                .AddScradotMiddlewares()
                .AddMiddleware(new MyFirstMidleware<Item>())
                .AddSpider(new QuotesSpiderOne())
                .AddSpider(new QuotesSpiderTwo())
                .StartSpiders();
        }
    }
}
