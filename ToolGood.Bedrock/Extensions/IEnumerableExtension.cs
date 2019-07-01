using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
    public static partial class IEnumerableExtension
    {
        /// <summary>
        /// 去重
        /// </summary>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            return source.Where(element => seenKeys.Add(keySelector(element)));
        }

        /// <summary>
        /// 遍历执行方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="action">要执行的方法</param>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var element in enumerable) {
                action(element);
            }
            return enumerable;
        }

        /// <summary>
        /// 转成集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static List<TResult> ToList<T, TResult>(this IEnumerable<T> source, Func<T, TResult> fun)
        {
            List<TResult> result = new List<TResult>();
            source.ForEach(m => result.Add(fun(m)));
            return result;
        }


        /// <summary>
        /// 遍历异步执行方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="action">要执行的方法</param>
        /// <returns></returns>
        public static Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> action)
        {
            return Task.WhenAll(from item in enumerable select Task.Run(() => action(item)));
        }

        /// <summary>
        /// 不为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static bool IsNotEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null) return false;
            if (enumerable.Any()) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            return !(enumerable.IsNotEmpty());
        }


        /// <summary>
        ///     Checks whatever given collection object is null or has no item.
        /// </summary>
        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            return source == null || source.Count <= 0;
        }

        /// <summary>
        ///     Adds an item to the collection if it's not already in the collection.
        /// </summary>
        /// <param name="source">Collection</param>
        /// <param name="item">Item to check and add</param>
        /// <typeparam name="T">Type of the items in the collection</typeparam>
        /// <returns>Returns True if added, returns False if not.</returns>
        public static bool AddIfNotContains<T>(this ICollection<T> source, T item)
        {
            if (source == null) {
                throw new ArgumentNullException(nameof(source));
            }

            if (source.Contains(item)) {
                return false;
            }

            source.Add(item);
            return true;
        }



        /// <summary>
        ///     Concatenates the members of a constructed <see cref="IEnumerable{T}" /> collection of type System.String, using the
        ///     specified separator between each member.
        ///     This is a shortcut for string.Join(...)
        /// </summary>
        /// <param name="source">A collection that contains the strings to concatenate.</param>
        /// <param name="separator">
        ///     The string to use as a separator. separator is included in the returned string only if values
        ///     has more than one element.
        /// </param>
        /// <returns>
        ///     A string that consists of the members of values delimited by the separator string. If values has no members,
        ///     the method returns System.String.Empty.
        /// </returns>
        public static string JoinAsString(this IEnumerable<string> source, string separator)
        {
            return string.Join(separator, source);
        }

        /// <summary>
        ///     Concatenates the members of a collection, using the specified separator between each member.
        ///     This is a shortcut for string.Join(...)
        /// </summary>
        /// <param name="source">A collection that contains the objects to concatenate.</param>
        /// <param name="separator">
        ///     The string to use as a separator. separator is included in the returned string only if values
        ///     has more than one element.
        /// </param>
        /// <typeparam name="T">The type of the members of values.</typeparam>
        /// <returns>
        ///     A string that consists of the members of values delimited by the separator string. If values has no members,
        ///     the method returns System.String.Empty.
        /// </returns>
        public static string JoinAsString<T>(this IEnumerable<T> source, string separator)
        {
            return string.Join(separator, source);
        }

        /// <summary>
        ///     Filters a <see cref="IEnumerable{T}" /> by given predicate if given condition is true.
        /// </summary>
        /// <param name="source">Enumerable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the enumerable</param>
        /// <returns>Filtered or not filtered enumerable based on <paramref name="condition" /></returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
        {
            return condition
                ? source.Where(predicate)
                : source;
        }

        /// <summary>
        ///     Filters a <see cref="IEnumerable{T}" /> by given predicate if given condition is true.
        /// </summary>
        /// <param name="source">Enumerable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the enumerable</param>
        /// <returns>Filtered or not filtered enumerable based on <paramref name="condition" /></returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, int, bool> predicate)
        {
            return condition
                ? source.Where(predicate)
                : source;
        }
    }

}
