using Microsoft.AspNetCore.Mvc;
using System.IO;
using ToolGood.Bedrock.Images;
using ToolGood.Bedrock.Web.Controllers.BaseCore;
using ToolGood.Bedrock.Web.Mime;

namespace ToolGood.Bedrock.Web
{
    public abstract class ImageControllerBase : WebControllerBaseCore
    {
        /// <summary>
        /// 剪切
        /// </summary>
        /// <param name="u"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public IActionResult C(string u, int w = 200, int h = 200)
        {
            var path = MyHostingEnvironment.MapWebRootPath(u);
            var thumbnailPath = MyHostingEnvironment.MapWebRootPath(("/Thumbnail/" + u).Replace("//", "/"));
            string newPath = Thumbnail.GetFileNameForCut(thumbnailPath, w, h);
            if (System.IO.File.Exists(newPath) == false) {
                var bytes = Thumbnail.MakeThumbnailImage(path, w, h, "CUT");
                System.IO.File.WriteAllBytes(newPath, bytes);
            }
            var contentType = new MimeMapper().GetMimeFromExtension(Path.GetExtension(path));
            return PhysicalFile(newPath, contentType);
        }
        /// <summary>
        /// 固定高
        /// </summary>
        /// <param name="u"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public IActionResult H(string u, int h = 200)
        {
            var path = MyHostingEnvironment.MapWebRootPath(u);
            var thumbnailPath = MyHostingEnvironment.MapWebRootPath(("/Thumbnail/" + u).Replace("//", "/"));
            var newPath = Thumbnail.GetFileNameForHeight(thumbnailPath, h);
            if (System.IO.File.Exists(newPath) == false) {
                var bytes = Thumbnail.MakeThumbnailImage(path, 0, h, "H");
                System.IO.File.WriteAllBytes(newPath, bytes);
            }
            var contentType = new MimeMapper().GetMimeFromExtension(Path.GetExtension(path));
            return PhysicalFile(newPath, contentType);
        }
        /// <summary>
        /// 固定宽
        /// </summary>
        /// <param name="u"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public IActionResult W(string u, int w = 200)
        {
            var path = MyHostingEnvironment.MapWebRootPath(u);
            var thumbnailPath = MyHostingEnvironment.MapWebRootPath(("/Thumbnail/" + u).Replace("//", "/"));
            var newPath = Thumbnail.GetFileNameForHeight(thumbnailPath, w);
            if (System.IO.File.Exists(newPath) == false) {
                var bytes = Thumbnail.MakeThumbnailImage(path, w, 0, "W");
                System.IO.File.WriteAllBytes(newPath, bytes);
            }
            var contentType = new MimeMapper().GetMimeFromExtension(Path.GetExtension(path));
            return PhysicalFile(newPath, contentType);
        }
        /// <summary>
        /// 缩小
        /// </summary>
        /// <param name="u"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public IActionResult T(string u, int w = 200, int h = 200)
        {
            var path = MyHostingEnvironment.MapWebRootPath(u);
            var thumbnailPath = MyHostingEnvironment.MapWebRootPath(("/Thumbnail/" + u).Replace("//", "/"));
            var newPath = Thumbnail.GetFileNameForThumbnail(thumbnailPath, w, h);
            if (System.IO.File.Exists(newPath) == false) {
                var bytes = Thumbnail.MakeThumbnailImage(path, w, h, "HW");
                System.IO.File.WriteAllBytes(newPath, bytes);
            }
            var contentType = new MimeMapper().GetMimeFromExtension(Path.GetExtension(path));
            return PhysicalFile(newPath, contentType);
        }

        /// <summary>
        /// 二维码
        /// </summary>
        /// <param name="u"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public IActionResult Qr(string u, int w = 200, int h=200)
        {
            var bytes = BarcodeHelper.CreateQrCode(u,w,h);
            return File(bytes, "image/jpeg");
        }



    }
}
