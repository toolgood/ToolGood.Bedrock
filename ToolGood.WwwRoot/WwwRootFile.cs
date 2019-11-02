using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Bedrock.Web.Mime;
using System.IO;
using ToolGood.Bedrock;
using WebMarkupMin.Yui;
using WebMarkupMin.MsAjax;
using WebMarkupMin.NUglify;
using WebMarkupMin.Core;

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

        public string GetFileContent(byte[] bytes, string file)
        {
            var ext = Path.GetExtension(file).ToLower();
            if (ext == ".css") {
                string tar = CssMinifier(bytes);
                bytes = Encoding.UTF8.GetBytes(tar);
            } else if (ext == ".js") {
                string tar = JsMinifier(bytes);
                bytes = Encoding.UTF8.GetBytes(tar);
            } else if (ext == ".html" || ext == ".htm") {
                string tar = HtmlMinifierBytes(bytes);
                bytes = Encoding.UTF8.GetBytes(tar);
            } else if (ext == ".xhtml" || ext == ".xhtm") {
                string tar = XHtmlMinifierBytes(bytes);
                bytes = Encoding.UTF8.GetBytes(tar);
            }
            bytes = CompressionUtil.BrCompress(bytes);
            return Convert.ToBase64String(bytes);
        }
        private static string JsMinifier(byte[] bytes)
        {
            var old = Encoding.UTF8.GetString(bytes);
            var tar = old;
            List<IJsMinifier> jsMinifiers = new List<IJsMinifier>() { new MsAjaxJsMinifier(), new NUglifyJsMinifier(), new YuiJsMinifier() };
            foreach (var jsMinifier in jsMinifiers) {
                var minJs = jsMinifier.Minify(old, true);
                if (minJs.Errors.Count == 0) {
                    if (minJs.MinifiedContent.Length < tar.Length) {
                        tar = minJs.MinifiedContent;
                    }
                }
                minJs = jsMinifier.Minify(old, false);
                if (minJs.Errors.Count == 0) {
                    if (minJs.MinifiedContent.Length < tar.Length) {
                        tar = minJs.MinifiedContent;
                    }
                }
            }
            return tar;
        }

        private static string CssMinifier(byte[] bytes)
        {
            var old = Encoding.UTF8.GetString(bytes);
            var tar = old;
            List<ICssMinifier> cssMinifiers = new List<ICssMinifier>() { new MsAjaxCssMinifier(), new NUglifyCssMinifier(), new YuiCssMinifier() };
            foreach (var cssMinifier in cssMinifiers) {
                var minCss = cssMinifier.Minify(old, true);
                if (minCss.Errors.Count == 0) {
                    if (minCss.MinifiedContent.Length < tar.Length) {
                        tar = minCss.MinifiedContent;
                    }
                }
                minCss = cssMinifier.Minify(old, false);
                if (minCss.Errors.Count == 0) {
                    if (minCss.MinifiedContent.Length < tar.Length) {
                        tar = minCss.MinifiedContent;
                    }
                }
            }
            return tar;
        }

        private static string HtmlMinifierBytes(byte[] bytes)
        {
            var old = Encoding.UTF8.GetString(bytes);
            var tar = old;
            List<IJsMinifier> jsMinifiers = new List<IJsMinifier>() { new MsAjaxJsMinifier(), new NUglifyJsMinifier(), new YuiJsMinifier() };
            List<ICssMinifier> cssMinifiers = new List<ICssMinifier>() { new MsAjaxCssMinifier(), new NUglifyCssMinifier(), new YuiCssMinifier() };

            for (int i = 0; i < jsMinifiers.Count; i++) {
                for (int j = 0; j < cssMinifiers.Count; j++) {
                    var js = jsMinifiers[i];
                    var css = cssMinifiers[i];
                    HtmlMinifier htmlMinifier = new HtmlMinifier(null, css, js, null);
                    var html = htmlMinifier.Minify(old);
                    if (html.Errors.Count == 0) {
                        if (html.MinifiedContent.Length < tar.Length) {
                            tar = html.MinifiedContent;
                        }
                    }
                }
            }
            return tar;
        }

        private static string XHtmlMinifierBytes(byte[] bytes)
        {
            var old = Encoding.UTF8.GetString(bytes);
            var tar = old;
            List<IJsMinifier> jsMinifiers = new List<IJsMinifier>() { new MsAjaxJsMinifier(), new NUglifyJsMinifier(), new YuiJsMinifier() };
            List<ICssMinifier> cssMinifiers = new List<ICssMinifier>() { new MsAjaxCssMinifier(), new NUglifyCssMinifier(), new YuiCssMinifier() };

            for (int i = 0; i < jsMinifiers.Count; i++) {
                for (int j = 0; j < cssMinifiers.Count; j++) {
                    var js = jsMinifiers[i];
                    var css = cssMinifiers[i];
                    XhtmlMinifier htmlMinifier = new XhtmlMinifier(null, css, js, null);
                    var html = htmlMinifier.Minify(old);
                    if (html.Errors.Count == 0) {
                        if (html.MinifiedContent.Length < tar.Length) {
                            tar = html.MinifiedContent;
                        }
                    }
                }
            }
            return tar;
        }

        public string GetFileContent(byte[] bytes)
        {
            bytes = CompressionUtil.BrCompress(bytes);
            return Convert.ToBase64String(bytes);
        }


    }
}
