using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Bedrock.Web.Mime;
using System.IO;
using ToolGood.Bedrock;

namespace ToolGood.WwwRoot
{
    internal class WwwRootFile
    {
        public string FilePath { get; set; }

        public string FileName { get; set; }

        public string FileUrl { get; set; }

        public string FileMime { get; set; }


        public string FileMethod { get; set; }

        public string FileContent { get; set; }

        public string FileHash { get; set; }


        public string GetFileMime(string file)
        {
            var ext = Path.GetExtension(file).ToLower();
            ext = ext.Replace(".", "");

            foreach (var item in DefaultMimeItems.Items) {
                if (item.Extension == ext) {
                    return item.MimeType;
                }
            }
            return "application/octet-stream";
        }

        public string GetFileContent(byte[] bytes)
        {
            bytes = CompressionUtil.BrCompress(bytes);
            return Convert.ToBase64String(bytes);
        }


    }
}
