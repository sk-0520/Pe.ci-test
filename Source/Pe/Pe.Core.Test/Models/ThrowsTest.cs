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
            Throws.ThrowIf<NullReferenceException>(true);
            Assert.True(true);

            Assert.Throws<NotImplementedException>(() => Throws.ThrowIf<NotImplementedException>(false));
            Throws.ThrowIf<NotImplementedException>(true);
            Assert.True(true);
        }

        [Fact]
        public void ThrowIf_Default_Test()
        {
            try {
                Throws.ThrowIf(1 == 0);
                Assert.Fail();
            } catch(LogicException ex) {
                Assert.Equal("1 == 0", ex.Message);
            }

            try {
                var a = 1;
                var b = 0;
                Throws.ThrowIf(a == b);
                Assert.Fail();
            } catch(LogicException ex) {
                Assert.Equal("a == b", ex.Message);
            }
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
        }

        [Fact]
        public void ThrowIfNullOrEmpty_Target_Test()
        {
            Assert.Throws<NullReferenceException>(() => Throws.ThrowIfNullOrEmpty<NullReferenceException>(default));
            Assert.Throws<NullReferenceException>(() => Throws.ThrowIfNullOrEmpty<NullReferenceException>(""));
            Throws.ThrowIfNullOrEmpty<NullReferenceException>(" ");
            Throws.ThrowIfNullOrEmpty<NullReferenceException>("a");
            Assert.True(true);

            Assert.Throws<NotImplementedException>(() => Throws.ThrowIfNullOrEmpty<NotImplementedException>(default));
            Assert.Throws<NotImplementedException>(() => Throws.ThrowIfNullOrEmpty<NotImplementedException>(""));
            Throws.ThrowIfNullOrEmpty<NotImplementedException>(" ");
            Throws.ThrowIfNullOrEmpty<NotImplementedException>("a");
            Assert.True(true);
        }

        [Fact]
        public void ThrowIfNullOrWhiteSpace_Target_Test()
        {
            Assert.Throws<NullReferenceException>(() => Throws.ThrowIfNullOrWhiteSpace<NullReferenceException>(default));
            Assert.Throws<NullReferenceException>(() => Throws.ThrowIfNullOrWhiteSpace<NullReferenceException>(""));
            Assert.Throws<NullReferenceException>(() => Throws.ThrowIfNullOrWhiteSpace<NullReferenceException>(" "));
            Throws.ThrowIfNullOrWhiteSpace<NullReferenceException>("a");
            Assert.True(true);

            Assert.Throws<NotImplementedException>(() => Throws.ThrowIfNullOrWhiteSpace<NotImplementedException>(default));
            Assert.Throws<NotImplementedException>(() => Throws.ThrowIfNullOrWhiteSpace<NotImplementedException>(""));
            Assert.Throws<NotImplementedException>(() => Throws.ThrowIfNullOrWhiteSpace<NotImplementedException>(" "));
            Throws.ThrowIfNullOrWhiteSpace<NotImplementedException>("a");
            Assert.True(true);
        }
    }
}
