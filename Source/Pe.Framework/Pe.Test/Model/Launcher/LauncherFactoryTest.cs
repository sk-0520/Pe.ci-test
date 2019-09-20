using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pe.Test.Model.Launcher
{
    [TestClass]
    public class LauncherFactoryTest
    {
        [DataRow("", "")]
        [DataRow("a", "A")]
        [DataRow("a", "あ")]
        [DataRow("kanji", "漢字")]
        [TestMethod]
        public void ToCodeTest(string test, string input)
        {
        }
    }
}
