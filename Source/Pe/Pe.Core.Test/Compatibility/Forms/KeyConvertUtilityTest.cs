using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Xunit;
using WinForms = System.Windows.Forms;

namespace ContentTypeTextNet.Pe.Core.Test.Compatibility.Forms
{
    public class KeyConvertUtilityTest
    {
        #region function

        [Theory]
        [InlineData(Key.A, WinForms.Keys.A)]
        [InlineData(Key.Z, WinForms.Keys.Z)]
        [InlineData(Key.D0, WinForms.Keys.D0)]
        [InlineData(Key.NumPad9, WinForms.Keys.NumPad9)]
        public void ConvertKeyTest(Key expected, WinForms.Keys key)
        {
            var actual = KeyConvertUtility.ConvertKey(key);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(ModifierKeys.None, MOD.None)]
        [InlineData(ModifierKeys.Alt, MOD.MOD_ALT)]
        [InlineData(ModifierKeys.Control, MOD.MOD_CONTROL)]
        [InlineData(ModifierKeys.Shift, MOD.MOD_SHIFT)]
        [InlineData(ModifierKeys.Windows, MOD.MOD_WIN)]
        [InlineData(ModifierKeys.None, MOD.MOD_NOREPEAT)]
        [InlineData(ModifierKeys.Alt | ModifierKeys.Control, MOD.MOD_ALT | MOD.MOD_CONTROL)]
        public void ConvertModifierKeysTest(ModifierKeys expected, MOD mod)
        {
            var actual = KeyConvertUtility.ConvertModifierKeys(mod);
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
