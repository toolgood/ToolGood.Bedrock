using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToolGood.Bedrock.Web.Controllers;
using ToolGood.Bedrock.Web.Theme;

namespace ToolGood.Bedrock.Web.Test.Controllers
{
    [ApiLogFilter]
    public class ApiController : ApiControllerBase
    {
        //[IgnoreLogFilter]
        public IActionResult Index()
        {
            return Execute(() => {
                return Success("ddd");
            });
        }


        public IActionResult Download()
        {

            return ResumeFile("1.zip", "1.zip");
        }

        public IActionResult TT(int aa)
        {
            HttpContext.Request.HttpContext.Items[ViewLocationExpander.ThemeKey] = "EE";
            //HttpContext.Request.HttpContext.Items[ViewLocationExpander.ThemeKey] = "EE";

            return View();

        }
    }
}