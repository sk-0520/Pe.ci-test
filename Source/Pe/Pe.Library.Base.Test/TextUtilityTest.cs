using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Base;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Base.Test
{
    public class TextUtilityTest
    {
        [Theory]
        [InlineData("", "", "<", ">")]
        [InlineData("a", "a", "<", ">")]
        [InlineData("<a", "<a", "<", ">")]
        [InlineData("a>", "a>", "<", ">")]
        [InlineData("[a]", "<a>", "<", ">")]
        [InlineData("<>", "<>", "<", ">")] // 何もない場合は何もしない
        [InlineData("[<a]>", "<<a>>", "<", ">")] // 近しい範囲
        [InlineData("[a][b]", "<a><b>", "<", ">")]
        public void ReplacePlaceholderTest(string expected, string src, string head, string tail)
        {
            var actual = TextUtility.ReplacePlaceholder(src, head, tail, s => "[" + s + "]");
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("a", "a", "<", ">")]
        [InlineData("A", "<a>", "<", ">")]
        [InlineData("<aa>", "<aa>", "<", ">")]
        [InlineData("AB", "<a><b>", "<", ">")]
        [InlineData("<a<a>>B", "<a<a>><b>", "<", ">")]
        [InlineData("a", "a", "@[", "]")]
        [InlineData("A", "@[a]", "@[", "]")]
        [InlineData("@[aa]", "@[aa]", "@[", "]")]
        [InlineData("AB", "@[a]@[b]", "@[", "]")]
        [InlineData("@[a@[a]]B", "@[a@[a]]@[b]", "@[", "]")]
        public void ReplaceRangeFromDictionaryTest(string expected, string src, string head, string tail)
        {
            var map = new Dictionary<string, string>() {
                ["A"] = "a",
                ["B"] = "b",
                ["C"] = "c",
                ["D"] = "d",
                ["E"] = "e",
                ["a"] = "A",
                ["b"] = "B",
                ["c"] = "C",
                ["d"] = "D",
                ["e"] = "E",
            };
            var actual = TextUtility.ReplacePlaceholderFromDictionary(src, head, tail, map);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("a", "${A}")]
        public void ReplaceFromDictionaryTest(string expected, string src)
        {
            var map = new Dictionary<string, string>() {
                ["A"] = "a",
                ["B"] = "b",
                ["C"] = "c",
                ["D"] = "d",
                ["E"] = "e",
                ["a"] = "A",
                ["b"] = "B",
                ["c"] = "C",
                ["d"] = "D",
                ["e"] = "E",
            };
            var actual = TextUtility.ReplaceFromDictionary(src, map);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, "")]
        [InlineData(1, "a")]
        [InlineData(1, "a\r\n")]
        [InlineData(2, "a\r\nb")]
        [InlineData(2, "a\rb")]
        [InlineData(2, "a\nb")]
        [InlineData(2, " a \r b ")]
        [InlineData(2, " a \n b ")]
        [InlineData(2, " a \r\n b ")]
        public void ReadLinesTest(int expected, string s)
        {
            var actual = TextUtility.ReadLines(s).Count();
            Assert.Equal(expected, actual);
        }

#if false
        public void ReadLinesTest_Null()
        {
            Assert.Throws<ArgumentException>(() => TextUtility.ReadLines(default(string)));
        }
#endif

        [Theory]
        [InlineData("a", "a", new[] { "" })]
        [InlineData("a", "a", new[] { "b" })]
        [InlineData("a(2)", "a", new[] { "a" })]
        [InlineData("A", "A", new[] { "A(2)" })]
        [InlineData("a(3)", "a", new[] { "a(5)", "a(2)", "a(4)", "a" })]
        public void ToUniqueDefaultTest(string expected, string src, string[] list)
        {
            var actual = TextUtility.ToUniqueDefault(src, list, StringComparison.Ordinal);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, "")]
        [InlineData(0, default(string))]
        [InlineData(1, "1")]
        [InlineData(2, "22")]
        [InlineData(1, "あ")]
        [InlineData(1, "🐙")]
        public void TextWidthTest(int expected, string? text)
        {
            var actual = TextUtility.TextWidth(text!);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, "")]
        [InlineData(1, "a")]
        [InlineData(1, "あ")]
        [InlineData(1, "亜")]
        [InlineData(2, "ab")]
        [InlineData(2, "あい")]
        [InlineData(2, "亜伊")]
        [InlineData(5, "ァｲu工ぉ")]
        [InlineData(1, "〇")]
        [InlineData(1, "⛄")]
        [InlineData(1, "🐎")]
        [InlineData(2, "🐎🐎")]
        public void GetCharactersText(int expected, string text)
        {
            var actual = TextUtility.GetCharacters(text);
            Assert.Equal(expected, actual.Count());
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("", null)]
        [InlineData("a", " a")]
        [InlineData("a", "a ")]
        [InlineData("a", " a ")]
        [InlineData("", "   ")]
        public void SafeTrimTest(string expected, string? text)
        {
            var actual = TextUtility.SafeTrim(text);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "", new[] { 'a', 'b', 'c' })]
        [InlineData("def", "def", new[] { 'a', 'b', 'c' })]
        [InlineData("", "abc", new[] { 'a', 'b', 'c' })]
        [InlineData("def", "abcdef", new[] { 'a', 'b', 'c' })]
        [InlineData("def", "abcdefabc", new[] { 'a', 'b', 'c' })]
        public void RemoveCharactersTest(string expected, string input, char[] cs)
        {
            var actual = TextUtility.RemoveCharacters(input, cs.ToHashSet());
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("def", "def")]
        [InlineData("ABCdef", "ABCdef")]
        [InlineData("ABCdefABC", "ABCdefABC")]
        [InlineData("ABCdef", "abcdef")]
        [InlineData("ABCdefABC", "abcdefabc")]
        public void ReplaceCharactersTest(string expected, string input)
        {
            var map = new Dictionary<char, char>() {
                ['a'] = 'A',
                ['b'] = 'B',
                ['c'] = 'C',
            };
            var actual = TextUtility.ReplaceCharacters(input, map);
            Assert.Equal(expected, actual);
        }
    }

}
