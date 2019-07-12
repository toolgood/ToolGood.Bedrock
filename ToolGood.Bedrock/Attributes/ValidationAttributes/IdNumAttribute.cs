using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ToolGood.Bedrock.Attributes
{
    /// <summary>
    /// 主键验证特性
    /// </summary>
    public class IdNumAttribute : ValidationAttribute
    {
        public IdNumAttribute()
        {
        }

        public override bool IsValid(object value)
        {
            if (value is null) {
                return false;
            }
            if (value is long) {
                return (long)value > 0;
            }
            if (value is int) {
                return (int)value > 0;
            }
            return false;
        }


        public override string FormatErrorMessage(string name)
        {
            return string.IsNullOrEmpty(ErrorMessage)
                ? $"The field {name} must be a number greater than 0."
                : string.Format(ErrorMessage, name);
        }
    }
}
