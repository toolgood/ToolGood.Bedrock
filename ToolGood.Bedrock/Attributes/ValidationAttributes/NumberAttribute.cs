using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ToolGood.Bedrock.Attributes
{
    /// <summary>
    /// 值范围验证特性
    /// </summary>
    public class NumberAttribute : RangeAttribute
    {
        /// <summary>
        /// 值范围验证特性
        /// </summary>
        /// <param name="minimum">Specifies the minimum value allowed for the data field value.</param>
        /// <param name="maximum">Specifies the maximum value allowed for the data field value.</param>
        public NumberAttribute(double minimum, double maximum) : base(minimum, maximum)
        {
        }
        /// <summary>
        /// 值范围验证特性
        /// </summary>
        /// <param name="minimum">Specifies the minimum value allowed for the data field value.</param>
        /// <param name="maximum">Specifies the maximum value allowed for the data field value.</param>
        public NumberAttribute(int minimum, int maximum) : base(minimum, maximum)
        {
        }
        /// <summary>
        /// 值范围验证特性
        /// </summary>
        /// <param name="minimum">Specifies the minimum value allowed for the data field value.</param>
        /// <param name="maximum">Specifies the maximum value allowed for the data field value.</param>
        public NumberAttribute(decimal minimum, decimal maximum) : base(typeof(decimal), minimum.ToString(), maximum.ToString())
        {
        }

        /// <summary>
        /// 值范围验证特性
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="minimum">Specifies the minimum value allowed for the data field value.</param>
        /// <param name="maximum">Specifies the maximum value allowed for the data field value.</param>
        public NumberAttribute(Type type, string minimum, string maximum) : base(type, minimum, maximum)
        {
        }
    }



}
