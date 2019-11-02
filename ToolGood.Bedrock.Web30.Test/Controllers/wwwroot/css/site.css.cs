using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.WwwRoot.Test
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("css/site.css")]
        public IActionResult css_site_css()
        {
            if (SetResponseHeaders("4B60555A29B6FB4F44BC33939AF9D2DE") == false) { return StatusCode(304); }
            const string s = "G+0BAGR1TtnLwZmUBeCmU+f470EtelGgUTQqygId6FTnDg4+QggUrF3nY4ztTJ6zO892ib2GBS2dY/ibDgIkOlZL1LVfMykbR3RM585zc30xvUxIpm7Ogr+O7a4vBTyw3yh/xLCCsvZ5epXVczAJ3RZmRmwx30fNClrVVwVZt/ZqGkKnvDd5RlSxL2TVohgBQ3agIQnc4f3ZqylQkP/vjWgxIC7SgaKRDfpeTzD6KDkDlBPuAIJDkWvVYpsgZMSpzOYo0xlHPqkQ";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/css");
        }
    }
}