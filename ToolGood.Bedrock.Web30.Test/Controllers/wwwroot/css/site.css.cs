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
                Response.Headers["Expires"] = DateTime.Now.AddYears(100).ToString("r");
                return StatusCode(304);
            }
            Response.Headers["Cache-Control"] = "max-age=315360000";
            Response.Headers["Etag"] = "4B60555A29B6FB4F44BC33939AF9D2DE";
            Response.Headers["Date"] = DateTime.Now.ToString("r");
            Response.Headers["Expires"] = DateTime.Now.AddYears(100).ToString("r");
            const string s = "77u/LyogUGxlYXNlIHNlZSBkb2N1bWVudGF0aW9uIGF0IGh0dHBzOi8vZG9jcy5taWNyb3NvZnQuY29tL2FzcG5ldC9jb3JlL2NsaWVudC1zaWRlL2J1bmRsaW5nLWFuZC1taW5pZmljYXRpb24NCmZvciBkZXRhaWxzIG9uIGNvbmZpZ3VyaW5nIHRoaXMgcHJvamVjdCB0byBidW5kbGUgYW5kIG1pbmlmeSBzdGF0aWMgd2ViIGFzc2V0cy4gKi8NCg0KYS5uYXZiYXItYnJhbmQgew0KICB3aGl0ZS1zcGFjZTogbm9ybWFsOw0KICB0ZXh0LWFsaWduOiBjZW50ZXI7DQogIHdvcmQtYnJlYWs6IGJyZWFrLWFsbDsNCn0NCg0KLyogU3RpY2t5IGZvb3RlciBzdHlsZXMNCi0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tICovDQpodG1sIHsNCiAgZm9udC1zaXplOiAxNHB4Ow0KfQ0KQG1lZGlhIChtaW4td2lkdGg6IDc2OHB4KSB7DQogIGh0bWwgew0KICAgIGZvbnQtc2l6ZTogMTZweDsNCiAgfQ0KfQ0KDQouYm9yZGVyLXRvcCB7DQogIGJvcmRlci10b3A6IDFweCBzb2xpZCAjZTVlNWU1Ow0KfQ0KLmJvcmRlci1ib3R0b20gew0KICBib3JkZXItYm90dG9tOiAxcHggc29saWQgI2U1ZTVlNTsNCn0NCg0KLmJveC1zaGFkb3cgew0KICBib3gtc2hhZG93OiAwIC4yNXJlbSAuNzVyZW0gcmdiYSgwLCAwLCAwLCAuMDUpOw0KfQ0KDQpidXR0b24uYWNjZXB0LXBvbGljeSB7DQogIGZvbnQtc2l6ZTogMXJlbTsNCiAgbGluZS1oZWlnaHQ6IGluaGVyaXQ7DQp9DQoNCi8qIFN0aWNreSBmb290ZXIgc3R5bGVzDQotLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLSAqLw0KaHRtbCB7DQogIHBvc2l0aW9uOiByZWxhdGl2ZTsNCiAgbWluLWhlaWdodDogMTAwJTsNCn0NCg0KYm9keSB7DQogIC8qIE1hcmdpbiBib3R0b20gYnkgZm9vdGVyIGhlaWdodCAqLw0KICBtYXJnaW4tYm90dG9tOiA2MHB4Ow0KfQ0KLmZvb3RlciB7DQogIHBvc2l0aW9uOiBhYnNvbHV0ZTsNCiAgYm90dG9tOiAwOw0KICB3aWR0aDogMTAwJTsNCiAgd2hpdGUtc3BhY2U6IG5vd3JhcDsNCiAgLyogU2V0IHRoZSBmaXhlZCBoZWlnaHQgb2YgdGhlIGZvb3RlciBoZXJlICovDQogIGhlaWdodDogNjBweDsNCiAgbGluZS1oZWlnaHQ6IDYwcHg7IC8qIFZlcnRpY2FsbHkgY2VudGVyIHRoZSB0ZXh0IHRoZXJlICovDQp9DQo=";
            var bytes = Convert.FromBase64String(s);
            return File(bytes, "text/css");
        }
    }
}