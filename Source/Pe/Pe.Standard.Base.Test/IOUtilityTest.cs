using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Standard.Base.Test
{
    [TestClass]
    public class IOUtilityTest
    {
        #region function

        [TestMethod]
        public void MakeFileParentDirectoryTest()
        {
            var dir = TestIO.InitializeMethod(this);

            var nextDirPath = Path.Combine(dir.FullName, "next");
            var nextFilePath = Path.Combine(nextDirPath, "file");
            var nextSubFilePath = Path.Combine(nextDirPath, "file-sub");

            Assert.IsFalse(Directory.Exists(nextDirPath));
            Assert.IsFalse(File.Exists(nextFilePath));

            IOUtility.MakeFileParentDirectory(nextFilePath);
            Assert.IsTrue(Directory.Exists(nextDirPath));
            Assert.IsFalse(File.Exists(nextFilePath));

            IOUtility.MakeFileParentDirectory(nextSubFilePath);
            Assert.IsTrue(Directory.Exists(nextDirPath));
            Assert.IsFalse(File.Exists(nextSubFilePath));
        }

        [TestMethod]
        public void ExistsTest()
        {
            var dir = TestIO.InitializeMethod(this);

            var f = TestIO.CreateEmptyFile(dir, "f");
            Assert.IsTrue(IOUtility.Exists(f.FullName));

            var d = TestIO.CreateDirectory(dir, "d");
            Assert.IsTrue(IOUtility.Exists(d.FullName));

            f.Delete();
            d.Delete(true);

            Assert.IsFalse(IOUtility.Exists(f.FullName));
            Assert.IsFalse(IOUtility.Exists(d.FullName));
        }

        [TestMethod]
        public async Task ExistsAsyncTest()
        {
            var dir = TestIO.InitializeMethod(this);

            var f = TestIO.CreateEmptyFile(dir, "f");
            Assert.IsTrue(await IOUtility.ExistsAsync(f.FullName));

            var d = TestIO.CreateDirectory(dir, "d");
            Assert.IsTrue(await IOUtility.ExistsAsync(d.FullName));

            f.Delete();
            d.Delete(true);

            Assert.IsFalse(await IOUtility.ExistsAsync(f.FullName));
            Assert.IsFalse(await IOUtility.ExistsAsync(d.FullName));
        }

        #endregion
    }
}
