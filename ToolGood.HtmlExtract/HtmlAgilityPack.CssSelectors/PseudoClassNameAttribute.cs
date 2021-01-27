using System;

namespace ToolGood.HtmlExtract.HtmlAgilityPack
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
