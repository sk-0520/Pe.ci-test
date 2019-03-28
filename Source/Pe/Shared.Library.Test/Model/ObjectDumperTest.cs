using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shared.Library.Test.Model
{
    [TestClass]
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
            public Simple Null { get; }
        }

        class Nest3
        {
            public string Value { get; } = "aaa";
            public Simple NotNullSimle { get; } = new Simple();
            public Simple NullSimle { get; }
            public Nest2 NotNullNest2 { get; } = new Nest2();
            public Nest2 NullNest2 { get; }
        }

        [TestMethod]
        public void Dump_Flat_Test()
        {
            var od = new ObjectDumper();
            var simple = new Simple();

            Assert.ThrowsException<ArgumentException>(() => od.Dump(simple, 0, false));

            var items1 = od.Dump(simple, 1, true).ToList();
            Assert.AreEqual(4, items1.Count);

            var items2 = od.Dump(simple, 1, false).ToList();
            Assert.AreEqual(6, items2.Count);
        }

        [TestMethod]
        public void Dump_Nest2_1_Test()
        {
            var od = new ObjectDumper();
            var nest = new Nest2();

            var items = od.Dump(nest, 1, true).ToList();
            Assert.AreEqual(3, items.Count);

            Assert.AreEqual(0, items.First(i => i.MemberInfo.Name == nameof(Nest2.Value)).Children.Count);
            Assert.AreEqual("aaa", items.First(i => i.MemberInfo.Name == nameof(Nest2.Value)).Value);

            Assert.AreEqual(0, items.First(i => i.MemberInfo.Name == nameof(Nest2.NotNull)).Children.Count);
        }

        [TestMethod]
        public void Dump_Nest2_2_Test()
        {
            var od = new ObjectDumper();
            var nest = new Nest2();

            var items = od.Dump(nest, 2, true);
            Assert.AreEqual(3, items.Count);

            Assert.AreEqual(0, items.First(i => i.MemberInfo.Name == nameof(Nest2.Value)).Children.Count);
            Assert.AreEqual("aaa", items.First(i => i.MemberInfo.Name == nameof(Nest2.Value)).Value);

            Assert.AreEqual(4, items.First(i => i.MemberInfo.Name == nameof(Nest2.NotNull)).Children.Count);

        }

        [TestMethod]
        public void Dump_Nest3_Test()
        {
            var od = new ObjectDumper();
            var nest3 = new Nest3();

            var items = od.Dump(nest3);
            Assert.AreEqual(5, items.Count);

            Assert.AreEqual(3, items.First(i => i.MemberInfo.Name == nameof(Nest3.NotNullNest2)).Children.Count);
            Assert.AreEqual(4, items.First(i => i.MemberInfo.Name == nameof(Nest3.NotNullNest2)).Children.First(i => i.MemberInfo.Name == nameof(Nest2.NotNull)).Children.Count);
            Assert.AreEqual(3, items.First(i => i.MemberInfo.Name == nameof(Nest3.NotNullNest2)).Children.First(i => i.MemberInfo.Name == nameof(Nest2.NotNull)).Children.First(i => i.MemberInfo.Name == nameof(Simple.PublicF)).Value);

        }
    }
}
