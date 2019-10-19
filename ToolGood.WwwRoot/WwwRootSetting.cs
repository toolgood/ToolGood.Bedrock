using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ToolGood.Bedrock;

namespace ToolGood.WwwRoot
{
    public class WwwRootSetting
    {
        public string FolderPath { get; set; }

        public string OutFolderPath { get; set; }



        public string NameSpace { get; set; } = "ToolGood.WwwRoot";

        public string ControllerName { get; set; } = "WwwRootController";


        public string Template = @"using Microsoft.AspNetCore.Mvc;
using System;
namespace {NameSpace}
{
    public partial class {ControllerName} : Controller
    {
        [HttpGet(""{FileUrl}"")]
        public IActionResult {FileMethod}()
        {
            if (Request.Headers[""If-None-Match""]  == ""{FileHash}"") {
                return StatusCode(304);
            }
            Response.Headers[""Cache-Control""] = ""max-age=315360000"";
            Response.Headers[""Etag""] = ""{FileHash}"";
            Response.Headers[""Date""] = DateTime.Now.ToString(""r"");
            Response.Headers[""Expires""] = DateTime.Now.AddYears(100).ToString(""r"");
            const string s = ""{FileContent}"";
            var bytes = Convert.FromBase64String(s);
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
            List<WwwRootFile> filesList = new List<WwwRootFile>();
            foreach (var file in files) {
                var bytes = File.ReadAllBytes(file);

                WwwRootFile wwwRootFile = new WwwRootFile() {
                    FilePath = file,
                    FileContent = Convert.ToBase64String(bytes),
                    FileHash = HashUtil.GetMd5String(bytes),
                    FileName = Path.GetFileName(file),
                };
                wwwRootFile.FileMime = wwwRootFile.GetFileMime(file);
                wwwRootFile.FileUrl = file.Substring(setting.FolderPath.Length).Replace("\\", "/").TrimStart('/');
                wwwRootFile.FileMethod = wwwRootFile.FileUrl.Replace("/", "_").Replace("-", "_").Replace(".", "_");

                filesList.Add(wwwRootFile);


                var txt = setting.Template;
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
            }
        }


    }
}
