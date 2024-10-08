using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Base.Test
{
    public class PathUtilityTest
    {
        [Theory]
        [InlineData("a.txt", "a", "txt")]
        [InlineData("a.txt.txt", "a.txt", "txt")]
        [InlineData("a.txt", "a.", "txt")]
        [InlineData("a.txt", "a", ".txt")]
        [InlineData("a.txt", "a.....", "......txt")]
        [InlineData("a.txt.", "a.....", "......txt.")]
        public void AddExtensionTest(string expected, string path, string ext)
        {
            var actual = PathUtility.AddExtension(path, ext);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "", "!")]
        [InlineData("", " ", "!")]
        [InlineData("a", "a", "!")]
#if OS_WINDOWS
        [InlineData("a!", "a?", "!")]
        [InlineData("a!", "a/", "!")]
        [InlineData("a/", "a/", "/")]
        [InlineData("a!", "a\\", "!")]
        [InlineData("a@b@c@d@e", "a?b\\c*d/e", "@")]
        [InlineData("a<>b<>c<>d<>e", "a?b\\c*d/e", "<>")]
#else
        [InlineData("a?", "a?", "!")]
        [InlineData("a!", "a/", "!")]
        [InlineData("a/", "a/", "/")]
        [InlineData("a\\", "a\\", "!")]
        [InlineData("a?b\\c*d@e", "a?b\\c*d/e", "@")]
        [InlineData("a?b\\c*d<>e", "a?b\\c*d/e", "<>")]
#endif
        public void ToSafeNameTest(string expected, string value, string c)
        {
            var actual = PathUtility.ToSafeName(value, v => c);
            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData("", "")]
        [InlineData("", " ")]
        [InlineData("a", "a")]
#if OS_WINDOWS
        [InlineData("a_", "a?")]
        [InlineData("a_b_c_d_e", "a?b\\c*d/e")]
#else
        [InlineData("a?", "a?")]
        [InlineData("a?b\\c*d_e", "a?b\\c*d/e")]
#endif
        public void ToSafeNameDefaultTest(string expected, string value)
        {
            var actual = PathUtility.ToSafeNameDefault(value);
            Assert.Equal(actual, expected);
        }

        [Theory]
#if OS_WINDOWS
        [InlineData(false, "exe")]
        [InlineData(false, "dll")]
        [InlineData(true, ".exe")]
        [InlineData(true, ".dll")]
        [InlineData(false, ".ico")]
        [InlineData(true, "a.exe")]
        [InlineData(true, "a.dll")]
        [InlineData(false, "a.ico")]
#else
        [InlineData(false, "exe")]
        [InlineData(false, "dll")]
        [InlineData(false, ".exe")]
        [InlineData(false, ".dll")]
        [InlineData(false, ".ico")]
        [InlineData(false, "a.exe")]
        [InlineData(false, "a.dll")]
        [InlineData(false, "a.ico")]
#endif
        public void HasIconTest(bool expected, string value)
        {
            var actual = PathUtility.HasIconPath(value);
            Assert.Equal(expected, actual);
        }

        [Theory]
#if OS_WINDOWS
        [InlineData(false, @"")]
        [InlineData(false, @"/")]
        [InlineData(false, @"//")]
        [InlineData(false, @"\")]
        [InlineData(false, @"\\")]
        [InlineData(true, @"\\a")]
#else
        [InlineData(false, @"")]
        [InlineData(false, @"/")]
        [InlineData(false, @"//")]
        [InlineData(false, @"\")]
        [InlineData(false, @"\\")]
        [InlineData(false, @"\\a")]
#endif
        public void IsNetworkDirectoryPathTest(bool expected, string value)
        {
            var actual = PathUtility.IsNetworkDirectoryPath(value);
            Assert.Equal(expected, actual);
        }

        [Theory]
#if OS_WINDOWS
        [InlineData(null, @"")]
        [InlineData(null, @"A")]
        [InlineData(null, @"\")]
        [InlineData("a", @"\a")]
        [InlineData("a", @"\\a")]
        [InlineData(null, @"\\a\")]
        [InlineData("b", @"\\a\b")]
        [InlineData("b", @"\\a\\b")]
#else
        [InlineData(null, @"")]
        [InlineData(null, @"A")]
        [InlineData(null, @"\")]
        [InlineData(null, @"\a")]
        [InlineData(null, @"\\a")]
        [InlineData(null, @"\\a\")]
        [InlineData(null, @"\\a\b")]
        [InlineData(null, @"\\a\\b")]
#endif
        public void GetNetworkDirectoryName(string? expected, string value)
        {
            var actual = PathUtility.GetNetworkDirectoryName(value);
            Assert.Equal(expected, actual);
        }

        [Theory]
#if OS_WINDOWS
        [InlineData(null, @"")]
        [InlineData(null, @"a")]
        [InlineData("", @"\a")]
        [InlineData(@"\a", @"\a\")]
        [InlineData(@"\a", @"\a\b")]
        [InlineData(@"\a\b", @"\a\b\c")]
#else
        [InlineData(null, @"")]
        [InlineData(null, @"a")]
        [InlineData(null, @"\a")]
        [InlineData(null, @"\a\")]
        [InlineData(null, @"\a\b")]
        [InlineData(null, @"\a\b\c")]
#endif
        public void GetNetworkOwnerName(string? expected, string value)
        {
            var actual = PathUtility.GetNetworkOwnerName(value);
            Assert.Equal(expected, actual);
        }

#if OS_WINDOWS

        [Theory]
        [InlineData(false, null)]
        [InlineData(false, @"")]
        [InlineData(false, @"A")]
        [InlineData(true, @"\A")]
        [InlineData(true, @"\\A")]
        //[InlineData(true, @"\\A")]
        [InlineData(true, @"\\A\")]
        [InlineData(false, @"\\A\B")]
        [InlineData(true, @"\\C:")]
        [InlineData(true, @"\\C:\")]
        [InlineData(false, @"\\C:\B")]
        [InlineData(true, @"C:")]
        [InlineData(true, @"C:\")]
        [InlineData(false, @"C:\A")]
        public void IsRootNameTest(bool expected, string? value)
        {
            var actual = PathUtility.IsRootName(value);
            Assert.Equal(expected, actual);
        }

#endif

        [Theory]
        [InlineData(true, null, null)]
        [InlineData(false, "", null)]
        [InlineData(false, null, "")]
        [InlineData(true, "", "")]
#if OS_WINDOWS
        [InlineData(true, "a", "A")]
        [InlineData(true, "A", "a")]
        [InlineData(true, "A", "A")]
        [InlineData(false, "A", "B")]
        [InlineData(true, "A\\", "A\\")]
        [InlineData(true, "A\\", "a\\")]
        [InlineData(true, "A\\", "a")]
        [InlineData(true, "A", "a\\")]
        [InlineData(true, "A\\", "A/")]
        [InlineData(true, "A/", "A/")]
        [InlineData(true, "A\\", "a/")]
        //[InlineData(true, "A\\", "a")]
        [InlineData(true, "A", "a/")]
#endif
        public void IsEqualsTest(bool expected, string? a, string? b)
        {
            var actual = PathUtility.IsEquals(a, b);
            Assert.Equal(expected, actual);
        }

#if OS_WINDOWS

        [Theory]
        [InlineData(new[] { "a" }, "a")]
        [InlineData(new[] { "a", "b" }, "a/b")]
        [InlineData(new[] { "a", "b", "c" }, "a/b/c")]
        [InlineData(new[] { "a", "b", "c" }, "/a/b/c")]
        [InlineData(new[] { "a", "b", "c" }, "a/b/c/")]
        [InlineData(new[] { "a", "b", "c" }, "/a/b/c/")]
        [InlineData(new[] { "a", "b", "c" }, "\\a\\b\\c\\")]
        public void SplitTest(string[] expected, string input)
        {
            var actual = PathUtility.Split(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(@"", @"", @"")]
        [InlineData(@"a", @"a", @"")]
        [InlineData(@"A", @"", @"A")]
        [InlineData(@"C:\dir", @"C:\", @"dir")]
        [InlineData(@"C:\dir\sub", @"C:\dir", @"sub")]
        [InlineData(@"C:\dir\sub", @"C:\dir\", @"\sub")]
        [InlineData(@"C:\dir\sub\sub2", @"C:\dir\", @"sub/./sub2")]
        [InlineData(@"C:\dir\sub2", @"C:\dir\", @"sub/../sub2")]
        //[InlineData(@"C:\dir\sub\sub2", @"C:\dir\", @"sub/./sub2")]
        [InlineData(@"C:\dir\next", @"C:\dir\", @"sub/../../../../next")]
        [InlineData(@"sub2\sub3", @"", @"sub/../../../../sub2/next/../sub3")]
        public void SafeCombineTest(string expected, string directory, string nodes)
        {
            var actual = PathUtility.SafeCombine(directory, nodes);
            Assert.Equal(expected, actual);
        }
#endif
    }
}
