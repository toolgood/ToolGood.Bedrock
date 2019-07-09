using Microsoft.AspNetCore.Html;
using System;
using System.IO;
using System.Text;
using System.Web;

namespace ToolGood.Bedrock.Web.Extensions
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        public static string ImageCutPrefix = "/Image/C{1}x{2}/{0}";
        /// <summary>
        /// 
        /// </summary>
        public static string ImageHeightPrefix = "/Image/H{1}/{0}";
        /// <summary>
        /// 
        /// </summary>
        public static string ImageWidthPrefix = "/Image/W{1}/{0}";
        /// <summary>
        /// 
        /// </summary>
        public static string ImageThumbnailPrefix = "/Image/T{1}x{2}/{0}";
        /// <summary>
        /// 
        /// </summary>
        public static string ImageQrCodePrefix = "/Image/Qr{1}x{2}/{0}";

        /// <summary>
        /// 转成 剪切的图片Url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static HtmlString ToImageUrlForCut(this string url, int width = 100, int height = 100)
        {
            return new HtmlString(string.Format(ImageCutPrefix, HttpUtility.UrlEncode(url), width, height));
        }
        /// <summary>
        /// 转成锁定高的图片Url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static HtmlString ToImageUrlForHeight(this string url, int height = 100)
        {
            return new HtmlString(string.Format(ImageHeightPrefix, HttpUtility.UrlEncode(url), height));
        }
        /// <summary>
        /// 转成锁定宽的图片Url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static HtmlString ToImageUrlForWidth(this string url, int width = 100)
        {
            return new HtmlString(string.Format(ImageWidthPrefix, HttpUtility.UrlEncode(url), width));
        }
        /// <summary>
        /// 转成缩小的图片Url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static HtmlString ToImageUrlForThumbnail(this string url, int width = 200, int height = 200)
        {
            return new HtmlString(string.Format(ImageThumbnailPrefix, HttpUtility.UrlEncode(url), width, height));
        }
        /// <summary>
        /// 转成二维码
        /// </summary>
        /// <param name="url"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static HtmlString ToImageUrlForQrCode(this string url, int width = 200, int height = 200)
        {
            return new HtmlString(string.Format(ImageQrCodePrefix, HttpUtility.UrlEncode(url), width, height));
        }


        /// <summary>
        /// 获取文件的 fa 的 class 名称
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static HtmlString GetIconClass(this string file)
        {
            if (string.IsNullOrWhiteSpace(file))
                return new HtmlString("");
            var ext = Path.GetExtension(file).ToLower();

            switch (ext) {
                case ".doc":
                case ".docx":
                    return new HtmlString("fa fa-file-word-o");
                case ".xls":
                case ".xlsx":
                case ".csv":
                    return new HtmlString("fa fa-file-excel-o");
                case ".pdf":
                    return new HtmlString("fa fa-file-pdf-o");
                case ".txt":
                case ".ini":
                case ".log":
                case ".config":
                    return new HtmlString("fa fa-file-text");
                case ".zip":
                case ".rar":
                case ".7z":
                case ".iso":
                case ".zipx":
                case ".tar":
                case ".taz":
                case ".gz":
                case ".gzip":
                case ".bzip":
                case ".bzip2":
                    return new HtmlString("fa fa-file-archive-o");
                case ".ppt":
                case ".pptx":
                    return new HtmlString("fa fa-file-powerpoint-o");
                case ".mov":
                case ".flv":
                case ".mp4":
                case ".wmv":
                case ".avi":
                case ".mpeg":
                case ".webm":
                case ".ogv":
                    return new HtmlString("fa fa-file-video-o");
                case ".mp3":
                    return new HtmlString("fa fa-file-audio-o");
                case ".htm":
                case ".html":
                    return new HtmlString("fa fa-html5");
                case ".bmp":
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".gif":
                case ".ico":
                    return new HtmlString("fa fa-image-o");
                case ".java":
                case ".py":
                case ".cs":
                case ".js":
                case ".ts":
                case ".vb":
                case ".css":
                case ".c":
                case ".cpp":
                case ".h":
                case ".xml":
                case ".json":
                    return new HtmlString("fa fa-file-code-o");


            }

            return new HtmlString("glyphicon glyphicon-file");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        /// <returns></returns>
        public static HtmlString SetUrlParameter(this string txt, string parameterName, string parameterValue)
        {
            if (string.IsNullOrEmpty(txt)) {
                return new HtmlString("");
            }

            var index = txt.IndexOf("?");
            if (index == -1) {
                return new HtmlString($"{txt}?{parameterName}={parameterValue.UrlEncode()}");
            }
            var url = txt.Substring(0, index);
            var ps = txt.Substring(index + 1);
            StringBuilder stringBuilder = new StringBuilder();
            //stringBuilder.Append(url);
            var sp = ps.Split("&");

            foreach (var item in sp) {
                var ss = item.Split("=");
                if (ss[0] != parameterName) {
                    if (stringBuilder.Length > 0) {
                        stringBuilder.Append("&");
                    } else {
                        stringBuilder.Append("?");
                    }
                    stringBuilder.Append(item);
                }
            }
            stringBuilder.Append("&");
            stringBuilder.Append(parameterName);
            stringBuilder.Append("=");
            stringBuilder.Append(parameterValue.UrlEncode());
            stringBuilder.Insert(0, url);
            return new HtmlString(stringBuilder.ToString());
        }


    }

}
