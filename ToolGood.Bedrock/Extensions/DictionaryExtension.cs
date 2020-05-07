using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace System
{
    public static partial class ObjectExtension
    {
        /// <summary>
        /// 尝试更新，注：字典为空，默认new 一个字典对象
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="data">为空时，默认new 一个字典对象</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<T1, T2> TryUpdateOnly<T1, T2>(this Dictionary<T1, T2> data, T1 key, T2 value)
        {
            if (data == null) { data = new Dictionary<T1, T2>(); }
            if (null != key) {
                if (data.ContainsKey(key)) {
                    data[key] = value;
                }
            }
            return data;
        }

        /// <summary>
        /// 尝试更新，注：字典为空，默认new 一个字典对象
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="data">为空时，默认new 一个字典对象</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<T1, T2> TryUpdateOnly<T1, T2>(this Dictionary<T1, T2> data, T1 key, Func<T2> value)
        {
            if (data == null) { data = new Dictionary<T1, T2>(); }
            if (null != key) {
                if (data.ContainsKey(key)) {
                    data[key] = value();
                }
            }
            return data;
        }

        /// <summary>
        /// 批量添加，注：字典为空，默认new 一个字典对象
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="data">为空时，默认new 一个字典对象</param>
        /// <param name="list"></param>
        /// <param name="keyFun"></param>
        /// <param name="valueFun"></param>
        /// <returns></returns>
        public static IDictionary<T1, T2> AddRange<T1, T2, T3>(this IDictionary<T1, T2> data, IEnumerable<T3> list, Func<T3, T1> keyFun, Func<T3, T2> valueFun)
        {
            if (data == null) { data = new Dictionary<T1, T2>(); }

            foreach (var item in list) {
                data[keyFun(item)] = valueFun(item);
            }
            return data;
        }

        /// <summary>
        /// 尝试获取，不存在则添加，注：字典为空，默认new 一个字典对象
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="data">为空时，默认new 一个字典对象</param>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T2 TryGetOrAdd<T1, T2>(this Dictionary<T1, T2> data, T1 key, Func<T2> func)
        {
            if (data == null) { data = new Dictionary<T1, T2>(); }

            if (data.TryGetValue(key, out T2 t2)) { return t2; }
            var val = func();
            data[key] = val;
            return val;
        }


        /// <summary>
        /// 尝试获取，不存在则添加，注：字典为空，默认new 一个字典对象
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="data">为空时，默认new 一个字典对象</param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T2 TryGetOrAdd<T1, T2>(this Dictionary<T1, T2> data, T1 key, T2 defaultValue)
        {
            if (data == null) { data = new Dictionary<T1, T2>(); }

            if (data.TryGetValue(key, out T2 t2)) { return t2; }
            data[key] = defaultValue;
            return defaultValue;
        }


        /// <summary>
        /// Extension method that turns a dictionary of string and object to an ExpandoObject
        /// Snagged from http://theburningmonk.com/2011/05/idictionarystring-object-to-expandoobject-extension-method/
        /// </summary>
        public static ExpandoObject ToExpando(this IDictionary<string, object> dictionary)
        {
            var expando = new ExpandoObject();
            var expandoDic = (IDictionary<string, object>)expando;

            // go through the items in the dictionary and copy over the key value pairs)
            foreach (var kvp in dictionary) {
                // if the value can also be turned into an ExpandoObject, then do it!
                if (kvp.Value is IDictionary<string, object>) {
                    var expandoValue = ((IDictionary<string, object>)kvp.Value).ToExpando();
                    expandoDic.Add(kvp.Key, expandoValue);
                } else if (kvp.Value is ICollection) {
                    // iterate through the collection and convert any strin-object dictionaries
                    // along the way into expando objects
                    var itemList = new List<object>();
                    foreach (var item in (ICollection)kvp.Value) {
                        if (item is IDictionary<string, object>) {
                            var expandoItem = ((IDictionary<string, object>)item).ToExpando();
                            itemList.Add(expandoItem);
                        } else {
                            itemList.Add(item);
                        }
                    }

                    expandoDic.Add(kvp.Key, itemList);
                } else {
                    expandoDic.Add(kvp);
                }
            }

            return expando;
        }
    }
}
