using System;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Launcher
{
    public class LauncherFactoryTest
    {
        #region function

        [Fact]
        public void ToCode_Null_Test()
        {
            var lf = new LauncherFactory(new IdFactory(Test.LoggerFactory), Test.LoggerFactory);
            Assert.Throws<ArgumentNullException>(() => lf.ToCode(null!));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("a", "a")]
        [InlineData("a", " a")]
        [InlineData("a", "a ")]
        [InlineData("a", " a ")]
        [InlineData("a", "ａ")]
        [InlineData("0", "０")]
        [InlineData("a_a", "a a")]
        [InlineData("a_a", "a\ra")]
        [InlineData("a.a", "a.a")]
        [InlineData("a_a", "a,a")]
        [InlineData("a[c-7f]a", "a\u007fa")]
        [InlineData("a", "ア")] // 全角から平仮名になる
        [InlineData("a", "ｱ")] // 半角から全角になって平仮名になる
        public void ToCodeTest(string expected, string s)
        {
            var lf = new LauncherFactory(new IdFactory(Test.LoggerFactory), Test.LoggerFactory);
            var actual = lf.ToCode(s);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
