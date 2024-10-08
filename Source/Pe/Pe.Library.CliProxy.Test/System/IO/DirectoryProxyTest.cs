using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.CliProxy.System.IO;
using ContentTypeTextNet.Pe.CommonTest;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.CliProxy.Test.System.IO
{
    public class DirectDirectoryProxyTest
    {
        #region function

        [Fact]
        public void CreateDirectoryTest()
        {
            var dir = TestIO.InitializeMethod(this);
            var test = new DirectDirectoryProxy();

            var dirPath = Path.Combine(dir.FullName, "dir");
            var actualDir1 = test.CreateDirectory(dirPath);
            Assert.True(actualDir1.Exists);

            var actualDir2 = test.CreateDirectory(dirPath);
            Assert.Equal(actualDir1.FullName, actualDir2.FullName);

            var file = TestIO.CreateEmptyFile(dir, "file");
            Assert.Throws<IOException>(() => test.CreateDirectory(file.FullName));
        }

        [Fact]
        public void ExistsTest()
        {
            var dir = TestIO.InitializeMethod(this);
            var test = new DirectDirectoryProxy();

            var dirPath = TestIO.CreateDirectory(dir, "dir").FullName;
            var filePath = TestIO.CreateEmptyFile(dir, "file").FullName;
            var notfoundPath = Path.Combine(dir.FullName, "notfound");

            Assert.True(test.Exists(dirPath));
            Assert.False(test.Exists(filePath));
            Assert.False(test.Exists(notfoundPath));
        }

        #endregion
    }
}
