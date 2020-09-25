using System;

namespace ToolGood.HtmlExtract.HtmlAgilityPack.CssSelectors
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PseudoClassNameAttribute : Attribute
    {
        public string FunctionName { get; private set; }

        public PseudoClassNameAttribute(string name)
        {
            FunctionName = name;
        }
    }
}
