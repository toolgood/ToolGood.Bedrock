using System;

namespace System
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// 转成简化时间
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToEasyString(this DateTime value)
        {
            DateTime now = DateTime.Now;
            if (now < value) return value.ToString("yyyy-MM-dd");
            TimeSpan dep = now - value;
            if (dep.TotalMinutes < 1) {
                return "刚刚";
            } else if (dep.TotalMinutes >= 1 && dep.TotalMinutes < 60) {
                return (int)dep.TotalMinutes + " 分钟前";
            } else if (dep.TotalHours < 24) {
                return (int)dep.TotalHours + " 小时前";
            } else if (dep.TotalDays < 5) {
                return (int)dep.TotalDays + " 天前";
            } else return value.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 转成简化时间
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToEasyString(this DateTime? value)
        {
            if (value.HasValue) return value.Value.ToEasyString();
            else return string.Empty;
        }


        /// <summary>
        /// Check if given <see cref="DayOfWeek"/> value is weekend.
        /// </summary>
        public static bool IsWeekend(this DayOfWeek dayOfWeek)
        {
            return dayOfWeek.IsIn(DayOfWeek.Saturday, DayOfWeek.Sunday);
        }

        /// <summary>
        /// Check if given <see cref="DayOfWeek"/> value is weekday.
        /// </summary>
        public static bool IsWeekday(this DayOfWeek dayOfWeek)
        {
            return dayOfWeek.IsIn(DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday);
        }

        /// <summary>
        /// 获取当前时间的时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long GetTimestamp(this DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Utc) {
                return (long)(dateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            } else if (dateTime.Kind == DateTimeKind.Unspecified) {
                return (long)(dateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)).TotalSeconds;
            }
            return (long)(dateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)).TotalSeconds;
        }

        /// <summary>
        /// 将时间戳转为时间
        /// </summary>
        /// <param name="totalSeconds"></param>
        /// <param name="kind"></param>
        /// <returns>时间</returns>
        public static DateTime ToDateTime(this long totalSeconds, DateTimeKind kind= DateTimeKind.Local)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, kind);
            return start.AddSeconds(totalSeconds);
        }


        /// <summary>
        /// 获取月初
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetMonthStart(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        /// <summary>
        /// 获取月末
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetMonthEnd(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// 获取下月月初
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetNextMonthStart(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1);
        }


        /// <summary>
        /// 获取年初
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetYearStart(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 1, 1);
        }

        /// <summary>
        /// 获取年末
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetYearEnd(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 1, 1).AddYears(1).AddDays(-1);
        }


        /// <summary>
        /// 获取下一年年初
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetNextYearStart(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 1, 1).AddYears(1);
        }

    }
}
