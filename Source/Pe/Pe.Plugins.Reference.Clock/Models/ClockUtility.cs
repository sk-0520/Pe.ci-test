using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Clock.Models
{
    public static class ClockUtility
    {
        #region function

        public static string GetClockEmoji(DateTime dateTime)
        {
            return (dateTime.Hour < 12 ? dateTime.Hour : dateTime.Hour - 12) switch
            {
                0 => "🕛",
                1 => "🕐",
                2 => "🕑",
                3 => "🕒",
                4 => "🕓",
                5 => "🕔",
                6 => "🕕",
                7 => "🕖",
                8 => "🕗",
                9 => "🕘",
                10 => "🕙",
                11 => "🕚",
                _ => "⏰",
            };
        }

        public static double GetSecondsAngle(DateTime dateTime) => dateTime.Second * 6.0;
        public static double GetMinuteAngle(DateTime dateTime) => dateTime.Minute * 6.0;
        public static double GetHourAngle(DateTime dateTime) => (dateTime.Hour * 30.0) + (dateTime.Minute * 0.5);

        #endregion
    }
}
