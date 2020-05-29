using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.WwwRoot.Test
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("css/site.css")]
        public IActionResult css_site_css()
        {
            if (SetResponseHeaders("6C33295C59433276904AAB6321F24544") == false) { return StatusCode(304); }
            const string s = "G6cCAIyUrtZEr5/7VBcV5kw0Vd70q3LE2NRN7gK1H7u2BlQ0Ns0FGmjgqc6dfIaIK9DNbo8xxibjmd+G78BeikHCOqqatFMjwbISqMuqZYGPR2XR/m+YEE2kaJ5n8AKbYYL0bP0lMjTHdVjiUUERK8MkXHfH48MaguAq9G3l24FLVoeaaU+g+9b9C9PrV4efpQxOu3Muf82puH4dwWv0VHwVYyHDZRVtd856CkYCXMuVAVOMb8FcyFGuQgvj7D6fTvU8fl8N26HfRxhmjupIYR4pkEmL9+s+n2od0K/WoVj0SATEAaOBmLseshD6oMT4imVjRySGkcswUVa18FC+tXmBoiUU";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/css");
        }
    }
}