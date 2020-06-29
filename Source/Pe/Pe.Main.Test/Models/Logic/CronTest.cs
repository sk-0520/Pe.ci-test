using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Logic
{
    [TestClass]
    public class IReadOnlyCronItemSettingExtensionsTest
    {
        #region function

        public void IsLiveTest()
        {
            // テストしたい思いと🍶で死んでる脳。心は何処
            var cisf = new CronItemSettingFactory();
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
                var full = cisf.Parse("* 0 0 0 0");
                Assert.AreEqual(60, full.Minutes.Count);
                Assert.AreEqual(1, full.Hours.Count);
                Assert.AreEqual(1, full.Days.Count);
                Assert.AreEqual(1, full.Months.Count);
                Assert.AreEqual(1, full.DayOfWeeks.Count);
            }

            {
                var full = cisf.Parse("0 * 0 0 0");
                Assert.AreEqual(1, full.Minutes.Count);
                Assert.AreEqual(24, full.Hours.Count);
                Assert.AreEqual(1, full.Days.Count);
                Assert.AreEqual(1, full.Months.Count);
                Assert.AreEqual(1, full.DayOfWeeks.Count);
            }

            {
                var full = cisf.Parse("0 0 * 0 0");
                Assert.AreEqual(1, full.Minutes.Count);
                Assert.AreEqual(1, full.Hours.Count);
                Assert.AreEqual(31, full.Days.Count);
                Assert.AreEqual(1, full.Months.Count);
                Assert.AreEqual(1, full.DayOfWeeks.Count);
            }

            {
                var full = cisf.Parse("0 0 0 * 0");
                Assert.AreEqual(1, full.Minutes.Count);
                Assert.AreEqual(1, full.Hours.Count);
                Assert.AreEqual(1, full.Days.Count);
                Assert.AreEqual(12, full.Months.Count);
                Assert.AreEqual(1, full.DayOfWeeks.Count);
            }

            {
                var full = cisf.Parse("0 0 0 0 *");
                Assert.AreEqual(1, full.Minutes.Count);
                Assert.AreEqual(1, full.Hours.Count);
                Assert.AreEqual(1, full.Days.Count);
                Assert.AreEqual(1, full.Months.Count);
                Assert.AreEqual(7, full.DayOfWeeks.Count);
            }
        }

        #endregion
    }

    [TestClass]
    public class CronSchedulerTest
    {
        #region function

        [TestMethod]
        [DataRow(60000, "2020-06-28T20:42:00.000")]
        [DataRow( 1000, "2020-06-28T20:42:59.000")]
        [DataRow(29999, "2020-06-28T20:42:30.001")]
        [DataRow(30000, "2020-06-28T20:42:30.000")]
        [DataRow(30001, "2020-06-28T20:42:29.999")]
        [DataRow( 1000, "2020-06-28T20:59:59.000")]
        [DataRow( 1000, "2020-06-30T23:59:59.000")]
        [DataRow( 1000, "2020-12-31T23:59:59.000")]
        [DataRow(  500, "2020-06-28T20:42:59.500")]
        [DataRow(    1, "2020-06-28T20:42:59.999")]
        public void GetNextJobWaitTimeTest(double result, string iso8601)
        {
            var input = DateTime.Parse(iso8601);
            var cs = new CronScheduler(Test.LoggerFactory);
            var actual = cs.GetNextJobWaitTime(input);
            Assert.AreEqual(result, actual.TotalMilliseconds);
        }

        //[TestMethod]
        //public void GetItemFromTimeTest()
        //{
        //    var cs = new CronScheduler(Timeout.InfiniteTimeSpan, TimeSpan.FromSeconds(1));
        //}

        #endregion
    }

}
