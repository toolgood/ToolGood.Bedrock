using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToolGood.ReadyGo3;

namespace ToolGood.Bedrock.DataCommon
{
    public class DataEnumAttribute : DataNameAttribute
    {
        public string[] EnumName { get; set; }

        public DataEnumAttribute(string displayName, params string[] enumName) : base(displayName)
        {
            EnumName = enumName;
        }
    }


}
