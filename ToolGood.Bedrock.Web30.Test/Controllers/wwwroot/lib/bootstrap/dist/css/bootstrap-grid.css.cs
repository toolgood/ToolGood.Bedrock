using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.WwwRoot.Test
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("lib/bootstrap/dist/css/bootstrap-grid.css")]
        public IActionResult lib_bootstrap_dist_css_bootstrap_grid_css()
        {
            if (SetResponseHeaders("6BBC09E9C3406B1280BB3474EC2B0DDC") == false) { return StatusCode(304); }
            const string s = "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";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/css");
        }
    }
}