﻿namespace ToolGood.Bedrock.Attributes
{
    /// <summary>
    /// 最大值验证特性
    /// </summary>
    public class NumberMaxAttribute : System.ComponentModel.DataAnnotations.RangeAttribute
    {
        /// <summary>
        /// 最大值验证特性
        /// </summary>
        /// <param name="maximum"></param>
        public NumberMaxAttribute(double maximum) : base(double.MinValue, maximum)
        {
        }
        /// <summary>
        /// 最大值验证特性
        /// </summary>
        /// <param name="maximum"></param>
        public NumberMaxAttribute(int maximum) : base(int.MinValue, maximum)
        {
        }
        /// <summary>
        /// 最大值验证特性
        /// </summary>
        /// <param name="maximum"></param>
        public NumberMaxAttribute(decimal maximum) : base(typeof(decimal) ,decimal.MinValue.ToString(), maximum.ToString())
        {
        }
    }
}
