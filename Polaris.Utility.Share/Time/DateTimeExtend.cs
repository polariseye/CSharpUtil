namespace Polaris.Utility
{
    using System;

    public static class DateTimeExtend
    {
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static Int64 ToUnixTime(this DateTime now)
        {
            TimeSpan ts = now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        /// <summary>
        /// 转换为时间戳
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        public static DateTime UnixTimeToDateTime(this Int64 now)
        {
            var timeValue = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            timeValue.AddSeconds(now);

            return timeValue;
        }

        /// <summary>
        /// 转换为时间字符串 HH:mm:ss
        /// </summary>
        /// <param name="timeVal"></param>
        /// <returns></returns>
        public static string ToTimeString(this DateTime timeVal)
        {
            return timeVal.ToString($"HH:mm:ss");
        }

        /// <summary>
        /// 转换为完整时间字符串 yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="timeVal"></param>
        /// <returns></returns>
        public static String ToDateTimeString(this DateTime timeVal)
        {
            return timeVal.ToString($"yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 转换为日期字符串 yyyy-MM-dd
        /// </summary>
        /// <param name="timeVal"></param>
        /// <returns></returns>
        public static String ToDateString(this DateTime timeVal)
        {
            return timeVal.ToString($"yyyy-MM-dd");
        }
    }
}
