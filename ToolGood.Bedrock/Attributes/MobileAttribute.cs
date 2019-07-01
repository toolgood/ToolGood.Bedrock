using System.ComponentModel.DataAnnotations;
using ToolGood.Bedrock;

namespace ToolGood.Bedrock.Attributes
{
    ///<summary>
    /// 手机号码验证特性
    /// </summary>
    public class MobileAttribute : RegularExpressionAttribute
    {
        public MobileAttribute() : base(ValidatorRegex.Mobile)
        {
            ErrorMessage = "手机号码不正确";
        }
    }
}
