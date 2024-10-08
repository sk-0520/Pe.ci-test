using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Base;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Base.Test
{
    public class NumericRangeTest
    {
        #region function

        [Fact]
        public void ConstructorTest()
        {
            Assert.Throws<ArgumentException>(() => new NumericRange(true, "", ""));
            Assert.Throws<ArgumentException>(() => new NumericRange(true, "a", "a"));
            Assert.Throws<ArgumentNullException>(() => new NumericRange(true, null!, ""));
            Assert.Throws<ArgumentNullException>(() => new NumericRange(true, "", null!));
        }

        [Theory]
        [InlineData("", new int[0])]
        [InlineData("1", new[] { 1 })]
        [InlineData("1, 3", new[] { 1, 3 })]
        [InlineData("1, 3, 5", new[] { 1, 3, 3, 5 })]
        [InlineData("1, 3-5", new[] { 1, 3, 4, 5 })]
        [InlineData("1, 3-5, 7", new[] { 1, 3, 4, 5, 7 })]
        [InlineData("1, 3-5, 7, 9-12", new[] { 1, 3, 4, 5, 7, 9, 10, 11, 12 })]
        [InlineData("1-2", new[] { 1, 2 })]
        [InlineData("1-2, 5, 9-10", new[] { 1, 2, 5, 9, 10 })]
        [InlineData("1-2, 5, 9-10", new[] { 1, 5, 2, 10, 5, 1, 9, 10 })]
        [InlineData("1-2, 5, 9-10, 99", new[] { 1, 5, 2, 99, 5, 1, 9, 10 })]
        [InlineData("-3, -1", new[] { -1, -3 })]
        [InlineData("-3--1, 1-3", new[] { -3, -2, -1, 1, 2, 3 })]
        [InlineData("-3-3", new[] { -3, -2, -1, 0, 1, 2, 3 })]
        public void ToStringTest(string expected, int[] values)
        {
            var nr = new NumericRange();
            var actual = nr.ToString(values);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", new int[0])]
        [InlineData("1", new[] { 1 })]
        [InlineData("1,3", new[] { 1, 3 })]
        [InlineData("1,3,5", new[] { 1, 3, 3, 5 })]
        [InlineData("1,3-5", new[] { 1, 3, 4, 5 })]
        [InlineData("1,3-5,7", new[] { 1, 3, 4, 5, 7 })]
        [InlineData("1,3-5,7,9-12", new[] { 1, 3, 4, 5, 7, 9, 10, 11, 12 })]
        public void ToString_NoSpace_Test(string expected, int[] values)
        {
            var nr = new NumericRange(false, ",", "-");
            var actual = nr.ToString(values);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", new int[0])]
        [InlineData("1", new[] { 1 })]
        [InlineData("1\t3", new[] { 1, 3 })]
        [InlineData("1\t3\t5", new[] { 1, 3, 3, 5 })]
        [InlineData("1\t3-5", new[] { 1, 3, 4, 5 })]
        [InlineData("1\t3-5\t7", new[] { 1, 3, 4, 5, 7 })]
        [InlineData("1\t3-5\t7\t9-12", new[] { 1, 3, 4, 5, 7, 9, 10, 11, 12 })]
        public void ToString_Value_Test(string expected, int[] values)
        {
            var nr = new NumericRange(false, "\t", "-");
            var actual = nr.ToString(values);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", new int[0])]
        [InlineData("1", new[] { 1 })]
        [InlineData("1,3", new[] { 1, 3 })]
        [InlineData("1,3,5", new[] { 1, 3, 3, 5 })]
        [InlineData("1,3/5", new[] { 1, 3, 4, 5 })]
        [InlineData("1,3/5,7", new[] { 1, 3, 4, 5, 7 })]
        [InlineData("1,3/5,7,9/12", new[] { 1, 3, 4, 5, 7, 9, 10, 11, 12 })]
        public void ToString_Range_Test(string expected, int[] values)
        {
            var nr = new NumericRange(false, ",", "/");
            var actual = nr.ToString(values);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(new int[0], "")]
        [InlineData(new int[0], " ")]
        [InlineData(new[] { 1, 2 }, "1, 2")]
        [InlineData(new[] { -1, 1, 2, 4 }, "1, 2, 4, -1")]
        [InlineData(new[] { -3, -2, -1 }, "-3--1")]
        [InlineData(new[] { 1, 2, 3, 4, 5 }, "1-5")]
        [InlineData(new[] { -5, -4, -3, 0, 2, 3, 4, 5 }, "-5--3, 0, 2-5")]
        [InlineData(new[] { 1, 2, 3, 4, 5 }, "+1-+5")]
        public void ParseTest(int[] expected, string values)
        {
            var nr = new NumericRange();
            var actual = nr.Parse(values).ToList();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RandomTest()
        {
            var rnd = new Random();
            foreach(var _ in Enumerable.Range(1, 1000)) {
                var length = rnd.Next(1, 1000);
                var values1 = Enumerable.Repeat(0, length).Select(_ => rnd.Next()).ToList();
                var nr = new NumericRange();
                var s1 = nr.ToString(values1);
                var values2 = nr.Parse(s1);
                var s2 = nr.ToString(values2);
                Assert.Equal(s1, s2);
            }
        }

        #endregion
    }
}
