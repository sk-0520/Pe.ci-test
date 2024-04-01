using System;
using System.Globalization;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Logic
{
    public class IReadOnlyCronItemSettingExtensionsTest
    {
        #region function

        [Fact]
        public void IsEnabled_Full_Test()
        {
            var cisf = new CronItemSettingFactory();
            var item = cisf.Parse("* * * * *");

            Assert.True(item.IsEnabled(DateTime.Now));
            Assert.True(item.IsEnabled(DateTime.UtcNow));
            Assert.True(item.IsEnabled(new DateTime(2000, 1, 1, 1, 1, 0)));
        }

        [Fact]
        public void IsEnabled_Minutes_Test()
        {
            var cisf = new CronItemSettingFactory();
            var item = cisf.Parse("1 * * * *");

            Assert.True(item.IsEnabled(new DateTime(2000, 1, 1, 1, 1, 0)));
            Assert.False(item.IsEnabled(new DateTime(2000, 1, 1, 1, 0, 0)));
            Assert.False(item.IsEnabled(new DateTime(2000, 1, 1, 1, 2, 0)));
        }

        [Fact]
        public void IsEnabled_Hour_Test()
        {
            var cisf = new CronItemSettingFactory();
            var item = cisf.Parse("* 1 * * *");

            Assert.True(item.IsEnabled(new DateTime(2000, 1, 1, 1, 0, 0)));
            Assert.True(item.IsEnabled(new DateTime(2000, 1, 1, 1, 1, 0)));
            Assert.True(item.IsEnabled(new DateTime(2000, 1, 1, 1, 2, 0)));

            Assert.True(item.IsEnabled(new DateTime(2000, 1, 1, 1, 0, 0)));
            Assert.False(item.IsEnabled(new DateTime(2000, 1, 1, 0, 0, 0)));
            Assert.False(item.IsEnabled(new DateTime(2000, 1, 1, 2, 0, 0)));
        }

        [Fact]
        public void IsEnabled_Day_Test()
        {
            var cisf = new CronItemSettingFactory();
            var item = cisf.Parse("* * 2 * *");

            Assert.True(item.IsEnabled(new DateTime(2000, 1, 2, 1, 0, 0)));
            Assert.True(item.IsEnabled(new DateTime(2000, 1, 2, 1, 1, 0)));
            Assert.True(item.IsEnabled(new DateTime(2000, 1, 2, 1, 2, 0)));

            Assert.True(item.IsEnabled(new DateTime(2000, 1, 2, 0, 0, 0)));
            Assert.True(item.IsEnabled(new DateTime(2000, 1, 2, 1, 0, 0)));
            Assert.True(item.IsEnabled(new DateTime(2000, 1, 2, 2, 0, 0)));

            Assert.True(item.IsEnabled(new DateTime(2000, 1, 2, 1, 0, 0)));
            Assert.False(item.IsEnabled(new DateTime(2000, 1, 1, 1, 0, 0)));
            Assert.False(item.IsEnabled(new DateTime(2000, 1, 3, 1, 0, 0)));
        }

        [Fact]
        public void IsEnabled_Month_Test()
        {
            var cisf = new CronItemSettingFactory();
            var item = cisf.Parse("* * * 2 *");

            Assert.True(item.IsEnabled(new DateTime(2000, 2, 2, 1, 0, 0)));
            Assert.True(item.IsEnabled(new DateTime(2000, 2, 2, 1, 1, 0)));
            Assert.True(item.IsEnabled(new DateTime(2000, 2, 2, 1, 2, 0)));

            Assert.True(item.IsEnabled(new DateTime(2000, 2, 2, 0, 0, 0)));
            Assert.True(item.IsEnabled(new DateTime(2000, 2, 2, 1, 0, 0)));
            Assert.True(item.IsEnabled(new DateTime(2000, 2, 2, 2, 0, 0)));

            Assert.True(item.IsEnabled(new DateTime(2000, 2, 1, 1, 0, 0)));
            Assert.True(item.IsEnabled(new DateTime(2000, 2, 2, 1, 0, 0)));
            Assert.True(item.IsEnabled(new DateTime(2000, 2, 3, 1, 0, 0)));

            Assert.True(item.IsEnabled(new DateTime(2000, 2, 2, 1, 0, 0)));
            Assert.False(item.IsEnabled(new DateTime(2000, 1, 1, 1, 0, 0)));
            Assert.False(item.IsEnabled(new DateTime(2000, 3, 3, 1, 0, 0)));
        }

        [Fact]
        public void IsEnabled_Weeks_Single_Test()
        {
            var cisf = new CronItemSettingFactory();
            for(var i = 0; i < 7; i++) {
                var item = cisf.Parse("* * * * " + i.ToString(CultureInfo.InvariantCulture));

                foreach(var weekDay in Enum.GetValues<DayOfWeek>()) {
                    var day = (int)weekDay;
                    var expected = item.IsEnabled(new DateTime(2020, 6, 21 + day, 1, 0, 0));
                    Assert.Equal(expected, i == day);
                }
            }
        }

        [Fact]
        public void IsEnabled_Weeks_Full_Test()
        {
            var cisf = new CronItemSettingFactory();
            var item = cisf.Parse("* * * * *");

            for(var i = 0; i < 7; i++) {
                Assert.True(item.IsEnabled(new DateTime(2020, 6, 21 + i, 1, 0, 0)));
            }
        }

        [Theory]
        [InlineData(true, "* * * * *", "2020-06-30T00:00")] // この日は火曜日
        [InlineData(false, "* * * * 0", "2020-06-30T00:00")]
        [InlineData(false, "* * * * 1", "2020-06-30T00:00")]
        [InlineData(true, "* * * * 2", "2020-06-30T00:00")]
        [InlineData(false, "* * * * 3", "2020-06-30T00:00")]
        [InlineData(false, "* * * * 4", "2020-06-30T00:00")]
        [InlineData(false, "* * * * 5", "2020-06-30T00:00")]
        [InlineData(false, "* * * * 6", "2020-06-30T00:00")]
        [InlineData(false, "* * * * 1,3,5", "2020-06-30T00:00")]
        [InlineData(true, "* * * * 2,4,6", "2020-06-30T00:00")]
        [InlineData(true, "* * * * 2,4,6", "2020-07-02T00:00")]
        [InlineData(true, "* * * * 2,4,6", "2020-07-04T00:00")]
        [InlineData(false, "1 * * * *", "2020-06-30T00:00")]
        [InlineData(true, "0 * * * *", "2020-06-30T00:00")]
        [InlineData(true, "0 0 * * *", "2020-06-30T00:00")]
        [InlineData(false, "0 1 * * *", "2020-06-30T00:00")]
        [InlineData(false, "1 0 * * *", "2020-06-30T00:00")]
        [InlineData(false, "1 1 * * *", "2020-06-30T00:00")]
        [InlineData(false, "0 0 29 * *", "2020-06-30T00:00")]
        [InlineData(false, "0 1 29 * *", "2020-06-30T00:00")]
        [InlineData(false, "1 0 29 * *", "2020-06-30T00:00")]
        [InlineData(false, "1 1 29 * *", "2020-06-30T00:00")]
        [InlineData(true, "0 0 30 * *", "2020-06-30T00:00")]
        [InlineData(false, "0 1 30 * *", "2020-06-30T00:00")]
        [InlineData(false, "1 0 30 * *", "2020-06-30T00:00")]
        [InlineData(false, "1 1 30 5 *", "2020-06-30T00:00")]
        [InlineData(false, "0 0 29 5 *", "2020-06-30T00:00")]
        [InlineData(false, "0 1 29 5 *", "2020-06-30T00:00")]
        [InlineData(false, "1 0 29 5 *", "2020-06-30T00:00")]
        [InlineData(false, "1 1 29 5 *", "2020-06-30T00:00")]
        [InlineData(false, "0 0 30 5 *", "2020-06-30T00:00")]
        [InlineData(false, "0 1 30 5 *", "2020-06-30T00:00")]
        [InlineData(false, "1 0 30 5 *", "2020-06-30T00:00")]
        //[InlineData(false, "1 1 30 5 *", "2020-06-30T00:00")]
        [InlineData(false, "1 1 30 6 *", "2020-06-30T00:00")]
        [InlineData(false, "0 0 29 6 *", "2020-06-30T00:00")]
        [InlineData(false, "0 1 29 6 *", "2020-06-30T00:00")]
        [InlineData(false, "1 0 29 6 *", "2020-06-30T00:00")]
        [InlineData(false, "1 1 29 6 *", "2020-06-30T00:00")]
        [InlineData(true, "0 0 30 6 *", "2020-06-30T00:00")]
        [InlineData(false, "0 1 30 6 *", "2020-06-30T00:00")]
        [InlineData(false, "1 0 30 6 *", "2020-06-30T00:00")]
        //[InlineData(false, "1 1 30 6 *", "2020-06-30T00:00")]
        // 13日の金曜日(曜日と日指定のなんだかなぁパターン)
        [InlineData(true, "* * * 11 5", "2020-11-06T00:00")]
        [InlineData(true, "* * * 11 5", "2020-11-13T00:00")]
        [InlineData(true, "* * * 11 5", "2020-11-20T00:00")]
        [InlineData(true, "* * * 11 5", "2020-11-27T00:00")]
        [InlineData(true, "* * 13 11 5", "2020-11-06T00:00")]
        [InlineData(true, "* * 13 11 5", "2020-11-13T00:00")]
        [InlineData(true, "* * 13 11 5", "2020-11-20T00:00")]
        [InlineData(true, "* * 13 11 5", "2020-11-27T00:00")]
        [InlineData(true, "* * 13 * 5", "2020-03-06T00:00")]
        [InlineData(true, "* * 13 * 5", "2020-03-13T00:00")]
        [InlineData(true, "* * 13 * 5", "2020-03-20T00:00")]
        [InlineData(true, "* * 13 * 5", "2020-03-27T00:00")]
        [InlineData(false, "* * 13 * *", "2020-03-06T00:00")]
        [InlineData(true, "* * 13 * *", "2020-03-13T00:00")]
        [InlineData(false, "* * 13 * *", "2020-03-20T00:00")]
        [InlineData(false, "* * 13 * *", "2020-03-27T00:00")]
        public void IsEnabledTest(bool expected, string cronPattern, string inputIso8601)
        {
            var cisf = new CronItemSettingFactory();
            var item = cisf.Parse(cronPattern);
            var actual = item.IsEnabled(DateTime.Parse(inputIso8601, CultureInfo.InvariantCulture));
            Assert.Equal(expected, actual);
        }

        #endregion
    }

    public class CronItemSettingFactoryTest
    {
        #region function

        [Theory]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("a b")]
        [InlineData("a b c")]
        [InlineData("a b c d")]
        [InlineData("a b c d e")]
        [InlineData("0 1")]
        [InlineData("0 1 2 3 4 5")]
        [InlineData("@")]
        [InlineData("@reboot")]
        [InlineData("@yearly")]
        [InlineData("@annually")]
        [InlineData("@monthly")]
        [InlineData("@weekly")]
        public void Parse_Exception_Test(string value)
        {
            var cisf = new CronItemSettingFactory();
            try {
                cisf.Parse(value);
                Assert.Fail();
            } catch(Exception) {
                Assert.True(true);
            }

        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Assertions", "xUnit2013:Do not use equality check to check for collection size.", Justification = "<保留中>")]
        public void Parse_At_Test()
        {
            var cisf = new CronItemSettingFactory();

            var h = cisf.Parse("@hourly");
            Assert.Equal(1, h.Minutes.Count);
            Assert.Equal(0, h.Hours.Count);
            Assert.Equal(0, h.Days.Count);
            Assert.Equal(0, h.Months.Count);
            Assert.Equal(0, h.DayOfWeeks.Count);

            var d = cisf.Parse("@daily");
            Assert.Equal(1, d.Minutes.Count);
            Assert.Equal(1, d.Hours.Count);
            Assert.Equal(0, d.Days.Count);
            Assert.Equal(0, d.Months.Count);
            Assert.Equal(0, d.DayOfWeeks.Count);
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Assertions", "xUnit2013:Do not use equality check to check for collection size.", Justification = "<保留中>")]
        public void Parse_Simple_Test()
        {
            var cisf = new CronItemSettingFactory();

            {
                var actual = cisf.Parse("* 0 0 0 0");
                Assert.Equal(60, actual.Minutes.Count);
                Assert.Equal(1, actual.Hours.Count);
                Assert.Equal(1, actual.Days.Count);
                Assert.Equal(1, actual.Months.Count);
                Assert.Equal(1, actual.DayOfWeeks.Count);
            }

            {
                var actual = cisf.Parse("0 * 0 0 0");
                Assert.Equal(1, actual.Minutes.Count);
                Assert.Equal(24, actual.Hours.Count);
                Assert.Equal(1, actual.Days.Count);
                Assert.Equal(1, actual.Months.Count);
                Assert.Equal(1, actual.DayOfWeeks.Count);
            }

            {
                var actual = cisf.Parse("0 0 * 0 0");
                Assert.Equal(1, actual.Minutes.Count);
                Assert.Equal(1, actual.Hours.Count);
                Assert.Equal(31, actual.Days.Count);
                Assert.Equal(1, actual.Months.Count);
                Assert.Equal(1, actual.DayOfWeeks.Count);
            }

            {
                var actual = cisf.Parse("0 0 0 * 0");
                Assert.Equal(1, actual.Minutes.Count);
                Assert.Equal(1, actual.Hours.Count);
                Assert.Equal(1, actual.Days.Count);
                Assert.Equal(12, actual.Months.Count);
                Assert.Equal(1, actual.DayOfWeeks.Count);
            }

            {
                var actual = cisf.Parse("0 0 0 0 *");
                Assert.Equal(1, actual.Minutes.Count);
                Assert.Equal(1, actual.Hours.Count);
                Assert.Equal(1, actual.Days.Count);
                Assert.Equal(1, actual.Months.Count);
                Assert.Equal(7, actual.DayOfWeeks.Count);
            }
        }

        [Theory]
        [InlineData(new[] { 0, 1 }, "0,1")]
        [InlineData(new[] { 0, 1 }, "0,1,1")]
        [InlineData(new[] { 2, 3, 4, 5 }, "2-5")]
        [InlineData(new[] { 0, 1, 2, 5, 10, 11, 12, 13, 57, 58, 59 }, "0,1,2,10-13,5,57-59")]
        public void Parse_Minutes_Test(int[] expecteds, string targetPattern)
        {
            var cisf = new CronItemSettingFactory();
            var actual = cisf.Parse(targetPattern + " * * * *");
            Assert.Equal(expecteds, actual.Minutes);
            Assert.Equal(24, actual.Hours.Count);
            Assert.Equal(31, actual.Days.Count);
            Assert.Equal(12, actual.Months.Count);
            Assert.Equal(7, actual.DayOfWeeks.Count);
        }

        #endregion
    }

    public class CronSchedulerTest
    {
        #region function

        [Theory]
        [InlineData(60000, "2020-06-28T20:42:00.000")]
        [InlineData(1000, "2020-06-28T20:42:59.000")]
        [InlineData(29999, "2020-06-28T20:42:30.001")]
        [InlineData(30000, "2020-06-28T20:42:30.000")]
        [InlineData(30001, "2020-06-28T20:42:29.999")]
        [InlineData(1000, "2020-06-28T20:59:59.000")]
        [InlineData(1000, "2020-06-30T23:59:59.000")]
        [InlineData(1000, "2020-12-31T23:59:59.000")]
        [InlineData(500, "2020-06-28T20:42:59.500")]
        [InlineData(1, "2020-06-28T20:42:59.999")]
        public void GetNextJobWaitTimeTest(double expected, string iso8601)
        {
            var input = DateTime.Parse(iso8601, CultureInfo.InvariantCulture);
            var cs = new CronScheduler(new IdFactory(Test.LoggerFactory),Test.LoggerFactory);
            var actual = cs.GetNextJobWaitTime(input);
            Assert.Equal(expected, actual.TotalMilliseconds);
        }

        //[Fact]
        //public void GetItemFromTimeTest()
        //{
        //    var cs = new CronScheduler(Timeout.InfiniteTimeSpan, TimeSpan.FromSeconds(1));
        //}

        #endregion
    }

}
