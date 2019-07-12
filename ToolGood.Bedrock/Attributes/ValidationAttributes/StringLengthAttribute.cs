namespace ToolGood.Bedrock.Attributes
{
    public class StringLengthAttribute : System.ComponentModel.DataAnnotations.StringLengthAttribute
    {
        public StringLengthAttribute(int maximumLength) : base(maximumLength)
        {
        }
    }


}
