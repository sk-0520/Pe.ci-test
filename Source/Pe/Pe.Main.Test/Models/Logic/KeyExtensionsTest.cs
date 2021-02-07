using System.Windows.Input;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Logic
{
    [TestClass]
    public class KeyExtensionsTest
    {
        #region function

        [TestMethod]
        [DataRow(false, Key.None)]
        [DataRow(false, Key.A)]
        [DataRow(true, Key.LeftShift)]
        [DataRow(true, Key.RightShift)]
        [DataRow(true, Key.LeftCtrl)]
        [DataRow(true, Key.RightCtrl)]
        [DataRow(true, Key.LeftAlt)]
        [DataRow(true, Key.RightAlt)]
        [DataRow(true, Key.LWin)]
        [DataRow(true, Key.RWin)]
        public void IsModifierKeyTest(bool expected, Key inputKey)
        {
            var actual = inputKey.IsModifierKey();
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
