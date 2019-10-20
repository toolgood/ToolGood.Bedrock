using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using ToolGood.Bedrock;

namespace ToolGood.WwwRoot
{
    public class WwwRootSetting
    {
        public string FolderPath { get; set; }

        public string OutFolderPath { get; set; }



        public string NameSpace { get; set; } = "ToolGood.WwwRoot";

        public string ControllerName { get; set; } = "WwwRootController";

        public string FirstTemplate = @"using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace {NameSpace}
{
    public partial class {ControllerName} : Controller
    {
        [HttpGet(""{FileUrl}"")]
        public IActionResult {FileMethod}()
        {
            if (SetResponseHeaders(""{FileHash}"") == false) { return StatusCode(304); }
            const string s = ""{FileContent}"";
            var bytes = UseCompressBytes(s);
            return File(bytes, ""{FileMime}"");
        }
        private bool SetResponseHeaders(string etag)
        {
            if (Request.Headers[""If-None-Match""] == etag) { return false; }
            Response.Headers[""Cache-Control""] = ""max-age=315360000"";
            Response.Headers[""Etag""] = etag;
            Response.Headers[""Date""] = DateTime.Now.ToString(""r"");
            Response.Headers[""Expires""] = DateTime.Now.AddYears(100).ToString(""r"");
            return true;
        }
        private byte[] UseCompressBytes(string s)
        {
            var bytes = Convert.FromBase64String(s);
            if (Request.Headers[""Accept-Encoding""].ToString().Replace("" "", """").Split(',').Contains(""br"")) {
                Response.Headers[""Content-Encoding""] = ""br"";
            } else {
                Response.Headers[""Content-Encoding""] = ""gzip"";
                using (MemoryStream stream = new MemoryStream(bytes)) {
                    using (BrotliStream zStream = new BrotliStream(stream, CompressionMode.Decompress)) {
                        using (var resultStream = new MemoryStream()) {
                            zStream.CopyTo(resultStream);
                            bytes = resultStream.ToArray();
                        }
                    }
                }
                using (MemoryStream stream = new MemoryStream()) {
                    using (GZipStream zStream = new GZipStream(stream, CompressionMode.Compress)) {
                        zStream.Write(bytes, 0, bytes.Length);
                    }
                    bytes = stream.ToArray();
                }
            }
            return bytes;
        }
    }
}";


        public string Template = @"using Microsoft.AspNetCore.Mvc;
using System;
namespace {NameSpace}
{
    public partial class {ControllerName} : Controller
    {
        [HttpGet(""{FileUrl}"")]
        public IActionResult {FileMethod}()
        {
            if (SetResponseHeaders(""{FileHash}"") == false) { return StatusCode(304); }
            const string s = ""{FileContent}"";
            var bytes = UseCompressBytes(s);
            return File(bytes, ""{FileMime}"");
        }
    }
}";

        public void BuildControllers()
        {
            var setting = this;
            if (setting.FolderPath.EndsWith("\\") == false) { setting.FolderPath += "\\"; }
            if (setting.OutFolderPath.EndsWith("\\") == false) { setting.OutFolderPath += "\\"; }


            var files = Directory.GetFiles(setting.FolderPath, "*.*", SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; i++) {
                var file = files[i];
                Console.WriteLine("Load:" + file);
                var bytes = File.ReadAllBytes(file);

                WwwRootFile wwwRootFile = new WwwRootFile() {
                    FilePath = file,
                    //FileContent = Convert.ToBase64String(bytes),
                    FileHash = HashUtil.GetMd5String(bytes),
                    FileName = Path.GetFileName(file),
                };
                wwwRootFile.FileContent = wwwRootFile.GetFileContent(bytes);
                wwwRootFile.FileMime = wwwRootFile.GetFileMime(file);
                wwwRootFile.FileUrl = file.Substring(setting.FolderPath.Length).Replace("\\", "/").TrimStart('/');
                wwwRootFile.FileMethod =
                    Regex.Replace(wwwRootFile.FileUrl, 
                    "[^0-9_a-zA-Z\u4E00-\u9FCB\u3400-\u4DB5\u20000-\u2A6D6\u2A700-\u2B734\u2B740-\u2B81D\u3007]", "_");
  



            var txt = setting.Template;
                if (i == 0) { txt = setting.FirstTemplate; }
                txt = txt.Replace("{NameSpace}", setting.NameSpace);
                txt = txt.Replace("{ControllerName}", setting.ControllerName);
                txt = txt.Replace("{FileUrl}", wwwRootFile.FileUrl);
                txt = txt.Replace("{FileMethod}", wwwRootFile.FileMethod);
                txt = txt.Replace("{FileHash}", wwwRootFile.FileHash);
                txt = txt.Replace("{FileContent}", wwwRootFile.FileContent);
                txt = txt.Replace("{FileMime}", wwwRootFile.FileMime);

                var outPath = file.Replace(setting.FolderPath, setting.OutFolderPath) + ".cs";
                Directory.CreateDirectory(Path.GetDirectoryName(outPath));

                File.WriteAllText(outPath, txt);
                Console.WriteLine("Out:" + outPath);

            }
        }


    }
}
