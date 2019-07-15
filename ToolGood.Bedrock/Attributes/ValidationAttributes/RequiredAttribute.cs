using System;
using System.ComponentModel.DataAnnotations;

namespace ToolGood.Bedrock.Attributes
{
    /// <summary>
    /// 必填验证特性
    /// </summary>
    public class RequiredAttribute : ValidationAttribute
    {
        /// <summary>
        /// 必填验证特性
        /// </summary>
        public RequiredAttribute()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value==null) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 必填验证特性
        /// </summary>
        /// <param name="errorMessageAccessor"></param>
        public RequiredAttribute(Func<string> errorMessageAccessor) : base(errorMessageAccessor)
        {
        }
        /// <summary>
        /// 必填验证特性
        /// </summary>
        /// <param name="errorMessage"></param>
        public RequiredAttribute(string errorMessage) : base(errorMessage)
        {
        }
    }



}
