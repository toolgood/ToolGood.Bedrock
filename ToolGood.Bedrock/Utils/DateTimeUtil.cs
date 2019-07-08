using System;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Bedrock
{
    /// <summary>
    /// 时间
    /// </summary>
    public static class DateTimeUtil
    {
        private static int _year;
        private static int _month;
        private static int _day;
        private static int _hour;
        private static int _minute;
        private static int _second;
        private static bool _useDateTime = false;
        private static bool _useDate = false;

        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <param name="isRealNow">是否取真实的电脑时间</param>
        /// <returns></returns>
        public static DateTime GetNow(bool isRealNow = false)
        {
            if (isRealNow) {
                return DateTime.Now;
            }
            if (_useDateTime) {
                return new DateTime(_year, _month, _day, _hour, _minute, _second);
            } else if (_useDate) {
                var now = DateTime.Now;
                return new DateTime(_year, _month, _day, now.Hour, now.Minute, now.Second);
            }
            return DateTime.Now;
        }

        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <param name="isRealNow">是否取真实的电脑时间</param>
        /// <returns></returns>
        public static DateTime GetUtcNow(bool isRealNow = false)
        {
            if (isRealNow) {
                return DateTime.UtcNow;
            }
            if (_useDateTime) {
                return new DateTime(_year, _month, _day, _hour, _minute, _second, DateTimeKind.Utc);
            } else if (_useDate) {
                var now = DateTime.UtcNow;
                return new DateTime(_year, _month, _day, now.Hour, now.Minute, now.Second, DateTimeKind.Utc);
            }
            return DateTime.UtcNow;
        }

        /// <summary>
        /// 获取真实的Now
        /// </summary>
        /// <returns></returns>
        public static DateTime GetRealNow()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// 获取真实的UtcNow
        /// </summary>
        /// <returns></returns>
        public static DateTime GetRealUtcNow()
        {
            return DateTime.UtcNow;
        }


        /// <summary>
        /// 使用自定义当前时间
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">时</param>
        /// <param name="minute">分</param>
        /// <param name="second">秒</param>
        public static void UseNow(int year, int month, int day, int hour, int minute, int second)
        {
            if (year < 1) { throw new ArgumentException(nameof(year)); }
            _year = year;
            if (month < 1 || month >= 12) { throw new ArgumentException(nameof(month)); }
            _month = month;
            if (day < 1 || day >= 31) { throw new ArgumentException(nameof(day)); }
            _day = day;
            if (hour < 0 || hour >= 24) { throw new ArgumentException(nameof(hour)); }
            _hour = hour;
            if (minute < 0 || minute >= 60) { throw new ArgumentException(nameof(minute)); }
            _minute = minute;
            if (second < 0 || second >= 60) { throw new ArgumentException(nameof(second)); }
            _second = second;
            new DateTime(_year, _month, _day, _hour, _minute, _second);
            _useDateTime = true;
            _useDate = false;
        }

        /// <summary>
        /// 使用自定义当前时间
        /// </summary>
        /// <param name="dateTime"></param>
        public static void UseNow(DateTime dateTime)
        {
            UseNow(dateTime.Year, dateTime.Month, dateTime.Day,dateTime.Hour,dateTime.Minute,dateTime.Second);
        }


        /// <summary>
        /// 使用自定义当前时间，定义日期
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        public static void UseToday(int year, int month, int day)
        {
            if (year < 1) { throw new ArgumentException(nameof(year)); }
            _year = year;
            if (month < 1 || month >= 12) { throw new ArgumentException(nameof(month)); }
            _month = month;
            if (day < 1 || day >= 31) { throw new ArgumentException(nameof(day)); }
            _day = day;
            _hour = 0;
            _minute = 0;
            _second = 0;
            new DateTime(_year, _month, _day);
            _useDateTime = false;
            _useDate = true;
        }

        /// <summary>
        /// 使用自定义当前时间，定义日期
        /// </summary>
        /// <param name="dateTime"></param>
        public static void UseToday(DateTime dateTime)
        {
            UseToday(dateTime.Year, dateTime.Month, dateTime.Day);
        }



        /// <summary>
        /// 关闭自定义当前时间
        /// </summary>
        public static void ColseCustomNow()
        {
            _useDateTime = false;
            _useDate = false;
        }


        /// <summary>
        /// 获取当前时间的时间戳
        /// </summary>
        /// <param name="isRealNow">是否取真实的电脑时间</param>
        /// <param name="isUtcNow">是否为UtcNow</param>
        /// <returns></returns>
        public static long GetTimestamp(bool isRealNow = false, bool isUtcNow = false)
        {
            var now = isUtcNow ? GetUtcNow(isRealNow) : GetNow(isRealNow);
            return (long)(now - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)).TotalSeconds;
        }

        /// <summary>
        /// 将时间戳转为时间
        /// </summary>
        /// <param name="totalSeconds">秒</param>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(long totalSeconds, DateTimeKind kind = DateTimeKind.Local)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, kind);
            return start.AddSeconds(totalSeconds);
        }

        /// <summary>
        /// 获取月初
        /// </summary>
        /// <param name="isRealNow">是否取真实的电脑时间</param>
        /// <param name="isUtcNow">是否为UtcNow</param>
        /// <returns></returns>
        public static DateTime GetMonthStart(bool isRealNow = false, bool isUtcNow = false)
        {
            var now = isUtcNow ? GetUtcNow(isRealNow) : GetNow(isRealNow);
            return new DateTime(now.Year, now.Month, 1);
        }

        /// <summary>
        /// 获取月末
        /// </summary>
        /// <param name="isRealNow">是否取真实的电脑时间</param>
        /// <param name="isUtcNow">是否为UtcNow</param>
        /// <returns></returns>
        public static DateTime GetMonthEnd(bool isRealNow = false, bool isUtcNow = false)
        {
            var now = isUtcNow ? GetUtcNow(isRealNow) : GetNow(isRealNow);
            return new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// 获取下月月初
        /// </summary>
        /// <param name="isRealNow">是否取真实的电脑时间</param>
        /// <param name="isUtcNow">是否为UtcNow</param>
        /// <returns></returns>
        public static DateTime GetNextMonthStart(bool isRealNow = false, bool isUtcNow = false)
        {
            var now = isUtcNow ? GetUtcNow(isRealNow) : GetNow(isRealNow);
            return new DateTime(now.Year, now.Month, 1).AddMonths(1).AddMonths(1);
        }

        /// <summary>
        /// 获取下月月末
        /// </summary>
        /// <param name="isRealNow">是否取真实的电脑时间</param>
        /// <param name="isUtcNow">是否为UtcNow</param>
        /// <returns></returns>
        public static DateTime GetNextMonthEnd(bool isRealNow = false, bool isUtcNow = false)
        {
            var now = isUtcNow ? GetUtcNow(isRealNow) : GetNow(isRealNow);
            return new DateTime(now.Year, now.Month, 1).AddMonths(2).AddMonths(-1);
        }

        /// <summary>
        /// 获取上月月初
        /// </summary>
        /// <param name="isRealNow">是否取真实的电脑时间</param>
        /// <param name="isUtcNow">是否为UtcNow</param>
        /// <returns></returns>
        public static DateTime GetPrevMonthStart(bool isRealNow = false, bool isUtcNow = false)
        {
            var now = isUtcNow ? GetUtcNow(isRealNow) : GetNow(isRealNow);
            return new DateTime(now.Year, now.Month, 1).AddMonths(-1).AddMonths(1);
        }

        /// <summary>
        /// 获取上月月末
        /// </summary>
        /// <param name="isRealNow">是否取真实的电脑时间</param>
        /// <param name="isUtcNow">是否为UtcNow</param>
        /// <returns></returns>
        public static DateTime GetPrevMonthEnd(bool isRealNow = false, bool isUtcNow = false)
        {
            var now = isUtcNow ? GetUtcNow(isRealNow) : GetNow(isRealNow);
            return new DateTime(now.Year, now.Month, 1).AddMonths(-1);
        }

        /// <summary>
        /// 获取年初
        /// </summary>
        /// <param name="isRealNow">是否取真实的电脑时间</param>
        /// <param name="isUtcNow">是否为UtcNow</param>
        /// <returns></returns>
        public static DateTime GetYearStart(bool isRealNow = false, bool isUtcNow = false)
        {
            var now = isUtcNow ? GetUtcNow(isRealNow) : GetNow(isRealNow);
            return new DateTime(now.Year, 1, 1);
        }

        /// <summary>
        /// 获取年末
        /// </summary>
        /// <param name="isRealNow">是否取真实的电脑时间</param>
        /// <param name="isUtcNow">是否为UtcNow</param>
        /// <returns></returns>
        public static DateTime GetYearEnd(bool isRealNow = false, bool isUtcNow = false)
        {
            var now = isUtcNow ? GetUtcNow(isRealNow) : GetNow(isRealNow);
            return new DateTime(now.Year, 1, 1).AddYears(1).AddDays(-1);
        }


        /// <summary>
        /// 获取下一年年初
        /// </summary>
        /// <param name="isRealNow">是否取真实的电脑时间</param>
        /// <param name="isUtcNow">是否为UtcNow</param>
        /// <returns></returns>
        public static DateTime GetNextYearStart(bool isRealNow = false, bool isUtcNow = false)
        {
            var now = isUtcNow ? GetUtcNow(isRealNow) : GetNow(isRealNow);
            return new DateTime(now.Year, 1, 1).AddYears(1);
        }

        /// <summary>
        /// 获取下一年年末
        /// </summary>
        /// <param name="isRealNow">是否取真实的电脑时间</param>
        /// <param name="isUtcNow">是否为UtcNow</param>
        /// <returns></returns>
        public static DateTime GetNextYearEnd(bool isRealNow = false, bool isUtcNow = false)
        {
            var now = isUtcNow ? GetUtcNow(isRealNow) : GetNow(isRealNow);
            return new DateTime(now.Year, 1, 1).AddYears(2).AddDays(-1);
        }

        /// <summary>
        /// 获取上一年年初
        /// </summary>
        /// <param name="isRealNow">是否取真实的电脑时间</param>
        /// <param name="isUtcNow">是否为UtcNow</param>
        /// <returns></returns>
        public static DateTime GetPrevYearStart(bool isRealNow = false, bool isUtcNow = false)
        {
            var now = isUtcNow ? GetUtcNow(isRealNow) : GetNow(isRealNow);
            return new DateTime(now.Year, 1, 1).AddYears(-1);
        }

        /// <summary>
        /// 获取上一年年末
        /// </summary>
        /// <param name="isRealNow">是否取真实的电脑时间</param>
        /// <param name="isUtcNow">是否为UtcNow</param>
        /// <returns></returns>
        public static DateTime GetPrevYearEnd(bool isRealNow = false, bool isUtcNow = false)
        {
            var now = isUtcNow ? GetUtcNow(isRealNow) : GetNow(isRealNow);
            return new DateTime(now.Year, 1, 1).AddDays(-1);
        }

    }
}
