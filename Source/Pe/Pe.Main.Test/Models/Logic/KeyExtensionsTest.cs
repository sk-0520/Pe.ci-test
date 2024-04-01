using System.Windows.Input;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Logic
{
    public class KeyExtensionsTest
    {
        #region function

        [Theory]
        [InlineData(false, Key.None)]
        [InlineData(false, Key.A)]
        [InlineData(true, Key.LeftShift)]
        [InlineData(true, Key.RightShift)]
        [InlineData(true, Key.LeftCtrl)]
        [InlineData(true, Key.RightCtrl)]
        [InlineData(true, Key.LeftAlt)]
        [InlineData(true, Key.RightAlt)]
        [InlineData(true, Key.LWin)]
        [InlineData(true, Key.RWin)]
        public void IsModifierKeyTest(bool expected, Key inputKey)
        {
            var actual = inputKey.IsModifierKey();
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
