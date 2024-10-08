using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using ContentTypeTextNet.Pe.CommonTest;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Base.Test
{
    public class DirectoryCleanerTest
    {
        #region function

        [Fact]
        public void ConstructorTest()
        {
            var methodDir = TestIO.InitializeMethod(this);
            var dir = TestIO.CreateDirectory(methodDir, "");

            var actual = new DirectoryCleaner(
                dir,
                10,
                TimeSpan.FromSeconds(3),
                NullLoggerFactory.Instance
            );

            Assert.Equal(dir, actual.Directory);
        }

        #endregion
    }
}
