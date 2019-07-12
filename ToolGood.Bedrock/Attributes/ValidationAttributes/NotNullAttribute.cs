using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ToolGood.Bedrock.Attributes
{
    public class NotNullAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return object.Equals(null, value) == false;
        }


        public override string FormatErrorMessage(string name)
        {
            return string.IsNullOrEmpty(ErrorMessage)
                 ? $"The field {name} must be a number greater than 0."
                 : string.Format(ErrorMessage, name);

         }
    }
}
