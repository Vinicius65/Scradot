using HtmlAgilityPack;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text.Json.Serialization;
using Scradot.Core.Configuration;

namespace Scradot.Core.Extensions
{
    public static class SelectorExtension
    {

        public static string Text(this HtmlNode htmlNode)
        {
            if (htmlNode is null) return null;
            return htmlNode.InnerText.Trim();
        }

        public static string Text(this HtmlNodeCollection htmlNodeCollection)
        {
            if (htmlNodeCollection is null) return null;
            var firsElement = htmlNodeCollection.FirstOrDefault();
            if (firsElement is null) return null;
            return firsElement.InnerText.Trim();
        }

        public static string TextAll(this HtmlNodeCollection htmlNodeCollection)
        {
            if (htmlNodeCollection is null) return null;
            return string.Join("", htmlNodeCollection.Select(e => e)).Trim();
        }

        public static string Html(this HtmlNode htmlNode)
        {
            if (htmlNode is null) return null;
            return htmlNode.InnerHtml.Trim();
        }

        public static string GetAttr(this HtmlNode htmlNode, string attr)
        {
            if (htmlNode is null) return null;
            return htmlNode.GetAttributeValue(attr, null);
        }

        public static string GetAttr(this HtmlNodeCollection htmlNodeCollection, string attr)
        {
            if (htmlNodeCollection is null) return null;
            foreach (var element in htmlNodeCollection)
            {
                if (element != null)
                {
                    var attrValue = element.GetAttributeValue(attr, null);
                    if (attrValue != null) return attrValue;
                }
            }
            return null;
        }

        public static string ReFirst(this HtmlNode htmlNode, string expression, RegexOptions regexOptions = RegexOptions.Multiline)
        {
            if (htmlNode is null || htmlNode.InnerText is null) return null;
            var re = Regex.Match(htmlNode.InnerText, expression, regexOptions);
            if (!re.Success) return null;
            return re.Groups.Values.ToList().Last().Value;
        }

        public static bool ReHas(this HtmlNode htmlNode, string expression)
        {
            if (htmlNode is null || htmlNode.InnerText is null) return false;
            return Regex.IsMatch(htmlNode.InnerHtml, expression, RegexOptions.Multiline & RegexOptions.IgnoreCase);
        }
    }
}
