using System;
using System.Collections.Generic;
using System.Linq;

namespace ToolGood.Bedrock
{
    public class StringUtil
    {
        #region LD编辑距离算法
        /// <summary>编辑距离搜索，从词组中找到最接近关键字的若干匹配项</summary>
        /// <remarks>
        /// 算法代码由@Aimeast 独立完成。http://www.cnblogs.com/Aimeast/archive/2011/09/05/2167844.html
        /// </remarks>
        /// <param name="key">关键字</param>
        /// <param name="words">词组</param>
        /// <returns></returns>
        public static String[] LevenshteinSearch(String key, String[] words)
        {
            if (string.IsNullOrWhiteSpace(key)) return new String[0];

            var keys = key.Split(new Char[] { ' ', '　' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in keys) {
                var maxDist = (item.Length - 1) / 2;

                var q = from str in words
                        where item.Length <= str.Length
                            && Enumerable.Range(0, maxDist + 1)
                            .Any(dist => {
                                return Enumerable.Range(0, Math.Max(str.Length - item.Length - dist + 1, 0))
                                    .Any(f => {
                                        return LevenshteinDistance(item, str.Substring(f, item.Length + dist)) <= maxDist;
                                    });
                            })
                        orderby str
                        select str;
                words = q.ToArray();
            }

            return words;
        }

        /// <summary>编辑距离</summary>
        /// <remarks>
        /// 又称Levenshtein距离（也叫做Edit Distance），是指两个字串之间，由一个转成另一个所需的最少编辑操作次数。
        /// 许可的编辑操作包括将一个字符替换成另一个字符，插入一个字符，删除一个字符。
        /// 算法代码由@Aimeast 独立完成。http://www.cnblogs.com/Aimeast/archive/2011/09/05/2167844.html
        /// </remarks>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static Int32 LevenshteinDistance(String str1, String str2)
        {
            var n = str1.Length;
            var m = str2.Length;
            var C = new Int32[n + 1, m + 1];
            Int32 i, j, x, y, z;
            for (i = 0; i <= n; i++)
                C[i, 0] = i;
            for (i = 1; i <= m; i++)
                C[0, i] = i;
            for (i = 0; i < n; i++)
                for (j = 0; j < m; j++) {
                    x = C[i, j + 1] + 1;
                    y = C[i + 1, j] + 1;
                    if (str1[i] == str2[j])
                        z = C[i, j];
                    else
                        z = C[i, j] + 1;
                    C[i + 1, j + 1] = Math.Min(Math.Min(x, y), z);
                }
            return C[n, m];
        }
        #endregion

        #region LCS算法
        /// <summary>最长公共子序列搜索，从词组中找到最接近关键字的若干匹配项</summary>
        /// <remarks>
        /// 算法代码由@Aimeast 独立完成。http://www.cnblogs.com/Aimeast/archive/2011/09/05/2167844.html
        /// </remarks>
        /// <param name="key"></param>
        /// <param name="words"></param>
        /// <returns></returns>
        public static String[] LCSSearch(String key, String[] words)
        {
            if (string.IsNullOrWhiteSpace(key) || words == null || words.Length == 0) return new String[0];

            var keys = key
                                .Split(new Char[] { ' ', '\u3000' }, StringSplitOptions.RemoveEmptyEntries)
                                .OrderBy(s => s.Length)
                                .ToArray();

            //var q = from sentence in items.AsParallel()
            var q = from word in words
                    let MLL = LCSDistance(word, keys)
                    where MLL >= 0
                    orderby (MLL + 0.5) / word.Length, word
                    select word;

            return q.ToArray();
        }

        /// <summary>
        /// 最长公共子序列问题是寻找两个或多个已知数列最长的子序列。
        /// 一个数列 S，如果分别是两个或多个已知数列的子序列，且是所有符合此条件序列中最长的，则 S 称为已知序列的最长公共子序列。
        /// The longest common subsequence (LCS) problem is to find the longest subsequence common to all sequences in a set of sequences (often just two). Note that subsequence is different from a substring, see substring vs. subsequence. It is a classic computer science problem, the basis of diff (a file comparison program that outputs the differences between two files), and has applications in bioinformatics.
        /// </summary>
        /// <remarks>
        /// 算法代码由@Aimeast 独立完成。http://www.cnblogs.com/Aimeast/archive/2011/09/05/2167844.html
        /// </remarks>
        /// <param name="word"></param>
        /// <param name="keys">多个关键字。长度必须大于0，必须按照字符串长度升序排列。</param>
        /// <returns></returns>
        public static Int32 LCSDistance(String word, String[] keys)
        {
            var sLength = word.Length;
            var result = sLength;
            var flags = new Boolean[sLength];
            var C = new Int32[sLength + 1, keys[keys.Length - 1].Length + 1];
            //int[,] C = new int[sLength + 1, words.Select(s => s.Length).Max() + 1];
            foreach (var key in keys) {
                var wLength = key.Length;
                Int32 first = 0, last = 0;
                Int32 i = 0, j = 0, LCS_L;
                //foreach 速度会有所提升，还可以加剪枝
                for (i = 0; i < sLength; i++)
                    for (j = 0; j < wLength; j++)
                        if (word[i] == key[j]) {
                            C[i + 1, j + 1] = C[i, j] + 1;
                            if (first < C[i, j]) {
                                last = i;
                                first = C[i, j];
                            }
                        } else
                            C[i + 1, j + 1] = Math.Max(C[i, j + 1], C[i + 1, j]);

                LCS_L = C[i, j];
                if (LCS_L <= wLength >> 1)
                    return -1;

                while (i > 0 && j > 0) {
                    if (C[i - 1, j - 1] + 1 == C[i, j]) {
                        i--;
                        j--;
                        if (!flags[i]) {
                            flags[i] = true;
                            result--;
                        }
                        first = i;
                    } else if (C[i - 1, j] == C[i, j])
                        i--;
                    else// if (C[i, j - 1] == C[i, j])
                        j--;
                }

                if (LCS_L <= (last - first + 1) >> 1)
                    return -1;
            }

            return result;
        }

        /// <summary>根据列表项成员计算距离</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="keys"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<T, Double>> LCS<T>(IEnumerable<T> list, String keys, Func<T, String> keySelector)
        {
            var rs = new List<KeyValuePair<T, Double>>();

            if (list == null || !list.Any()) return rs;
            if (keys.IsNullOrWhiteSpace()) return rs;
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            var ks = keys.Split(" ").OrderBy(_ => _.Length).ToArray();

            // 计算每个项到关键字的距离
            foreach (var item in list) {
                var name = keySelector(item);
                if (name.IsNullOrEmpty()) continue;

                var dist = LCSDistance(name, ks);
                if (dist >= 0) {
                    var val = (Double)dist / name.Length;
                    rs.Add(new KeyValuePair<T, Double>(item, val));
                }
            }

            //return rs.OrderBy(e => e.Value);
            return rs;
        }

        /// <summary>在列表项中进行模糊搜索</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="keys"></param>
        /// <param name="keySelector"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<T> LCSSearch<T>(IEnumerable<T> list, String keys, Func<T, String> keySelector, Int32 count = -1)
        {
            var rs = LCS(list, keys, keySelector);

            if (count >= 0)
                rs = rs.OrderBy(e => e.Value).Take(count);
            else
                rs = rs.OrderBy(e => e.Value);

            return rs.Select(e => e.Key);
        }
        #endregion

        #region 字符串模糊匹配
        /// <summary>模糊匹配</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="keys"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<T, Double>> Match<T>(IEnumerable<T> list, String keys, Func<T, String> keySelector)
        {
            var rs = new List<KeyValuePair<T, Double>>();

            if (list == null || !list.Any()) return rs;
            if (keys.IsNullOrWhiteSpace()) return rs;
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            var ks = keys.Split(" ").OrderBy(_ => _.Length).ToArray();

            // 计算每个项到关键字的权重
            foreach (var item in list) {
                var name = keySelector(item);
                if (name.IsNullOrEmpty()) continue;

                var dist = ks.Sum(e => {
                    var kv = Match(name, e, e.Length);
                    return kv.Key - kv.Value * 0.5;
                });
                if (dist > 0) {
                    var val = dist / keys.Length;
                    //var val = dist;
                    rs.Add(new KeyValuePair<T, Double>(item, val));
                }
            }

            return rs;
        }

        /// <summary>模糊匹配</summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <param name="maxError"></param>
        /// <returns></returns>
        public static KeyValuePair<Int32, Int32> Match(String str, String key, Int32 maxError = 0)
        {
            /*
             * 字符串 abcdef
             * 少字符 ace      (3, 0)
             * 多字符 abkcd    (4, 1)
             * 改字符 abmd     (3, 1)
             */

            // str下一次要匹配的位置
            var m = 0;
            // key下一次要匹配的位置
            var k = 0;

            // 总匹配数
            var match = 0;
            // 跳过次数
            var skip = 0;

            while (skip <= maxError && k < key.Length) {
                // 向前逐个匹配
                for (var i = m; i < str.Length; i++) {
                    if (str[i] == key[k]) {
                        k++;
                        m = i + 1;
                        match++;

                        // 如果已完全匹配，则结束
                        if (k == key.Length) break;
                    }
                }

                // 如果已完全匹配，则结束
                if (k == key.Length) break;

                // 没有完全匹配，跳过关键字中的一个字符串，从上一次匹配后面继续找
                k++;
                skip++;
            }

            return new KeyValuePair<Int32, Int32>(match, skip);
        }

        /// <summary>模糊匹配</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">列表项</param>
        /// <param name="keys">关键字</param>
        /// <param name="keySelector">匹配字符串选择</param>
        /// <param name="count">获取个数</param>
        /// <param name="confidence">权重阀值</param>
        /// <returns></returns>
        public static IEnumerable<T> Match<T>(IEnumerable<T> list, String keys, Func<T, String> keySelector, Int32 count, Double confidence = 0.5)
        {
            var rs = Match(list, keys, keySelector).Where(e => e.Value >= confidence);

            if (count >= 0)
                rs = rs.OrderByDescending(e => e.Value).Take(count);
            else
                rs = rs.OrderByDescending(e => e.Value);

            return rs.Select(e => e.Key);
        }
        #endregion
    }
}
