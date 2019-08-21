namespace ToolGood.Bedrock.Attributes
{
    /// <summary>
    /// 最大长度验证特性
    /// </summary>
    public class LengthMaxAttribute : System.ComponentModel.DataAnnotations.MaxLengthAttribute
    {
        /// <summary>
        /// 最大长度验证特性
        /// </summary>
        /// <param name="length"></param>
        public LengthMaxAttribute(int length) : base(length)
        {
        }
    }
}
