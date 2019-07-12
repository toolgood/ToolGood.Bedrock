namespace ToolGood.Bedrock.Attributes
{
    /// <summary>
    /// 字符串长度验证
    /// </summary>
    public class StringLengthAttribute : System.ComponentModel.DataAnnotations.StringLengthAttribute
    {
        public StringLengthAttribute(int maximumLength) : base(maximumLength)
        {
        }
    }


}
