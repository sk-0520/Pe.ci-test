using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.CommonTest;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Database.Test
{
    public class DatabaseImplementationTest
    {
        #region function

        [Fact]
        public void PreFormatStatementTest()
        {
            var test = new DatabaseImplementation();
            var actual = test.PreFormatStatement("statement");
            Assert.Equal("statement", actual);
        }

        [Fact]
        public void ToStatementColumnNameTest()
        {
            var test = new DatabaseImplementation();
            var actual = test.ToStatementColumnName("column");
            Assert.Equal("column", actual);
        }

        [Fact]
        public void ToStatementTableNameTest()
        {
            var test = new DatabaseImplementation();
            var actual = test.ToStatementTableName("table");
            Assert.Equal("table", actual);
        }

        [Theory]
        [InlineData("@param", "param", 0)]
        [InlineData("@param", "param", 1)]
        public void ToStatementParameterNameTest(string expected, string parameterName, int index)
        {
            var test = new DatabaseImplementation();
            var actual = test.ToStatementParameterName(parameterName, index);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToLineCommentTest()
        {
            var test = new DatabaseImplementation();
            var actual = test.ToLineComment("ABC\rDEF\nGHI\r\nJKL");
            AssertEx.EqualMultiLineTextIgnoreNewline("--ABC\n--DEF\n--GHI\n--JKL", actual);
        }

        [Fact]
        public void ToBlockComment()
        {
            var test = new DatabaseImplementation();
            var actual = test.ToBlockComment("ABC");
            AssertEx.EqualMultiLineTextIgnoreNewline("\n/*\nABC\n*/\n", actual);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(@"\\", "\\")]
        [InlineData("''", "\'")]
        [InlineData(@"\r", "\r")]
        [InlineData(@"\n", "\n")]
        [InlineData(@"\n\r''''\\", "\n\r''\\")]
        public void EscapeTest(string expected, string word)
        {
            var test = new DatabaseImplementation();
            var actual = test.Escape(word);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(@"\\", "\\")]
        [InlineData(@"\%", "%")]
        [InlineData(@"\_", "_")]
        [InlineData(@"\\\_\\\%\\\\", "\\_\\%\\\\")]
        public void EscapeLikeTest(string expected, string word)
        {
            var test = new DatabaseImplementation();
            var actual = test.EscapeLike(word);
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
