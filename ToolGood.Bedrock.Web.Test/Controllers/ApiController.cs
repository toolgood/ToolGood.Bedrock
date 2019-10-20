﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToolGood.Bedrock.Web.Controllers;

namespace ToolGood.Bedrock.Web.Test.Controllers
{
    [ApiLogFilter]
    public class ApiController : ApiControllerCore
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
            HttpContext.Request.HttpContext.Items[Constants.WebConstants.Theme] = "EE";
            //HttpContext.Request.HttpContext.Items[ViewLocationExpander.ThemeKey] = "EE";

            return View();

        }

        public IActionResult AA(QueryArgs2 queryArgs)
        {
            
            var t = QueryArgs.BatchNum;
            return Success();
        }

    }
    public class QueryArgs2 : QueryArgs
    {
        public string id { get; set; }
    }
}