using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    public class ThrowsTest
    {
        [Fact]
        public void ThrowIf_Target_Test()
        {
            Assert.Throws<NullReferenceException>(() => Throws.ThrowIf<NullReferenceException>(false));
            var exception1 = Record.Exception(() => Throws.ThrowIf<NullReferenceException>(true));
            Assert.Null(exception1);

            Assert.Throws<NotImplementedException>(() => Throws.ThrowIf<NotImplementedException>(false));
            var exception2 = Record.Exception(() => Throws.ThrowIf<NotImplementedException>(true));
            Assert.Null(exception2);
        }

        [Fact]
        public void ThrowIf_Default_Test()
        {
            var exception1 = Assert.Throws<LogicException>(() => Throws.ThrowIf<LogicException>(1 == 0));
            Assert.Equal("1 == 0", exception1.Message);

            var a = 1;
            var b = 0;
            var exception2 = Assert.Throws<LogicException>(() => Throws.ThrowIf<LogicException>(a == b));
            Assert.Equal("a == b", exception2.Message);
        }

        [Fact]
        public void ThrowIfNull_Target_Test()
        {
            Assert.Throws<NullReferenceException>(() => Throws.ThrowIfNull<object, NullReferenceException>(default));
            Assert.Throws<NotImplementedException>(() => Throws.ThrowIfNull<object, NotImplementedException>(default));
        }

        [Fact]
        public void ThrowIfNull_Default_Test()
        {
            Assert.Throws<LogicException>(() => Throws.ThrowIfNull(default(object)));

            var exception = Record.Exception(() => Throws.ThrowIfNull<object>(new object()));
            Assert.Null(exception);
        }

        [Fact]
        public void ThrowIfNullOrEmpty_Target_Test()
        {
            Assert.Throws<NullReferenceException>(() => Throws.ThrowIfNullOrEmpty<NullReferenceException>(default));
            Assert.Throws<NullReferenceException>(() => Throws.ThrowIfNullOrEmpty<NullReferenceException>(""));
            Throws.ThrowIfNullOrEmpty<NullReferenceException>(" ");
            Throws.ThrowIfNullOrEmpty<NullReferenceException>("a");

            Assert.Throws<NotImplementedException>(() => Throws.ThrowIfNullOrEmpty<NotImplementedException>(default));
            Assert.Throws<NotImplementedException>(() => Throws.ThrowIfNullOrEmpty<NotImplementedException>(""));
            Throws.ThrowIfNullOrEmpty<NotImplementedException>(" ");
            Throws.ThrowIfNullOrEmpty<NotImplementedException>("a");
        }

        [Fact]
        public void ThrowIfNullOrWhiteSpace_Target_Test()
        {
            Assert.Throws<NullReferenceException>(() => Throws.ThrowIfNullOrWhiteSpace<NullReferenceException>(default));
            Assert.Throws<NullReferenceException>(() => Throws.ThrowIfNullOrWhiteSpace<NullReferenceException>(""));
            Assert.Throws<NullReferenceException>(() => Throws.ThrowIfNullOrWhiteSpace<NullReferenceException>(" "));
            Throws.ThrowIfNullOrWhiteSpace<NullReferenceException>("a");

            Assert.Throws<NotImplementedException>(() => Throws.ThrowIfNullOrWhiteSpace<NotImplementedException>(default));
            Assert.Throws<NotImplementedException>(() => Throws.ThrowIfNullOrWhiteSpace<NotImplementedException>(""));
            Assert.Throws<NotImplementedException>(() => Throws.ThrowIfNullOrWhiteSpace<NotImplementedException>(" "));
            Throws.ThrowIfNullOrWhiteSpace<NotImplementedException>("a");
        }
    }
}
