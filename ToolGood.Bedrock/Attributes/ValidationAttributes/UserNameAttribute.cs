namespace ToolGood.Bedrock.Attributes
{
    /// <summary>
    /// 用户名验证
    /// </summary>
    public class UserNameAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute
    {
        /// <summary>
        /// 用户名验证
        /// </summary>
        public UserNameAttribute() : base("^[a-z0-9_]{4,20}$")
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            return string.IsNullOrEmpty(ErrorMessage)
                ? $"The field {name} must be 4-20 bit characters starting with letters and consisting of lowercase letters and numbers."
                : string.Format(ErrorMessage, name);
        }

    }



}
