#define IGNORE_NET5

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Standard.Base.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Standard.Base.Test.Models
{
    [TestClass]
    public class TextConverterTest
    {
        [TestMethod]
        public void ConvertHiraganaToKatakanaTest_Test()
        {
            var textMatcher = new TextConverter();
            Assert.ThrowsException<ArgumentNullException>(() => textMatcher.ConvertHiraganaToKatakana(null!));
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
        public void ConvertHiraganaToKatakaTest_Normal(string expected, string input)
        {
            var textMatcher = new TextConverter();
            var actual = textMatcher.ConvertHiraganaToKatakana(input);
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void ConvertKatakanaToHiraganaTest_Null()
        {
            var textMatcher = new TextConverter();
            Assert.ThrowsException<ArgumentNullException>(() => textMatcher.ConvertKatakanaToHiragana(null!));
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
        public void ConvertKatakanaToHiraganaTest_Normal(string expected, string input)
        {
            var textMatcher = new TextConverter();
            var actual = textMatcher.ConvertKatakanaToHiragana(input);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertHankakuKatakanaToZenkakuKatakanaTest_Null()
        {
            var textMatcher = new TextConverter();
            Assert.ThrowsException<ArgumentNullException>(() => textMatcher.ConvertHankakuKatakanaToZenkakuKatakana(null!));
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
        [DataRow("゛", "ﾞ")]
        [DataRow("゜", "ﾟ")]
        [DataRow("「ー・、。」", "｢ｰ･､｡｣")]
#if !IGNORE_NET5
        [DataRow("ア゙", "ｱﾞ")]
        [DataRow("ア゚", "ｱﾟ")]
        [DataRow("ア゙ﾞ", "ｱﾞﾞ")]
        [DataRow("ア゚ﾟ", "ｱﾟﾟ")]
#endif
        public void ConvertHankakuKatakanaToZenkakuKatakanaTest_Normal(string expected, string input)
        {
            var textMatcher = new TextConverter();
            var actual = textMatcher.ConvertHankakuKatakanaToZenkakuKatakana(input);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertZenkakuKatakanaToHankakuKatakanaTest_Null()
        {
            var textMatcher = new TextConverter();
            Assert.ThrowsException<ArgumentNullException>(() => textMatcher.ConvertZenkakuKatakanaToHankakuKatakana(null!));
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
        [DataRow("ﾞ", "゛")]
        [DataRow("ﾟ", "゜")]
        [DataRow("ｱﾞ", "ア゙")]
        [DataRow("ｱﾟ", "ア゚")]
#if !IGNORE_NET5
        [DataRow("ｱﾞﾞ", "ア゙ﾞ")]
        [DataRow("ｱﾟﾟ", "ア゚ﾟ")]
#endif
        public void ConvertZenkakuKatakanaToHankakuKatakanaTest_Normal(string expected, string input)
        {
            var textMatcher = new TextConverter();
            var actual = textMatcher.ConvertZenkakuKatakanaToHankakuKatakana(input);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertAsciiAlphabetToZenkakuAlphabetTest_Null()
        {
            var textMatcher = new TextConverter();
            Assert.ThrowsException<ArgumentNullException>(() => textMatcher.ConvertAsciiAlphabetToZenkakuAlphabet(null!));
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow("ａ", "a")]
        [DataRow("Ａ", "A")]
        [DataRow("1", "1")]
        [DataRow("１", "１")]
        [DataRow("ａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚ", "abcdefghijklmnopqrstuvwxyz")]
        [DataRow("ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺ", "ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
        public void ConvertAsciiAlphabetToZenkakuAlphabetTest_Normal(string expected, string input)
        {
            var textMatcher = new TextConverter();
            var actual = textMatcher.ConvertAsciiAlphabetToZenkakuAlphabet(input);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertZenkakuAlphabetToAsciiAlphabetTest_Null()
        {
            var textMatcher = new TextConverter();
            Assert.ThrowsException<ArgumentNullException>(() => textMatcher.ConvertZenkakuAlphabetToAsciiAlphabet(null!));
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow("a", "ａ")]
        [DataRow("A", "Ａ")]
        [DataRow("1", "1")]
        [DataRow("１", "１")]
        [DataRow("abcdefghijklmnopqrstuvwxyz", "ａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚ")]
        [DataRow("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺ")]
        public void ConvertZenkakuAlphabetToAsciiAlphabetTest_Normal(string expected, string input)
        {
            var textMatcher = new TextConverter();
            var actual = textMatcher.ConvertZenkakuAlphabetToAsciiAlphabet(input);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertAsciiDigitToZenkakuDigitTest_Null()
        {
            var textMatcher = new TextConverter();
            Assert.ThrowsException<ArgumentNullException>(() => textMatcher.ConvertAsciiDigitToZenkakuDigit(null!));
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow("ａ", "ａ")]
        [DataRow("Ａ", "Ａ")]
        [DataRow("１", "1")]
        [DataRow("１", "１")]
        [DataRow("０１２３４５６７８９", "0123456789")]
        public void ConvertAsciiDigitToZenkakuDigitTest_Normal(string expected, string input)
        {
            var textMatcher = new TextConverter();
            var actual = textMatcher.ConvertAsciiDigitToZenkakuDigit(input);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertZenkakuDigitToAsciiDigitTest_Null()
        {
            var textMatcher = new TextConverter();
            Assert.ThrowsException<ArgumentNullException>(() => textMatcher.ConvertZenkakuDigitToAsciiDigit(null!));
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow("ａ", "ａ")]
        [DataRow("Ａ", "Ａ")]
        [DataRow("1", "1")]
        [DataRow("1", "１")]
        [DataRow("0123456789", "０１２３４５６７８９")]
        public void ConvertZenkakuDigitToAsciiDigitTest_Normal(string expected, string input)
        {
            var textMatcher = new TextConverter();
            var actual = textMatcher.ConvertZenkakuDigitToAsciiDigit(input);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow("a", "あ")]
        [DataRow("a i u e o", "あ い う え お")]
        [DataRow("アイウエオ", "アイウエオ")]
        [DataRow("shi", "し")]
        [DataRow("n", "ん")]
        [DataRow("gya", "ぎゃ")]
        [DataRow("ja", "じゃ")]
        [DataRow("jajujo", "じゃじゅじょ")]
        [DataRow("nyanyunyo", "にゃにゅにょ")]
        [DataRow("shafu", "しゃふ")]
        public void ConvertHiraganaToAsciiRomeTest(string expected, string input)
        {
            var textConverter = new TextConverter();
            var actual = textConverter.ConvertHiraganaToAsciiRome(input);
            Assert.AreEqual(expected, actual);
        }
    }

}
