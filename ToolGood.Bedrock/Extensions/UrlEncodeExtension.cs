using System.Text;
using System.Web;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class ObjectExtension
    {
        #region 对字符串进行Url编码

        /// <summary>
        /// 对字符串进行Url编码
        /// </summary>
        /// <param name="str">要编码的字符串</param>
        /// <param name="encodeName">编码格式</param>
        /// <returns>Url编码后的字符</returns>
        public static string UrlEncode(this string str, string encodeName)
        {
            return HttpUtility.UrlEncode(str, Encoding.GetEncoding(encodeName));
        }

        /// <summary>
        /// 对字符串进行Url编码
        /// </summary>
        /// <param name="str">要编码的字符串</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>Url编码后的字符</returns>
        public static string UrlEncode(this string str, Encoding encoding)
        {
            return HttpUtility.UrlEncode(str, encoding);
        }

        /// <summary>
        /// 对字符串进行Url编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlEncode(this string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        #endregion 对字符串进行Url编码

        #region 对字符串进行Url解码

        /// <summary>
        /// 对字符串进行Url解码
        /// </summary>
        /// <param name="str">需要解码的字符串</param>
        /// <param name="decodeName">编码格式</param>
        /// <returns>Url解码后的字符串</returns>
        public static string UrlDecode(this string str, string decodeName)
        {
            return HttpUtility.UrlDecode(str, Encoding.GetEncoding(decodeName));
        }

        /// <summary>
        /// 对字符串进行Url解码
        /// </summary>
        /// <param name="str">需要解码的字符串</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>Url解码后的字符串</returns>
        public static string UrlDecode(this string str, Encoding encoding)
        {
            return HttpUtility.UrlDecode(str, encoding);
        }


        /// <summary>
        /// 对字符串进行Url解码
        /// </summary>
        /// <param name="str">需要解码的字符串</param>
        /// <returns>Url解码后的字符串</returns>
        public static string UrlDecode(this string str)
        {
            return HttpUtility.UrlDecode(str);
        }

        #endregion 对字符串进行Url解码




    }

}
