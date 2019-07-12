using System.ComponentModel.DataAnnotations;
using ToolGood.Bedrock;

namespace ToolGood.Bedrock.Attributes
{
    ///<summary>
    /// 邮箱验证特性
    /// </summary>
    public class EmailAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// 邮箱验证特性
        /// </summary>
        public EmailAttribute(): base(ValidatorRegex.Email)
        {
            ErrorMessage = "邮箱格式不正确";
        }
    }
}
