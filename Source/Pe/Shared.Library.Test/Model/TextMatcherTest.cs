using System;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shared.Library.Test.Model
{
    [TestClass]
    public class TextMatcherTestTest
    {
        [TestMethod]
        public void ConvertHiraganaToKatakaTest_Test()
        {
            var textMatcher = new TextMatcher();
            Assert.ThrowsException<ArgumentNullException>(() => textMatcher.ConvertHiraganaToKataka(null));
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow("a", "a")]
        [DataRow("ア", "あ")]
        [DataRow("アア亜", "あア亜")]
        [DataRow("アイウエオ", "あいうえお")]
        [DataRow("カキクケコ", "かきくけこ")]
        [DataRow("サシスセソ", "さしすせそ")]
        [DataRow("タチツテト", "たちつてと")]
        [DataRow("ナニヌネノ", "なにぬねの")]
        [DataRow("ハヒフヘホ", "はひふへほ")]
        [DataRow("マミムメモ", "まみむめも")]
        [DataRow("ヤユヨ", "やゆよ")]
        [DataRow("ラリルレロ", "らりるれろ")]
        public void ConvertHiraganaToKatakaTest_Normal(string test, string input)
        {
            var textMatcher = new TextMatcher();
            var result = textMatcher.ConvertHiraganaToKataka(input);
            Assert.AreEqual(test, result);
        }


        [TestMethod]
        public void ConvertKatakaToHiragana_Test_Null()
        {
            var textMatcher = new TextMatcher();
            Assert.ThrowsException<ArgumentNullException>(() => textMatcher.ConvertKatakaToHiragana(null));
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow("a", "a")]
        [DataRow("あ", "ア")]
        [DataRow("ああ亜", "あア亜")]
        [DataRow("あいうえお", "アイウエオ")]
        [DataRow("かきくけこ", "カキクケコ")]
        [DataRow("さしすせそ", "サシスセソ")]
        [DataRow("たちつてと", "タチツテト")]
        [DataRow("なにぬねの", "ナニヌネノ")]
        [DataRow("はひふへほ", "ハヒフヘホ")]
        [DataRow("まみむめも", "マミムメモ")]
        [DataRow("やゆよ", "ヤユヨ")]
        [DataRow("らりるれろ", "ラリルレロ")]
        public void ConvertKatakaToHiragana_Normal(string test, string input)
        {
            var textMatcher = new TextMatcher();
            var result = textMatcher.ConvertKatakaToHiragana(input);
            Assert.AreEqual(test, result);
        }

    }
}
