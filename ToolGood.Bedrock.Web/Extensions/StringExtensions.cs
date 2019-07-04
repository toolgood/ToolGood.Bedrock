using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Bedrock.Images;
using System.Web;

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
        public static string ImageQrCodePrefix = "/Image/Qr/?u={0}";


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

        public static HtmlString ToImageUrlForThumbnail(this string url, int width = 100, int height = 100)
        {
            return new HtmlString(string.Format(HttpUtility.UrlDecode(ImageThumbnailPrefix), url, width, height));
        }

        public static HtmlString ToImageUrlForQrCode(this string url)
        {
            return new HtmlString(string.Format(HttpUtility.UrlDecode(ImageQrCodePrefix), url));
        }




    }
}
