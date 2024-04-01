using System;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Logic
{
    public class VersionConverterTest
    {
        #region function

        [Theory]
        [InlineData("0.00.000", ".", 0, 0, 0, 0)]
        [InlineData("0-00-000", "-", 0, 0, 0, 0)]
        [InlineData("000000", "", 0, 0, 0, 0)]
        [InlineData("0.00.000", ".", 0, 0, 0, 9)]
        [InlineData("1.00.000", ".", 1, 0, 0, 0)]
        [InlineData("1.02.000", ".", 1, 2, 0, 0)]
        [InlineData("1.02.003", ".", 1, 2, 3, 0)]
        [InlineData("11.02.003", ".", 11, 2, 3, 0)]
        [InlineData("11.222.003", ".", 11, 222, 3, 0)]
        [InlineData("11.222.3333", ".", 11, 222, 3333, 0)]
        public void ConvertDisplayVersionTest(string expected, string separator, int major, int minor, int build, int revisio)
        {
            var version = new Version(major, minor, build, revisio);
            var vc = new VersionConverter();
            var actual = vc.ConvertDisplayVersion(version, separator);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("A_1-02-003_B.C", "A", "B", "C")]
        [InlineData("A_1-02-003.C", "A", "", "C")]
        [InlineData("A_1-02-003", "A", "", "")]
        [InlineData("A_1-02-003_B", "A", "B", "")]
        public void ConvertFileNameTest(string expected, string head, string tail, string extension)
        {
            var vc = new VersionConverter();
            var actual = vc.ConvertFileName(head, new Version(1, 2, 3, 4), tail, extension);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertFileNameTest_HeadEmpty()
        {
            var vc = new VersionConverter();
            Assert.Throws<ArgumentException>(() => vc.ConvertFileName(string.Empty, new Version(1, 2, 3, 4), string.Empty, string.Empty));
        }

        #endregion
    }
}
