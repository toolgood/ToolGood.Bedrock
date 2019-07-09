using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ToolGood.Bedrock.Web.Test.Controllers
{
    public class ImageController : ImageControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}