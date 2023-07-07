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

        [TestMethod]
        public async Task ExistsFileAsyncTest()
        {
            var dir = TestIO.InitializeMethod(this);

            var f = TestIO.CreateEmptyFile(dir, "f");
            Assert.IsTrue(await IOUtility.ExistsFileAsync(f.FullName));

            var d = TestIO.CreateDirectory(dir, "d");
            Assert.IsFalse(await IOUtility.ExistsFileAsync(d.FullName));

            f.Delete();
            d.Delete(true);

            Assert.IsFalse(await IOUtility.ExistsAsync(f.FullName));
            Assert.IsFalse(await IOUtility.ExistsAsync(d.FullName));
        }

        [TestMethod]
        public async Task ExistsDirectoryAsyncTest()
        {
            var dir = TestIO.InitializeMethod(this);

            var f = TestIO.CreateEmptyFile(dir, "f");
            Assert.IsFalse(await IOUtility.ExistsDirectoryAsync(f.FullName));

            var d = TestIO.CreateDirectory(dir, "d");
            Assert.IsTrue(await IOUtility.ExistsDirectoryAsync(d.FullName));

            f.Delete();
            d.Delete(true);

            Assert.IsFalse(await IOUtility.ExistsDirectoryAsync(f.FullName));
            Assert.IsFalse(await IOUtility.ExistsDirectoryAsync(d.FullName));
        }

        [TestMethod]
        public void DeleteTest()
        {
            var dir = TestIO.InitializeMethod(this);

            var f = TestIO.CreateEmptyFile(dir, "f");
            var d = TestIO.CreateDirectory(dir, "d");

            var child = TestIO.CreateEmptyFile(d, "child");

            Assert.IsTrue(IOUtility.Exists(f.FullName));
            Assert.IsTrue(IOUtility.Exists(d.FullName));
            Assert.IsTrue(IOUtility.Exists(child.FullName));

            IOUtility.Delete(f.FullName);
            Assert.IsFalse(IOUtility.Exists(f.FullName));

            IOUtility.Delete(dir.FullName);
            Assert.IsFalse(IOUtility.Exists(d.FullName));
            Assert.IsFalse(IOUtility.Exists(child.FullName));
            Assert.IsFalse(IOUtility.Exists(dir.FullName));
        }

        [TestMethod]
        public async Task DeleteAsyncTest()
        {
            var dir = TestIO.InitializeMethod(this);

            var f = TestIO.CreateEmptyFile(dir, "f");
            var d = TestIO.CreateDirectory(dir, "d");

            var child = TestIO.CreateEmptyFile(d, "child");

            Assert.IsTrue(IOUtility.Exists(f.FullName));
            Assert.IsTrue(IOUtility.Exists(d.FullName));
            Assert.IsTrue(IOUtility.Exists(child.FullName));

            await IOUtility.DeleteAsync(f.FullName);
            Assert.IsFalse(IOUtility.Exists(f.FullName));

            await IOUtility.DeleteAsync(dir.FullName);
            Assert.IsFalse(IOUtility.Exists(d.FullName));
            Assert.IsFalse(IOUtility.Exists(child.FullName));
            Assert.IsFalse(IOUtility.Exists(dir.FullName));
        }

        [TestMethod]
        public void CreateTemporaryDirectoryTest()
        {
            var tmp = IOUtility.CreateTemporaryDirectory();
            var dir = tmp.Directory;
            using(tmp) {
                dir.Refresh();
                Assert.IsTrue(dir.Exists);
            }
            dir.Refresh();
            Assert.IsFalse(dir.Exists);
        }

        [TestMethod]
        public void CreateTemporaryDirectory_Prefix_Test()
        {
            var tmp = IOUtility.CreateTemporaryDirectory(new TemporaryOptions {
                Prefix = "prefix_"
            });
            var dir = tmp.Directory;
            using(tmp) {
                dir.Refresh();
                Assert.IsTrue(dir.Exists);
            }
            dir.Refresh();
            Assert.IsFalse(dir.Exists);
        }

        [TestMethod]
        public void CreateTemporaryFileTest()
        {
            var tmp = IOUtility.CreateTemporaryFile();
            var file = tmp.File;
            using(tmp) {
                file.Refresh();
                Assert.IsTrue(file.Exists);
            }
            file.Refresh();
            Assert.IsFalse(file.Exists);
        }

        #endregion
    }
}
