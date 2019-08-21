namespace ToolGood.Bedrock.Attributes
{
    /// <summary>
    /// 最小长度验证特性
    /// </summary>
    public class LengthMinAttribute : System.ComponentModel.DataAnnotations.MinLengthAttribute
    {
        /// <summary>
        /// 最小长度验证特性
        /// </summary>
        /// <param name="length"></param>
        public LengthMinAttribute(int length) : base(length)
        {
        }
    }



}
