using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    public class EnforceTest
    {
        [Fact]
        public void ThrowIf_Target_Test()
        {
            Assert.Throws<NullReferenceException>(() => Enforce.ThrowIf<NullReferenceException>(false));
            Enforce.ThrowIf<NullReferenceException>(true);
            Assert.True(true);

            Assert.Throws<NotImplementedException>(() => Enforce.ThrowIf<NotImplementedException>(false));
            Enforce.ThrowIf<NotImplementedException>(true);
            Assert.True(true);
        }

        [Fact]
        public void ThrowIf_Default_Test()
        {
            try {
                Enforce.ThrowIf(1 == 0);
                Assert.Fail();
            } catch(EnforceException ex) {
                Assert.Equal("1 == 0", ex.Message);
            }

            try {
                var a = 1;
                var b = 0;
                Enforce.ThrowIf(a == b);
                Assert.Fail();
            } catch(EnforceException ex) {
                Assert.Equal("a == b", ex.Message);
            }
        }

        [Fact]
        public void ThrowIfNull_Target_Test()
        {
            Assert.Throws<NullReferenceException>(() => Enforce.ThrowIfNull<object, NullReferenceException>(default));
            Assert.Throws<NotImplementedException>(() => Enforce.ThrowIfNull<object, NotImplementedException>(default));
        }

        [Fact]
        public void ThrowIfNull_Default_Test()
        {
            Assert.Throws<EnforceException>(() => Enforce.ThrowIfNull(default(object)));
        }

        [Fact]
        public void ThrowIfNullOrEmpty_Target_Test()
        {
            Assert.Throws<NullReferenceException>(() => Enforce.ThrowIfNullOrEmpty<NullReferenceException>(default));
            Assert.Throws<NullReferenceException>(() => Enforce.ThrowIfNullOrEmpty<NullReferenceException>(""));
            Enforce.ThrowIfNullOrEmpty<NullReferenceException>(" ");
            Enforce.ThrowIfNullOrEmpty<NullReferenceException>("a");
            Assert.True(true);

            Assert.Throws<NotImplementedException>(() => Enforce.ThrowIfNullOrEmpty<NotImplementedException>(default));
            Assert.Throws<NotImplementedException>(() => Enforce.ThrowIfNullOrEmpty<NotImplementedException>(""));
            Enforce.ThrowIfNullOrEmpty<NotImplementedException>(" ");
            Enforce.ThrowIfNullOrEmpty<NotImplementedException>("a");
            Assert.True(true);
        }

        [Fact]
        public void ThrowIfNullOrWhiteSpace_Target_Test()
        {
            Assert.Throws<NullReferenceException>(() => Enforce.ThrowIfNullOrWhiteSpace<NullReferenceException>(default));
            Assert.Throws<NullReferenceException>(() => Enforce.ThrowIfNullOrWhiteSpace<NullReferenceException>(""));
            Assert.Throws<NullReferenceException>(() => Enforce.ThrowIfNullOrWhiteSpace<NullReferenceException>(" "));
            Enforce.ThrowIfNullOrWhiteSpace<NullReferenceException>("a");
            Assert.True(true);

            Assert.Throws<NotImplementedException>(() => Enforce.ThrowIfNullOrWhiteSpace<NotImplementedException>(default));
            Assert.Throws<NotImplementedException>(() => Enforce.ThrowIfNullOrWhiteSpace<NotImplementedException>(""));
            Assert.Throws<NotImplementedException>(() => Enforce.ThrowIfNullOrWhiteSpace<NotImplementedException>(" "));
            Enforce.ThrowIfNullOrWhiteSpace<NotImplementedException>("a");
            Assert.True(true);
        }
    }
}
