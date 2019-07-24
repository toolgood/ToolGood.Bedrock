using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace ToolGood.Bedrock.Web
{
    public abstract class ImageSelectControllerCore : FileManagerControllerCore
    {


        protected ImageSelectControllerCore() : base()
        {
            // FileManager Content Folder Path
            _webPath = "";
            _webRootPath = Path.Combine(MyHostingEnvironment.WebRootPath, _webPath);
            _allowedExtensions = new List<string> { "jpg", "jpe", "jpeg", "gif", "png" };
        }

        protected override dynamic GetFolder(string path)
        {
            if (path == null) path = string.Empty;
            var rootpath = Path.Combine(_webRootPath, path);
            var rootDirectory = new DirectoryInfo(rootpath);
            var data = new List<dynamic>();

            foreach (var directory in rootDirectory.GetDirectories()) {
                var item = new {
                    Id = MakeWebPath(Path.Combine(path, directory.Name), false, true),
                    Type = "folder",
                    Attributes = new {
                        Name = directory.Name,
                        Path = MakeWebPath(Path.Combine(_webPath, path, directory.Name), true, true),
                        Readable = 1,
                        Writable = 1,
                        Created = directory.CreationTime.ToString(CultureInfo.InvariantCulture),
                        Modified = directory.LastWriteTime.ToString(CultureInfo.InvariantCulture),
                        Timestamp = (int)(DateTime.Now - directory.LastWriteTime).Ticks
                    }
                };

                data.Add(item);
            }

            foreach (var file in rootDirectory.GetFiles()) {
                if (!_allowedExtensions.Contains(file.Extension.ToLower().Substring(1))) continue;

                var item = new {
                    Id = MakeWebPath(Path.Combine(path, file.Name)),
                    Type = "file",
                    Attributes = new {
                        Name = file.Name,
                        Path = MakeWebPath(Path.Combine(_webPath, path, file.Name), true),
                        Readable = 1,
                        Writable = 1,
                        Created = file.CreationTime.ToString(CultureInfo.InvariantCulture),
                        Modified = file.LastWriteTime.ToString(CultureInfo.InvariantCulture),
                        Extension = file.Extension.Replace(".", ""),
                        Size = file.Length,
                        Timestamp = (int)(DateTime.Now - file.LastWriteTime).Ticks
                    }
                };

                data.Add(item);
            }

            var result = new {
                Data = data

            };

            return result;
        }

        private static string MakeWebPath(string path, bool addSeperatorToBegin = false, bool addSeperatorToLast = false)
        {
            path = path.Replace("\\", "/");

            if (addSeperatorToBegin) path = "/" + path;
            if (addSeperatorToLast) path = path + "/";

            return path;
        }
    }

}
