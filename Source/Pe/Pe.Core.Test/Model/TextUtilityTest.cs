using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Model
{
    [TestClass]
    public class TextUtilityTest
    {
        [TestMethod]
        [DataRow("", "", "<", ">")]
        [DataRow("a", "a", "<", ">")]
        [DataRow("<a", "<a", "<", ">")]
        [DataRow("a>", "a>", "<", ">")]
        [DataRow("<a>", "[a]", "<", ">")]
        [DataRow("<a><b>", "[a][b]", "<", ">")]
        public void ReplaceRangeTest(string src, string result, string head, string tail)
        {
            var conv = TextUtility.ReplaceRange(src, head, tail, s => "[" + s + "]");
            Assert.AreEqual(conv, result);
        }

        [TestMethod]
        [DataRow("a", "<", ">", "a")]
        [DataRow("<a>", "<", ">", "A")]
        [DataRow("<aa>", "<", ">", "<aa>")]
        [DataRow("<a><b>", "<", ">", "AB")]
        [DataRow("<a<a>><b>", "<", ">", "<a<a>>B")]
        [DataRow("a", "@[", "]", "a")]
        [DataRow("@[a]", "@[", "]", "A")]
        [DataRow("@[aa]", "@[", "]", "@[aa]")]
        [DataRow("@[a]@[b]", "@[", "]", "AB")]
        [DataRow("@[a@[a]]@[b]", "@[", "]", "@[a@[a]]B")]
        public void ReplaceRangeFromDictionaryTest(string src, string head, string tail, string result)
        {
            var map = new Dictionary<string, string>() {
                    { "A", "a" },
                    { "B", "b" },
                    { "C", "c" },
                    { "D", "d" },
                    { "E", "e" },
                    { "a", "A" },
                    { "b", "B" },
                    { "c", "C" },
                    { "d", "D" },
                    { "e", "E" },
                };
            Assert.AreEqual(result, TextUtility.ReplaceRangeFromDictionary(src, head, tail, map));
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
        public void ReadLinesTest(int result, string s)
        {
            Assert.AreEqual(result, TextUtility.ReadLines(s).Count(), TextUtility.ReadLines(s).Count().ToString());
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
        public void ToUniqueDefaultTest(string result, string src, params string[] list)
        {
            Assert.AreEqual(result, TextUtility.ToUniqueDefault(src, list, StringComparison.Ordinal));
        }

        [TestMethod]
        [DataRow(0, "")]
        [DataRow(0, default(string))]
        [DataRow(1, "1")]
        [DataRow(2, "22")]
        [DataRow(1, "あ")]
        public void TextWidthTest(int result, string text)
        {
            Assert.AreEqual(result, TextUtility.TextWidth(text));
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
        public void GetCharactersText(int count, string text)
        {
            var chars = TextUtility.GetCharacters(text);
            Assert.AreEqual(count, chars.Count());
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow("", null)]
        [DataRow("a", " a")]
        [DataRow("a", "a ")]
        [DataRow("a", " a ")]
        [DataRow("", "   ")]
        public void SafeTrimTest(string result, string text)
        {
            Assert.AreEqual(result, TextUtility.SafeTrim(text), $"`{result}` - `{text}`");
        }
    }
}
