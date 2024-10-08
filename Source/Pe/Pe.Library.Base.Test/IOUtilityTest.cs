using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.CommonTest;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Base.Test
{
    public class IOUtilityTest
    {
        #region function

        [Fact]
        public void MakeFileParentDirectoryTest()
        {
            var dir = TestIO.InitializeMethod(this);

            var nextDirPath = Path.Combine(dir.FullName, "next");
            var nextFilePath = Path.Combine(nextDirPath, "file");
            var nextSubFilePath = Path.Combine(nextDirPath, "file-sub");

            Assert.False(Directory.Exists(nextDirPath));
            Assert.False(File.Exists(nextFilePath));

            IOUtility.MakeFileParentDirectory(nextFilePath);
            Assert.True(Directory.Exists(nextDirPath));
            Assert.False(File.Exists(nextFilePath));

            IOUtility.MakeFileParentDirectory(nextSubFilePath);
            Assert.True(Directory.Exists(nextDirPath));
            Assert.False(File.Exists(nextSubFilePath));
        }

        [Fact]
        public void MakeFileParentDirectory_FileInfo_Test()
        {
            var dir = TestIO.InitializeMethod(this);

            var nextDirPath = Path.Combine(dir.FullName, "next");
            var nextFilePath = Path.Combine(nextDirPath, "file");
            var nextSubFilePath = Path.Combine(nextDirPath, "file-sub");

            Assert.False(Directory.Exists(nextDirPath));
            Assert.False(File.Exists(nextFilePath));

            IOUtility.MakeFileParentDirectory(new FileInfo(nextFilePath));
            Assert.True(Directory.Exists(nextDirPath));
            Assert.False(File.Exists(nextFilePath));

            IOUtility.MakeFileParentDirectory(new FileInfo(nextSubFilePath));
            Assert.True(Directory.Exists(nextDirPath));
            Assert.False(File.Exists(nextSubFilePath));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("/")]
#if OS_WINDOWS
        [InlineData("c:\\")]
#endif
        public void MakeFileParentDirectory_throw_Test(string? s)
        {
            Assert.Throws<IOUtilityException>(() => IOUtility.MakeFileParentDirectory(s!));
        }

        [Theory]
        [InlineData("/")]
#if OS_WINDOWS
        [InlineData("c:\\")]
#endif
        public void MakeFileParentDirectory_FileInfo_throw_Test(string s)
        {
            Assert.Throws<IOUtilityException>(() => IOUtility.MakeFileParentDirectory(new FileInfo(s)));
        }

        [Fact]
        public void ExistsTest()
        {
            var dir = TestIO.InitializeMethod(this);

            var f = TestIO.CreateEmptyFile(dir, "f");
            Assert.True(IOUtility.Exists(f.FullName));

            var d = TestIO.CreateDirectory(dir, "d");
            Assert.True(IOUtility.Exists(d.FullName));

            f.Delete();
            d.Delete(true);

            Assert.False(IOUtility.Exists(f.FullName));
            Assert.False(IOUtility.Exists(d.FullName));
            Assert.False(IOUtility.Exists(null));
        }

        [Fact]
        public async Task ExistsAsyncTest()
        {
            var dir = TestIO.InitializeMethod(this);

            var f = TestIO.CreateEmptyFile(dir, "f");
            Assert.True(await IOUtility.ExistsAsync(f.FullName));

            var d = TestIO.CreateDirectory(dir, "d");
            Assert.True(await IOUtility.ExistsAsync(d.FullName));

            f.Delete();
            d.Delete(true);

            Assert.False(await IOUtility.ExistsAsync(f.FullName));
            Assert.False(await IOUtility.ExistsAsync(d.FullName));
        }

        [Fact]
        public async Task ExistsFileAsyncTest()
        {
            var dir = TestIO.InitializeMethod(this);

            var f = TestIO.CreateEmptyFile(dir, "f");
            Assert.True(await IOUtility.ExistsFileAsync(f.FullName));

            var d = TestIO.CreateDirectory(dir, "d");
            Assert.False(await IOUtility.ExistsFileAsync(d.FullName));

            f.Delete();
            d.Delete(true);

            Assert.False(await IOUtility.ExistsAsync(f.FullName));
            Assert.False(await IOUtility.ExistsAsync(d.FullName));
        }

        [Fact]
        public async Task ExistsDirectoryAsyncTest()
        {
            var dir = TestIO.InitializeMethod(this);

            var f = TestIO.CreateEmptyFile(dir, "f");
            Assert.False(await IOUtility.ExistsDirectoryAsync(f.FullName));

            var d = TestIO.CreateDirectory(dir, "d");
            Assert.True(await IOUtility.ExistsDirectoryAsync(d.FullName));

            f.Delete();
            d.Delete(true);

            Assert.False(await IOUtility.ExistsDirectoryAsync(f.FullName));
            Assert.False(await IOUtility.ExistsDirectoryAsync(d.FullName));
        }

        [Fact]
        public void DeleteTest()
        {
            var dir = TestIO.InitializeMethod(this);

            var f = TestIO.CreateEmptyFile(dir, "f");
            var d = TestIO.CreateDirectory(dir, "d");

            var child = TestIO.CreateEmptyFile(d, "child");

            Assert.True(IOUtility.Exists(f.FullName));
            Assert.True(IOUtility.Exists(d.FullName));
            Assert.True(IOUtility.Exists(child.FullName));

            IOUtility.Delete(f.FullName);
            Assert.False(IOUtility.Exists(f.FullName));

            IOUtility.Delete(dir.FullName);
            Assert.False(IOUtility.Exists(d.FullName));
            Assert.False(IOUtility.Exists(child.FullName));
            Assert.False(IOUtility.Exists(dir.FullName));
        }

        [Fact]
        public async Task DeleteAsyncTest()
        {
            var dir = TestIO.InitializeMethod(this);

            var f = TestIO.CreateEmptyFile(dir, "f");
            var d = TestIO.CreateDirectory(dir, "d");

            var child = TestIO.CreateEmptyFile(d, "child");

            Assert.True(IOUtility.Exists(f.FullName));
            Assert.True(IOUtility.Exists(d.FullName));
            Assert.True(IOUtility.Exists(child.FullName));

            await IOUtility.DeleteAsync(f.FullName);
            Assert.False(IOUtility.Exists(f.FullName));

            await IOUtility.DeleteAsync(dir.FullName);
            Assert.False(IOUtility.Exists(d.FullName));
            Assert.False(IOUtility.Exists(child.FullName));
            Assert.False(IOUtility.Exists(dir.FullName));
        }

        [Fact]
        public void CreateTemporaryDirectoryTest()
        {
            var tmp = IOUtility.CreateTemporaryDirectory();
            var dir = tmp.Directory;
            using(tmp) {
                dir.Refresh();
                Assert.True(dir.Exists);
            }
            dir.Refresh();
            Assert.False(dir.Exists);
        }

        [Fact]
        public void CreateTemporaryDirectory_Prefix_Test()
        {
            var tmp = IOUtility.CreateTemporaryDirectory(new TemporaryDirectoryOptions {
                Prefix = "prefix_"
            });
            var dir = tmp.Directory;
            using(tmp) {
                dir.Refresh();
                Assert.True(dir.Exists);
            }
            dir.Refresh();
            Assert.False(dir.Exists);
        }

        [Fact]
        public void CreateTemporaryFileTest()
        {
            var tmp = IOUtility.CreateTemporaryFile();
            var file = tmp.File;
            using(tmp) {
                file.Refresh();
                Assert.True(file.Exists);
            }
            file.Refresh();
            Assert.False(file.Exists);
        }

        [Fact]
        public void CreateTemporaryFile_Stream_Test()
        {
            var tmp = IOUtility.CreateTemporaryFile();
            using(tmp) {
                tmp.DoStream(s => {
                    using var w = new StreamWriter(s, leaveOpen: true);
                    w.Write("abc");
                });

                tmp.DoStream(s => {
                    s.Position = 0;
                    using var r = new StreamReader(s, leaveOpen: true);
                    var actual = r.ReadLine();
                    Assert.Equal("abc", actual);
                });

                tmp.DoStream(s => {
                    using var w = new StreamWriter(s, leaveOpen: true);
                    w.Write("def");
                });

                tmp.DoStream(s => {
                    s.Position = 0;
                    using var r = new StreamReader(s, leaveOpen: true);
                    var actual = r.ReadLine();
                    Assert.Equal("abcdef", actual);
                });

                using var r2 = new StreamReader(tmp.CreateStream());
                Assert.Equal("abcdef", r2.ReadToEnd());

                using var w2 = new StreamWriter(tmp.CreateStream());
                w2.Write("ABC");
                w2.Flush();

                using var r3 = new StreamReader(tmp.CreateStream());
                Assert.Equal("ABCdef", r3.ReadToEnd());
            }
        }

        [Fact]
        public void CreateTemporaryFile_PS_Test()
        {
            var tmp = IOUtility.CreateTemporaryFile(new TemporaryFileOptions {
                Prefix = "prefix_",
                Suffix = ".tmp",
            });
            var file = tmp.File;
            using(tmp) {
                file.Refresh();
                Assert.True(file.Exists);
            }
            file.Refresh();
            Assert.False(file.Exists);
        }

        #endregion
    }
}
