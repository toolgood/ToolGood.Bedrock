using System;

namespace ToolGood.HtmlExtract.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class HtmlLocationAttribute : Attribute
    {
        public string JqSelector { get; private set; }
        public int? Index { get; private set; }

        public HtmlLocationAttribute(string jqSelector = "body")
        {
            JqSelector = jqSelector;
        }
        public HtmlLocationAttribute(string jqSelector, int index)
        {
            JqSelector = jqSelector;
            Index = index;
        }

    }


}
