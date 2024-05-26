using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Unmanaged;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Unmanaged
{
    public class ComTest
    {
        #region function

        [Fact]
        public void Constructor_IShellLink_Test()
        {
            using var test = new Com<IShellLink>((IShellLink)new ShellLinkObject());
            Assert.NotNull(test);
        }

        [Fact]
        public void Constructor_throw_Test()
        {
            var exception = Assert.Throws<ArgumentException>(() => new Com<object>(new object()));
            Assert.Equal("comInstance", exception.ParamName);
            Assert.StartsWith("Marshal.IsComObject", exception.Message);
        }

        [Fact]
        public void CastTest()
        {
            using var test = new Com<IShellLink>((IShellLink)new ShellLinkObject());
            using var actual1 = test.Cast<IPersistFile>();
            Assert.NotNull(actual1);

            using var actual2 = test.Cast<IPersistFile>();
            Assert.NotNull(actual2);

            Assert.NotEqual(actual1, actual2);
        }

        [Fact]
        public void Cast_throw_Test()
        {
            using var test = new Com<IShellLink>((IShellLink)new ShellLinkObject());
            Assert.Throws<InvalidCastException>(() => test.Cast<IImageList>());
        }

        #endregion
    }

    public class ComWrapperTest
    {
        #region function

        [Fact]
        public void CreateTest()
        {
            using var actual = ComWrapper.Create((IShellLink)new ShellLinkObject());
            Assert.NotNull(actual);
        }


        [Fact]
        public void Create_throw_Test()
        {
            var exception = Assert.Throws<ArgumentException>(() => ComWrapper.Create(new object()));
            Assert.Equal("comInstance", exception.ParamName);
            Assert.StartsWith("Marshal.IsComObject", exception.Message);
        }

        #endregion
    }
}
