using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ToolGood.Bedrock.Attributes
{
    /// <summary>
    /// 值范围验证
    /// </summary>
    public class BetweenAttribute : RangeAttribute
    {
        public BetweenAttribute(double minimum, double maximum) : base(minimum, maximum)
        {
        }

        public BetweenAttribute(int minimum, int maximum) : base(minimum, maximum)
        {
        }
        public BetweenAttribute(decimal minimum, decimal maximum) : base(typeof(decimal), minimum.ToString(), maximum.ToString())
        {
        }

        public BetweenAttribute(Type type, string minimum, string maximum) : base(type, minimum, maximum)
        {
        }
    }



}
