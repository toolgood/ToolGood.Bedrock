using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        [HttpGet]
        public virtual IActionResult C(string u, int w = 200, int h = 200)
        {
            var thumbnailPath = MapWebRootPath(("/Thumbnail/" + u).Replace("//", "/"));
            string newPath = Thumbnail.GetFileNameForCut(thumbnailPath, w, h);
            if (System.IO.File.Exists(newPath) == false) {
                var path = MapWebRootPath(u);
                var bytes = Thumbnail.MakeThumbnailImage(path, w, h, "CUT");
                Directory.CreateDirectory(Path.GetDirectoryName(newPath));
                System.IO.File.WriteAllBytes(newPath, bytes);
            }
            var contentType = new MimeMapper().GetMimeFromExtension(Path.GetExtension(newPath));
            return PhysicalFile(newPath, contentType);
        }
        /// <summary>
        /// 固定高
        /// </summary>
        /// <param name="u"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual IActionResult H(string u, int h = 200)
        {
            var thumbnailPath = MapWebRootPath(("/Thumbnail/" + u).Replace("//", "/"));
            var newPath = Thumbnail.GetFileNameForHeight(thumbnailPath, h);
            if (System.IO.File.Exists(newPath) == false) {
                var path = MapWebRootPath(u);
                var bytes = Thumbnail.MakeThumbnailImage(path, 0, h, "H");
                Directory.CreateDirectory(Path.GetDirectoryName(newPath));
                System.IO.File.WriteAllBytes(newPath, bytes);
            }
            var contentType = new MimeMapper().GetMimeFromExtension(Path.GetExtension(newPath));
            return PhysicalFile(newPath, contentType);
        }
        /// <summary>
        /// 固定宽
        /// </summary>
        /// <param name="u"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual IActionResult W(string u, int w = 200)
        {
            var thumbnailPath = MapWebRootPath(("/Thumbnail/" + u).Replace("//", "/"));
            var newPath = Thumbnail.GetFileNameForHeight(thumbnailPath, w);
            if (System.IO.File.Exists(newPath) == false) {
                var path = MapWebRootPath(u);
                var bytes = Thumbnail.MakeThumbnailImage(path, w, 0, "W");
                Directory.CreateDirectory(Path.GetDirectoryName(newPath));
                System.IO.File.WriteAllBytes(newPath, bytes);
            }
            var contentType = new MimeMapper().GetMimeFromExtension(Path.GetExtension(newPath));
            return PhysicalFile(newPath, contentType);
        }
        /// <summary>
        /// 缩小
        /// </summary>
        /// <param name="u"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual IActionResult T(string u, int w = 200, int h = 200)
        {
            var thumbnailPath = MapWebRootPath(("/Thumbnail/" + u).Replace("//", "/"));
            var newPath = Thumbnail.GetFileNameForThumbnail(thumbnailPath, w, h);
            if (System.IO.File.Exists(newPath) == false) {
                var path = MapWebRootPath(u);
                var bytes = Thumbnail.MakeThumbnailImage(path, w, h, "HW");
                Directory.CreateDirectory(Path.GetDirectoryName(newPath));
                System.IO.File.WriteAllBytes(newPath, bytes);
            }
            var contentType = new MimeMapper().GetMimeFromExtension(Path.GetExtension(newPath));
            return PhysicalFile(newPath, contentType);
        }

        /// <summary>
        /// 二维码
        /// </summary>
        /// <param name="u"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual IActionResult Qr(string u, int w = 200, int h = 200)
        {
            var bytes = BarcodeHelper.CreateQrCode(u, w, h);
            return File(bytes, "image/jpeg");
        }

        /// <summary>
        /// 剪切
        /// </summary>
        /// <param name="u"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual IActionResult ImageCut(string u, int w, int h)
        {
            var thumbnailUrl = ("/Thumbnail/" + u).Replace("//", "/");
            var thumbnailPath = MapWebRootPath(thumbnailUrl);
            string newPath = Thumbnail.GetFileNameForCut(thumbnailPath, w, h);
            var path = MapWebRootPath(u);
            var bytes = Thumbnail.MakeThumbnailImage(path, w, h, "CUT");
            Directory.CreateDirectory(Path.GetDirectoryName(newPath));
            System.IO.File.WriteAllBytes(newPath, bytes);
            return Success(thumbnailUrl);
        }
        /// <summary>
        /// 锁定高
        /// </summary>
        /// <param name="u"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual IActionResult ImageLockHeight(string u, int h = 200)
        {
            var thumbnailUrl = ("/Thumbnail/" + u).Replace("//", "/");
            var thumbnailPath = MapWebRootPath(thumbnailUrl);
            var newPath = Thumbnail.GetFileNameForHeight(thumbnailPath, h);
            var path = MapWebRootPath(u);
            var bytes = Thumbnail.MakeThumbnailImage(path, 0, h, "H");
            Directory.CreateDirectory(Path.GetDirectoryName(newPath));
            System.IO.File.WriteAllBytes(newPath, bytes);
            return Success(thumbnailUrl);
        }

        /// <summary>
        /// 锁定宽
        /// </summary>
        /// <param name="u"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual IActionResult ImageLockWidth(string u, int w = 200)
        {
            var thumbnailUrl = ("/Thumbnail/" + u).Replace("//", "/");
            var thumbnailPath = MapWebRootPath(thumbnailUrl);
            var newPath = Thumbnail.GetFileNameForHeight(thumbnailPath, w);
            var path = MapWebRootPath(u);
            var bytes = Thumbnail.MakeThumbnailImage(path, w, 0, "W");
            Directory.CreateDirectory(Path.GetDirectoryName(newPath));
            System.IO.File.WriteAllBytes(newPath, bytes);
            return Success(thumbnailUrl);
        }

        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="u"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual IActionResult ImageThumbnail(string u, int w = 200, int h = 200)
        {
            var thumbnailUrl = ("/Thumbnail/" + u).Replace("//", "/");
            var thumbnailPath = MapWebRootPath(thumbnailUrl);
            var newPath = Thumbnail.GetFileNameForThumbnail(thumbnailPath, w, h);
            var path = MapWebRootPath(u);
            var bytes = Thumbnail.MakeThumbnailImage(path, w, h, "HW");
            Directory.CreateDirectory(Path.GetDirectoryName(newPath));
            System.IO.File.WriteAllBytes(newPath, bytes);
            return Success(thumbnailUrl);
        }

        /// <summary>
        /// 获取 字体名称
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual IActionResult GetFontNames()
        {
            var fontFamilies = System.Drawing.FontFamily.Families;
            List<string> list = new List<string>();
            foreach (var font in fontFamilies) {
                list.Add(font.Name);
            }
            return Success(list);
        }

        /// <summary>
        /// logo水印
        /// </summary>
        /// <param name="u"></param>
        /// <param name="l">logo水印文件</param>
        /// <param name="s">图片水印位置 0=不使用 1=左上 2=中上 3=右上 4=左中  9=右下</param>
        /// <param name="q">附加水印图片质量,0-100</param>
        /// <param name="t">水印的透明度 1--100 100为不透明</param>
        /// <returns></returns>
        [HttpPost]
        public virtual IActionResult ImageWaterMarkLogo(string u, string l, int s, int q, int t)
        {
            var thumbnailUrl = ("/WaterMark/" + u).Replace("//", "/");
            var newPath = MapWebRootPath(thumbnailUrl);

            var path = MapWebRootPath(u);
            var logoPath = MapWebRootPath(l);
            var bytes = WaterMark.AddImageSignPic(path, logoPath, s, q, t);
            Directory.CreateDirectory(Path.GetDirectoryName(newPath));
            System.IO.File.WriteAllBytes(newPath, bytes);

            return Success(thumbnailUrl);
        }

        /// <summary>
        /// 文字水印
        /// </summary>
        /// <param name="u"></param>
        /// <param name="t">水印文字</param>
        /// <param name="s">图片水印位置 0=不使用 1=左上 2=中上 3=右上 4=左中  9=右下</param>
        /// <param name="q">附加水印图片质量,0-100</param>
        /// <param name="fn">字体</param>
        /// <param name="fs">字体大小</param>
        /// <returns></returns>
        [HttpPost]
        public virtual IActionResult ImageWaterMarkText(string u, string t, int s, int q, string fn, int fs)
        {
            var thumbnailUrl = ("/WaterMark/" + u).Replace("//", "/");
            var newPath = MapWebRootPath(thumbnailUrl);

            var path = MapWebRootPath(u);
            var bytes = WaterMark.AddImageSignText(path, t, s, q, fn, fs);
            Directory.CreateDirectory(Path.GetDirectoryName(newPath));
            System.IO.File.WriteAllBytes(newPath, bytes);

            return Success(thumbnailUrl);
        }





    }
}
