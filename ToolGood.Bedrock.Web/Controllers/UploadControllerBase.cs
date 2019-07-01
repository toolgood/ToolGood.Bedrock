using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using ToolGood.Bedrock.Web.Constants;
using ToolGood.Bedrock.Web.Controllers.BaseCore;

namespace ToolGood.Bedrock.Web
{
    public abstract class UploadControllerBase : WebControllerBaseCore
    {
        protected virtual string GetFileUrl(DateTime time, string md5, string fileExt)
        {
            return FileConstants.FilePath + time.ToString("yyyyMMdd") + "/" + md5 + fileExt;
        }

        protected virtual string GetImageUrl(DateTime time, string md5, string fileExt)
        {
            return FileConstants.ImagePath + time.ToString("yyyyMMdd") + "/" + md5 + fileExt;
        }

        protected virtual string GetVideoUrl(DateTime time, string md5, string fileExt)
        {
            return FileConstants.VideoPath + time.ToString("yyyyMMdd") + "/" + md5 + fileExt;
        }

        public async Task<IActionResult> UploadImage(IFormFile file, string md5, string fileExt)
        {
            if (file == null) return Json(new { code = ErrorCode, success = false, msg = "请上传文件!" });

            if (file.Length > FileConstants.ImageMaxSize) return Json(new { code = ErrorCode, success = false, msg = "文件过大!" });
            if (string.IsNullOrEmpty(fileExt)) fileExt = Path.GetExtension(file.FileName).ToLower();
            if (!FileConstants.ImageAllowExt.Contains(fileExt)) return Json(new { code = ErrorCode, success = false, msg = "无效后缀!" });

            if (string.IsNullOrEmpty(md5)) {
                var date = Request.Form["lastModifiedDate"];
                var f = file.FileName + "|" + file.Length.ToString() + "|" + date;
                md5 = HashUtil.GetMd5String(f);
            }

            var url = GetImageUrl(DateTime.Now, md5, fileExt);
            var path = MapWebRootPath(url);

            if (!Directory.Exists(Path.GetDirectoryName(path))) {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            try {
                if (System.IO.File.Exists(path) == false) {
                    using (var fileStream = new FileStream(path, FileMode.Create)) {
                        await file.CopyToAsync(fileStream);
                    }
                }
            } catch (Exception) {
                return Json(new { code = ErrorCode, success = false, msg = "网络错误!" });
            }
            return Json(new { code = SuccessCode, success = true, file_path = url, file_Name = file.FileName, msg = url });
        }

        public async Task<IActionResult> UploadVideo(IFormFile file, string md5, string fileExt)
        {
            if (file == null) return Json(new { code = ErrorCode, success = false, msg = "请上传文件!" });

            if (file.Length > FileConstants.VideoMaxSize) return Json(new { code = ErrorCode, success = false, msg = "文件过大!" });
            if (string.IsNullOrEmpty(fileExt)) fileExt = Path.GetExtension(file.FileName).ToLower();
            if (!FileConstants.VideoAllowExt.Contains(fileExt)) return Json(new { code = ErrorCode, success = false, msg = "无效后缀!" });

            if (string.IsNullOrEmpty(md5)) {
                var date = Request.Form["lastModifiedDate"];
                var f = file.FileName + "|" + file.Length.ToString() + "|" + date;
                md5 = HashUtil.GetMd5String(f);
            }

            var url = GetVideoUrl(DateTime.Now, md5, fileExt);
            var path = MapWebRootPath(url);

            if (!Directory.Exists(Path.GetDirectoryName(path))) {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            try {
                if (System.IO.File.Exists(path) == false) {
                    using (var fileStream = new FileStream(path, FileMode.Create)) {
                        await file.CopyToAsync(fileStream);
                    }
                }
            } catch (Exception) {
                return Json(new { code = ErrorCode, success = false, msg = "网络错误!" });
            }
            return Json(new { code = SuccessCode, success = true, file_path = url, file_Name = file.FileName, msg = url });
        }

        public async Task<IActionResult> UploadFile(IFormFile file, string md5, string fileExt)
        {
            if (file == null) return Json(new { code = ErrorCode, success = false, msg = "请上传文件!" });

            if (file.Length > FileConstants.FileMaxSize) return Json(new { code = ErrorCode, success = false, msg = "文件过大!" });
            if (string.IsNullOrEmpty(fileExt)) fileExt = Path.GetExtension(file.FileName).ToLower();
            if (!FileConstants.FileAllowExt.Contains(fileExt)) return Json(new { code = ErrorCode, success = false, msg = "无效后缀!" });

            if (string.IsNullOrEmpty(md5)) {
                var date = Request.Form["lastModifiedDate"];
                var f = file.FileName + "|" + file.Length.ToString() + "|" + date;
                md5 = HashUtil.GetMd5String(f);
            }
            var url = GetFileUrl(DateTime.Now, md5, fileExt);
            var path = MapWebRootPath(url);

            if (!Directory.Exists(Path.GetDirectoryName(path))) {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            try {
                if (System.IO.File.Exists(path) == false) {
                    using (var fileStream = new FileStream(path, FileMode.Create)) {
                        await file.CopyToAsync(fileStream);
                    }
                }
            } catch (Exception) {
                return Json(new { code = ErrorCode, success = false, msg = "网络错误!" });
            }

            return Json(new { code = SuccessCode, success = true, file_path = url, file_Name = file.FileName, msg = url });
        }

        public IActionResult UploadBigFile(IFormFile file, string status, string md5, string fileExt, long start, long end)
        {
            if (file == null) return Json(new { code = ErrorCode, success = false, msg = "请上传文件!" });

            if (string.IsNullOrEmpty(fileExt)) fileExt = Path.GetExtension(file.FileName).ToLower();
            if (!FileConstants.FileAllowExt.Contains(fileExt)) return Json(new { code = ErrorCode, success = false, msg = "无效后缀!" });

            if (string.IsNullOrEmpty(md5)) {
                var date = Request.Form["lastModifiedDate"];
                var f = file.FileName + "|" + file.Length.ToString() + "|" + date;
                md5 = HashUtil.GetMd5String(f);
            }

            var url = GetFileUrl(DateTime.Now, md5, fileExt);
            var path = MapWebRootPath(url);

            if (status == "chunkCheck") {
                if (System.IO.File.Exists(path) == false) return Json(new { code = ErrorCode, success = false, msg = "文件不存在!" });
                var fi = new FileInfo(path);
                if (fi.Length >= end) return Json(new { code = SuccessCode, success = true, msg = "可写入!" });
                return Json(new { code = ErrorCode, success = false, msg = "已上传!" });
            } else {
                if (end > FileConstants.FileMaxSize) return new StatusCodeResult(500);


                FileStream fs;
                if (System.IO.File.Exists(path) == false || start == 0) {
                    fs = System.IO.File.Open(path, FileMode.OpenOrCreate);
                } else {
                    fs = System.IO.File.Open(path, FileMode.Open);
                    fs.Seek(start, SeekOrigin.Begin);
                }
                byte[] bytes = new byte[1024];
                var stream = file.OpenReadStream();
                var length = stream.Read(bytes, 0, 1024);
                while (length > 0) {
                    fs.Write(bytes, 0, length);
                    length = stream.Read(bytes, 0, 1024);
                }
                fs.Close();
                fs.Dispose();
                return Success(url);
            }
        }

    }
}
