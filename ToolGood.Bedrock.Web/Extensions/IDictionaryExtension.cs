using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Bedrock.Web.Extensions
{
    public static partial class ObjectExtensions
    {
        /// <summary>
        /// ToHtmlFromDictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dict"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static HtmlString ToHtmlFromDictionary(this string key, IDictionary<string, string> dict, string defaultString = "")
        {
            if (dict.ContainsKey(key)) {
                return new HtmlString(dict[key]);
            }
            return new HtmlString(defaultString);
        }
        /// <summary>
        /// ToHtmlFromDictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dict"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static HtmlString ToHtmlFromDictionary(this string key, IDictionary<int, string> dict, string defaultString = "")
        {
            if (int.TryParse(key, out int index)) {
                if (dict.ContainsKey(index)) {
                    return new HtmlString(dict[index]);
                }
            }
            return new HtmlString(defaultString);
        }
        /// <summary>
        /// ToHtmlFromDictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dict"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static HtmlString ToHtmlFromDictionary(this string key, IDictionary<long, string> dict, string defaultString = "")
        {
            if (long.TryParse(key, out long index)) {
                if (dict.ContainsKey(index)) {
                    return new HtmlString(dict[index]);
                }
            }
            return new HtmlString(defaultString);
        }
        /// <summary>
        /// ToHtmlFromDictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dict"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static HtmlString ToHtmlFromDictionary(this int key, IDictionary<int, string> dict, string defaultString = "")
        {
            if (dict.ContainsKey(key)) {
                return new HtmlString(dict[key]);
            }
            return new HtmlString(defaultString);
        }
        /// <summary>
        /// ToHtmlFromDictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dict"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static HtmlString ToHtmlFromDictionary(this int key, IDictionary<long, string> dict, string defaultString = "")
        {
            if (dict.ContainsKey(key)) {
                return new HtmlString(dict[key]);
            }
            return new HtmlString(defaultString);
        }
        /// <summary>
        /// ToHtmlFromDictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dict"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static HtmlString ToHtmlFromDictionary(this int key, IDictionary<string, string> dict, string defaultString = "")
        {
            if (dict.ContainsKey(key.ToString())) {
                return new HtmlString(dict[key.ToString()]);
            }
            return new HtmlString(defaultString);
        }
        /// <summary>
        /// ToHtmlFromDictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dict"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static HtmlString ToHtmlFromDictionary(this long key, IDictionary<long, string> dict, string defaultString = "")
        {
            if (dict.ContainsKey(key)) {
                return new HtmlString(dict[key]);
            }
            return new HtmlString(defaultString);
        }
        /// <summary>
        /// ToHtmlFromDictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dict"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static HtmlString ToHtmlFromDictionary(this long key, IDictionary<int, string> dict, string defaultString = "")
        {
            if (dict.ContainsKey((int)key)) {
                return new HtmlString(dict[(int)key]);
            }
            return new HtmlString(defaultString);
        }
        /// <summary>
        /// ToHtmlFromDictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dict"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static HtmlString ToHtmlFromDictionary(this long key, IDictionary<string, string> dict, string defaultString = "")
        {
            if (dict.ContainsKey(key.ToString())) {
                return new HtmlString(dict[key.ToString()]);
            }
            return new HtmlString(defaultString);
        }

        /// <summary>
        /// ToHtmlFromList
        /// </summary>
        /// <param name="key"></param>
        /// <param name="list"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static HtmlString ToHtmlFromList(this int key, IList<string> list, string defaultString = "")
        {
            if (key >= 0 && key < list.Count) {
                return new HtmlString(list[key]);
            }
            return new HtmlString(defaultString);
        }
        /// <summary>
        /// ToHtmlFromList
        /// </summary>
        /// <param name="key"></param>
        /// <param name="list"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static HtmlString ToHtmlFromList(this long key, IList<string> list, string defaultString = "")
        {
            if (key >= 0 && key < list.Count) {
                return new HtmlString(list[(int)key]);
            }
            return new HtmlString(defaultString);
        }

        /// <summary>
        /// GetItemHtml
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static HtmlString GetItemHtml(IDictionary<string, string> dict, string key, string defaultString = "")
        {
            if (dict.ContainsKey(key)) {
                return new HtmlString(dict[key]);
            }
            return new HtmlString(defaultString);
        }
        /// <summary>
        /// GetItemHtml
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static HtmlString GetItemHtml(IDictionary<string, string> dict, int key, string defaultString = "")
        {
            if (dict.ContainsKey(key.ToString())) {
                return new HtmlString(dict[key.ToString()]);
            }
            return new HtmlString(defaultString);
        }
        /// <summary>
        /// GetItemHtml
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static HtmlString GetItemHtml(IDictionary<string, string> dict, long key, string defaultString = "")
        {
            if (dict.ContainsKey(key.ToString())) {
                return new HtmlString(dict[key.ToString()]);
            }
            return new HtmlString(defaultString);
        }

        /// <summary>
        /// GetItemHtml
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static HtmlString GetItemHtml(IDictionary<int, string> dict, string key, string defaultString = "")
        {
            if (int.TryParse(key, out int index)) {
                if (dict.ContainsKey(index)) {
                    return new HtmlString(dict[index]);
                }
            }
            return new HtmlString(defaultString);
        }
        /// <summary>
        /// GetItemHtml
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static HtmlString GetItemHtml(IDictionary<int, string> dict, int key, string defaultString = "")
        {
            if (dict.ContainsKey(key)) {
                return new HtmlString(dict[key]);
            }
            return new HtmlString(defaultString);
        }
        /// <summary>
        /// GetItemHtml
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static HtmlString GetItemHtml(IDictionary<int, string> dict, long key, string defaultString = "")
        {
            if (dict.ContainsKey((int)key)) {
                return new HtmlString(dict[(int)key]);
            }
            return new HtmlString(defaultString);
        }

        /// <summary>
        /// GetItemHtml
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static HtmlString GetItemHtml(IDictionary<long, string> dict, string key, string defaultString = "")
        {
            if (long.TryParse(key, out long index)) {
                if (dict.ContainsKey(index)) {
                    return new HtmlString(dict[index]);
                }
            }
            return new HtmlString(defaultString);
        }
        /// <summary>
        /// GetItemHtml
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static HtmlString GetItemHtml(IDictionary<long, string> dict, int key, string defaultString = "")
        {
            if (dict.ContainsKey(key)) {
                return new HtmlString(dict[key]);
            }
            return new HtmlString(defaultString);
        }
        /// <summary>
        /// GetItemHtml
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static HtmlString GetItemHtml(IDictionary<long, string> dict, long key, string defaultString = "")
        {
            if (dict.ContainsKey((int)key)) {
                return new HtmlString(dict[(int)key]);
            }
            return new HtmlString(defaultString);
        }

        /// <summary>
        /// GetItemHtml
        /// </summary>
        /// <param name="list"></param>
        /// <param name="key"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static HtmlString GetItemHtml(this IList<string> list, int key, string defaultString = "")
        {
            if (key >= 0 && key < list.Count) {
                return new HtmlString(list[key]);
            }
            return new HtmlString(defaultString);
        }
        /// <summary>
        /// GetItemHtml
        /// </summary>
        /// <param name="list"></param>
        /// <param name="key"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static HtmlString GetItemHtml(this IList<string> list, long key, string defaultString = "")
        {
            if (key >= 0 && key < list.Count) {
                return new HtmlString(list[(int)key]);
            }
            return new HtmlString(defaultString);
        }


    }
}
