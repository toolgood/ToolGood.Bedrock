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
            var g = Guid.Parse("DC1F08A3-9B49-4F01-A99D-534D755823F2").ToString("N");
            var gg = Guid.Parse("F7E8DDF1-62B9-4D2F-BB1A-57E6898C5A91").ToString("N");

            var t = Guid.NewGuid().ToString("N");

 

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
