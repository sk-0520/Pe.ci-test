using System;
using System.Globalization;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public class TimestampConverter
    {
        public TimestampConverter(CultureInfo cultureInfo)
        {
            CultureInfo = cultureInfo;
        }

        #region property

        public CultureInfo CultureInfo { get; }

        #endregion

        #region function

        private DateTime ToLocal(DateTime dateTime) => dateTime.Kind != DateTimeKind.Local ? dateTime.ToLocalTime() : dateTime;

        /// <summary>
        /// 指定日時をローカル日時文字列に変換。
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public string ToViewFullString(DateTime dateTime)
        {
            var localDateTime = ToLocal(dateTime);
            return localDateTime.ToString(CultureInfo);
        }

        #endregion
    }
}
