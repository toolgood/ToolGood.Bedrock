using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.WwwRoot.Test
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("css/site.css")]
        public IActionResult css_site_css()
        {
            if (Request.Headers["If-None-Match"]  == "4B60555A29B6FB4F44BC33939AF9D2DE") {
                return StatusCode(304);
            }
            SetResponseHeaders("4B60555A29B6FB4F44BC33939AF9D2DE");
            const string s = "G4oEAIyULhLvcvf3/XBbHQn9IjZ5ldA/pA0OfI+p9X/pXYJdA4Y3s/9vSuvPm9UiEOIGGEDJot43XFCgdsLSYg5Qija4f3/hvYc09SbWGqoICj0rmlJhy/ScZhpIEoK/MdmpIE9URIQKSJGoJEx8nEumFv5r+PjQovVkl8JDUns9G2IqIBQBSE/bSdoDyr89BM/7ZmqCKtFLURMbYDmfMqaISsp3UEDyMe2Y9JgEXaWIPRz2evMBKLnRGDIHfm5cmcCLxQ+ea2qkRa4RXsLpnk3htSSa+ibPE6tI1OrDwuV8LUcbbgfDgTJw7gBsnN/S00IEZDeb2LV/EU9tklc4k9NqraNPa1hmxkTdVDruxR040P2pxqI3cFmpIyks7kFnSvnJRiW+n6axnyaufURhsRqLO1iI1Zvu/bfpdLVQwx6TYDTLD6LdVog+AS4z5z7iRss5yaAAk9gDyKKRcBsjCYRgPBbXmGQhU3wMvUh5OhQckr75obEStg8emoPdScppucbCMK0Mqj12pwCxPcSxkk8nOCAUuWZVcph0";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/css");
        }
    }
}