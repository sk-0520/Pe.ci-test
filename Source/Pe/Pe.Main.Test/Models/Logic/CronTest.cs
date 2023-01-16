using System;
using System.Globalization;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Logic
{
    [TestClass]
    public class IReadOnlyCronItemSettingExtensionsTest
    {
        #region function

        [TestMethod]
        public void IsEnabled_Full_Test()
        {
            var cisf = new CronItemSettingFactory();
            var item = cisf.Parse("* * * * *");

            Assert.IsTrue(item.IsEnabled(DateTime.Now));
            Assert.IsTrue(item.IsEnabled(DateTime.UtcNow));
            Assert.IsTrue(item.IsEnabled(new DateTime(2000, 1, 1, 1, 1, 0)));
        }

        [TestMethod]
        public void IsEnabled_Minutes_Test()
        {
            var cisf = new CronItemSettingFactory();
            var item = cisf.Parse("1 * * * *");

            Assert.IsTrue(item.IsEnabled(new DateTime(2000, 1, 1, 1, 1, 0)));
            Assert.IsFalse(item.IsEnabled(new DateTime(2000, 1, 1, 1, 0, 0)));
            Assert.IsFalse(item.IsEnabled(new DateTime(2000, 1, 1, 1, 2, 0)));
        }

        [TestMethod]
        public void IsEnabled_Hour_Test()
        {
            var cisf = new CronItemSettingFactory();
            var item = cisf.Parse("* 1 * * *");

            Assert.IsTrue(item.IsEnabled(new DateTime(2000, 1, 1, 1, 0, 0)));
            Assert.IsTrue(item.IsEnabled(new DateTime(2000, 1, 1, 1, 1, 0)));
            Assert.IsTrue(item.IsEnabled(new DateTime(2000, 1, 1, 1, 2, 0)));

            Assert.IsTrue(item.IsEnabled(new DateTime(2000, 1, 1, 1, 0, 0)));
            Assert.IsFalse(item.IsEnabled(new DateTime(2000, 1, 1, 0, 0, 0)));
            Assert.IsFalse(item.IsEnabled(new DateTime(2000, 1, 1, 2, 0, 0)));
        }

        [TestMethod]
        public void IsEnabled_Day_Test()
        {
            var cisf = new CronItemSettingFactory();
            var item = cisf.Parse("* * 2 * *");

            Assert.IsTrue(item.IsEnabled(new DateTime(2000, 1, 2, 1, 0, 0)));
            Assert.IsTrue(item.IsEnabled(new DateTime(2000, 1, 2, 1, 1, 0)));
            Assert.IsTrue(item.IsEnabled(new DateTime(2000, 1, 2, 1, 2, 0)));

            Assert.IsTrue(item.IsEnabled(new DateTime(2000, 1, 2, 0, 0, 0)));
            Assert.IsTrue(item.IsEnabled(new DateTime(2000, 1, 2, 1, 0, 0)));
            Assert.IsTrue(item.IsEnabled(new DateTime(2000, 1, 2, 2, 0, 0)));

            Assert.IsTrue(item.IsEnabled(new DateTime(2000, 1, 2, 1, 0, 0)));
            Assert.IsFalse(item.IsEnabled(new DateTime(2000, 1, 1, 1, 0, 0)));
            Assert.IsFalse(item.IsEnabled(new DateTime(2000, 1, 3, 1, 0, 0)));
        }

        [TestMethod]
        public void IsEnabled_Month_Test()
        {
            var cisf = new CronItemSettingFactory();
            var item = cisf.Parse("* * * 2 *");

            Assert.IsTrue(item.IsEnabled(new DateTime(2000, 2, 2, 1, 0, 0)));
            Assert.IsTrue(item.IsEnabled(new DateTime(2000, 2, 2, 1, 1, 0)));
            Assert.IsTrue(item.IsEnabled(new DateTime(2000, 2, 2, 1, 2, 0)));

            Assert.IsTrue(item.IsEnabled(new DateTime(2000, 2, 2, 0, 0, 0)));
            Assert.IsTrue(item.IsEnabled(new DateTime(2000, 2, 2, 1, 0, 0)));
            Assert.IsTrue(item.IsEnabled(new DateTime(2000, 2, 2, 2, 0, 0)));

            Assert.IsTrue(item.IsEnabled(new DateTime(2000, 2, 1, 1, 0, 0)));
            Assert.IsTrue(item.IsEnabled(new DateTime(2000, 2, 2, 1, 0, 0)));
            Assert.IsTrue(item.IsEnabled(new DateTime(2000, 2, 3, 1, 0, 0)));

            Assert.IsTrue(item.IsEnabled(new DateTime(2000, 2, 2, 1, 0, 0)));
            Assert.IsFalse(item.IsEnabled(new DateTime(2000, 1, 1, 1, 0, 0)));
            Assert.IsFalse(item.IsEnabled(new DateTime(2000, 3, 3, 1, 0, 0)));
        }

        [TestMethod]
        public void IsEnabled_Weeks_Single_Test()
        {
            var cisf = new CronItemSettingFactory();
            for(var i = 0; i < 7; i++) {
                var item = cisf.Parse("* * * * " + i.ToString(CultureInfo.InvariantCulture));

                foreach(var weekDay in Enum.GetValues<DayOfWeek>()) {
                    var day = (int)weekDay;
                    var expected = item.IsEnabled(new DateTime(2020, 6, 21 + day, 1, 0, 0));
                    Assert.AreEqual(expected, i == day);
                }
            }
        }

        [TestMethod]
        public void IsEnabled_Weeks_Full_Test()
        {
            var cisf = new CronItemSettingFactory();
            var item = cisf.Parse("* * * * *");

            for(var i = 0; i < 7; i++) {
                Assert.IsTrue(item.IsEnabled(new DateTime(2020, 6, 21 + i, 1, 0, 0)));
            }
        }

        [TestMethod]
        [DataRow(true, "* * * * *", "2020-06-30T00:00")] // この日は火曜日
        [DataRow(false, "* * * * 0", "2020-06-30T00:00")]
        [DataRow(false, "* * * * 1", "2020-06-30T00:00")]
        [DataRow(true, "* * * * 2", "2020-06-30T00:00")]
        [DataRow(false, "* * * * 3", "2020-06-30T00:00")]
        [DataRow(false, "* * * * 4", "2020-06-30T00:00")]
        [DataRow(false, "* * * * 5", "2020-06-30T00:00")]
        [DataRow(false, "* * * * 6", "2020-06-30T00:00")]
        [DataRow(false, "* * * * 1,3,5", "2020-06-30T00:00")]
        [DataRow(true, "* * * * 2,4,6", "2020-06-30T00:00")]
        [DataRow(true, "* * * * 2,4,6", "2020-07-02T00:00")]
        [DataRow(true, "* * * * 2,4,6", "2020-07-04T00:00")]
        [DataRow(false, "1 * * * *", "2020-06-30T00:00")]
        [DataRow(true, "0 * * * *", "2020-06-30T00:00")]
        [DataRow(true, "0 0 * * *", "2020-06-30T00:00")]
        [DataRow(false, "0 1 * * *", "2020-06-30T00:00")]
        [DataRow(false, "1 0 * * *", "2020-06-30T00:00")]
        [DataRow(false, "1 1 * * *", "2020-06-30T00:00")]
        [DataRow(false, "0 0 29 * *", "2020-06-30T00:00")]
        [DataRow(false, "0 1 29 * *", "2020-06-30T00:00")]
        [DataRow(false, "1 0 29 * *", "2020-06-30T00:00")]
        [DataRow(false, "1 1 29 * *", "2020-06-30T00:00")]
        [DataRow(true, "0 0 30 * *", "2020-06-30T00:00")]
        [DataRow(false, "0 1 30 * *", "2020-06-30T00:00")]
        [DataRow(false, "1 0 30 * *", "2020-06-30T00:00")]
        [DataRow(false, "1 1 30 5 *", "2020-06-30T00:00")]
        [DataRow(false, "0 0 29 5 *", "2020-06-30T00:00")]
        [DataRow(false, "0 1 29 5 *", "2020-06-30T00:00")]
        [DataRow(false, "1 0 29 5 *", "2020-06-30T00:00")]
        [DataRow(false, "1 1 29 5 *", "2020-06-30T00:00")]
        [DataRow(false, "0 0 30 5 *", "2020-06-30T00:00")]
        [DataRow(false, "0 1 30 5 *", "2020-06-30T00:00")]
        [DataRow(false, "1 0 30 5 *", "2020-06-30T00:00")]
        [DataRow(false, "1 1 30 5 *", "2020-06-30T00:00")]
        [DataRow(false, "1 1 30 6 *", "2020-06-30T00:00")]
        [DataRow(false, "0 0 29 6 *", "2020-06-30T00:00")]
        [DataRow(false, "0 1 29 6 *", "2020-06-30T00:00")]
        [DataRow(false, "1 0 29 6 *", "2020-06-30T00:00")]
        [DataRow(false, "1 1 29 6 *", "2020-06-30T00:00")]
        [DataRow(true, "0 0 30 6 *", "2020-06-30T00:00")]
        [DataRow(false, "0 1 30 6 *", "2020-06-30T00:00")]
        [DataRow(false, "1 0 30 6 *", "2020-06-30T00:00")]
        [DataRow(false, "1 1 30 6 *", "2020-06-30T00:00")]
        // 13日の金曜日(曜日と日指定のなんだかなぁパターン)
        [DataRow(true, "* * * 11 5", "2020-11-06T00:00")]
        [DataRow(true, "* * * 11 5", "2020-11-13T00:00")]
        [DataRow(true, "* * * 11 5", "2020-11-20T00:00")]
        [DataRow(true, "* * * 11 5", "2020-11-27T00:00")]
        [DataRow(true, "* * 13 11 5", "2020-11-06T00:00")]
        [DataRow(true, "* * 13 11 5", "2020-11-13T00:00")]
        [DataRow(true, "* * 13 11 5", "2020-11-20T00:00")]
        [DataRow(true, "* * 13 11 5", "2020-11-27T00:00")]
        [DataRow(true, "* * 13 * 5", "2020-03-06T00:00")]
        [DataRow(true, "* * 13 * 5", "2020-03-13T00:00")]
        [DataRow(true, "* * 13 * 5", "2020-03-20T00:00")]
        [DataRow(true, "* * 13 * 5", "2020-03-27T00:00")]
        [DataRow(false, "* * 13 * *", "2020-03-06T00:00")]
        [DataRow(true, "* * 13 * *", "2020-03-13T00:00")]
        [DataRow(false, "* * 13 * *", "2020-03-20T00:00")]
        [DataRow(false, "* * 13 * *", "2020-03-27T00:00")]
        public void IsEnabledTest(bool expected, string cronPattern, string inputIso8601)
        {
            var cisf = new CronItemSettingFactory();
            var item = cisf.Parse(cronPattern);
            var actual = item.IsEnabled(DateTime.Parse(inputIso8601, CultureInfo.InvariantCulture));
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }

    [TestClass]
    public class CronItemSettingFactoryTest
    {
        #region function

        [TestMethod]
        [DataRow("")]
        [DataRow("a")]
        [DataRow("a b")]
        [DataRow("a b c")]
        [DataRow("a b c d")]
        [DataRow("a b c d e")]
        [DataRow("0 1")]
        [DataRow("0 1 2 3 4 5")]
        [DataRow("@")]
        [DataRow("@reboot")]
        [DataRow("@yearly")]
        [DataRow("@annually")]
        [DataRow("@monthly")]
        [DataRow("@weekly")]
        public void Parse_Exception_Test(string value)
        {
            var cisf = new CronItemSettingFactory();
            try {
                cisf.Parse(value);
                Assert.Fail();
            } catch(Exception) {
                Assert.IsTrue(true);
            }

        }

        [TestMethod]
        public void Parse_At_Test()
        {
            var cisf = new CronItemSettingFactory();

            var h = cisf.Parse("@hourly");
            Assert.AreEqual(1, h.Minutes.Count);
            Assert.AreEqual(0, h.Hours.Count);
            Assert.AreEqual(0, h.Days.Count);
            Assert.AreEqual(0, h.Months.Count);
            Assert.AreEqual(0, h.DayOfWeeks.Count);

            var d = cisf.Parse("@daily");
            Assert.AreEqual(1, d.Minutes.Count);
            Assert.AreEqual(1, d.Hours.Count);
            Assert.AreEqual(0, d.Days.Count);
            Assert.AreEqual(0, d.Months.Count);
            Assert.AreEqual(0, d.DayOfWeeks.Count);
        }

        [TestMethod]
        public void Parse_Simple_Test()
        {
            var cisf = new CronItemSettingFactory();

            {
                var actual = cisf.Parse("* 0 0 0 0");
                Assert.AreEqual(60, actual.Minutes.Count);
                Assert.AreEqual(1, actual.Hours.Count);
                Assert.AreEqual(1, actual.Days.Count);
                Assert.AreEqual(1, actual.Months.Count);
                Assert.AreEqual(1, actual.DayOfWeeks.Count);
            }

            {
                var actual = cisf.Parse("0 * 0 0 0");
                Assert.AreEqual(1, actual.Minutes.Count);
                Assert.AreEqual(24, actual.Hours.Count);
                Assert.AreEqual(1, actual.Days.Count);
                Assert.AreEqual(1, actual.Months.Count);
                Assert.AreEqual(1, actual.DayOfWeeks.Count);
            }

            {
                var actual = cisf.Parse("0 0 * 0 0");
                Assert.AreEqual(1, actual.Minutes.Count);
                Assert.AreEqual(1, actual.Hours.Count);
                Assert.AreEqual(31, actual.Days.Count);
                Assert.AreEqual(1, actual.Months.Count);
                Assert.AreEqual(1, actual.DayOfWeeks.Count);
            }

            {
                var actual = cisf.Parse("0 0 0 * 0");
                Assert.AreEqual(1, actual.Minutes.Count);
                Assert.AreEqual(1, actual.Hours.Count);
                Assert.AreEqual(1, actual.Days.Count);
                Assert.AreEqual(12, actual.Months.Count);
                Assert.AreEqual(1, actual.DayOfWeeks.Count);
            }

            {
                var actual = cisf.Parse("0 0 0 0 *");
                Assert.AreEqual(1, actual.Minutes.Count);
                Assert.AreEqual(1, actual.Hours.Count);
                Assert.AreEqual(1, actual.Days.Count);
                Assert.AreEqual(1, actual.Months.Count);
                Assert.AreEqual(7, actual.DayOfWeeks.Count);
            }
        }

        [TestMethod]
        [DataRow(new[] { 0, 1 }, "0,1")]
        [DataRow(new[] { 0, 1 }, "0,1,1")]
        [DataRow(new[] { 2, 3, 4, 5 }, "2-5")]
        [DataRow(new[] { 0, 1, 2, 5, 10, 11, 12, 13, 57, 58, 59 }, "0,1,2,10-13,5,57-59")]
        public void Parse_Minutes_Test(int[] expecteds, string targetPattern)
        {
            var cisf = new CronItemSettingFactory();
            var actual = cisf.Parse(targetPattern + " * * * *");
            CollectionAssert.AreEqual(expecteds, actual.Minutes);
            Assert.AreEqual(24, actual.Hours.Count);
            Assert.AreEqual(31, actual.Days.Count);
            Assert.AreEqual(12, actual.Months.Count);
            Assert.AreEqual(7, actual.DayOfWeeks.Count);
        }

        #endregion
    }

    [TestClass]
    public class CronSchedulerTest
    {
        #region function

        [TestMethod]
        [DataRow(60000, "2020-06-28T20:42:00.000")]
        [DataRow(1000, "2020-06-28T20:42:59.000")]
        [DataRow(29999, "2020-06-28T20:42:30.001")]
        [DataRow(30000, "2020-06-28T20:42:30.000")]
        [DataRow(30001, "2020-06-28T20:42:29.999")]
        [DataRow(1000, "2020-06-28T20:59:59.000")]
        [DataRow(1000, "2020-06-30T23:59:59.000")]
        [DataRow(1000, "2020-12-31T23:59:59.000")]
        [DataRow(500, "2020-06-28T20:42:59.500")]
        [DataRow(1, "2020-06-28T20:42:59.999")]
        public void GetNextJobWaitTimeTest(double expected, string iso8601)
        {
            var input = DateTime.Parse(iso8601, CultureInfo.InvariantCulture);
            var cs = new CronScheduler(new IdFactory(Test.LoggerFactory),Test.LoggerFactory);
            var actual = cs.GetNextJobWaitTime(input);
            Assert.AreEqual(expected, actual.TotalMilliseconds);
        }

        //[TestMethod]
        //public void GetItemFromTimeTest()
        //{
        //    var cs = new CronScheduler(Timeout.InfiniteTimeSpan, TimeSpan.FromSeconds(1));
        //}

        #endregion
    }

}
