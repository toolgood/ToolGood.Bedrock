using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.WwwRoot.Test
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("lib/bootstrap/dist/css/bootstrap-reboot.css")]
        public IActionResult lib_bootstrap_dist_css_bootstrap_reboot_css()
        {
            if (SetResponseHeaders("E14F00A9406E96D7725CC652D99F1211") == false) { return StatusCode(304); }
            const string s = "G6cPAIzUYU2Zl9qWlv/7vBynNOEtLYGUUl/7AiN7yRnkwfhKNCqWLr93mZpVQkyxGfLNjP4+S9o995JaUZV25CZfWm0A8ASGxUZ0WQtOU50e/2zx4MZZ9aB6RVSmku1Y/UJHVKqLbbNqNtW9s2i6rcfi0nGH7f2sQSkkgFbp22v/nLBIc1SopAzm+FOCJ/rGEimmvWu/fvzTjt2hnGbXgv3l0k2LRh2xcAO5RafKxVUxzG185h3lczzyvxwDaSr8x31wAf20ar6XsKXBTouUV61tqon+9cjF2QN4oPWs0388TUvs6EoIHxBSr09+f+hK5GsDz0A4vdG87F01q50RJasmkMpbQmX9RDot57eq4hQLoRM4ez371zibC563o8KD0MsxOvfO3lvCz6n78qIsUIBh0QhGo0YANgQWbluhC70oeMCpBxKxAEv6k9FoAcJ7IdkLwP1G5oo6/EbD5ZT0lYrB7St5m/lqW2L7v99RKlD/xp6w+vux9oscFYL6Aw4XWEJrq284Y+1lDnYgoCmoXxbMO9S/422kf6G+VwLl4F6Nx/u/UaGckQDflzEaFr7Rdrmc4WpHxx+6Xq1368dGahVBfabrOjmqr91qVdeCXzJJnaeXu/qcLycUGrmnR/Bhdrm5gGGJA+S0gtP6V8emY9uxO3fPdHKBpdl8+BPvMkYZk9nVAylXdzxdEgf/swoF/XoVuzkTFfUsFgQ91W+rPJWC3jzU7fdPxJgxjDi6FALNcicwmSJJQgvxdBYn9/j7hAaYB/DDmEzTUKXvr14b/3fPFeUNWnWdLwyjjsPDxHuehw2G6/vuWZJY57VPV8uq+nt9l7hHOI2SLa04UD559hCmMYvOCNXhQY+Wt2Wac91kHlmRBGmvP+xuj9SFlkb1kBKjRFRiyT06VDXrHUYpf7LQGEQP51jea/GlB9d1RoEpEhOquSQQWXV5pox+LMW2Cip8xN3ebWDVyLMrUbl3PH0jf/9/yGdfmJQEeGjNXlEhxs1MTycEsLbV1fLIX5QxI7TkEc6dh8nGUWUd/fvdV0qkjnT4x+ArpoHgKyXbErymNNFgJ6idwkZ8D9XqZ+YcMF/m1cNqkRKZLBFvEqFcVsbngLjYjEvWdk4UkSIJDwqxZ0SK7QZigLwTQm940bMZped3YmZl2dbCP6Uoxw186GZ3EOE6A83EGDB+8L497A5eRZgirdK9Xme+V8qJk4Kb2HbcnJ0endPF/kRfTsjWh3nSy6g6vMytxit6RXDnjvxiCB7nssqQF+l0Yp56KNOD+bwFaCzmAexn3QZAtwLgIFQnZVMN15lndRc3QsausKl5+pGKFohkjlsujYYwdrWCB+z3PJYHILgIXpIdY7lXq0j/x+eDQkqYoZaUVKg37Q0t0e+b7MoqaJe5QT091gfaAOHBDsUttG6bpoK4OPJN6h6uKyEGIyVFOeXC1CVgfylhZMlJIsu6wE4NQ4iPWmmh9eKGHYWFY0jueXPZdjBOMULzeimn4pRhTyp0t++N9koFFcWp4hXxHQeqyM0YSmd3zSq6hP1RibPXEitVM3Li8xEU8MlfU2oa2yb4TkveC83Fc1N5aFGLuKJt4ASv/NMhrRxrZcIEanH+sFr499Hl9kUjV5Ij1HYtcaVBPvLUvfKk0TOalmLAEyVXnH6dSEvxxZGAYflBKhSMZjVbJU44UpsxkYsCFMdEtjw=";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/css");
        }
    }
}