using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Database.Test
{
    public class CommentNotSupportDatabaseImplementationTest
    {
        #region define

        private class CommentNotSupportDatabaseImplementation: DatabaseImplementation
        {
            #region DatabaseImplementation

            public override bool SupportedLineComment => false;
            public override bool SupportedBlockComment => false;

            #endregion
        }

        #endregion

        #region function

        [Fact]
        public void ToLineCommentTest()
        {
            var test = new CommentNotSupportDatabaseImplementation();
            Assert.Throws<InvalidOperationException>(() => test.ToLineComment("ABC\rDEF\nGHI\r\nJKL"));
        }

        [Fact]
        public void ToBlockComment()
        {
            var test = new CommentNotSupportDatabaseImplementation();
            Assert.Throws<InvalidOperationException>(() => test.ToBlockComment("ABC"));
        }

        #endregion
    }
}
