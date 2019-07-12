using System;
using System.ComponentModel.DataAnnotations;

namespace ToolGood.Bedrock.Attributes
{
    public class RequiredAttribute : ValidationAttribute
    {
        public RequiredAttribute()
        {
        }

        public RequiredAttribute(Func<string> errorMessageAccessor) : base(errorMessageAccessor)
        {
        }

        public RequiredAttribute(string errorMessage) : base(errorMessage)
        {
        }
    }



}
