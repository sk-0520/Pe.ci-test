using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.CommonTest.Test
{
    public class AssertExTest
    {
        #region function

        public static TheoryData<string, string> EqualMultiLineTextIgnoreNewlineData => new() {
            {
                "",
                ""
            },
            {
                "\n",
                ""
            },
             {
                "",
                "\n"
            },
            {
                "A\rB",
                "A\rB"
            },
            {
                "A\nB",
                "A\nB"
            },
            {
                "A\r\nB",
                "A\r\nB"
            },
            {
                "A\rB",
                "A\nB"
            },
            {
                "A\r\nB",
                "A\nB"
            },
            {
                "A\nB",
                "A\r\nB"
            },
        };

        [Theory]
        [MemberData(nameof(EqualMultiLineTextIgnoreNewlineData))]
        public void EqualMultiLineTextIgnoreNewlineTest(string expectedLines, string actualLines)
        {
            AssertEx.EqualMultiLineTextIgnoreNewline(expectedLines, actualLines);
        }

        public static TheoryData<string, string> NotEqualMultiLineTextIgnoreNewlineData => new() {
            {
                "A\nB",
                "a\nb"
            },
            {
                "A\rB",
                "a\rb"
            },
            {
                "A\r\nB",
                "a\rb"
            },
            {
                "A\rB",
                "a\r\nb"
            },
       };

        [Theory]
        [MemberData(nameof(NotEqualMultiLineTextIgnoreNewlineData))]
        public void NotEqualMultiLineTextIgnoreNewlineTest(string expectedLines, string actualLines)
        {
            AssertEx.NotEqualMultiLineTextIgnoreNewline(expectedLines, actualLines);
        }

        #endregion
    }
}
