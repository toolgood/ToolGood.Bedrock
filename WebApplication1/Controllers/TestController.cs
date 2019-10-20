using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace WebApplication1.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            if (Request.Headers["If-None-Match"] == "{FileHash}") {
                return StatusCode(304);
            }
            SetResponseHeaders("");

            const string s = "";
            var bytes = UseCompressBytes(s);
            return File(bytes, "");
        }
        private void SetResponseHeaders(string etag)
        {
            Response.Headers["Etag"] = etag;
            Response.Headers["Cache-Control"] = "max-age=315360000";
            Response.Headers["Date"] = DateTime.Now.ToString("r");
            Response.Headers["Expires"] = DateTime.Now.AddYears(100).ToString("r");
        }
        private byte[] UseCompressBytes(string s)
        {
            var bytes = Convert.FromBase64String(s);
            if (Request.Headers["Accept-Encoding"].ToString().Replace(" ","").Split(',').Contains("br")) {
                Response.Headers["Content-Encoding"] = "br";
            } else {
                Response.Headers["Content-Encoding"] = "gzip";
                using (MemoryStream stream = new MemoryStream()) {
                    using (BrotliStream zStream = new BrotliStream(stream, CompressionMode.Decompress)) {
                        zStream.Write(bytes, 0, bytes.Length);
                        zStream.Close();
                    }
                    bytes = stream.ToArray();
                }
                using (MemoryStream stream = new MemoryStream()) {
                    using (GZipStream zStream = new GZipStream(stream, CompressionMode.Decompress)) {
                        zStream.Write(bytes, 0, bytes.Length);
                        zStream.Close();
                    }
                    bytes = stream.ToArray();
                }
            }
            return bytes;
        }


    }
}