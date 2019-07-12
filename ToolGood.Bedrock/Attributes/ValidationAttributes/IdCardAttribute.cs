using System.ComponentModel.DataAnnotations;
using ToolGood.Bedrock;

namespace ToolGood.Bedrock.Attributes
{
    ///<summary>
    /// 身份证验证特性
    /// </summary>
    public class IdCardAttribute : ValidationAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            return Regexs.IsIdCard(value.ToString());
            //if (value == null)
            //    return false;
            //var v = value.ToString();
            //if (Regexs.IdCardRegex.IsMatch(v) == false) {
            //    return false;
            //}
            //if (v.Length == 18) {
            //    return Check18(v);
            //} else if (v.Length == 15) {
            //    return true;
            //}
            //return false;
        }
        //private bool Check18(string id)
        //{
        //    int[] weights = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
        //    string checks = "10X98765432";
        //    int val = 0;
        //    for (var i = 0; i < 17; i++) {
        //        val += (id[i] - '0') * weights[i];
        //    }
        //    return id[17] == checks[val % 11];
        //}

    }
}
