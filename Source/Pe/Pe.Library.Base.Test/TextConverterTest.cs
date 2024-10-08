#define IGNORE_NET5

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Base;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Base.Test
{
    public class TextConverterTest
    {
        [Fact]
        public void ConvertHiraganaToKatakanaTest_Test()
        {
            var textMatcher = new TextConverter();
            Assert.Throws<ArgumentNullException>(() => textMatcher.ConvertHiraganaToKatakana(null!));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("a", "a")]
        [InlineData("ア", "あ")]
        [InlineData("アア亜", "あア亜")]
        [InlineData("アイウエオ", "あいうえお")]
        [InlineData("カキクケコ", "かきくけこ")]
        [InlineData("サシスセソ", "さしすせそ")]
        [InlineData("タチツテト", "たちつてと")]
        [InlineData("ナニヌネノ", "なにぬねの")]
        [InlineData("ハヒフヘホ", "はひふへほ")]
        [InlineData("マミムメモ", "まみむめも")]
        [InlineData("ヤユヨ", "やゆよ")]
        [InlineData("ラリルレロ", "らりるれろ")]
        [InlineData("ワヲン", "わをん")]
        [InlineData("ァィゥェォ", "ぁぃぅぇぉ")]
        [InlineData("ャュョ", "ゃゅょ")]
        [InlineData("ヵヶ", "ゕゖ")]
        public void ConvertHiraganaToKatakaTest_Normal(string expected, string input)
        {
            var textMatcher = new TextConverter();
            var actual = textMatcher.ConvertHiraganaToKatakana(input);
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ConvertKatakanaToHiraganaTest_Null()
        {
            var textMatcher = new TextConverter();
            Assert.Throws<ArgumentNullException>(() => textMatcher.ConvertKatakanaToHiragana(null!));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("a", "a")]
        [InlineData("あ", "ア")]
        [InlineData("ああ亜", "あア亜")]
        [InlineData("あいうえお", "アイウエオ")]
        [InlineData("かきくけこ", "カキクケコ")]
        [InlineData("さしすせそ", "サシスセソ")]
        [InlineData("たちつてと", "タチツテト")]
        [InlineData("なにぬねの", "ナニヌネノ")]
        [InlineData("はひふへほ", "ハヒフヘホ")]
        [InlineData("まみむめも", "マミムメモ")]
        [InlineData("やゆよ", "ヤユヨ")]
        [InlineData("らりるれろ", "ラリルレロ")]
        [InlineData("わをん", "ワヲン")]
        [InlineData("ぁぃぅぇぉ", "ァィゥェォ")]
        [InlineData("ゃゅょ", "ャュョ")]
        [InlineData("ゕゖ", "ヵヶ")]
        public void ConvertKatakanaToHiraganaTest_Normal(string expected, string input)
        {
            var textMatcher = new TextConverter();
            var actual = textMatcher.ConvertKatakanaToHiragana(input);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertHankakuKatakanaToZenkakuKatakanaTest_Null()
        {
            var textMatcher = new TextConverter();
            Assert.Throws<ArgumentNullException>(() => textMatcher.ConvertHankakuKatakanaToZenkakuKatakana(null!));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("a", "a")]
        [InlineData("あ", "あ")]
        [InlineData("ア", "ア")]
        [InlineData("アイウエオ", "ｱｲｳｴｵ")]
        [InlineData("カキクケコ", "ｶｷｸｹｺ")]
        [InlineData("サシスセソ", "ｻｼｽｾｿ")]
        [InlineData("タチツテト", "ﾀﾁﾂﾃﾄ")]
        [InlineData("ナニヌネノ", "ﾅﾆﾇﾈﾉ")]
        [InlineData("ハヒフヘホ", "ﾊﾋﾌﾍﾎ")]
        [InlineData("マミムメモ", "ﾏﾐﾑﾒﾓ")]
        [InlineData("ヤユヨ", "ﾔﾕﾖ")]
        [InlineData("ラリルレロ", "ﾗﾘﾙﾚﾛ")]
        [InlineData("ワヲン", "ﾜｦﾝ")]
        [InlineData("ァィゥェォ", "ｧｨｩｪｫ")]
        [InlineData("ャュョ", "ｬｭｮ")]
        [InlineData("ガギグゲゴ", "ｶﾞｷﾞｸﾞｹﾞｺﾞ")]
        [InlineData("ザジズゼゾ", "ｻﾞｼﾞｽﾞｾﾞｿﾞ")]
        [InlineData("ダヂヅデド", "ﾀﾞﾁﾞﾂﾞﾃﾞﾄﾞ")]
        [InlineData("バビブベボ", "ﾊﾞﾋﾞﾌﾞﾍﾞﾎﾞ")]
        [InlineData("パピプペポ", "ﾊﾟﾋﾟﾌﾟﾍﾟﾎﾟ")]
        [InlineData("ヴ", "ｳﾞ")]
        [InlineData("゛", "ﾞ")]
        [InlineData("゜", "ﾟ")]
        [InlineData("「ー・、。」", "｢ｰ･､｡｣")]
#if !IGNORE_NET5
        [InlineData("ア゙", "ｱﾞ")]
        [InlineData("ア゚", "ｱﾟ")]
        [InlineData("ア゙ﾞ", "ｱﾞﾞ")]
        [InlineData("ア゚ﾟ", "ｱﾟﾟ")]
#endif
        public void ConvertHankakuKatakanaToZenkakuKatakanaTest_Normal(string expected, string input)
        {
            var textMatcher = new TextConverter();
            var actual = textMatcher.ConvertHankakuKatakanaToZenkakuKatakana(input);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertZenkakuKatakanaToHankakuKatakanaTest_Null()
        {
            var textMatcher = new TextConverter();
            Assert.Throws<ArgumentNullException>(() => textMatcher.ConvertZenkakuKatakanaToHankakuKatakana(null!));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("a", "a")]
        [InlineData("あ", "あ")]
        [InlineData("ｱ", "ア")]
        [InlineData("ｱｲｳｴｵ", "アイウエオ")]
        [InlineData("ｶｷｸｹｺ", "カキクケコ")]
        [InlineData("ｻｼｽｾｿ", "サシスセソ")]
        [InlineData("ﾀﾁﾂﾃﾄ", "タチツテト")]
        [InlineData("ﾅﾆﾇﾈﾉ", "ナニヌネノ")]
        [InlineData("ﾊﾋﾌﾍﾎ", "ハヒフヘホ")]
        [InlineData("ﾏﾐﾑﾒﾓ", "マミムメモ")]
        [InlineData("ﾔﾕﾖ", "ヤユヨ")]
        [InlineData("ﾗﾘﾙﾚﾛ", "ラリルレロ")]
        [InlineData("ﾜｦﾝ", "ワヲン")]
        [InlineData("ｧｨｩｪｫ", "ァィゥェォ")]
        [InlineData("ｬｭｮ", "ャュョ")]
        [InlineData("ｶﾞｷﾞｸﾞｹﾞｺﾞ", "ガギグゲゴ")]
        [InlineData("ｻﾞｼﾞｽﾞｾﾞｿﾞ", "ザジズゼゾ")]
        [InlineData("ﾀﾞﾁﾞﾂﾞﾃﾞﾄﾞ", "ダヂヅデド")]
        [InlineData("ﾊﾞﾋﾞﾌﾞﾍﾞﾎﾞ", "バビブベボ")]
        [InlineData("ﾊﾟﾋﾟﾌﾟﾍﾟﾎﾟ", "パピプペポ")]
        [InlineData("ｳﾞ", "ヴ")]
        [InlineData("ﾞ", "ﾞ")]
        [InlineData("ﾟ", "ﾟ")]
        [InlineData("ﾞ", "゛")]
        [InlineData("ﾟ", "゜")]
        [InlineData("ｱﾞ", "ア゙")]
        [InlineData("ｱﾟ", "ア゚")]
#if !IGNORE_NET5
        [InlineData("ｱﾞﾞ", "ア゙ﾞ")]
        [InlineData("ｱﾟﾟ", "ア゚ﾟ")]
#endif
        public void ConvertZenkakuKatakanaToHankakuKatakanaTest_Normal(string expected, string input)
        {
            var textMatcher = new TextConverter();
            var actual = textMatcher.ConvertZenkakuKatakanaToHankakuKatakana(input);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertAsciiAlphabetToZenkakuAlphabetTest_Null()
        {
            var textMatcher = new TextConverter();
            Assert.Throws<ArgumentNullException>(() => textMatcher.ConvertAsciiAlphabetToZenkakuAlphabet(null!));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("ａ", "a")]
        [InlineData("Ａ", "A")]
        [InlineData("1", "1")]
        [InlineData("１", "１")]
        [InlineData("ａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚ", "abcdefghijklmnopqrstuvwxyz")]
        [InlineData("ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺ", "ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
        public void ConvertAsciiAlphabetToZenkakuAlphabetTest_Normal(string expected, string input)
        {
            var textMatcher = new TextConverter();
            var actual = textMatcher.ConvertAsciiAlphabetToZenkakuAlphabet(input);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertZenkakuAlphabetToAsciiAlphabetTest_Null()
        {
            var textMatcher = new TextConverter();
            Assert.Throws<ArgumentNullException>(() => textMatcher.ConvertZenkakuAlphabetToAsciiAlphabet(null!));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("a", "ａ")]
        [InlineData("A", "Ａ")]
        [InlineData("1", "1")]
        [InlineData("１", "１")]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "ａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚ")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺ")]
        public void ConvertZenkakuAlphabetToAsciiAlphabetTest_Normal(string expected, string input)
        {
            var textMatcher = new TextConverter();
            var actual = textMatcher.ConvertZenkakuAlphabetToAsciiAlphabet(input);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertAsciiDigitToZenkakuDigitTest_Null()
        {
            var textMatcher = new TextConverter();
            Assert.Throws<ArgumentNullException>(() => textMatcher.ConvertAsciiDigitToZenkakuDigit(null!));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("ａ", "ａ")]
        [InlineData("Ａ", "Ａ")]
        [InlineData("１", "1")]
        [InlineData("１", "１")]
        [InlineData("０１２３４５６７８９", "0123456789")]
        public void ConvertAsciiDigitToZenkakuDigitTest_Normal(string expected, string input)
        {
            var textMatcher = new TextConverter();
            var actual = textMatcher.ConvertAsciiDigitToZenkakuDigit(input);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertZenkakuDigitToAsciiDigitTest_Null()
        {
            var textMatcher = new TextConverter();
            Assert.Throws<ArgumentNullException>(() => textMatcher.ConvertZenkakuDigitToAsciiDigit(null!));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("ａ", "ａ")]
        [InlineData("Ａ", "Ａ")]
        [InlineData("1", "1")]
        [InlineData("1", "１")]
        [InlineData("0123456789", "０１２３４５６７８９")]
        public void ConvertZenkakuDigitToAsciiDigitTest_Normal(string expected, string input)
        {
            var textMatcher = new TextConverter();
            var actual = textMatcher.ConvertZenkakuDigitToAsciiDigit(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("a", "あ")]
        [InlineData("a i u e o", "あ い う え お")]
        [InlineData("アイウエオ", "アイウエオ")]
        [InlineData("shi", "し")]
        [InlineData("n", "ん")]
        [InlineData("gya", "ぎゃ")]
        [InlineData("ja", "じゃ")]
        [InlineData("jajujo", "じゃじゅじょ")]
        [InlineData("nyanyunyo", "にゃにゅにょ")]
        [InlineData("shafu", "しゃふ")]
        public void ConvertHiraganaToAsciiRomeTest(string expected, string input)
        {
            var textConverter = new TextConverter();
            var actual = textConverter.ConvertHiraganaToAsciiRome(input);
            Assert.Equal(expected, actual);
        }
    }

}
