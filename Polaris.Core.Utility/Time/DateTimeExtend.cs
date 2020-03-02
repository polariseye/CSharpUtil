namespace Polaris.Utility
{
    using System;

    public static class ExtendHelper
    {
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static Int64 ToUnixTime(this DateTime now)
        {
            TimeSpan ts = now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        public static DateTime UnixTimeToDateTime(this Int64 now)
        {
            var timeValue = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            timeValue.AddSeconds(now);

            return timeValue;
        }
    }
}
