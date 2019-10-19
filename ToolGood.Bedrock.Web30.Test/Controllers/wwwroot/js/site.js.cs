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
                Response.Headers["Expires"] = DateTime.Now.AddYears(100).ToString("r");
                return StatusCode(304);
            }
            Response.Headers["Cache-Control"] = "max-age=315360000";
            Response.Headers["Etag"] = "DC0EDA3CD0141331EC73B748BF9B9FF5";
            Response.Headers["Date"] = DateTime.Now.ToString("r");
            Response.Headers["Expires"] = DateTime.Now.AddYears(100).ToString("r");
            const string s = "77u/Ly8gUGxlYXNlIHNlZSBkb2N1bWVudGF0aW9uIGF0IGh0dHBzOi8vZG9jcy5taWNyb3NvZnQuY29tL2FzcG5ldC9jb3JlL2NsaWVudC1zaWRlL2J1bmRsaW5nLWFuZC1taW5pZmljYXRpb24NCi8vIGZvciBkZXRhaWxzIG9uIGNvbmZpZ3VyaW5nIHRoaXMgcHJvamVjdCB0byBidW5kbGUgYW5kIG1pbmlmeSBzdGF0aWMgd2ViIGFzc2V0cy4NCg0KLy8gV3JpdGUgeW91ciBKYXZhc2NyaXB0IGNvZGUuDQo=";
            var bytes = Convert.FromBase64String(s);
            return File(bytes, "text/javascript");
        }
    }
}