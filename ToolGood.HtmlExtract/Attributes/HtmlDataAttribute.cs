using System;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.HtmlExtract.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class HtmlDataAttribute : Attribute
    {
        public string JqSelector { get; private set; }
        public int? Index { get; private set; }
        public string AttrName { get; private set; }

        public HtmlDataAttribute(string jqSelector)
        {
            JqSelector = jqSelector;
        }
        public HtmlDataAttribute(string jqSelector, int index)
        {
            JqSelector = jqSelector;
            Index = index;
        }
        public HtmlDataAttribute(string jqSelector, string attrName)
        {
            JqSelector = jqSelector;
            AttrName = attrName;
        }
        public HtmlDataAttribute(string jqSelector, int index, string attrName)
        {
            JqSelector = jqSelector;
            Index = index;
            AttrName = attrName;
        }
    }


}
