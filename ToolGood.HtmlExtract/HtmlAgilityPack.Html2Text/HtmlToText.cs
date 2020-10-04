using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ToolGood.HtmlExtract.HtmlAgilityPack;

namespace HtmlAgilityPack.Html2Text
{
    public static class TextExtractor
    {
        private static HashSet<string> FormElementNames = new HashSet<string> { "label", "textarea", "button", "option", "select" };
        private static HashSet<string> ElementNamesThatImplyNewLine = new HashSet<string> { "li", "div", "br", "p", "h1", "h2", "h3", "h4" };


        public static string ToText(this HtmlNode node)
        {
            StringBuilder writer = new StringBuilder();

            ExtractTextAndWrite(node, writer);

            return writer.ToString();
        }

        //private static void ExtractTextAndWrite(IEnumerable<HtmlNode> nodes, StringBuilder writer)
        //{
        //    foreach (var node in nodes) {
        //        ExtractTextAndWrite(node, writer);
        //    }
        //}

        private static bool ExtractTextAndWrite(HtmlNode node, StringBuilder writer)
        {
            if (node.NodeType == HtmlNodeType.Comment)
                return false;

            if (node.NodeType == HtmlNodeType.Text) {
                var text = node.InnerText;
                text = HtmlEntity.DeEntitize(text);
                text = Regex.Replace(text.Trim(), @"\s+", " ");

                writer.Append(text);
                return true;
            }

            if (node.Name == "script" || node.Name == "style" || FormElementNames.Contains(node.Name))
                return false;

            if (node.Name.Equals("a", StringComparison.OrdinalIgnoreCase)) {
                var siblings = node.ParentNode.ChildNodes;
                if (!siblings.Any(n => n.Name != "a" && !string.IsNullOrEmpty(node.InnerText)))
                    return false;
            }

            if (node.Name.EndsWith("img", StringComparison.OrdinalIgnoreCase)) {
                string altText = node.GetAttributeValue("alt", null);
                if (!string.IsNullOrWhiteSpace(altText)) {
                    writer.Append("[IMG: ");
                    writer.Append(altText);
                    writer.Append("] ");
                    return true;
                }
                return false;
            }

            if (node.Name.Equals("ul", StringComparison.OrdinalIgnoreCase)) {
                writer.Append("\r\n");
                var written = false;
                foreach (var child in node.ChildNodes) {
                    written = ExtractTextAndWrite(child, writer);
                }
                writer.Append("\r\n");
                return true;
            }

            if (node.Name.Equals("li", StringComparison.OrdinalIgnoreCase)) {
                writer.Append("* ");
                var written = false;
                foreach (var child in node.ChildNodes) {
                    written = ExtractTextAndWrite(child, writer);
                }
                writer.Append("\r\n");
                return true;
            }

            if (node.Name.Equals("tr", StringComparison.OrdinalIgnoreCase)) {
                writer.Append("| ");
                var written = false;
                foreach (var child in node.ChildNodes) {
                    written = ExtractTextAndWrite(child, writer);
                }
                writer.Append("\r\n\r\n");
                return true;
            }

            if (node.Name.Equals("td", StringComparison.OrdinalIgnoreCase)|| node.Name.EndsWith("th", StringComparison.OrdinalIgnoreCase)) {
                var written = false;
                foreach (var child in node.ChildNodes) {
                    written = ExtractTextAndWrite(child, writer);
                }
                writer.Append("| ");
                return false;
            }


            {
                var written = false;
                foreach (var child in node.ChildNodes) {
                    if (ExtractTextAndWrite(child, writer)) {
                        written = true;
                    }
                }
                if (written && ElementNamesThatImplyNewLine.Contains(node.Name))
                    writer.AppendLine();

                return written;
            }
   
        }
    }
}
