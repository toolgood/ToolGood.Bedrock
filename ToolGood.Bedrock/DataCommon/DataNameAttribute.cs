using System;

namespace ToolGood.Bedrock.DataCommon
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DataNameAttribute : Attribute
    {
        public string DisplayName { get; private set; }

        public DataNameAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }


}
