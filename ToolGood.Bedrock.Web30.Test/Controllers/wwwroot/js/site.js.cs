using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.WwwRoot.Test
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("js/site.js")]
        public IActionResult js_site_js()
        {
            if (Request.Headers["If-None-Match"]  == "DC0EDA3CD0141331EC73B748BF9B9FF5") {
                return StatusCode(304);
            }
            SetResponseHeaders("DC0EDA3CD0141331EC73B748BF9B9FF5");
            const string s = "G+UAAJ0JdozKCxH7JZOVwnu81O6vzbaQPnRC4YmZB96x6eH8d051Wz28PMS8nQd5UauhC4LdhxbUrVGzZGXDJC/eXSULtqLOFS7LmW4vW50Agj113Ygt/KYgb2zoh4wXfiYOVjhfiZxpurPQ0KyH7g4t5s8YAZDRyoZKepAo5R8iWV+Mj54Dd5z12OT3SWrCfXNGAw==";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/javascript");
        }
    }
}