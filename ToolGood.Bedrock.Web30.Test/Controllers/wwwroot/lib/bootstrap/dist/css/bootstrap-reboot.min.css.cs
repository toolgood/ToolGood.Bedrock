using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.WwwRoot.Test
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("lib/bootstrap/dist/css/bootstrap-reboot.min.css")]
        public IActionResult lib_bootstrap_dist_css_bootstrap_reboot_min_css()
        {
            if (Request.Headers["If-None-Match"]  == "F96A1272581712B419D1287A659C67FF") {
                return StatusCode(304);
            }
            SetResponseHeaders("F96A1272581712B419D1287A659C67FF");
            const string s = "G7kPAByHcSyyJjyYMylfVEBCYbNZPl73+HOkNBnYnpiUUl/dxmLXOWwxxlzTyA/X3iQaU6trZIXqTl5mIUmB6IAdTXa2tL9HSEL4O3nudIjqfz5XCEjaetm/8/FR95QoTzmZsfiKligXp+tqUa2Km3+j6fIes01neuxu5RXlkAgWSuNe+/2AZdrLhaqWw14EFSBFKflM6Yb6rv3w5ntT9vh8mG0jDuQzO9XtOqq2A9m6Y6H+dwyjmc+8pHSCF/+PC0Ia95d4RlzGf7V6vhexo8FMdcyrVjdRBfe8x23QeoKSn4GAebZ0LocP8HGvV+DvPxcR28GTIGc4mpp9i2qxaaWpUHNI5c2jMm4unRb1W1VhimW5A6K9BsA11qSCZ8yoIAGG2a4B0i+PS2IOsDQa4SgIwPrBwG0Jer8XBQ849UAiBcCSfjRoLADC+yCaU8D9TuaKMvxGY+WU9AXF4MVTeZv8YDpi+79fUsxQfsM9YfHjTQlfyVImKF/jcIrZd6b4iDOW8CR5MxDQOJRPbs0Z6t/xItB/X5alBOLCvZL7+z9SppyRAN+XMLQsfKN100xv94aOP3S5WG6W91qpVQT1mb7v5af62qMW5W8t+IFJ6lzdnEfpOjkkBFqxp3umw9rm1AKWJA6SwwIOy2/Nq+Z18+bcLbPJBZr21MOfeJMwyJisWNyRbnU/tSYzrlOLlDiIxe+NmaeoY7EQ6Kl+W+EoZ3TtXdnZ/okXa1pCHGqEPxtwJzCXkCIQQjyb6WqPv0/cADTAPIzJGA1F4uSn5rl5bqM0r2jVdS4zipp3DxPneB52tFQ/0IggGZXXNFE0VX+v6yP36HifzeA7saB76tlBmMAkKkNqw0PuNtdkmnPdZB5Zj4i01+8210bqQk2reUiHUSIqMXCPDlfVcoNByh/LNAbR3TmGz7T40p3t+1Z/SYFRiKwSiAp1OFNWPQYxrUSFj7jZ2hWskjzrkfLNn8sb+Vu/s3h2haEkhofW7A0VAtnePaRDo7e8uphG/qJ05BBOrIMxIUwmjOrq2G8vP1AkdUGHfww+YBwIPlA0HcEzihMNZoLSJ0zi26mE8pk5eUyHvHpECYEiWSwRbRKhXFbCR0FcxoqrrOmc6CFFEh7kw54RCdONiYHlnRB6w9M9W1F6fidWVi2bWvxLEN24gy+52e1EuMw+QwxriE5vu91m52yRhVK9Xme+V/KBkzI3GdOxMfs8OqfD/OS+nJCM8/Okm6gyvEwvxnN65eCWDbnFLHgmV6HseEinLc3BP6luksZsFsCM7gbzWTcBuFsCOByqknJcWS2rO06BujF2qUmN6Xc/XUGzmJqHAByCS4ACaFMJYhE6/Ju7Lo69C9E7Je0xWqtAlwP0Th8jJqhSTSIbob/iWonsYe9VQ7gZH5TOC9FthwiztlgFS7anLTwqilYiq4NAcjec8k3iaOMGxa+UWB9H8oXIek4xUsIEcEW7tXB68baOwczBRy+8p2kzM0s6KjO6kcWaSmxR6dAluc49V0FpcaoYRbyGQA2hNS11G6/MKVq0dka3+HSNWXmmGIVnR6jAyOFjXE1jywDVKok7oDkHbiYNq60BB5vDUu+VA9qvleesTRVkR+aWVSJwhzosX25e9fRSl+2KKxT5yA33yk1GDTB30mGuKJ3w6HWaDOJocwIDxg9SPmNo96F1i4fNqe144Dh/4hhpqO898pVz6dvLil758fX9A/d2XCX8fb4KPq4fkYXg8eI2";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/css");
        }
    }
}