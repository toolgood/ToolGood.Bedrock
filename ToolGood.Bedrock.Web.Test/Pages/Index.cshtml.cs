using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToolGood.Bedrock.Web.Controllers;

namespace ToolGood.Bedrock.Web.Test.Pages
{
    [ApiLogFilter]
    public class IndexModel : WebPageModelBase
    {
        public void OnGet(int id)
        {
            var ss = id;
            var value = GetCookie("key");

            SetCookie("key", "4444");

            var s = GetSession("key");

            SetSession("key", "123456");
             
        }

        //public void OnPost()
        //{

        //}

    }
    public class QueryArgs : QueryArgsBase
    {
        //public override bool UseDebuggingMode => false;
    }
}
