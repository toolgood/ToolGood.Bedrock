using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using ToolGood.Bedrock.Web.Constants;
using ToolGood.Bedrock.Web.Controllers.BaseCore;

namespace ToolGood.Bedrock.Web
{
    /// <summary>
    /// 上传控制器基类
    /// </summary>
    public abstract class UploadControllerBase : WebControllerBaseCore
    {
        #region Url
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
        #endregion

        #region Error
        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected override IActionResult Error(string msg)
        {
            return Json(new { code = ErrorCode, success = false, msg = msg });
        }
        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected virtual IActionResult Success(string url, string fileName)
        {
            return Json(new { code = SuccessCode, success = true, file_path = url, file_Name = fileName, msg = url });
        }

        #endregion

        #region UploadImage 上传图片
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="file"></param>
        /// <param name="md5"></param>
        /// <param name="fileExt"></param>
        /// <param name="virtDir"></param>
        /// <returns></returns>
        public async Task<IActionResult> UploadImage(IFormFile file, string md5, string fileExt, string virtDir = null)
        {
            if (file == null) return Error("请上传文件!");


            if (file.Length > FileConstants.ImageMaxSize) return Error("文件过大!");
            if (string.IsNullOrEmpty(fileExt)) fileExt = Path.GetExtension(file.FileName).ToLower();
            if (!FileConstants.ImageAllowExt.Contains(fileExt)) return Error("无效后缀!");

            if (string.IsNullOrEmpty(md5)) {
                var date = Request.Form["lastModifiedDate"];
                var f = file.FileName + "|" + file.Length.ToString() + "|" + date;
                md5 = HashUtil.GetMd5String(f);
            }

            var url = GetImageUrl(DateTime.Now, md5, fileExt);
            var path = MapWebRootPath(url);
            if (CheckUploadImage(file.FileName, path, url, md5, fileExt, virtDir, out string errMsg) == false) { return Error(errMsg); }

            if (!Directory.Exists(Path.GetDirectoryName(path))) {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            try {
                if (System.IO.File.Exists(path) == false) {
                    using (var fileStream = new FileStream(path, FileMode.Create)) {
                        await file.CopyToAsync(fileStream);
                        AfterUploadImage(file.FileName, path, url, md5, fileExt, virtDir);
                    }
                }
            } catch (Exception) {
                return Error("网络错误!");
            }
            return Success(url, file.FileName);
        }
        /// <summary>
        /// 核对上传图片
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="filePath"></param>
        /// <param name="fileUrl"></param>
        /// <param name="md5"></param>
        /// <param name="fileExt"></param>
        /// <param name="virtDir"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        protected virtual bool CheckUploadImage(string srcName, string filePath, string fileUrl, string md5, string fileExt, string virtDir, out string errMsg)
        {
            errMsg = null;
            return true;
        }
        /// <summary>
        /// 上传图片后操作
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="filePath"></param>
        /// <param name="fileUrl"></param>
        /// <param name="md5"></param>
        /// <param name="fileExt"></param>
        /// <param name="virtDir"></param>
        protected virtual void AfterUploadImage(string srcName, string filePath, string fileUrl, string md5, string fileExt, string virtDir) { }

        #endregion

        #region UploadVideo 上传视频
        /// <summary>
        /// 上传视频
        /// </summary>
        /// <param name="file"></param>
        /// <param name="md5"></param>
        /// <param name="fileExt"></param>
        /// <param name="virtDir"></param>
        /// <returns></returns>
        public async Task<IActionResult> UploadVideo(IFormFile file, string md5, string fileExt, string virtDir = null)
        {
            if (file == null) return Error("请上传文件!");

            if (file.Length > FileConstants.VideoMaxSize) return Error("文件过大!");
            if (string.IsNullOrEmpty(fileExt)) fileExt = Path.GetExtension(file.FileName).ToLower();
            if (!FileConstants.VideoAllowExt.Contains(fileExt)) return Error("无效后缀!");

            if (string.IsNullOrEmpty(md5)) {
                var date = Request.Form["lastModifiedDate"];
                var f = file.FileName + "|" + file.Length.ToString() + "|" + date;
                md5 = HashUtil.GetMd5String(f);
            }

            var url = GetVideoUrl(DateTime.Now, md5, fileExt);
            var path = MapWebRootPath(url);
            if (CheckUploadVideo(file.FileName, path, url, md5, fileExt, virtDir, out string errMsg) == false) { return Error(errMsg); }


            if (!Directory.Exists(Path.GetDirectoryName(path))) {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            try {
                if (System.IO.File.Exists(path) == false) {
                    using (var fileStream = new FileStream(path, FileMode.Create)) {
                        await file.CopyToAsync(fileStream);
                        AfterUploadVideo(file.FileName, path, url, md5, fileExt, virtDir);
                    }
                }
            } catch (Exception) {
                return Error("网络错误!");
            }
            return Success(url, file.FileName);
        }
        /// <summary>
        /// 核对上传视频
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="filePath"></param>
        /// <param name="fileUrl"></param>
        /// <param name="md5"></param>
        /// <param name="fileExt"></param>
        /// <param name="virtDir"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        protected virtual bool CheckUploadVideo(string srcName, string filePath, string fileUrl, string md5, string fileExt, string virtDir, out string errMsg)
        {
            errMsg = null;
            return true;
        }
        /// <summary>
        /// 上传视频后操作
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="filePath"></param>
        /// <param name="fileUrl"></param>
        /// <param name="md5"></param>
        /// <param name="fileExt"></param>
        /// <param name="virtDir"></param>
        protected virtual void AfterUploadVideo(string srcName, string filePath, string fileUrl, string md5, string fileExt, string virtDir) { }

        #endregion

        #region UploadFile 上传文件
        /// <summary> 
        /// 上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="md5"></param>
        /// <param name="fileExt"></param>
        /// <param name="virtDir"></param>
        /// <returns></returns>
        public async Task<IActionResult> UploadFile(IFormFile file, string md5, string fileExt, string virtDir = null)
        {
            if (file == null) return Error("请上传文件!");

            if (file.Length > FileConstants.FileMaxSize) return Error("文件过大!");
            if (string.IsNullOrEmpty(fileExt)) fileExt = Path.GetExtension(file.FileName).ToLower();
            if (!FileConstants.FileAllowExt.Contains(fileExt)) return Error("无效后缀!");

            if (string.IsNullOrEmpty(md5)) {
                var date = Request.Form["lastModifiedDate"];
                var f = file.FileName + "|" + file.Length.ToString() + "|" + date;
                md5 = HashUtil.GetMd5String(f);
            }
            var url = GetFileUrl(DateTime.Now, md5, fileExt);
            var path = MapWebRootPath(url);
            if (CheckUploadFile(file.FileName, path, url, md5, fileExt, virtDir, out string errMsg) == false) { return Error("网络错误!"); }


            if (!Directory.Exists(Path.GetDirectoryName(path))) {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            try {
                if (System.IO.File.Exists(path) == false) {
                    using (var fileStream = new FileStream(path, FileMode.Create)) {
                        await file.CopyToAsync(fileStream);
                        AfterUploadFile(file.FileName, path, url, md5, fileExt, virtDir);
                    }
                }
            } catch (Exception) {
                return Error("网络错误!");
            }
            return Success(url, file.FileName);
        }
        /// <summary>
        /// 核对上传文件
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="filePath"></param>
        /// <param name="fileUrl"></param>
        /// <param name="md5"></param>
        /// <param name="fileExt"></param>
        /// <param name="virtDir"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        protected virtual bool CheckUploadFile(string srcName, string filePath, string fileUrl, string md5, string fileExt, string virtDir, out string errMsg)
        {
            errMsg = null;
            return true;
        }
        /// <summary>
        /// 上传文件后操作
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="filePath"></param>
        /// <param name="fileUrl"></param>
        /// <param name="md5"></param>
        /// <param name="fileExt"></param>
        /// <param name="virtDir"></param>
        protected virtual void AfterUploadFile(string srcName, string filePath, string fileUrl, string md5, string fileExt, string virtDir) { }

        #endregion

        #region UploadBigFile 上传大文件
        /// <summary>
        /// 上传大文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="status"></param>
        /// <param name="md5"></param>
        /// <param name="fileExt"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="virtDir"></param>
        /// <returns></returns>
        public IActionResult UploadBigFile(IFormFile file, string status, string md5, string fileExt, long start, long end, string virtDir = null)
        {
            if (file == null) return Error("请上传文件!");

            if (string.IsNullOrEmpty(fileExt)) fileExt = Path.GetExtension(file.FileName).ToLower();
            if (!FileConstants.FileAllowExt.Contains(fileExt)) return Error("无效后缀!");

            if (string.IsNullOrEmpty(md5)) {
                var date = Request.Form["lastModifiedDate"];
                var f = file.FileName + "|" + file.Length.ToString() + "|" + date;
                md5 = HashUtil.GetMd5String(f);
            }

            var url = GetFileUrl(DateTime.Now, md5, fileExt);
            var path = MapWebRootPath(url);
            if (CheckUploadBigFile(file.FileName, path, url, md5, fileExt, virtDir, out string errMsg) == false) { return Error(errMsg); }


            if (status == "chunkCheck") {
                if (System.IO.File.Exists(path) == false) return Error("文件不存在!");
                var fi = new FileInfo(path);
                if (fi.Length >= end) return Success("可写入!");
                return Error("已上传!");
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

                var fi = new FileInfo(path);
                if (fi.Length <= end) {
                    AfterUploadBigFile(file.FileName, path, url, md5, fileExt, virtDir);
                }
            }
            return Success(url, file.FileName);
        }
        /// <summary>
        /// 核对大文件
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="filePath"></param>
        /// <param name="fileUrl"></param>
        /// <param name="md5"></param>
        /// <param name="fileExt"></param>
        /// <param name="virtDir"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        protected virtual bool CheckUploadBigFile(string srcName, string filePath, string fileUrl, string md5, string fileExt, string virtDir, out string errMsg)
        {
            errMsg = null;
            return true;
        }
        /// <summary>
        /// 大文件后操作
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="filePath"></param>
        /// <param name="fileUrl"></param>
        /// <param name="md5"></param>
        /// <param name="fileExt"></param>
        /// <param name="virtDir"></param>
        protected virtual void AfterUploadBigFile(string srcName, string filePath, string fileUrl, string md5, string fileExt, string virtDir) { }

        #endregion


    }
}
