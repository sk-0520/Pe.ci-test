using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.Standard.Base.Test
{
    public class Get
    {
        private int Property { get; } = 1;
    }

    public class GetSet
    {
        private int Property { get; set; }
    }

    public interface ITypeData
    {
        int Property { get; set; }
    }

    public class TypeData: ITypeData
    {
        public int Property { get; set; }
        private string Custom { get; set; } = string.Empty;
    }

    public class PropertyFactoryTest
    {
        #region function

        [Fact]
        public void CreateGetterTest()
        {
            var gi = new Get();
            var go = PropertyExpressionFactory.CreateOwner(gi);
            var propertyGetter = PropertyExpressionFactory.CreateGetter<Get, int>(go, "Property");
            var gi1 = propertyGetter(gi);
            Assert.Equal(1, gi1);
        }

        [Fact]
        public void CreateSetterOnlyTest()
        {
            var gsi = new Get();
            var gso = PropertyExpressionFactory.CreateOwner(gsi);
            var exception = Record.Exception(() => PropertyExpressionFactory.CreateGetter<Get, int>(gso, "Property"));
            Assert.Null(exception);
            Assert.Throws<PropertyCanNotWriteException>(() => PropertyExpressionFactory.CreateSetter<Get, int>(gso, "Property"));
        }

        [Fact]
        public void CreateSetterTest()
        {
            var gsi = new GetSet();
            var gso = PropertyExpressionFactory.CreateOwner(gsi);
            var propertyGetter = PropertyExpressionFactory.CreateGetter<GetSet, int>(gso, "Property");
            var propertySetter = PropertyExpressionFactory.CreateSetter<GetSet, int>(gso, "Property");
            propertySetter(gsi, 10);
            var gi1 = propertyGetter(gsi);
            Assert.Equal(10, gi1);
        }

        [Fact]
        public void CreateObjectGetterSetterTest()
        {
            var gsi = new GetSet();
            var gso = PropertyExpressionFactory.CreateOwner(gsi);
            var propertyGetter = PropertyExpressionFactory.CreateGetter<GetSet, object>(gso, "Property");
            var propertySetter = PropertyExpressionFactory.CreateSetter<GetSet, object>(gso, "Property");
            propertySetter(gsi, 10);
            var gi1 = propertyGetter(gsi);
            Assert.Equal(10, gi1);
        }

        [Fact]
        public void CreateDelegateGetterSetterTest()
        {
            var gsi = new GetSet();
            var gso = PropertyExpressionFactory.CreateOwner(gsi);
            var propertyGetter = PropertyExpressionFactory.CreateGetter(gso, "Property");
            var propertySetter = PropertyExpressionFactory.CreateSetter(gso, "Property");
            propertySetter.DynamicInvoke(gsi, 10);
            var gi1 = propertyGetter.DynamicInvoke(gsi);
            Assert.Equal(10, gi1);
        }

        [Fact]
        public void CreateType_interface_Test()
        {
            var td = new TypeData();

            var tdo = PropertyExpressionFactory.CreateOwner<ITypeData>();
            var propertyGetter = PropertyExpressionFactory.CreateGetter(tdo, "Property");
            var propertySetter = PropertyExpressionFactory.CreateSetter(tdo, "Property");

            propertySetter.DynamicInvoke(td, 10);
            var gi1 = propertyGetter.DynamicInvoke(td);
            Assert.Equal(10, gi1);
            Assert.Equal(10, td.Property);
        }

        [Fact]
        public void CreateType_interface_no_property_Test()
        {
            var td = new TypeData();

            var tdo = PropertyExpressionFactory.CreateOwner<ITypeData>();
            // 型から作成しているので存在しないプロパティは許容されない
            Assert.Throws<ArgumentException>(() => PropertyExpressionFactory.CreateGetter(tdo, "Custom"));
            Assert.Throws<ArgumentException>(() => PropertyExpressionFactory.CreateSetter(tdo, "Custom"));
        }


        #endregion
    }

    public class PropertyAccessorTest
    {
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

        #endregion
    }

    public class PropertyAccessorFactoryTest
    {
        #region function


        #endregion
    }
}
