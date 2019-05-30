using System;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shared.Library.Test.Model
{
    [TestClass]
    public class TextConverterTest
    {
        [TestMethod]
        public void ConvertHiraganaToKatakaTest_Test()
        {
            var textMatcher = new TextConverter();
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
        [DataRow("ワヲン", "わをん")]
        [DataRow("ァィゥェォ", "ぁぃぅぇぉ")]
        [DataRow("ャュョ", "ゃゅょ")]
        [DataRow("ヵヶ", "ゕゖ")]
        public void ConvertHiraganaToKatakaTest_Normal(string test, string input)
        {
            var textMatcher = new TextConverter();
            var result = textMatcher.ConvertHiraganaToKataka(input);
            Assert.AreEqual(test, result);
        }


        [TestMethod]
        public void ConvertKatakaToHiraganaTest_Null()
        {
            var textMatcher = new TextConverter();
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
        [DataRow("わをん", "ワヲン")]
        [DataRow("ぁぃぅぇぉ", "ァィゥェォ")]
        [DataRow("ゃゅょ", "ャュョ")]
        [DataRow("ゕゖ", "ヵヶ")]
        public void ConvertKatakaToHiraganaTest_Normal(string test, string input)
        {
            var textMatcher = new TextConverter();
            var result = textMatcher.ConvertKatakaToHiragana(input);
            Assert.AreEqual(test, result);
        }

        [TestMethod]
        public void ConvertHankakuKatakanaToZenkakuKatakanaTest_Null()
        {
            var textMatcher = new TextConverter();
            Assert.ThrowsException<ArgumentNullException>(() => textMatcher.ConvertHankakuKatakanaToZenkakuKatakana(null));
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow("a", "a")]
        [DataRow("あ", "あ")]
        [DataRow("ア", "ア")]
        [DataRow("アイウエオ", "ｱｲｳｴｵ")]
        [DataRow("カキクケコ", "ｶｷｸｹｺ")]
        [DataRow("サシスセソ", "ｻｼｽｾｿ")]
        [DataRow("タチツテト", "ﾀﾁﾂﾃﾄ")]
        [DataRow("ナニヌネノ", "ﾅﾆﾇﾈﾉ")]
        [DataRow("ハヒフヘホ", "ﾊﾋﾌﾍﾎ")]
        [DataRow("マミムメモ", "ﾏﾐﾑﾒﾓ")]
        [DataRow("ヤユヨ", "ﾔﾕﾖ")]
        [DataRow("ラリルレロ", "ﾗﾘﾙﾚﾛ")]
        [DataRow("ワヲン", "ﾜｦﾝ")]
        [DataRow("ァィゥェォ", "ｧｨｩｪｫ")]
        [DataRow("ャュョ", "ｬｭｮ")]
        [DataRow("ガギグゲゴ", "ｶﾞｷﾞｸﾞｹﾞｺﾞ")]
        [DataRow("ザジズゼゾ", "ｻﾞｼﾞｽﾞｾﾞｿﾞ")]
        [DataRow("ダヂヅデド", "ﾀﾞﾁﾞﾂﾞﾃﾞﾄﾞ")]
        [DataRow("バビブベボ", "ﾊﾞﾋﾞﾌﾞﾍﾞﾎﾞ")]
        [DataRow("パピプペポ", "ﾊﾟﾋﾟﾌﾟﾍﾟﾎﾟ")]
        [DataRow("ヴ", "ｳﾞ")]
        [DataRow("ﾞ", "ﾞ")]
        [DataRow("ﾟ", "ﾟ")]
        [DataRow("ア゙", "ｱﾞ")]
        [DataRow("ア゚", "ｱﾟ")]
        [DataRow("ア゙ﾞ", "ｱﾞﾞ")]
        [DataRow("ア゚ﾟ", "ｱﾟﾟ")]
        public void ConvertHankakuKatakanaToZenkakuKatakanaTest_Normal(string test, string input)
        {
            var textMatcher = new TextConverter();
            var result = textMatcher.ConvertHankakuKatakanaToZenkakuKatakana(input);
            Assert.AreEqual(test, result);
        }

        [TestMethod]
        public void ConvertZenkakuKatakanaToHankakuKatakanaTest_Null()
        {
            var textMatcher = new TextConverter();
            Assert.ThrowsException<ArgumentNullException>(() => textMatcher.ConvertZenkakuKatakanaToHankakuKatakana(null));
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow("a", "a")]
        [DataRow("あ", "あ")]
        [DataRow("ｱ", "ア")]
        [DataRow("ｱｲｳｴｵ", "アイウエオ")]
        [DataRow("ｶｷｸｹｺ", "カキクケコ")]
        [DataRow("ｻｼｽｾｿ", "サシスセソ")]
        [DataRow("ﾀﾁﾂﾃﾄ", "タチツテト")]
        [DataRow("ﾅﾆﾇﾈﾉ", "ナニヌネノ")]
        [DataRow("ﾊﾋﾌﾍﾎ", "ハヒフヘホ")]
        [DataRow("ﾏﾐﾑﾒﾓ", "マミムメモ")]
        [DataRow("ﾔﾕﾖ", "ヤユヨ")]
        [DataRow("ﾗﾘﾙﾚﾛ", "ラリルレロ")]
        [DataRow("ﾜｦﾝ", "ワヲン")]
        [DataRow("ｧｨｩｪｫ", "ァィゥェォ")]
        [DataRow("ｬｭｮ", "ャュョ")]
        [DataRow("ｶﾞｷﾞｸﾞｹﾞｺﾞ", "ガギグゲゴ")]
        [DataRow("ｻﾞｼﾞｽﾞｾﾞｿﾞ", "ザジズゼゾ")]
        [DataRow("ﾀﾞﾁﾞﾂﾞﾃﾞﾄﾞ", "ダヂヅデド")]
        [DataRow("ﾊﾞﾋﾞﾌﾞﾍﾞﾎﾞ", "バビブベボ")]
        [DataRow("ﾊﾟﾋﾟﾌﾟﾍﾟﾎﾟ", "パピプペポ")]
        [DataRow("ｳﾞ", "ヴ")]
        [DataRow("ﾞ", "ﾞ")]
        [DataRow("ﾟ", "ﾟ")]
        // これどうしようかねぇ
//        [DataRow("ｱﾞ", "ア゙")]
//        [DataRow("ｱﾟ", "ア゚")]
//        [DataRow("ｱﾞﾞ", "ア゙ﾞ")]
//        [DataRow("ｱﾟﾟ", "ア゚ﾟ")]
        public void ConvertZenkakuKatakanaToHankakuKatakanaTest_Normal(string test, string input)
        {
            var textMatcher = new TextConverter();
            var result = textMatcher.ConvertZenkakuKatakanaToHankakuKatakana(input);
            Assert.AreEqual(test, result);
        }
    }
}
