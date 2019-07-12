namespace ToolGood.Bedrock.Attributes
{
    /// <summary>
    /// 最大长度验证特性
    /// </summary>
    public class MaxLengthAttribute : System.ComponentModel.DataAnnotations.MaxLengthAttribute
    {
        public MaxLengthAttribute(int length) : base(length)
        {
        }
    }
}
