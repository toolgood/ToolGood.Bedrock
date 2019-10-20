using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.WwwRoot.Test
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("lib/jquery-validation/LICENSE.md")]
        public IActionResult lib_jquery_validation_LICENSE_md()
        {
            if (Request.Headers["If-None-Match"]  == "41EF1B919DB5E3AEAF47308518E7BCEE") {
                return StatusCode(304);
            }
            SetResponseHeaders("41EF1B919DB5E3AEAF47308518E7BCEE");
            const string s = "G1wEIJwH2SnzJE3bm7/GqhXy7ecU6gv/rmpjoYAK2XVJgR0P2euBQw7o4GlbZpHlUCj0W5lb3aux1RIaSmLtEPdmtlwiVKJYyQzpXwe4vaJx4HywDWsKTgMGW+HdNqMIp79q7K/V//chy2ua+l7fq6tTEqXdA32WLX7YTeDNB/yJxZkn5Off6QuenUa78wb55ZaWujS7PubuRTX/nq4HaZUcaDXkqKH5ac0dwrlwpHZnaMYwV3Vst91aTxobVz+rwThMbqonvB51jzIM8z52zA03cPLxU/3ExrSnxwshEcOBKQ3tIltenV+XXuT6cAD879mPgioZXNQsg9PfPRTZq+XHtm7uo4KHx9Mss5H6/Tc47dLB9mVT/i0Hv3qSpoSa30xV2kmJFa/Aau3OqrQZqvP78aJtLjbo+FBf5CwdplYNMOmBNivI1wLDK9T01ZtxkEIMzDhl7p/AaDyFeOq/EaZolhbAjxW8oNhRaV2JdN82tEPLP5TQF4h0m1wY4sH8KjLB+c+Ml2BhYsMjNnmsOSYGCwifWd9CNdnnioCU5MXD5zNsURX9y8dch02zua13Ls0pXwMZLGRq8FACS/eUTf0oAsuZJI5YrDdzYvAr8CoQL2AXoQiCCnIK0YuEQVhapGiFRphBT6dMWrxS9dDXewgRnp0lNn0QT0fDbJgU";
            var bytes = UseCompressBytes(s);
            return File(bytes, "application/octet-stream");
        }
    }
}