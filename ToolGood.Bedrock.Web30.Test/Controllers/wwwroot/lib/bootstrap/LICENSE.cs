using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.WwwRoot.Test
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("lib/bootstrap/LICENSE")]
        public IActionResult lib_bootstrap_LICENSE()
        {
            if (Request.Headers["If-None-Match"]  == "C6E5AA75B8CC204E62AC4ED2B78678FE") {
                return StatusCode(304);
            }
            SetResponseHeaders("C6E5AA75B8CC204E62AC4ED2B78678FE");
            const string s = "G4AEAByJcSzKmtRuC/Lt5xQqwr+r2tjOtpxBUkA5xN9RKORbWZ0Va2udUNKzJ4hYE2tmCx4qNBolMzRs1NbTq9xKEweczQ9hwfl4wFjnHu/GNCmod9bdyWtIxqnJDiNrCrhUaf7Q9rFThup62zVkmE3jLepHA+eXVn+AJzCHgm6HcaqF5e0/jkPT3PB422M5TSZHylFA+iyOlMiPC0uJt3l02sMW1srZEl2bkFS7dLzRbOmTfk7Nm3tyzLIr4QU6FKzJdWrhbbYD8jF8pe34nMh3toVUXqxjMoB5JUli8E197DUlrkIj4c2NqhA+iKyMPwXSZZAfT/indzREG2vDHLYaPzZgLMN2Iq3RTuIg8gr5YY1UkepF108pUUGJ0v+WlsyrVZiTOU+/u7ceX53KU1nd16EpzihHPpatjELpnmVn4MwaYPUaVvU1tSVvoAFsaTgAaFgbT6ApHUBhRNi0oYJMGITCVjOvzn42OIHRdhgTBjcTgZOnQFMsYE1wOP/Y4kFUWIKUJWTE5ogFokh9lrII+uTgbFJUUtEUxM9ERHr0rwyOjznbzty3ryAN8wMVUWToFORguzRnR5pUc+ZYHir7m2UNmBr4lmua85TQpEUF7EQMkRZ5Qk0igqIj0NB61OCK3zpF5rsIMNBOs4QZWCLSORKTJw==";
            var bytes = UseCompressBytes(s);
            return File(bytes, "application/octet-stream");
        }
    }
}