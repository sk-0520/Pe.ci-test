using System;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Property.Test
{
    public class PropertyAccessorTest
    {
        #region define

        private class Get
        {
            private int Property { get; } = 1;
        }

        private class GetSet
        {
            private int Property { get; set; }
        }

        #endregion

        #region function

        [Fact]
        public void GetterTest()
        {
            var gi = new Get();
            var gp = new PropertyAccessor<Get, int>(gi, "Property");
            var gi1 = gp.Get(gi);
            Assert.Equal(1, gi1);
        }

        [Fact]
        public void SetterTest()
        {
            {
                var gi = new Get();
                var gp = new PropertyAccessor<Get, int>(gi, "Property");
                Assert.False(gp.PropertyInfo.CanWrite);
                Assert.Throws<NotSupportedException>(() => gp.Set(gi, 100));
            }

            {
                var gsi = new GetSet();
                var gsp = new PropertyAccessor<GetSet, int>(gsi, "Property");
                Assert.True(gsp.PropertyInfo.CanWrite);
                gsp.Set(gsi, 100);
                var gsi1 = gsp.Get(gsi);
                Assert.Equal(100, gsi1);
            }

        }

        [Fact]
        public void Constructor_throw_Test()
        {
            var obj = new Get();
            Assert.Throws<PropertyNotFoundException>(() => new PropertyAccessor(obj, "NotFoundProperty"));
        }

        #endregion
    }
}
