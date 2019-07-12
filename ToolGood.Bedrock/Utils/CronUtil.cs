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
        public enum CronType
        {
            Quartz,

            Hangfire
        }


        /// <summary>Returns cron expression that fires every minute.</summary>
        public static string Minutely(CronType type = CronType.Quartz)
        {
            if (type == CronType.Quartz) {
                return "* * * * * ?";
            }
            return "* * * * *";
        }

        /// <summary>
        /// Returns cron expression that fires every hour at the specified minute.
        /// </summary>
        /// <param name="minute">The minute in which the schedule will be activated (0-59).</param>
        /// <param name="type"></param>
        public static string Hourly(int minute = 0, CronType type = CronType.Quartz)
        {
            if (type == CronType.Quartz) {
                return string.Format("* {0} * * * ?", minute);
            }
            return string.Format("{0} * * * *", minute);
        }


        /// <summary>
        /// Returns cron expression that fires every day at the specified hour and minute
        /// in UTC.
        /// </summary>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        /// <param name="minute">The minute in which the schedule will be activated (0-59).</param>
        /// <param name="type"></param>
        public static string Daily(int hour = 0, int minute = 0, CronType type = CronType.Quartz)
        {
            if (type == CronType.Quartz) {
                return string.Format("* {0} {1} * * ?", minute, hour);
            }
            return string.Format("{0} {1} * * *", minute, hour);
        }

        /// <summary>
        /// Returns cron expression that fires every week at the specified day
        /// of week, hour and minute in UTC.
        /// </summary>
        /// <param name="dayOfWeek">The day of week in which the schedule will be activated.</param>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        /// <param name="minute">The minute in which the schedule will be activated (0-59).</param>
        /// <param name="type"></param>
        public static string Weekly(DayOfWeek dayOfWeek, int hour = 0, int minute = 0, CronType type = CronType.Quartz)
        {
            if (type == CronType.Quartz) {
                return string.Format("* {0} {1} * * {2}", minute, hour, dayOfWeek);
            }
            return string.Format("{0} {1} * * {2}", minute, hour, dayOfWeek);
        }



        /// <summary>
        /// Returns cron expression that fires every month at the specified day of month,
        /// hour and minute in UTC.
        /// </summary>
        /// <param name="day">The day of month in which the schedule will be activated (1-31).</param>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        /// <param name="minute">The minute in which the schedule will be activated (0-59).</param>
        /// <param name="type"></param>
        public static string Monthly(int day = 0, int hour = 0, int minute = 0, CronType type = CronType.Quartz)
        {
            if (type == CronType.Quartz) {
                return string.Format("* {0} {1} {2} * ?", minute, hour, day);
            }
            return string.Format("{0} {1} {2} * *", minute, hour, day);
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
        public static string Yearly(int month = 0, int day = 0, int hour = 0, int minute = 0, CronType type = CronType.Quartz)
        {
            if (type == CronType.Quartz) {
                return string.Format("* {0} {1} {2} {3} ?", minute, hour, day, month);
            }
            return string.Format("{0} {1} {2} {3} *", minute, hour, day, month);
        }




    }
}
