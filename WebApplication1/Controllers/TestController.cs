using Microsoft.AspNetCore.Mvc;
using System;

namespace WebApplication1.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            Response.Headers["Cache-Control"] = "max-age=315360000";
            Response.Headers["Etag"] = DateTime.Now.ToString("yyyyMMddHHmmss");
            Response.Headers["Date"] = DateTime.Now.ToString("r");
            Response.Headers["Expires"] = DateTime.Now.AddYears(100).ToString("r");

            const string s = "";
            var bytes = Convert.FromBase64String(s);
            //var bytes = System.Text.Encoding.ASCII.GetBytes(s);
            return File(bytes, "");
        }
    }
}