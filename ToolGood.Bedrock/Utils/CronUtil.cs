using System;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Bedrock.Utils
{
    /// <summary>
    /// Helper class that provides common values for the cron expressions.
    /// </summary>
    public static class CronUtil
    {
        /// <summary>
        /// cron expressions type
        /// </summary>
        public enum CronType
        {
            Hangfire,

            Quartz,

        }


        /// <summary>Returns cron expression that fires every minute.</summary>
        /// <param name="type"></param>
        public static string Minutely(CronType type = CronType.Quartz)
        {
            if (type == CronType.Quartz) {
                return "* * * * * ?";
            }
            return "* * * * *";
        }

        /// <summary>
        /// Returns cron expression that fires every hour at the first minute.
        /// </summary>
        /// <param name="type"></param>
        public static string Hourly(CronType type = CronType.Quartz)
        {
            return Hourly(0, type);
        }

        /// <summary>
        /// Returns cron expression that fires every hour at the specified minute.
        /// </summary>
        /// <param name="minute">The minute in which the schedule will be activated (0-59).</param>
        /// <param name="type"></param>
        public static string Hourly(int minute, CronType type = CronType.Quartz)
        {
            if (type == CronType.Quartz) {
                return string.Format("* {0} * * * ?", minute);
            }
            return string.Format("{0} * * * *", minute);
        }


        /// <summary>
        /// Returns cron expression that fires every day at 00:00 UTC.
        /// </summary>
        /// <param name="type"></param>
        public static string Daily(CronType type = CronType.Quartz)
        {
            return Daily(0, type);
        }

        /// <summary>
        /// Returns cron expression that fires every day at the first minute of
        /// the specified hour in UTC.
        /// </summary>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        /// <param name="type"></param>
        public static string Daily(int hour, CronType type = CronType.Quartz)
        {
            return Daily(hour, 0, type);
        }

        /// <summary>
        /// Returns cron expression that fires every day at the specified hour and minute
        /// in UTC.
        /// </summary>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        /// <param name="minute">The minute in which the schedule will be activated (0-59).</param>
        /// <param name="type"></param>
        public static string Daily(int hour, int minute, CronType type = CronType.Quartz)
        {
            if (type == CronType.Quartz) {
                return string.Format("* {0} {1} * * ?", minute, hour);
            }
            return string.Format("{0} {1} * * *", minute, hour);
        }

        /// <summary>
        /// Returns cron expression that fires every week at 00:00 UTC of the specified
        /// day of the week.
        /// </summary>
        /// <param name="dayOfWeek">The day of week in which the schedule will be activated.</param>
        /// <param name="type"></param>
        public static string Weekly(DayOfWeek dayOfWeek, CronType type = CronType.Quartz)
        {
            return Weekly(dayOfWeek, 0, type);
        }

        /// <summary>
        /// Returns cron expression that fires every week at the first minute
        /// of the specified day of week and hour in UTC.
        /// </summary>
        /// <param name="dayOfWeek">The day of week in which the schedule will be activated.</param>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        /// <param name="type"></param>
        public static string Weekly(DayOfWeek dayOfWeek, int hour, CronType type = CronType.Quartz)
        {
            return Weekly(dayOfWeek, hour, 0, type);
        }


        /// <summary>
        /// Returns cron expression that fires every week at the specified day
        /// of week, hour and minute in UTC.
        /// </summary>
        /// <param name="dayOfWeek">The day of week in which the schedule will be activated.</param>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        /// <param name="minute">The minute in which the schedule will be activated (0-59).</param>
        /// <param name="type"></param>
        public static string Weekly(DayOfWeek dayOfWeek, int hour, int minute, CronType type = CronType.Quartz)
        {
            if (type == CronType.Quartz) {
                return string.Format("* {0} {1} * * {2}", minute, hour, dayOfWeek);
            }
            return string.Format("{0} {1} * * {2}", minute, hour, dayOfWeek);
        }



        /// <summary>
        /// Returns cron expression that fires every month at 00:00 UTC of the first
        /// day of month.
        /// </summary>
        /// <param name="type"></param>
        public static string Monthly(CronType type = CronType.Quartz)
        {
            return Monthly(1, type);
        }

        /// <summary>
        /// Returns cron expression that fires every month at 00:00 UTC of the specified
        /// day of month.
        /// </summary>
        /// <param name="day">The day of month in which the schedule will be activated (1-31).</param>
        /// <param name="type"></param>
        public static string Monthly(int day, CronType type = CronType.Quartz)
        {
            return Monthly(day, 0, type);
        }

        /// <summary>
        /// Returns cron expression that fires every month at the first minute of the
        /// specified day of month and hour in UTC.
        /// </summary>
        /// <param name="day">The day of month in which the schedule will be activated (1-31).</param>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        /// <param name="type"></param>
        public static string Monthly(int day, int hour, CronType type = CronType.Quartz)
        {
            return Monthly(day, hour, 0, type);
        }


        /// <summary>
        /// Returns cron expression that fires every month at the specified day of month,
        /// hour and minute in UTC.
        /// </summary>
        /// <param name="day">The day of month in which the schedule will be activated (1-31).</param>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        /// <param name="minute">The minute in which the schedule will be activated (0-59).</param>
        /// <param name="type"></param>
        public static string Monthly(int day, int hour, int minute, CronType type = CronType.Quartz)
        {
            if (type == CronType.Quartz) {
                return string.Format("* {0} {1} {2} * ?", minute, hour, day);
            }
            return string.Format("{0} {1} {2} * *", minute, hour, day);
        }


        /// <summary>
        /// Returns cron expression that fires every year on Jan, 1st at 00:00 UTC.
        /// </summary>
        /// <param name="type"></param>
        public static string Yearly(CronType type = CronType.Quartz)
        {
            return Yearly(1, type);
        }

        /// <summary>
        /// Returns cron expression that fires every year in the first day at 00:00 UTC
        /// of the specified month.
        /// </summary>
        /// <param name="month">The month in which the schedule will be activated (1-12).</param>
        /// <param name="type"></param>
        public static string Yearly(int month, CronType type = CronType.Quartz)
        {
            return Yearly(month, 1, type);
        }

        /// <summary>
        /// Returns cron expression that fires every year at 00:00 UTC of the specified
        /// month and day of month.
        /// </summary>
        /// <param name="month">The month in which the schedule will be activated (1-12).</param>
        /// <param name="day">The day of month in which the schedule will be activated (1-31).</param>
        /// <param name="type"></param>
        public static string Yearly(int month, int day, CronType type = CronType.Quartz)
        {
            return Yearly(month, day, 0, type);
        }

        /// <summary>
        /// Returns cron expression that fires every year at the first minute of the
        /// specified month, day and hour in UTC.
        /// </summary>
        /// <param name="month">The month in which the schedule will be activated (1-12).</param>
        /// <param name="day">The day of month in which the schedule will be activated (1-31).</param>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        /// <param name="type"></param>
        public static string Yearly(int month, int day, int hour, CronType type = CronType.Quartz)
        {
            return Yearly(month, day, hour, 0, type);
        }

        /// <summary>
        /// Returns cron expression that fires every year at the specified month, day,
        /// hour and minute in UTC.
        /// </summary>
        /// <param name="month">The month in which the schedule will be activated (1-12).</param>
        /// <param name="day">The day of month in which the schedule will be activated (1-31).</param>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        /// <param name="minute">The minute in which the schedule will be activated (0-59).</param>
        /// <param name="type"></param>
        public static string Yearly(int month, int day, int hour, int minute, CronType type = CronType.Quartz)
        {
            if (type == CronType.Quartz) {
                return string.Format("* {0} {1} {2} {3} ?", minute, hour, day, month);
            }
            return string.Format("{0} {1} {2} {3} *", minute, hour, day, month);
        }




    }
}
