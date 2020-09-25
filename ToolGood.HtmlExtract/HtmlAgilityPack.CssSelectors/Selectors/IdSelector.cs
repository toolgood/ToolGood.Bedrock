using System;
using System.Collections.Generic;

namespace ToolGood.HtmlExtract.HtmlAgilityPack.CssSelectors.Selectors
{
    internal class IdSelector : CssSelector
    {
        public override string Token => "#";

        protected internal override IEnumerable<HtmlNode> FilterCore(IEnumerable<HtmlNode> currentNodes)
        {
            foreach (var node in currentNodes)
            {
                if (node.Id.Equals(Selector, StringComparison.OrdinalIgnoreCase))
                    return new[] { node };
            }

            return new HtmlNode[0];
        }
    }
}
