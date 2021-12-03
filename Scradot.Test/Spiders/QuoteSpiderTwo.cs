using Scradot.Core;
using Scradot.Core.Abstract;
using Scradot.Core.Models;
using Scradot.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scradot.Test
{
    public class QuotesSpiderTwo : AbstractSpider<Item>
    {
        public QuotesSpiderTwo() : base(new SpiderConfig(TimeSpan.FromSeconds(1), 4, 3)) { }

        public override IEnumerable<(Item, Request<Item>)> BeginRequests()
        {
            yield return (null, new Request<Item>(url: "http://quotes.toscrape.com", callback: Parse));
        }

        public override IEnumerable<(Item, Request<Item>)> Parse(Response response)
        {

            foreach (var nextPage in Enumerable.Range(2, 9))
            {
                var nextUrlPage = $"http://quotes.toscrape.com/page/{nextPage}";
                yield return (null, new Request<Item>(url: nextUrlPage, ParseList));
            }
            ParseList(response);
        }

        public IEnumerable<(Item, Request<Item>)> ParseList(Response response)
        {
            Console.WriteLine($"SPIDER TWO: {response.Url}");

            foreach (var sel in response.Xpath("//div[@class='quote']").Select((div, index) => new { Div = div, Index = index }))
            {
                var baseUrl = "http://quotes.toscrape.com";
                var partial = sel.Div.SelectSingleNode(".//a").GetAttr("href");
                yield return (null, new Request<Item>(url: baseUrl + partial, callback: ParseItem));
            }
        }

        public IEnumerable<(Item, Request<Item>)> ParseEnterPage(Response response)
        {
            foreach (var div in response.Xpath("//div[@class='quote']"))
            {
                var baseUrl = "http://quotes.toscrape.com";
                var partial = div.SelectSingleNode(".//a").GetAttr("href");
                yield return (null, new Request<Item>(url: baseUrl + partial, callback: ParseItem));
            }
        }

        public IEnumerable<(Item, Request<Item>)> ParseItem(Response response)
        {
            yield return (new Item
            {
                Autor = response.Url.Split("/").SkipLast(1).Last(),
                Url = response.Url,
            }, null);
        }
    }
}
