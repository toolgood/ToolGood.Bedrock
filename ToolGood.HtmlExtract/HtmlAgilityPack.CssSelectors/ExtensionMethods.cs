using System;
using System.Collections.Generic;
using System.Linq;

namespace ToolGood.HtmlExtract.HtmlAgilityPack
{
    public static partial class HapCssExtensionMethods
    {
        public static HtmlNode QuerySelector(this HtmlDocument doc, string cssSelector)
        {
            return doc.QuerySelectorAll(cssSelector).FirstOrDefault();
        }

        public static HtmlNode QuerySelector(this HtmlNode node, string cssSelector)
        {
            return node.QuerySelectorAll(cssSelector).FirstOrDefault();
        }

        public static IList<HtmlNode> QuerySelectorAll(this HtmlDocument doc, string cssSelector)
        {
            return doc.DocumentNode.QuerySelectorAll(cssSelector);
        }

        public static IList<HtmlNode> QuerySelectorAll(this HtmlNode node, string cssSelector)
        {
            return new[] { node }.QuerySelectorAll(cssSelector);
        }
        public static IList<HtmlNode> QuerySelectorAll(this IEnumerable<HtmlNode> nodes, string cssSelector)
        {
            if (cssSelector == null)
                throw new ArgumentNullException(nameof(cssSelector));

            if (cssSelector.Contains(','))
            {
                var combinedSelectors = cssSelector.Split(',');
                var rt = nodes.QuerySelectorAll(combinedSelectors[0]);
                foreach (var s in combinedSelectors.Skip(1))
                    foreach (var n in nodes.QuerySelectorAll(s))
                        if (!rt.Contains(n))
                            rt.Add(n);

                return rt;
            }

            cssSelector = cssSelector.Trim();

            var selectors = CssSelector.Parse(cssSelector);

            bool allowTraverse = true;

            for (int i = 0; i < selectors.Count; i++)
            {
                var selector = selectors[i];

                if (allowTraverse && selector.AllowTraverse)
                {
                    // If this is not the first selector then we must make filter against the child nodes of the current set of nodes
                    // since any selector that follows another selector always scopes down the nodes to the descendants of the last scope.
                    // Example: "span span" Should only resolve with span elements that are descendants of another span element.
                    // Any span elements that are not descendant of another span element shoud not be included in the output.
                    if (i > 0)
                    {
                        nodes = nodes.SelectMany(n => n.ChildNodes);
                    }

                    nodes = Traverse(nodes);
                }

                nodes = selector.Filter(nodes);
                allowTraverse = selector.AllowTraverse;
            }

            return nodes.Distinct().ToList();
        }


        private static IEnumerable<HtmlNode> Traverse(IEnumerable<HtmlNode> nodes)
        {
            foreach (var node in nodes)
                foreach (var n in Traverse(node).Where(i => i.NodeType == HtmlNodeType.Element))
                    yield return n;
        }
        private static IEnumerable<HtmlNode> Traverse(HtmlNode node)
        {
            yield return node;

            foreach (var child in node.ChildNodes)
                foreach (var n in Traverse(child))
                    yield return n;
        }

        public static void RemoveOthers(this HtmlDocument doc, params string[] cssSelectors)
        {
            List<HtmlNode> nodes = new List<HtmlNode>();

            foreach (var css in cssSelectors) {
                var nds = doc.QuerySelectorAll(css);
                foreach (var nd in nds) {
                    nodes.Add(nd);
                    var root = nd;
                    while (root.ParentNode != null) {
                        nodes.Add(root);
                        root = root.ParentNode;
                    }
                }
            }
            foreach (var node in nodes) {
                var pNode = node.ParentNode;
                var removeNode = new List<HtmlNode>();
                foreach (var nd in pNode.ChildNodes) {
                    if (nodes.Contains(nd)) { continue; }
                    removeNode.Add(nd);
                }
                foreach (var nd in removeNode) {
                    nd.Remove();
                }
                removeNode.Clear();
                removeNode = null;
            }
            nodes.Clear();
            nodes = null;
        }


        public static string ToHtml(this HtmlDocument doc)
        {
            return doc.DocumentNode.OuterHtml;
            //StringWriter stringWriter = new StringWriter();
            //doc.Save(stringWriter);
            //var html = stringWriter.ToString();
            //stringWriter.Dispose();
            //stringWriter = null;
            //return html;
        }
    }
}