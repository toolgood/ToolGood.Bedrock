namespace ToolGood.Bedrock.Attributes
{
    /// <summary>
    /// 最小长度验证特性
    /// </summary>
    public class MinLengthAttribute : System.ComponentModel.DataAnnotations.MinLengthAttribute
    {
        public MinLengthAttribute(int length) : base(length)
        {
        }
    }



}
