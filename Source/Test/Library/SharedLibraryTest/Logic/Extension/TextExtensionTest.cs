/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;

namespace ContentTypeTextNet.Test.Library.SharedLibraryTest.Logic.Extension
{
    [TestFixture]
    class TextExtensionTest
    {
        [TestCase("", "", "<", ">")]
        [TestCase("a", "a", "<", ">")]
        [TestCase("<a", "<a", "<", ">")]
        [TestCase("a>", "a>", "<", ">")]
        [TestCase("<a>", "[a]", "<", ">")]
        [TestCase("<a><b>", "[a][b]", "<", ">")]
        public void ReplaceRangeTest(string src, string result, string head, string tail)
        {
            var conv = src.ReplaceRange(head, tail, s => "[" + s + "]");
            Assert.AreEqual(conv, result);
        }

        [TestCase("a", "<", ">", "a")]
        [TestCase("<a>", "<", ">", "A")]
        [TestCase("<aa>", "<", ">", "<aa>")]
        [TestCase("<a><b>", "<", ">", "AB")]
        [TestCase("<a<a>><b>", "<", ">", "<a<a>>B")]
        [TestCase("a", "@[", "]", "a")]
        [TestCase("@[a]", "@[", "]", "A")]
        [TestCase("@[aa]", "@[", "]", "@[aa]")]
        [TestCase("@[a]@[b]", "@[", "]", "AB")]
        [TestCase("@[a@[a]]@[b]", "@[", "]", "@[a@[a]]B")]
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
            Assert.IsTrue(src.ReplaceRangeFromDictionary(head, tail, map) == result);
        }

        [TestCase("a", "A")]
        [TestCase("aa", "AA")]
        [TestCase("az", "Az")]
        [TestCase("Aa", "aA")]
        [TestCase("aAa", "AaA")]
        public void ReplaceFromDictionaryTest(string src, string result)
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
            var conv = src.ReplaceFromDictionary(map);
            Assert.AreEqual(conv, result);
        }

        [TestCase(0, null)]
        [TestCase(0, "")]
        [TestCase(1, "a")]
        [TestCase(1, "a\r\n")]
        [TestCase(2, "a\r\nb")]
        [TestCase(2, "a\rb")]
        [TestCase(2, "a\nb")]
        [TestCase(2, " a \r b ")]
        [TestCase(2, " a \n b ")]
        [TestCase(2, " a \r\n b ")]
        public void SplitLinesTest(int result, string s)
        {
            Assert.IsTrue(s.SplitLines().Count() == result);
        }

    }
}
