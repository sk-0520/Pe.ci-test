using System.Windows.Media;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Base;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    public class MediaUtilityTest
    {
        #region function

        [Theory]
        [InlineData(0xff000000, 0xff, 0x00, 0x00, 0x00)]
        [InlineData(0x00ff0000, 0x00, 0xff, 0x00, 0x00)]
        [InlineData(0x0000ff00, 0x00, 0x00, 0xff, 0x00)]
        [InlineData(0x000000ff, 0x00, 0x00, 0x00, 0xff)]
        public void ConvertRawColorFromColorTest(uint expected, byte a, byte r, byte g, byte b)
        {
            var color = Color.FromArgb(a, r, g, b);
            var actual = MediaUtility.ConvertRawColorFromColor(color);

            Assert.Equal(expected, actual);
            Assert.Equal(color.A, a);
            Assert.Equal(color.R, r);
            Assert.Equal(color.G, g);
            Assert.Equal(color.B, b);
        }

        [Theory]
        [InlineData(0xff, 0x00, 0x00, 0x00, 0xff000000)]
        [InlineData(0x00, 0xff, 0x00, 0x00, 0x00ff0000)]
        [InlineData(0x00, 0x00, 0xff, 0x00, 0x0000ff00)]
        [InlineData(0x00, 0x00, 0x00, 0xff, 0x000000ff)]
        public void ConvertColorFromRawColorTest(byte expectedA, byte expectedR, byte expectedG, byte expectedB, uint rawColor)
        {
            var actual = MediaUtility.ConvertColorFromRawColor(rawColor);

            Assert.Equal(expectedR, actual.R);
            Assert.Equal(expectedG, actual.G);
            Assert.Equal(expectedB, actual.B);
            Assert.Equal(expectedA, actual.A);
        }

        [Theory]
        [InlineData(
            0x00, 0x00, 0x00, 0x00,
            0x00, 0xff, 0xff, 0xff
        )]
        [InlineData(
            0x00, 0xff, 0xff, 0xff,
            0x00, 0x00, 0x00, 0x00
        )]
        [InlineData(
            0x00, 0x10, 0xff, 0xff,
            0x00, 0xef, 0x00, 0x00
        )]
        public void GetNegativeColorTest(byte expectedA, byte expectedR, byte expectedG, byte expectedB, byte a, byte r, byte g, byte b)
        {
            var expected = Color.FromArgb(expectedA, expectedR, expectedG, expectedB);
            var color = Color.FromArgb(a, r, g, b);

            var actual = MediaUtility.GetNegativeColor(color);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(
            MediaUtility.Opaque, 0xff, 0xff, 0xff,
            MediaUtility.Opaque, 0x00, 0x00, 0x00
        )]
        [InlineData(
            MediaUtility.Opaque, 0x00, 0x00, 0x00,
            MediaUtility.Opaque, 0xff, 0xff, 0xff
        )]
        public void GetAutoColorTest(byte expectedA, byte expectedR, byte expectedG, byte expectedB, byte a, byte r, byte g, byte b)
        {
            var expected = Color.FromArgb(expectedA, expectedR, expectedG, expectedB);
            var color = Color.FromArgb(a, r, g, b);

            var actual = MediaUtility.GetAutoColor(color);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(MediaUtility.Opaque, 0xff, 0xff, 0xff)]
        [InlineData(0xE7, 0xff, 0xff, 0xff)]
        [InlineData(0x00, 0xff, 0xff, 0xff)]
        public void GetOpaqueColorTest(byte a, byte r, byte g, byte b)
        {
            var color = Color.FromArgb(a, r, g, b);

            var actual = MediaUtility.GetOpaqueColor(color);

            Assert.Equal(r, actual.R);
            Assert.Equal(g, actual.G);
            Assert.Equal(b, actual.B);
            Assert.Equal(MediaUtility.Opaque, actual.A);
        }


        #endregion
    }
}
