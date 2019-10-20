using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.WwwRoot.Test
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("lib/jquery/LICENSE.txt")]
        public IActionResult lib_jquery_LICENSE_txt()
        {
            if (Request.Headers["If-None-Match"]  == "828D0378AE850B159C2FF139FD371F9B") {
                return StatusCode(304);
            }
            SetResponseHeaders("828D0378AE850B159C2FF139FD371F9B");
            const string s = "G2gGAIzUUc3m2fz8Of2SJ/ylauN4LtFdYaiYfkeh4Pctp0VYe3/271FV6U3PjgJ3uKJIXCTC5sVl0U2xtyICWcoU/RDk5AIdYOFVVH/cX6pOIJsmTgocgmULKMWPtyqY4VFAlz4vX/8CmR1+BEISdyu3dgzfrwttRGsEs6ZvglL2Dmk7dvHH/bE4vFfgHMHtYbnAhhzhjyFY6uHi+9Cnri33tlSoU+U12sD8TJcAWw3WREYgzsqrP1YrsCzD3XwS9pZu/4Edl9EOjrc9li1R2ymIMJWzwxpCKqvLcnHYQrskXa5iFS7QgeD00n6+y1qQbC7nqECN3sbiUFQJkwdvta3DzOGvVA92bus7Vgsv65i5imRSivfPPFjeNq37q/8RlzyhG9WdDLp33IKgQADbYBbCg94fAwQ7UIftRtV6cyd1v18WjDJymHppPEwh0hCPK1V3VU6JC5UVKS3dt+9WW0xgXS/9df3qSlsFnL9yqDij4tkYfNW4N61rei5W0eoYio1U/Z7pPJyROKTzjA4+sYxHjKQs2btGUIFzTVwoK8yLwlaaV/ft/L8CuKr57i3kc//55EiGAVk+Lp3/6WyBhX12Mp/d8+dc+kwsCAot5x7Gl/noeFYlQ11ozgO4+s+cDBuRrxuRbWGRZ2c+6BCpDK2CMFEopRuJzGVYWtL5iWedQJyzHPwylGx4lIZEkjUOUZDYb86JScTosvwi0G8obvAFsYdRbrsI4G9/9tnNyWcNN83ioaVfLtnUaDqiGs7Pb9/CjoRvuKktjpCcvLb4TVz+nUfeGgYK2dGxSg5pgxpX397g/r0VglPVpu9F81Ato+4BUCm55WofaWhOr8s/3a0C08+SryiNDIS0DA==";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/plain");
        }
    }
}