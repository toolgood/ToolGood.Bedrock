namespace ToolGood.Bedrock.Attributes
{
    /// <summary>
    /// 最小值验证特性
    /// </summary>
    public class MinNumberAttribute : System.ComponentModel.DataAnnotations.RangeAttribute
    {
        /// <summary>
        /// 最小值验证特性
        /// </summary>
        /// <param name="minimum"></param>
        public MinNumberAttribute(double minimum) : base(minimum, double.MaxValue)
        {
        }
        /// <summary>
        /// 最小值验证特性
        /// </summary>
        /// <param name="minimum"></param>
        public MinNumberAttribute(int minimum) : base(minimum, int.MaxValue)
        {
        }
        /// <summary>
        /// 最小值验证特性
        /// </summary>
        /// <param name="minimum"></param>
        public MinNumberAttribute(decimal minimum) : base(typeof(decimal), minimum.ToString(), decimal.MaxValue.ToString())
        {
        }


    }
}
