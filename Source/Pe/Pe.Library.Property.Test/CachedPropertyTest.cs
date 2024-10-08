using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Property;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Property.Test
{
    public class CachedPropertyTest
    {
        public class Class
        {
            #region property

            public int EditableNumber { get; set; } = 123;
            public int ReadonlyNumber { get; } = 777;

            #endregion
        }

        #region function

        [Fact]
        public void Get_EditableNumber_Test()
        {
            var obj = new Class();
            var test = new CachedProperty(obj);
            var actual = test.Get(nameof(obj.EditableNumber));
            Assert.Equal(123, actual);
        }

        [Fact]
        public void Set_EditableNumber_Test()
        {
            var obj = new Class();
            var test = new CachedProperty(obj);
            test.Set(nameof(obj.EditableNumber), 456);
            Assert.Equal(456, test.Get(nameof(obj.EditableNumber)));
            Assert.Equal(456, obj.EditableNumber);
        }

        [Fact]
        public void Get_ReadonlyNumber_Test()
        {
            var obj = new Class();
            var test = new CachedProperty(obj);
            var actual = test.Get(nameof(obj.ReadonlyNumber));
            Assert.Equal(777, actual);
        }

        [Fact]
        public void Set_ReadonlyNumber_Test()
        {
            var obj = new Class();
            var test = new CachedProperty(obj);
            Assert.Throws<NotSupportedException>(() => test.Set(nameof(obj.ReadonlyNumber), 789));
        }

        [Fact]
        public void Get_not_found_property_Test()
        {
            var obj = new Class();
            var test = new CachedProperty(obj);
            Assert.Throws<PropertyNotFoundException>(() => test.Get("<NAME>"));
        }

        [Fact]
        public void Set_not_found_property_Test()
        {
            var obj = new Class();
            var test = new CachedProperty(obj);
            Assert.Throws<PropertyNotFoundException>(() => test.Set("<NAME>", 3.14));
        }


        #endregion
    }
}
