namespace ToolGood.Bedrock.Attributes
{
    /// <summary>
    /// 最大值验证特性
    /// </summary>
    public class MaxNumberAttribute : System.ComponentModel.DataAnnotations.RangeAttribute
    {
        public MaxNumberAttribute(double maximum) : base(double.MinValue, maximum)
        {
        }

        public MaxNumberAttribute(int maximum) : base(int.MinValue, maximum)
        {
        }

        public MaxNumberAttribute(decimal maximum) : base(typeof(decimal) ,decimal.MinValue.ToString(), maximum.ToString())
        {
        }
    }
}
