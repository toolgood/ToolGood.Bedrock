using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace System
{
    public static partial class ObjectExtension
    {
        /// <summary>
        /// 尝试更新
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="data"></param>
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
        /// 设置键-值,更新或添加
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<T1, T2> SetValue<T1, T2>(this Dictionary<T1, T2> data, T1 key, T2 value)
        {
            if (data == null) { data = new Dictionary<T1, T2>(); }
            if (null != key) {
                if (data.ContainsKey(key)) {
                    data[key] = value;
                } else {
                    data.Add(key, value);
                }
            }
            return data;
        }

        /// <summary>
        /// 根据键移除
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="data"></param>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public static Dictionary<T1, T2> DeleteKey<T1, T2>(this Dictionary<T1, T2> data, T1 key)
        {
            if (data == null) return data;
            if (data.ContainsKey(key)) {
                data.Remove(key);
            }
            return data;
        }


        /// <summary>
        /// 批量添加
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="data"></param>
        /// <param name="list"></param>
        /// <param name="keyFun"></param>
        /// <param name="valueFun"></param>
        /// <returns></returns>
        public static Dictionary<T1, T2> AddRange<T1, T2, T3>(this Dictionary<T1, T2> data, IEnumerable<T3> list, Func<T3, T1> keyFun, Func<T3, T2> valueFun)
        {
            if (data == null) return data;
            //var keyFun = key.Compile();
            //var valueFun = value.Compile();
            foreach (var item in list) {
                data[keyFun(item)] = valueFun(item);
            }
            return data;
        }




    }
}
