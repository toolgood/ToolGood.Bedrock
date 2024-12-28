using System;

namespace ToolGood.Bedrock.DataCommon
{
    /// <summary>
    /// 数据名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DataNameAttribute : Attribute
    {
        public string DisplayName { get; private set; }

        /// <summary>
        /// 数据名称
        /// </summary>
        /// <param name="displayName">显示名称</param>
        public DataNameAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
}