namespace ToolGood.Bedrock.Attributes
{
    public class UserNameAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute
    {
        public UserNameAttribute() : base("^[a-z0-9_]{4,20}$")
        {

        }


        public override string FormatErrorMessage(string name)
        {
            return string.IsNullOrEmpty(ErrorMessage)
                ? $"The field {name} must be 4-20 bit characters starting with letters and consisting of lowercase letters and numbers."
                : string.Format(ErrorMessage, name);
        }

    }



}
