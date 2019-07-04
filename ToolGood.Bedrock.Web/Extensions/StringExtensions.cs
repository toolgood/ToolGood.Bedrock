using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Bedrock.Images;
using System.Web;
using System.IO;

namespace ToolGood.Bedrock.Web.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        public static string ImageCutPrefix = "/Image/C/?u={0}&w={1}&h={2}";
        public static string ImageHeightPrefix = "/Image/H/?u={0}&h={1}";
        public static string ImageWidthPrefix = "/Image/W/?u={0}&w={1}";
        public static string ImageThumbnailPrefix = "/Image/T/?u={0}&w={1}&h={2}";
        public static string ImageQrCodePrefix = "/Image/Qr/?u={0}&w={1}&h={2}";


        public static HtmlString ToImageUrlForCut(this string url, int width = 100, int height = 100)
        {
            return new HtmlString(string.Format(HttpUtility.UrlDecode(ImageCutPrefix), url, width, height));
        }

        public static HtmlString ToImageUrlForHeight(this string url, int height = 100)
        {
            return new HtmlString(string.Format(HttpUtility.UrlDecode(ImageHeightPrefix), url, height));
        }

        public static HtmlString ToImageUrlForWidth(this string url, int width = 100)
        {
            return new HtmlString(string.Format(HttpUtility.UrlDecode(ImageWidthPrefix), url, width));
        }

        public static HtmlString ToImageUrlForThumbnail(this string url, int width = 200, int height = 200)
        {
            return new HtmlString(string.Format(HttpUtility.UrlDecode(ImageThumbnailPrefix), url, width, height));
        }

        public static HtmlString ToImageUrlForQrCode(this string url, int width = 200, int height = 200)
        {
            return new HtmlString(string.Format(HttpUtility.UrlDecode(ImageQrCodePrefix), url, width, height));
        }



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
                    if (stringBuilder.Length>0) {
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
