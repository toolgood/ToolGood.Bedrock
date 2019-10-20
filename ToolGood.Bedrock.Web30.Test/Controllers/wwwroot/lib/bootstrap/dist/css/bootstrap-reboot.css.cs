using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.WwwRoot.Test
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("lib/bootstrap/dist/css/bootstrap-reboot.css")]
        public IActionResult lib_bootstrap_dist_css_bootstrap_reboot_css()
        {
            if (Request.Headers["If-None-Match"]  == "E14F00A9406E96D7725CC652D99F1211") {
                return StatusCode(304);
            }
            SetResponseHeaders("E14F00A9406E96D7725CC652D99F1211");
            const string s = "G2kUAByHsbNlE5ovpHHevrr8nYZdTYGs0HH6AXC6yY4VaGMrz3C914RCsqR0WbXf58n/ZUCeUJEyTt1NDySPAvwCBC5VO7O7YVTAClhHCHMVF+eySGfDE0WaIjx0+HfZzckx3MAb5uCDkyP8IcUcYDdPi3QGV5fR9EBLQaVDjdvrvAkcIsEGJe6T/zqCKfuFKsshu6DiIYWU6pUC63vy++d/qBztQxcVEifDo/IZXudkamCVVazPbo5hRvO9D+weaPf/vCOksX8iQtSt34rK+Z2lmgfpM5s3W9zW1OjXw9nxjfi9vqq6n/8Hxyb9Y1S8J8qP9LatYD7+tfu7vZSvPT6gO07r2HPa+RTvOPfeIi0WMUJxDfYzFTOXbJnU99HP5vn5QxmfRKmUx//6CsbLM5R05eyUI46iAbewuci1Sl7lAn4N1xVrXsFWHsI1FL3AjUbNIbNVNnLj1e1N386syfNe8qykNNDxNzZwpc1it1u5m0sLRUbkIaWBuyGNEfe175q/RVrv0V5s3pD3tu+y5qN05WL/lPyllgn+f05MW76SwxfqEw07Cn0t4QdFSsBr18tBXAKTNvEjwo33yfeG7/sEYu6D9ylGjy/+4MCZgb3jEKMjE+6swOfO83wU8YMyyrPKolyUm2DHgoll4DmnQUL0SctesZWtR7FIrrgSN7Ww0flHx410+OeIRPiUPG4EwJorYqqb8DcDd7gQ0JW/p7PpfLrolkWknFFABeLwgjxdODLnWzHmvLJ4Ij2i3U7B0guBvqCypsN4QA9lkskTT3SFSc8FzSGQ7rL6t2jHB7RkGIGBXjDklRFz8npTpf0YU4bBrDIzymwyTIV9Bg/iOP7xqPXQxtt5ACgtvE64j2HXYJhxrV6uA0yEMVlZmutrGDosrgJ72rKCtUDBiHjK3ZBj/1HdWJgaTueIWVWD2y6OzYYMtq3SQfQVmvy1JuLfuc7PE7URYhcHsvmyTFeJ8i9aLbQDPcFFKiOpH5Q0LY9L0nY7iGm5qH2CyBh4jE8fnpIwcaBvkOcr1TRDGgppARtKNZpi4mnw6mvJ5pCKhqjUITJvsliqGW0lUI5YDlfbia7a/jdeOBfmYCs9FFayHqwsgShPqPz3P8Mgz9iNb0L1GWvWf255UPrhVV6aUbc5/++H72wZ7zDbw9/JDnxaALayZgFv2XoepBeQ+BcoG4nBv/0tR9eTgx/0mADDlv0oaxKGKUN/dI6evZyBvVCtKaE4aCs1HGG2N21IX2xDOPZsPDYlukW71qMp5mA5WOSHMYDn0CrS/zS9uDXx7FwOO/N0xRsj88LHK1F9w2W9Wqy0biGlNaj79RUvMazb7sVlDiULzKNI5/MmVZ3AZ6vLtw02rL4renaPdHkieKoY96LW4iYLjCDjSwDQoFnLHEzhw2YbRDnz/7BdORHbeAxq7sZj47JOe9Um8jRhUXWCWhFhzB4UJq0rxjrtLzNR/QjWTGL1XGO7SiBpThIKL4Z70J0KiAKljZoYuesrABR12fEKDT+1vV0adED1BefTeVTybx65/NlTD3qrnLbR5AIYZj2awMUkG/XfxGalLzlbBHkM4oenQ28iLDVFtuhciJPAl1HcMeTGMFPXjeMSPst3K5WxMs3MHpSlFLab3i5eu2ZeZUcgrtnT5rDJ2fJWotmpaiED4t17DM8XmlcdvUxCTDEJ5gU2XDYtSjnVtbUu6vi96UlZk25jZxDGrHzha0Q/VkIptCzDcjgGRoArpJ3pC2WX1YxnnB06s8YK0A2XH0FGInmIhpV6Bt01ftZaVOZRHQinWhwSIOTy69AlJ0WaluO6tJ5VsT+xVIBpYvH5+UcmsQb9k97GU6euZFf3X/ToG5ivkVQuWWy1b8tuTvcsIvu3P/j/z7fnLrkRHf1P892sMK8j3GQ=";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/css");
        }
    }
}