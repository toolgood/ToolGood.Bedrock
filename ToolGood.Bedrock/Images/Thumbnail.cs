using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using ToolGood.Bedrock.Images.GifLab;

namespace ToolGood.Bedrock.Images
{
    /// <summary>
    /// Thumbnail 的摘要说明。
    /// </summary>
    public class Thumbnail
    {
        #region GetFileName GetFileNameForCut GetFileNameForHeight GetFileNameForWidth GetFileNameForThumbnail
        /// <summary>
        /// 获取 剪切图片名称
        /// </summary>
        /// <param name="path"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static string GetFileNameForCut(string path, int width, int height)
        {
            var fileDir = Path.GetDirectoryName(path);
            var fileName = Path.GetFileNameWithoutExtension(path);
            var fileExt = Path.GetExtension(path);
            return Path.Combine(fileDir, $"{fileName}_C{height}x{width}{fileExt}");
        }
        /// <summary>
        /// 获取 锁定高的图片名称
        /// </summary>
        /// <param name="path"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static string GetFileNameForHeight(string path,   int height)
        {
            var fileDir = Path.GetDirectoryName(path);
            var fileName = Path.GetFileNameWithoutExtension(path);
            var fileExt = Path.GetExtension(path);
            return Path.Combine(fileDir, $"{fileName}_H{height}{fileExt}");
        }
        /// <summary>
        /// 获取 锁定宽的图片名称
        /// </summary>
        /// <param name="path"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static string GetFileNameForWidth(string path, int width)
        {
            var fileDir = Path.GetDirectoryName(path);
            var fileName = Path.GetFileNameWithoutExtension(path);
            var fileExt = Path.GetExtension(path);
            return Path.Combine(fileDir, $"{fileName}_W{width}{fileExt}");
        }
        /// <summary>
        /// 获取 锁定高宽的图片名称
        /// </summary>
        /// <param name="path"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static string GetFileNameForThumbnail(string path, int width, int height)
        {
            var fileDir = Path.GetDirectoryName(path);
            var fileName = Path.GetFileNameWithoutExtension(path);
            var fileExt = Path.GetExtension(path);
            return Path.Combine(fileDir, $"{fileName}_HW{height}x{width}{fileExt}");
        }
        #endregion

