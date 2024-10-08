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
    public class DirectFileProxyTest
    {
        #region function

        [Fact]
        public void CopyTest()
        {
            var dir = TestIO.InitializeMethod(this);
            var dest = Path.Combine(dir.FullName, "dst.txt");
            var test = new DirectFileProxy();

            var srcFile1 = TestIO.CreateTextFile(dir, "src1.txt", "abc");
            test.Copy(srcFile1.FullName, dest);
            var actual = File.ReadAllText(dest);
            Assert.Equal("abc", actual);

            Assert.Throws<IOException>(() => test.Copy(srcFile1.FullName, dest));

            var srcFile2 = TestIO.CreateTextFile(dir, "src2.txt", "xyz");
            Assert.Throws<IOException>(() => test.Copy(srcFile2.FullName, dest, false));
            Assert.Equal("abc", File.ReadAllText(dest));

            test.Copy(srcFile2.FullName, dest, true);
            Assert.Equal("xyz", File.ReadAllText(dest));
        }

        [Fact]
        public void MoveTest()
        {
            var dir = TestIO.InitializeMethod(this);
            var dest = Path.Combine(dir.FullName, "dst.txt");
            var test = new DirectFileProxy();

            var srcFile1 = TestIO.CreateTextFile(dir, "src1.txt", "abc");
            test.Move(srcFile1.FullName, dest);
            Assert.False(File.Exists(srcFile1.FullName));
            Assert.True(File.Exists(dest));

            Assert.Throws<FileNotFoundException>(() => test.Move(srcFile1.FullName, dest));

            var srcFile2 = TestIO.CreateTextFile(dir, "src2.txt", "xyz");
            Assert.Throws<IOException>(() => test.Move(srcFile2.FullName, dest));
        }

        [Fact]
        public void DeleteTest()
        {
            var dir = TestIO.InitializeMethod(this);
            var test = new DirectFileProxy();

            var file = TestIO.CreateEmptyFile(dir, "file.txt").FullName;
            var notfound = Path.Combine(dir.FullName, "notfound.txt");
            var directory = TestIO.CreateDirectory(dir, "dir").FullName;

            Assert.True(File.Exists(file));
            test.Delete(file);
            Assert.False(File.Exists(file));

            Assert.False(File.Exists(notfound));
            var notfoundException = Record.Exception(() => test.Delete(notfound));
            Assert.Null(notfoundException);

            Assert.Throws<UnauthorizedAccessException>(() => test.Delete(directory));
        }

        [Fact]
        public void ExistsTest()
        {
            var dir = TestIO.InitializeMethod(this);
            var test = new DirectFileProxy();

            var file = TestIO.CreateEmptyFile(dir, "file.txt").FullName;
            var notfound = Path.Combine(dir.FullName, "notfound.txt");
            var directory = TestIO.CreateDirectory(dir, "dir").FullName;

            Assert.True(test.Exists(file));
            Assert.False(test.Exists(notfound));
            Assert.False(test.Exists(directory));
        }

        #endregion
    }
}
