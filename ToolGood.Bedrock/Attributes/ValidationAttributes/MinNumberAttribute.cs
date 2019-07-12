namespace ToolGood.Bedrock.Attributes
{
    public class MinNumberAttribute : System.ComponentModel.DataAnnotations.RangeAttribute
    {
        public MinNumberAttribute(double minimum) : base(minimum, double.MaxValue)
        {
        }

        public MinNumberAttribute(int minimum) : base(minimum, int.MaxValue)
        {
        }

        public MinNumberAttribute(decimal minimum) : base(typeof(decimal), minimum.ToString(), decimal.MaxValue.ToString())
        {
        }


    }
}
