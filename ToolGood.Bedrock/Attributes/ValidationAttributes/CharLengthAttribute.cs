using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ToolGood.Bedrock.Attributes
{
    /// <summary>
    /// 字符长度验证特性
    /// </summary>
    public class CharLengthAttribute : System.ComponentModel.DataAnnotations.StringLengthAttribute
    {
        public CharLengthAttribute(int maximumLength) : base(maximumLength)
        {
        }
    }


}
