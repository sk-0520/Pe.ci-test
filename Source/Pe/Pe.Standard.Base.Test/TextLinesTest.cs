using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Standard.Base.Test
{
    [TestClass]
    public class TextLinesTest
    {
        #region property

        private static string NewLine = "\r\n";

        #endregion

        #region function

        static TextLines CreateTextLines() => new TextLines(NewLine);

        [TestMethod]
        public void Constructor_Throw_Test()
        {
            Assert.ThrowsException<ArgumentException>(() => new TextLines("", 0));
            Assert.ThrowsException<ArgumentException>(() => new TextLines("", -1));
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow("a\r\nb\r\nc\r\nd", "a\r\nb\rc\rd")]
        [DataRow("a\r\n\r\n\r\nd", "a\r\n\r\rd")]
        public void AggregateSimple_Test(string expected, string input)
        {
            var tl = CreateTextLines();
            var actual = tl.Aggregate(input, i => i.Line);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow("1: a\r\n2: b\r\n3: c\r\n4: d", "a\r\nb\r\nc\r\nd")]
        public void AggregateLine_Test(string expected, string input)
        {
            var tl = CreateTextLines();
            var actual = tl.Aggregate(input, i => $"{i.Number}: {i.Line}");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow("2\r\n4", "1\r\n2\r\n3\r\n4")]
        public void AggregateNull_Test(string expected, string input)
        {
            var tl = CreateTextLines();
            var actual = tl.Aggregate(input, i => i.Number % 2 == 0 ? i.Line : null);
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }

}
