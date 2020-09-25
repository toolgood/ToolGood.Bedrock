﻿using System.Linq;

namespace ToolGood.HtmlExtract.HtmlAgilityPack.CssSelectors.PseudoClassSelectors
{
    [PseudoClassName("not")]
    internal class NotPseudoClass : PseudoClass
    {
        protected override bool CheckNode(HtmlNode node, string parameter)
        {
            var selectors = CssSelector.Parse(parameter);
            var nodes = new[] { node };

            foreach (var selector in selectors)
                if (selector.FilterCore(nodes).Count() == 1)
                    return false;

            return true;
        }
    }
}