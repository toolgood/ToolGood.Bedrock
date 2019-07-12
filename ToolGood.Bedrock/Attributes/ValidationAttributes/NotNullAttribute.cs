using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ToolGood.Bedrock.Attributes
{
    /// <summary>
    /// 非空验证特性
    /// </summary>
    public class NotNullAttribute : ValidationAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            return object.Equals(null, value) == false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            return string.IsNullOrEmpty(ErrorMessage)
                 ? $"The field {name} must be a number greater than 0."
                 : string.Format(ErrorMessage, name);

         }
    }
}
