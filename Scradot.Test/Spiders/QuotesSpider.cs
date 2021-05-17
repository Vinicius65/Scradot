using Scradot.Core;
using Scradot.Core.Abstract;
using Scradot.Core.Models;
using Scradot.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    public class QuotesSpider : AbstractSpider
    {
        public QuotesSpider() : base(new SpiderConfig(TimeSpan.FromSeconds(1), 4, 3)) {}

        public override  IEnumerable<Request> BeginRequests()
        {
            yield return new Request(url: "http://quotes.toscrape.com", callback: Parse);
        }

        public override  IEnumerable<object> Parse(Response response)
        {
            Console.WriteLine(response.Url);

            var nextPage = response.Xpath("//li[@class='next']/a").GetAttr("href");
            if (nextPage != null)
            {
                var nextUrlPage = $"http://quotes.toscrape.com/{nextPage}";
                yield return new Request(url: nextUrlPage, Parse);
            }

            var currentPage = int.Parse(response.Url.ReFirst(@"\d+") ?? "1");

            foreach (var sel in response.Xpath("//div[@class='quote']").Select((div, index) => new { Div = div, Index = index }))
            {
                var baseUrl = "http://quotes.toscrape.com";
                var partial = sel.Div.SelectSingleNode(".//a").GetAttr("href");
                yield return new Request(url: baseUrl + partial, callback: ParseItem, args: new DictArgs { {"pageOrder", $"{currentPage}{sel.Index}" } });
            }
        }

        public IEnumerable<object> ParseEnterPage(Response response)
        {
            foreach (var div in response.Xpath("//div[@class='quote']"))
            {
                var baseUrl = "http://quotes.toscrape.com";
                var partial = div.SelectSingleNode(".//a").GetAttr("href");
                yield return new Request(url: baseUrl + partial, callback: ParseItem);
            }
        }

        public IEnumerable<object> ParseItem(Response response)
        {
            yield return new Item
            {
                Autor = response.Url.Split("/").SkipLast(1).Last(),
                Url = response.Url,
            };
        }
    }
}
