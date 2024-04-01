using System.Windows.Media;
using ContentTypeTextNet.Pe.Core.Models;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    public class MediaUtilityTest
    {
        [Theory]
        [InlineData(0x00, 0x00, 0x00, 0x00, 0x00, 0xff, 0xff, 0xff)]
        [InlineData(0x00, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00)]
        [InlineData(0x00, 0x10, 0xff, 0xff, 0x00, 0xef, 0x00, 0x00)]
        public void GetNegativeColorTest(int expectedA, int expectedR, int expectedG, int expectedB, int argA, int argR, int argG, int argB)
        {
            var expected = Color.FromArgb((byte)expectedA, (byte)expectedR, (byte)expectedG, (byte)expectedB);
            var arg = Color.FromArgb((byte)argA, (byte)argR, (byte)argG, (byte)argB);

            var result = MediaUtility.GetNegativeColor(arg);

            Assert.Equal(expected, result);
        }
    }
}