        /// <summary>
        /// 得到图片格式
        /// </summary>
        /// <param name="ext">文件后缀</param>
        /// <returns></returns>
        private static ImageFormat GetFormat(string ext)
        {
            switch (ext.ToLower()) {
                case "bmp":
                case ".bmp":
                    return ImageFormat.Bmp;
                case "png":
                case ".png":
                    return ImageFormat.Png;
                case "gif":
                case ".gif":
                    return ImageFormat.Gif;
                default:
                    return ImageFormat.Jpeg;
            }
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="filePath">文件</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式：HW、H、W、CUT</param>
        /// <returns></returns>
        public static byte[] MakeThumbnailImage(string filePath, int width, int height, string mode)
        {
            var bytes = File.ReadAllBytes(filePath);
            var fileExt = Path.GetExtension(filePath);
            return MakeThumbnailImage(bytes, fileExt, width, height, mode);
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="byteData">文件字节数组</param>
        /// <param name="fileExt">文件扩展名</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式：HW、H、W、CUT</param>
        public static byte[] MakeThumbnailImage(byte[] byteData, string fileExt, int width, int height, string mode)
        {
            MemoryStream memoryStream = new MemoryStream(byteData);
            Image originalImage = Image.FromStream(memoryStream);
            int towidth = width;
            int toheight = height;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            if (ow < towidth && oh < toheight) {
                originalImage.Dispose();
                memoryStream.Dispose();
                return byteData;
            }
            int x = 0;
            int y = 0;


            switch (mode.ToUpper()) {
                case "HW"://指定高宽缩放（补白）
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight) {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    } else {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    break;
                case "W"://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "CUT"://指定高宽裁减（不变形）                
                case "C"://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight) {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    } else {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            Bitmap b = new Bitmap(towidth, toheight);
            try {
                //新建一个画板
                Graphics g = Graphics.FromImage(b);
                //设置高质量插值法
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                //清空画布并以透明背景色填充
                g.Clear(Color.White);
                //g.Clear(Color.Transparent);
                //在指定位置并且按指定大小绘制原图片的指定部分
                g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight), new Rectangle(x, y, ow, oh), GraphicsUnit.Pixel);

                using (MemoryStream ms = new MemoryStream()) {
                    b.Save(ms, GetFormat(fileExt));
                    byte[] buffer = new byte[ms.Length];
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Read(buffer, 0, buffer.Length);
                    return buffer;
                }
            } catch (System.Exception e) {
                throw e;
            } finally {
                memoryStream.Close();
                originalImage.Dispose();
                b.Dispose();
            }
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImage">Image</param>
        /// <param name="fileExt">文件扩展名</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式：HW、H、W、CUT</param>
        public static byte[] MakeThumbnailImage(Image originalImage, string fileExt, int width, int height, string mode)
        {
            int towidth = width;
            int toheight = height;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            if (ow < towidth && oh < toheight) {
                using (MemoryStream ms = new MemoryStream()) {
                    originalImage.Save(ms, GetFormat(fileExt));
                    byte[] buffer = new byte[ms.Length];
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Read(buffer, 0, buffer.Length);
                    return buffer;
                }
            }
            int x = 0;
            int y = 0;

            switch (mode.ToUpper()) {
                case "HW"://指定高宽缩放（补白）
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight) {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    } else {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    break;
                case "W"://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "CUT"://指定高宽裁减（不变形）                
                case "C"://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight) {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    } else {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            Bitmap b = new Bitmap(towidth, toheight);
            try {
                //新建一个画板
                Graphics g = Graphics.FromImage(b);
                //设置高质量插值法
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                //清空画布并以透明背景色填充
                g.Clear(Color.White);
                //g.Clear(Color.Transparent);
                //在指定位置并且按指定大小绘制原图片的指定部分
                g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight), new Rectangle(x, y, ow, oh), GraphicsUnit.Pixel);

                using (MemoryStream ms = new MemoryStream()) {
                    b.Save(ms, GetFormat(fileExt));
                    byte[] buffer = new byte[ms.Length];
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Read(buffer, 0, buffer.Length);
                    return buffer;
                }
            } catch (System.Exception e) {
                throw e;
            } finally {
                originalImage.Dispose();
                b.Dispose();
            }
        }

        /// <summary>
        /// 制作远程缩略图
        /// </summary>
        /// <param name="url">图片URL</param>
        /// <param name="newFileName">新图路径</param>
        /// <param name="maxWidth">最大宽度</param>
        /// <param name="maxHeight">最大高度</param>
        public static void MakeRemoteThumbnailImage(string url, string newFileName, int maxWidth, int maxHeight)
        {
            Stream stream = GetRemoteImage(url);
            if (stream == null)
                return;
            Image original = Image.FromStream(stream);
            stream.Close();
            MakeThumbnailImage(original, newFileName, maxWidth, maxHeight, "CUT");
        }

        /// <summary>
        /// 获取图片流
        /// </summary>
        /// <param name="url">图片URL</param>
        /// <returns></returns>
        private static Stream GetRemoteImage(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            request.ContentLength = 0;
            request.Timeout = 20000;
            HttpWebResponse response = null;

            try {
                response = (HttpWebResponse)request.GetResponse();
                return response.GetResponseStream();
            } catch {
                return null;
            }
        }

        public static byte[] MakeThumbnailImage(string filePath, int maxWidth, int maxHeight, int X, int Y)
        {
            var bytes = File.ReadAllBytes(filePath);
            var fileExt = Path.GetExtension(filePath);
            return MakeThumbnailImage(bytes, fileExt, maxWidth, maxHeight, X, Y);
        }

        public static byte[] MakeThumbnailImage(byte[] imageBytes, string fileExt, int maxWidth, int maxHeight, int X, int Y)
        {
            Image originalImage = Image.FromStream(new System.IO.MemoryStream(imageBytes));
            Bitmap b = new Bitmap(maxWidth, maxHeight);
            try {
                using (Graphics g = Graphics.FromImage(b)) {
                    //设置高质量插值法
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    //设置高质量,低速度呈现平滑程度
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    //清空画布并以透明背景色填充
                    g.Clear(Color.Transparent);
                    //在指定位置并且按指定大小绘制原图片的指定部分
                    g.DrawImage(originalImage, new Rectangle(0, 0, maxWidth, maxHeight), X, Y, maxWidth, maxHeight, GraphicsUnit.Pixel);
                    Image displayImage = new Bitmap(b, maxWidth, maxHeight);

                    using (MemoryStream ms = new MemoryStream()) {
                        displayImage.Save(ms, GetFormat(fileExt));
                        byte[] buffer = new byte[ms.Length];
                        ms.Seek(0, SeekOrigin.Begin);
                        ms.Read(buffer, 0, buffer.Length);
                        return buffer;
                    }
                }
            } finally {
                originalImage.Dispose();
                b.Dispose();
            }
        }



    }
}
