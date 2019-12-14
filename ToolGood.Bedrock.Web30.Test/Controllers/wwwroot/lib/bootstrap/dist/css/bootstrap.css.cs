using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.WwwRoot.Test
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("lib/bootstrap/dist/css/bootstrap.css")]
        public IActionResult lib_bootstrap_dist_css_bootstrap_css()
        {
            if (SetResponseHeaders("81FD5FFCE7EC00515ABE40D854FB4BEF") == false) { return StatusCode(304); }
            const string s = "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";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/css");
        }
    }
}