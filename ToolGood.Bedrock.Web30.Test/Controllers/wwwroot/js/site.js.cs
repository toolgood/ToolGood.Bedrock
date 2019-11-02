using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.WwwRoot.Test
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("js/site.js")]
        public IActionResult js_site_js()
        {
            if (SetResponseHeaders("DC0EDA3CD0141331EC73B748BF9B9FF5") == false) { return StatusCode(304); }
            const string s = "";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/javascript");
        }
    }
}