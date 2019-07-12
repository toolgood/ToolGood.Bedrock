using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ToolGood.Bedrock;

namespace ToolGood.Bedrock.Attributes
{
    /// <summary>
    /// 坐机号码验证特性
    /// </summary>
    public class TelephoneAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// 坐机号码验证特性
        /// </summary>
        public TelephoneAttribute() : base(ValidatorRegex.Telephone)
        {
            ErrorMessage = "坐机号码不正确";
        }
    }
}
