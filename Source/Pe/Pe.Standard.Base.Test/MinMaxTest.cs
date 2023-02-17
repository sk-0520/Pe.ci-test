using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Standard.Base.Test
{
    [TestClass]
    public class MinMaxTest
    {
        [TestMethod]
        [DataRow(false, 4, 5, 10)]
        [DataRow(true, 5, 5, 10)]
        [DataRow(true, 8, 5, 10)]
        [DataRow(true, 10, 5, 10)]
        [DataRow(false, 11, 5, 10)]
        public void CompareTest(bool expected, int value, int head, int tail)
        {
            var actual = MinMax.Create(head, tail);
            Assert.AreEqual(expected, actual.IsIn(value));
        }

        [TestMethod]
        [DataRow(1, 2, "1,2")]
        [DataRow(-1, -2, "-1,-2")]
        [DataRow(1, 2, " 1 , 2 ")]
        public void ParseTest(int expectedMin, int expectedMax, string s)
        {
            var actual = MinMax.Parse<int>(s);
            Assert.AreEqual(expectedMin, actual.Minimum);
            Assert.AreEqual(expectedMax, actual.Maximum);
        }
    }
}
