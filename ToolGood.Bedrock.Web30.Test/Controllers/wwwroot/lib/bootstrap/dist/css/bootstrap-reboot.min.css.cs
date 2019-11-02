using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.WwwRoot.Test
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("lib/bootstrap/dist/css/bootstrap-reboot.min.css")]
        public IActionResult lib_bootstrap_dist_css_bootstrap_reboot_min_css()
        {
            if (SetResponseHeaders("F96A1272581712B419D1287A659C67FF") == false) { return StatusCode(304); }
            const string s = "G08B4CwObMckiQanX+wbmHV0jvR0+Skywa9edCoIGaULClU2SdllQ8iZ2Q7Y979BXSDlSZB03l0TGGCgAWHOzbvncNPBKgiS6B/qA3qotfM4d7FCRrZ2whyZGfQ40745JQCl5i5oSyBzL01bbdybZGoI5YOq9xR7CRA7jsKaFy1rGOH0l2Sf5yMPJGhBMjeJKiroH4ap77rtyfj+0wGpht/cwLXG9xG8qlEtiwPVoCNPU/QA";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/css");
        }
    }
}