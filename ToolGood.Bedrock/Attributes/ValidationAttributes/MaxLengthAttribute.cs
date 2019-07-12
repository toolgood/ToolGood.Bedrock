namespace ToolGood.Bedrock.Attributes
{
    public class MaxLengthAttribute : System.ComponentModel.DataAnnotations.MaxLengthAttribute
    {
        public MaxLengthAttribute(int length) : base(length)
        {
        }
    }
}
