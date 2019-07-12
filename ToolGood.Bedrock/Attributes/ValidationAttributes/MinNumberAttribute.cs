namespace ToolGood.Bedrock.Attributes
{
    /// <summary>
    /// 最小值验证特性
    /// </summary>
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
