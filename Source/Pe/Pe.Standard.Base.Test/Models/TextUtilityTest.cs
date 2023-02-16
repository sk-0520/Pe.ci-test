using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Standard.Base.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Standard.Test.Models
{
    [TestClass]
    public class TextUtilityTest
    {
        [TestMethod]
        [DataRow("", "", "<", ">")]
        [DataRow("a", "a", "<", ">")]
        [DataRow("<a", "<a", "<", ">")]
        [DataRow("a>", "a>", "<", ">")]
        [DataRow("[a]", "<a>", "<", ">")]
        [DataRow("[a][b]", "<a><b>", "<", ">")]
        public void ReplacePlaceholderTest(string expected, string src, string head, string tail)
        {
            var actual = TextUtility.ReplacePlaceholder(src, head, tail, s => "[" + s + "]");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow("a", "a", "<", ">")]
        [DataRow("A", "<a>", "<", ">")]
        [DataRow("<aa>", "<aa>", "<", ">")]
        [DataRow("AB", "<a><b>", "<", ">")]
        [DataRow("<a<a>>B", "<a<a>><b>", "<", ">")]
        [DataRow("a", "a", "@[", "]")]
        [DataRow("A", "@[a]", "@[", "]")]
        [DataRow("@[aa]", "@[aa]", "@[", "]")]
        [DataRow("AB", "@[a]@[b]", "@[", "]")]
        [DataRow("@[a@[a]]B", "@[a@[a]]@[b]", "@[", "]")]
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
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow("a", "${A}")]
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
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(0, "")]
        [DataRow(1, "a")]
        [DataRow(1, "a\r\n")]
        [DataRow(2, "a\r\nb")]
        [DataRow(2, "a\rb")]
        [DataRow(2, "a\nb")]
        [DataRow(2, " a \r b ")]
        [DataRow(2, " a \n b ")]
        [DataRow(2, " a \r\n b ")]
        public void ReadLinesTest(int expected, string s)
        {
            var actual = TextUtility.ReadLines(s).Count();
            Assert.AreEqual(expected, actual, TextUtility.ReadLines(s).Count().ToString(CultureInfo.InvariantCulture));
        }

#if false
        public void ReadLinesTest_Null()
        {
            Assert.ThrowsException<ArgumentException>(() => TextUtility.ReadLines(default(string)));
        }
#endif

        [TestMethod]
        [DataRow("a", "a", new[] { "" })]
        [DataRow("a", "a", new[] { "b" })]
        [DataRow("a(2)", "a", new[] { "a" })]
        [DataRow("A", "A", new[] { "A(2)" })]
        [DataRow("a(3)", "a", new[] { "a(5)", "a(2)", "a(4)", "a" })]
        public void ToUniqueDefaultTest(string expected, string src, string[] list)
        {
            var actual = TextUtility.ToUniqueDefault(src, list, StringComparison.Ordinal);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(0, "")]
        [DataRow(0, default(string))]
        [DataRow(1, "1")]
        [DataRow(2, "22")]
        [DataRow(1, "あ")]
        [DataRow(1, "🐙")]
        public void TextWidthTest(int expected, string text)
        {
            var actual = TextUtility.TextWidth(text);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(0, "")]
        [DataRow(1, "a")]
        [DataRow(1, "あ")]
        [DataRow(1, "亜")]
        [DataRow(2, "ab")]
        [DataRow(2, "あい")]
        [DataRow(2, "亜伊")]
        [DataRow(5, "ァｲu工ぉ")]
        [DataRow(1, "〇")]
        [DataRow(1, "⛄")]
        [DataRow(1, "🐎")]
        [DataRow(2, "🐎🐎")]
        public void GetCharactersText(int expected, string text)
        {
            var actual = TextUtility.GetCharacters(text);
            Assert.AreEqual(expected, actual.Count());
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow("", null)]
        [DataRow("a", " a")]
        [DataRow("a", "a ")]
        [DataRow("a", " a ")]
        [DataRow("", "   ")]
        public void SafeTrimTest(string expected, string text)
        {
            var actual = TextUtility.SafeTrim(text);
            Assert.AreEqual(expected, actual, $"`{expected}` - `{text}`");
        }

        [TestMethod]
        [DataRow("", "", new[] { 'a', 'b', 'c' })]
        [DataRow("def", "def", new[] { 'a', 'b', 'c' })]
        [DataRow("", "abc", new[] { 'a', 'b', 'c' })]
        [DataRow("def", "abcdef", new[] { 'a', 'b', 'c' })]
        [DataRow("def", "abcdefabc", new[] { 'a', 'b', 'c' })]
        public void RemoveCharactersTest(string expected, string input, char[] cs)
        {
            var actual = TextUtility.RemoveCharacters(input, cs.ToHashSet());
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow("def", "def")]
        [DataRow("ABCdef", "ABCdef")]
        [DataRow("ABCdefABC", "ABCdefABC")]
        [DataRow("ABCdef", "abcdef")]
        [DataRow("ABCdefABC", "abcdefabc")]
        public void ReplaceCharactersTest(string expected, string input)
        {
            var map = new Dictionary<char, char>() {
                ['a'] = 'A',
                ['b'] = 'B',
                ['c'] = 'C',
            };
            var actual = TextUtility.ReplaceCharacters(input, map);
            Assert.AreEqual(expected, actual);
        }
    }

}
