using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Library.Base;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Base.Test
{
    public class ObjectDumperTest
    {
        class Simple
        {
#pragma warning disable 414
            int PrivateF = 1;
#pragma warning restore 414

            int PrivateP { get; set; } = 2; // 自動実装

            public int PublicF = 3;
            public int PublicP { get; set; } = 4; // 自動実装
        }

        class Nest2
        {
            public string Value { get; } = "aaa";
            public Simple NotNull { get; } = new Simple();
            public Simple? Null { get; }
        }

        class Nest3
        {
            public string Value { get; } = "aaa";
            public Simple NotNullSimle { get; } = new Simple();
            public Simple? NullSimle { get; }
            public Nest2 NotNullNest2 { get; } = new Nest2();
            public Nest2? NullNest2 { get; }
        }

        [Fact]
        public void Dump_Flat_Test()
        {
            var od = new ObjectDumper();
            var simple = new Simple();

            Assert.Throws<ArgumentException>(() => od.Dump(simple, 0, false));

            var items1 = od.Dump(simple, 1, true).ToList();
            Assert.Equal(4, items1.Count);

            var items2 = od.Dump(simple, 1, false).ToList();
            Assert.Equal(6, items2.Count);
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Assertions", "xUnit2013:Do not use equality check to check for collection size.", Justification = "<保留中>")]
        public void Dump_Nest2_1_Test()
        {
            var od = new ObjectDumper();
            var nest = new Nest2();

            var items = od.Dump(nest, 1, true).ToList();
            Assert.Equal(3, items.Count);

            Assert.Equal(0, items.First(i => i.MemberInfo.Name == nameof(Nest2.Value)).Children.Count);
            Assert.Equal("aaa", items.First(i => i.MemberInfo.Name == nameof(Nest2.Value)).Value);

            Assert.Equal(0, items.First(i => i.MemberInfo.Name == nameof(Nest2.NotNull)).Children.Count);
        }

        [Fact]
        public void Dump_Nest2_2_Test()
        {
            var od = new ObjectDumper();
            var nest = new Nest2();

            var items = od.Dump(nest, 2, true);
            Assert.Equal(3, items.Count);

            Assert.Empty(items.First(i => i.MemberInfo.Name == nameof(Nest2.Value)).Children);
            Assert.Equal("aaa", items.First(i => i.MemberInfo.Name == nameof(Nest2.Value)).Value);

            Assert.Empty(items.First(i => i.MemberInfo.Name == nameof(Nest2.NotNull)).Children);
        }

        [Fact]
        public void Dump_Nest3_Test()
        {
            var od = new ObjectDumper();
            var nest3 = new Nest3();

            var items = od.Dump(nest3);
            Assert.Equal(5, items.Count);

            //Assert.Equal(3, items.First(i => i.MemberInfo.Name == nameof(Nest3.NotNullNest2)).Children.Count);
            //Assert.Equal(4, items.First(i => i.MemberInfo.Name == nameof(Nest3.NotNullNest2)).Children.First(i => i.MemberInfo.Name == nameof(Nest2.NotNull)).Children.Count);
            //Assert.Equal(3, items.First(i => i.MemberInfo.Name == nameof(Nest3.NotNullNest2)).Children.First(i => i.MemberInfo.Name == nameof(Nest2.NotNull)).Children.First(i => i.MemberInfo.Name == nameof(Simple.PublicF)).Value);
        }

        [Fact]
        public void DictionaryTest()
        {
            var od = new ObjectDumper();
            var dic = new Dictionary<string, object>() {
                ["a"] = 1,
                ["b"] = "2",
                ["c"] = 0.3m
            };

            try {
                var items = od.Dump(dic);
                Assert.True(true);
            } catch(StackOverflowException ex) {
                Assert.Fail(ex.Message);
            }
        }
    }
}
