using System.Collections.Generic;

namespace System
{
    public static class DictionaryExtension
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
        public static Dictionary<T1, T2> TryUpdate<T1, T2>(this Dictionary<T1, T2> data, T1 key, T2 value)
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
        /// 更新或添加
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<T1, T2> AddOrUpdate<T1, T2>(this Dictionary<T1, T2> data, T1 key, T2 value)
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
            return data.AddOrUpdate<T1, T2>(key, value);
        }

        /// <summary>
        /// 根据键移除
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="data"></param>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public static Dictionary<T1, T2> RemoveKey<T1, T2>(this Dictionary<T1, T2> data, T1 key)
        {
            if (data == null) return data;
            if (data.ContainsKey(key)) {
                data.Remove(key);
            }
            return data;
        }



    }
}
