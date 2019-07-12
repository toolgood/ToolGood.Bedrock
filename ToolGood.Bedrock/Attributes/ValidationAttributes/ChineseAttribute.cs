using System.ComponentModel.DataAnnotations;
using ToolGood.Bedrock;

namespace ToolGood.Bedrock.Attributes
{
    ///<summary>
    /// 中文验证特性
    /// </summary>
    public class ChineseAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// 中文验证特性
        /// </summary>
        public ChineseAttribute() : base(Regexs.Chinese)
        {
            ErrorMessage = "请输入中文";
        }
    }
}
