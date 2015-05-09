using HtmlAgilityPack;
using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyauthWeb
{
    public static class HtmlSanitizer
    {
        private static readonly string[] TagWhitelist = { "a", "div", "p", "br", "img", "ul", "li", "ol", "pre" };
        private static readonly string[] AttributeWhitelist = { "href", "src" };

        public static HtmlString Sanitized(this IHtmlHelper helper, string str)
        {
            var root = HtmlNode.CreateNode(str).ParentNode;
            WriteNode(root, true);
            return new HtmlString(root.WriteTo());
        }

        private static void WriteNode(HtmlNode node, bool root = false)
        {
            if (node is HtmlTextNode)
                return;

            foreach (var child in node.ChildNodes)
                WriteNode(child);

            if (!TagWhitelist.Contains(node.Name))
                node.Name = "div";

            foreach (var attr in node.Attributes.Select(x => x.Name).Where(x => !AttributeWhitelist.Contains(x)).ToArray())
                node.Attributes.Remove(attr);
        }
    }
}
