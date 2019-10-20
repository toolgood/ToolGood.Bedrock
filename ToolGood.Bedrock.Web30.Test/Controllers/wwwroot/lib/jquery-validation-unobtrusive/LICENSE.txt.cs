using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.WwwRoot.Test
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("lib/jquery-validation-unobtrusive/LICENSE.txt")]
        public IActionResult lib_jquery_validation_unobtrusive_LICENSE_txt()
        {
            if (Request.Headers["If-None-Match"]  == "29BE5FAF434F4D08C3EEA0F55557A5A1") {
                return StatusCode(304);
            }
            SetResponseHeaders("29BE5FAF434F4D08C3EEA0F55557A5A1");
            const string s = "G0oCoIzUUc3m2Yy6BnTgO0cfrU8RiWnRoQD/79+QA9D3/rkfWtLW2qqgNAvoAoYBBpjn0tsyehTsvd+F81gmrKnZ/sLMLKTUqSMQWoxtMz3oovAAQTYjw+ZmXOO8cYBfHDU0SQHzdbTOlNUljCHv8b8rIVyqVMU5Ag+2/j4es1Bzk1XC0EoQii7nFcAJ4wcu/U2wVzuCmYwDwYZXztzwLmqScjIzuclvMutsiVVvfDARB5jVR1ZOqAtBErnWd1euP8YD1KgQVHRJ+uR08p/OKEsjrQOnQlaSvI2cMhdGOixtH/oLvBQQkh8ArBREepOlvofEC3UIYsz1fDEei8LK8zg1egI=";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/plain");
        }
    }
}