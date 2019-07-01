using System;

namespace ToolGood.Bedrock.Web
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class IgnoreLogFilterAttribute : Attribute
    {
    }
}
