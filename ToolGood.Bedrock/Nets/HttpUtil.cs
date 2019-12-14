using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace System.Net
{
    /// <summary>
    /// 用于Web API操作 不支持跳转
    /// </summary>
    public static class HttpUtil
    {
        [ThreadStatic]
        public static Hashtable Headers = new Hashtable();
        [ThreadStatic]
        public static Encoding ReadEncoding = Encoding.UTF8;
        [ThreadStatic]
        public static int? Timeout = 15000;
        [ThreadStatic]
        public static int? ReadWriteTimeout;
        [ThreadStatic]
        public static string UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/48.0.2564.48 Safari/537.36";

        [ThreadStatic]
        public static string Referer;
        [ThreadStatic]
        public static CookieContainer Cookies = new CookieContainer();
        [ThreadStatic]
        public static string ResponseUrl;


        public static void ResetConfig()
        {
            Headers.Clear();
            ReadEncoding = Encoding.UTF8;
            Timeout = 15000;
            ReadWriteTimeout = null;
            UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/48.0.2564.48 Safari/537.36";
        }

        #region CreateWebRequest ReadResponse
        private static HttpWebRequest CreateWebRequest(string url)
        {
            HttpWebRequest request;
            if (url.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            } else {
                request = WebRequest.Create(url) as HttpWebRequest;
            }

            request.Accept = "*/*";
            request.AllowAutoRedirect = true;
            request.AllowReadStreamBuffering = true;
            request.AllowWriteStreamBuffering = true;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.UnsafeAuthenticatedConnectionSharing = true;
            request.Referer = Referer;
            request.CookieContainer = Cookies;

            if (Timeout.HasValue) { request.Timeout = Timeout.Value; }
            if (ReadWriteTimeout.HasValue) { request.ReadWriteTimeout = ReadWriteTimeout.Value; }

            foreach (DictionaryEntry item in Headers) {
                request.Headers.Add(item.Key.ToString(), item.Value.ToString());
            }
            return request;
        }
        private static HttpWebRequest CreateWebRequest2(string url)
        {
            HttpWebRequest request;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase)) {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            } else {
                request = WebRequest.Create(url) as HttpWebRequest;
            }

            request.Accept = "*/*";
            request.AllowAutoRedirect = true;
            request.AllowReadStreamBuffering = true;
            request.AllowWriteStreamBuffering = true;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.UnsafeAuthenticatedConnectionSharing = true;
            request.Referer = Referer;
            request.CookieContainer = Cookies;

            foreach (DictionaryEntry item in Headers) {
                request.Headers.Add(item.Key.ToString(), item.Value.ToString());
            }
            return request;
        }

        private static string ReadResponse(WebResponse response)
        {
            string responseStr = null;
            if (response != null) {
                ResponseUrl = response.ResponseUri.ToString();
                StreamReader reader = new StreamReader(response.GetResponseStream(), ReadEncoding);
                responseStr = reader.ReadToEnd();
                reader.Close();
            }
            return responseStr;
        }
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }

        private static string BuildParam(IDictionary<object, object> param)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in param) {
                if (stringBuilder.Length > 0) {
                    stringBuilder.Append("&");
                }
                stringBuilder.Append(item.Key);
                stringBuilder.Append("=");
                if (null != item.Value) {
                    stringBuilder.Append(HttpUtility.UrlEncode(item.Value.ToString()));
                }
            }
            return stringBuilder.ToString();
        }

        #endregion

        #region POST
        /// <summary>
        /// HTTP POST方式请求数据
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static string HttpPost(string url, string param = null)
        {
            HttpWebRequest request = CreateWebRequest(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            StreamWriter requestStream = null;
            WebResponse response = null;
            try {
                requestStream = new StreamWriter(request.GetRequestStream());
                requestStream.Write(param);
                requestStream.Close();

                response = request.GetResponse();
                return ReadResponse(response);
            } finally {
                request = null;
                requestStream = null;
                response = null;
            }
        }
        /// <summary>
        /// HTTP POST方式请求数据
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static string HttpPost(string url, IDictionary<object, object> param)
        {
            return HttpPost(url, BuildParam(param));
        }

        #endregion

        #region Put
        /// <summary>
        /// HTTP Put方式请求数据.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string HttpPut(string url, string param = null)
        {
            HttpWebRequest request = CreateWebRequest(url);
            request.Method = "PUT";
            request.ContentType = "application/x-www-form-urlencoded";

            StreamWriter requestStream = null;
            WebResponse response = null;
            try {
                requestStream = new StreamWriter(request.GetRequestStream());
                requestStream.Write(param);
                requestStream.Close();

                response = request.GetResponse();
                return ReadResponse(response);

            } finally {
                request = null;
                requestStream = null;
                response = null;
            }
        }

        /// <summary>
        /// HTTP Put方式请求数据
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static string HttpPut(string url, IDictionary<object, object> param)
        {
            return HttpPut(url, BuildParam(param));
        }
        #endregion


        #region Delete
        /// <summary>
        /// HTTP Delete方式请求数据.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string HttpDelete(string url, string param = null)
        {
            HttpWebRequest request = CreateWebRequest(url);
            request.Method = "Delete";
            request.ContentType = "application/x-www-form-urlencoded";

            StreamWriter requestStream = null;
            WebResponse response = null;
            try {
                requestStream = new StreamWriter(request.GetRequestStream());
                requestStream.Write(param);
                requestStream.Close();

                response = request.GetResponse();
                return ReadResponse(response);
            } finally {
                request = null;
                requestStream = null;
                response = null;
            }
        }

        /// <summary>
        /// HTTP Delete方式请求数据
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static string HttpDelete(string url, IDictionary<object, object> param)
        {
            return HttpDelete(url, BuildParam(param));
        }
        #endregion

        #region Get
        /// <summary>
        /// HTTP GET方式请求数据.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns></returns>
        public static string HttpGet(string url)
        {
            HttpWebRequest request = CreateWebRequest(url);
            request.Method = "GET";

            WebResponse response = null;
            try {
                response = request.GetResponse();
                return ReadResponse(response);
            } finally {
                response = null;
                request = null;
            }
        }

        /// <summary>
        /// HTTP GET方式请求数据
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static string HttpGet(string url, string param)
        {
            if (param != null) {
                if (url.Contains("?")) {
                    url += "&" + param;
                } else {
                    url += "?" + param;
                }
            }
            return HttpGet(url);
        }

        /// <summary>
        /// HTTP GET方式请求数据
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static string HttpGet(string url, IDictionary<object, object> param)
        {
            return HttpGet(url, BuildParam(param));
        }
        #endregion

        #region Post With Pic
        public static string HttpPost(string url, string filePath, IDictionary<object, object> param=null)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = CreateWebRequest2(url);// (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            if (param!=null) {
                foreach (string key in param.Keys) {
                    rs.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, key, param[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    rs.Write(formitembytes, 0, formitembytes.Length);
                }
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, "pic", filePath, "text/plain");
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0) {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try {
                wresp = wr.GetResponse();
                return ReadResponse(wresp);
            } finally {
                wr = null;
                wresp = null;
            }
        }
        #endregion

        #region Post With Pic
        /// <summary>
        /// HTTP POST方式请求数据(带图片)
        /// </summary>
        /// <param name="url">URL</param>        
        /// <param name="param">参数</param>
        /// <param name="fileByte">图片</param>
        /// <returns></returns>
        public static string HttpPost(string url, byte[] fileByte, IDictionary<object, object> param=null)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = CreateWebRequest2(url);// (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            if (param!=null) {
                foreach (string key in param.Keys) {
                    rs.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, key, param[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    rs.Write(formitembytes, 0, formitembytes.Length);
                }
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, "pic", fileByte, "text/plain");//image/jpeg
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            rs.Write(fileByte, 0, fileByte.Length);

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try {
                wresp = wr.GetResponse();
                return ReadResponse(wresp);
            } finally {
                wr = null;
                wresp = null;
            }
        }
        #endregion


    }
}
