using System;

namespace System
{
    public static class NumberExtension
    {
        /// <summary>
        /// 显示文件大小
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDisplayFileSize(this int value)
        {
            if (value < 1000) {
                return string.Format("{0} Byte", value);
            } else if (value >= 1000 && value < 1000000) {
                return string.Format("{0:F2} Kb", ((double)value) / 1024);
            } else if (value >= 1000 && value < 1000000000) {
                return string.Format("{0:F2} M", ((double)value) / 1048576);
            } else {
                return string.Format("{0:F2} G", ((double)value) / 1073741824);
            }
        }

        /// <summary>
        /// 显示文件大小
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDisplayFileSize(this long value)
        {
            if (value < 1000) {
                return string.Format("{0} Byte", value);
            } else if (value >= 1000 && value < 1000000) {
                return string.Format("{0:F2} Kb", ((double)value) / 1024);
            } else if (value >= 1000 && value < 1000000000) {
                return string.Format("{0:F2} M", ((double)value) / 1048576);
            } else if (value >= 1000000000 && value < 1000000000000) {
                return string.Format("{0:F2} G", ((double)value) / 1073741824);
            } else {
                return string.Format("{0:F2} T", ((double)value) / 1099511627776);
            }
        }

        /// <summary>
        /// 将double数字四舍五入保留两位小数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double Round(this double input, int digits = 2)
        {
            return (double)Math.Round((decimal)input, digits, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 将double数字四舍五入保留两位小数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double Round(this double? input, int digits = 2)
        {
            if (null == input) {
                return 0.0;
            }
            return (double)Math.Round((decimal)input.Value, digits, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 将decimal数字四舍五入保留两位小数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static decimal Round(this decimal input, int digits = 2)
        {
            return Math.Round(input, 2, MidpointRounding.AwayFromZero);
        }


        /// <summary>
        /// 将decimal数字四舍五入保留两位小数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static decimal Round(this decimal? input, int digits = 2)
        {
            if (null == input) {
                return 0M;
            }
            return Math.Round(input.Value, 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Checks a value is between a minimum and maximum value.
        /// </summary>
        /// <param name="value">The value to be checked</param>
        /// <param name="minInclusiveValue">Minimum (inclusive) value</param>
        /// <param name="maxInclusiveValue">Maximum (inclusive) value</param>
        public static bool IsBetween<T>(this T value, T minInclusiveValue, T maxInclusiveValue) where T : IComparable//<T>
        {
            return value.CompareTo(minInclusiveValue) >= 0 && value.CompareTo(maxInclusiveValue) <= 0;
        }
    }
}
