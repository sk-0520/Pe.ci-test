using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    [TestClass]
    public class NumericRangeTest
    {
        #region function

        [TestMethod]
        public void ConstructorTest()
        {
            Assert.ThrowsException<ArgumentException>(() => new NumericRange(true, "", ""));
            Assert.ThrowsException<ArgumentException>(() => new NumericRange(true, "a", "a"));
            Assert.ThrowsException<ArgumentNullException>(() => new NumericRange(true, null!, ""));
            Assert.ThrowsException<ArgumentNullException>(() => new NumericRange(true, "", null!));
        }

        [TestMethod]
        [DataRow("", new int[0])]
        [DataRow("1", new[] { 1 })]
        [DataRow("1, 3", new[] { 1, 3 })]
        [DataRow("1, 3, 5", new[] { 1, 3, 3, 5 })]
        [DataRow("1, 3-5", new[] { 1, 3, 4, 5 })]
        [DataRow("1, 3-5, 7", new[] { 1, 3, 4, 5, 7 })]
        [DataRow("1, 3-5, 7, 9-12", new[] { 1, 3, 4, 5, 7, 9, 10, 11, 12 })]
        [DataRow("1-2", new[] { 1, 2 })]
        [DataRow("1-2, 5, 9-10", new[] { 1, 2, 5, 9, 10 })]
        [DataRow("1-2, 5, 9-10", new[] { 1, 5, 2, 10, 5, 1, 9, 10 })]
        [DataRow("1-2, 5, 9-10, 99", new[] { 1, 5, 2, 99, 5, 1, 9, 10 })]
        [DataRow("-3, -1", new[] { -1, -3 })]
        [DataRow("-3--1, 1-3", new[] { -3, -2, -1, 1, 2, 3 })]
        [DataRow("-3-3", new[] { -3, -2, -1, 0, 1, 2, 3 })]
        public void ToStringTest(string result, int[] values)
        {
            var nr = new NumericRange();
            var actual = nr.ToString(values);
            Assert.AreEqual(result, actual);
        }

        [TestMethod]
        [DataRow("", new int[0])]
        [DataRow("1", new[] { 1 })]
        [DataRow("1,3", new[] { 1, 3 })]
        [DataRow("1,3,5", new[] { 1, 3, 3, 5 })]
        [DataRow("1,3-5", new[] { 1, 3, 4, 5 })]
        [DataRow("1,3-5,7", new[] { 1, 3, 4, 5, 7 })]
        [DataRow("1,3-5,7,9-12", new[] { 1, 3, 4, 5, 7, 9, 10, 11, 12 })]
        public void ToString_NoSpace_Test(string result, int[] values)
        {
            var nr = new NumericRange(false, ",", "-");
            var actual = nr.ToString(values);
            Assert.AreEqual(result, actual);
        }

        [TestMethod]
        [DataRow("", new int[0])]
        [DataRow("1", new[] { 1 })]
        [DataRow("1\t3", new[] { 1, 3 })]
        [DataRow("1\t3\t5", new[] { 1, 3, 3, 5 })]
        [DataRow("1\t3-5", new[] { 1, 3, 4, 5 })]
        [DataRow("1\t3-5\t7", new[] { 1, 3, 4, 5, 7 })]
        [DataRow("1\t3-5\t7\t9-12", new[] { 1, 3, 4, 5, 7, 9, 10, 11, 12 })]
        public void ToString_Value_Test(string result, int[] values)
        {
            var nr = new NumericRange(false, "\t", "-");
            var actual = nr.ToString(values);
            Assert.AreEqual(result, actual);
        }

        [TestMethod]
        [DataRow("", new int[0])]
        [DataRow("1", new[] { 1 })]
        [DataRow("1,3", new[] { 1, 3 })]
        [DataRow("1,3,5", new[] { 1, 3, 3, 5 })]
        [DataRow("1,3/5", new[] { 1, 3, 4, 5 })]
        [DataRow("1,3/5,7", new[] { 1, 3, 4, 5, 7 })]
        [DataRow("1,3/5,7,9/12", new[] { 1, 3, 4, 5, 7, 9, 10, 11, 12 })]
        public void ToString_Range_Test(string result, int[] values)
        {
            var nr = new NumericRange(false, ",", "/");
            var actual = nr.ToString(values);
            Assert.AreEqual(result, actual);
        }

        [TestMethod]
        [DataRow(new int[0], "")]
        [DataRow(new int[0], " ")]
        [DataRow(new[] { 1, 2 }, "1, 2")]
        [DataRow(new[] { -1, 1, 2, 4 }, "1, 2, 4, -1")]
        [DataRow(new[] { -3, -2, -1 }, "-3--1")]
        [DataRow(new[] { 1, 2, 3, 4, 5 }, "1-5")]
        [DataRow(new[] { -5, -4, -3, 0, 2, 3, 4, 5 }, "-5--3, 0, 2-5")]
        [DataRow(new[] { 1, 2, 3, 4, 5 }, "+1-+5")]
        public void ParseTest(int[] result, string values)
        {
            var nr = new NumericRange();
            var actual = nr.Parse(values).ToList();
            CollectionAssert.AreEqual(result, actual);
        }

        #endregion
    }
}
