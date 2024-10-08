using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Base.Test
{
    public class MinMaxTest
    {
        [Theory]
        [InlineData(false, 4, 5, 10)]
        [InlineData(true, 5, 5, 10)]
        [InlineData(true, 8, 5, 10)]
        [InlineData(true, 10, 5, 10)]
        [InlineData(false, 11, 5, 10)]
        public void CompareTest(bool expected, int value, int head, int tail)
        {
            var actual = MinMax.Create(head, tail);
            Assert.Equal(expected, actual.IsIn(value));
        }

        [Theory]
        [InlineData(1, 2, "1,2")]
        [InlineData(-1, -2, "-1,-2")]
        [InlineData(1, 2, " 1 , 2 ")]
        public void ParseTest(int expectedMin, int expectedMax, string s)
        {
            var actual = MinMax.Parse<int>(s);
            Assert.Equal(expectedMin, actual.Minimum);
            Assert.Equal(expectedMax, actual.Maximum);
        }

        [Theory]
        [InlineData("")]
        [InlineData("0")]
        [InlineData("0,1,2")]
        public void Parse_throw_Test(string s)
        {
            Assert.Throws<ArgumentException>(() => MinMax.Parse<int>(s));
        }

        [Theory]
        [InlineData(1, 2, "1,2")]
        [InlineData(-1, -2, "-1,-2")]
        [InlineData(1, 2, " 1 , 2 ")]
        public void TryParse_success_Test(int expectedMin, int expectedMax, string s)
        {
            var result = MinMax.TryParse<int>(s, out var actual);
            Assert.True(result);
            Assert.Equal(expectedMin, actual.Minimum);
            Assert.Equal(expectedMax, actual.Maximum);
        }

        [Theory]
        [InlineData("")]
        [InlineData("0")]
        [InlineData("0,1,2")]
        public void TryParse_failure_Test(string s)
        {
            var result = MinMax.TryParse<int>(s, out var actual);
            Assert.False(result);
            Assert.Equal(0, actual.Minimum);
            Assert.Equal(0, actual.Maximum);
        }
    }

    public class MinMaxDefaultTest
    {
        #region function

        [Theory]
        [InlineData(false, 5, 10, 4)]
        [InlineData(true, 5, 10, 5)]
        [InlineData(true, 5, 10, 8)]
        [InlineData(true, 5, 10, 10)]
        [InlineData(false, 5, 10, 11)]
        public void CompareTest(bool expected, int minimum, int maximum, int defaultValue)
        {
            var actual = MinMaxDefault.Create(minimum, maximum, defaultValue);
            Assert.Equal(expected, actual.IsIn(actual.Default));
        }

        [Theory]
        [InlineData(10, 20, 15, "10,20,15")]
        [InlineData(-10, -20, -15, "-10,-20,-15")]
        [InlineData(10, 20, 15, " 10 , 20 , 15 ")]
        public void ParseTest(int expectedMin, int expectedMax, int expectedDefault, string s)
        {
            var actual = MinMaxDefault.Parse<int>(s);
            Assert.Equal(expectedMin, actual.Minimum);
            Assert.Equal(expectedMax, actual.Maximum);
            Assert.Equal(expectedDefault, actual.Default);
        }

        [Theory]
        [InlineData("")]
        [InlineData("0")]
        [InlineData("0,9")]
        [InlineData("0,9,5,0")]
        public void Parse_throw_Test(string s)
        {
            Assert.Throws<ArgumentException>(() => MinMaxDefault.Parse<int>(s));
        }

        [Theory]
        [InlineData(10, 20, 15, "10,20,15")]
        [InlineData(-10, -20, -15, "-10,-20,-15")]
        [InlineData(10, 20, 15, " 10 , 20 , 15 ")]
        public void TryParse_success_Test(int expectedMin, int expectedMax, int expectedDefault, string s)
        {
            var result = MinMaxDefault.TryParse<int>(s, out var actual);
            Assert.True(result);
            Assert.Equal(expectedMin, actual.Minimum);
            Assert.Equal(expectedMax, actual.Maximum);
            Assert.Equal(expectedDefault, actual.Default);
        }

        [Theory]
        [InlineData("")]
        [InlineData("0")]
        [InlineData("0,9")]
        [InlineData("0,9,5,0")]
        public void TryParse_failure_Test(string s)
        {
            var result = MinMaxDefault.TryParse<int>(s, out var actual);
            Assert.False(result);
            Assert.Equal(0, actual.Minimum);
            Assert.Equal(0, actual.Maximum);
            Assert.Equal(0, actual.Default);
        }

        #endregion
    }
}
