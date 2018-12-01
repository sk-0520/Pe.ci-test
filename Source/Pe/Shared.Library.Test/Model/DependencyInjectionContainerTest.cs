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
    public class DependencyInjectionContainerTest
    {
        #region define

        interface I1
        {
            int Add(int a, int b);
        }

        class C1: I1
        {
            public int Add(int a, int b) => a + b;
        }

        class C2
        {
            public C2(I1 i1) => I1 = i1;

            I1 I1 { get; }

            public int Plus(int a, int b) => I1.Add(a, b);
        }

        class C3
        {
            public C3(int a, int b)
            {
                A = a;
                B = b;
            }

            int A { get; }
            int B { get; }
            public int Get() => A + B;
        }

        class C4
        {
            public C4(int a, I1 i1, int b)
            {
                A = a;
                B = b;
                I1 = i1;
            }

            int A { get; }
            int B { get; }
            I1 I1 { get; }
            public int Get() => I1.Add( A, B);
        }

        abstract class C5
        {
            public C5(int a, int b, IEnumerable<I1> i1s)
            {
                A = a;
                B = b;
                I1 = i1s.ToArray();
            }

            int A { get; }
            int B { get; }
            I1[] I1 { get; }
            public int Get() => I1.Sum(i => i.Add(A, B));
        }

        class C5_LongLong: C5
        {
            public C5_LongLong(int a, I1 i1, int b)
                :base(a, b, new[] { i1 })
            { }

            public C5_LongLong(int a, I1 i1, int b, I1 i2, I1 i3)
                :base(a, b, new[] { i1, i2, i3, })
            { }

            private C5_LongLong(int a, I1 i1, int b, I1 i2, I1 i3, I1 i4)
                :base(a, b, new[] { i1, i2, i3, i4 })
            { }
        }

        class C5_Private : C5
        {
            public C5_Private(int a, I1 i1, int b)
                :base(a, b, new[] { i1 })
            { }

            public C5_Private(int a, I1 i1, int b, I1 i2, I1 i3)
                :base(a, b, new[] { i1, i2, i3, })
            { }

            [DiInjection]
            private C5_Private(int a, I1 i1, int b, I1 i2, I1 i3, I1 i4)
                : base(a, b, new[] { i1, i2, i3, i4 })
            { }
        }

        class C5_Minimum : C5
        {
            [DiInjection]
            public C5_Minimum(int a, I1 i1, int b)
                : base(a, b, new[] { i1 })
            { }

            public C5_Minimum(int a, I1 i1, int b, I1 i2, I1 i3)
                : base(a, b, new[] { i1, i2, i3, })
            { }

            private C5_Minimum(int a, I1 i1, int b, I1 i2, I1 i3, I1 i4)
                : base(a, b, new[] { i1, i2, i3, i4 })
            { }
        }

        #endregion

        [TestMethod]
        public void GetTest_Create()
        {
            var dic = new DependencyInjectionContainer();
            dic.Add<I1, C1>(DiLifecycle.Create);

            var i1_1 = dic.Get<I1>();
            Assert.AreEqual(3, i1_1.Add(1, 2));

            var i1_2 = dic.Get<I1>();
            Assert.AreEqual(30, i1_2.Add(10, 20));

            Assert.IsFalse(i1_1 == i1_2);
        }

        [TestMethod]
        public void GetTest_Singleton()
        {
            var dic = new DependencyInjectionContainer();
            dic.Add<I1, C1>(DiLifecycle.Singleton);

            var i1_1 = dic.Get<I1>();
            Assert.AreEqual(3, i1_1.Add(1, 2));

            var i1_2 = dic.Get<I1>();
            Assert.AreEqual(30, i1_2.Add(10, 20));

            Assert.IsTrue(i1_1 == i1_2);
        }

        [TestMethod]
        public void NewTest_C1()
        {
            var dic = new DependencyInjectionContainer();
            dic.Add<I1, C1>(DiLifecycle.Create);

            // 引数のない人はそのまんま生成される
            var c1 = dic.New<C1>();
            Assert.AreEqual(4, c1.Add(2, 2));
        }

        [TestMethod]
        public void NewTest_C2()
        {
            var dic = new DependencyInjectionContainer();
            dic.Add<I1, C1>(DiLifecycle.Create);

            // 引数から頑張ってパラメータ割り当て
            var c2 = dic.New<C2>();
            Assert.AreEqual(-1, c2.Plus(1, -2));
        }

        [TestMethod]
        public void NewTest_Manual_C3()
        {
            var dic = new DependencyInjectionContainer();
            dic.Add<I1, C1>(DiLifecycle.Create);

            Assert.ThrowsException<Exception>(() => dic.New<C3>(new object[] { 1 }));

            var c3 = dic.New<C3>(new object[] { 1, 10 });
            Assert.AreEqual(11, c3.Get());
        }

        [TestMethod]
        public void NewTest_Manual_C4()
        {
            var dic = new DependencyInjectionContainer();
            dic.Add<I1, C1>(DiLifecycle.Create);

            Assert.ThrowsException<Exception>(() => dic.New<C4>(new object[] { 1 }));

            var c4 = dic.New<C4>(new object[] { 99, 1 });
            Assert.AreEqual(100, c4.Get());
        }

        [TestMethod]
        public void NewTest_Manual_C5_LongLong()
        {
            var dic = new DependencyInjectionContainer();
            dic.Add<I1, C1>(DiLifecycle.Create);

            var c5 = dic.New<C5_LongLong>(new object[] { 99, 1 });
            Assert.AreEqual((99 + 1) * 3, c5.Get());
        }

        [TestMethod]
        public void NewTest_Manual_C5_Private()
        {
            var dic = new DependencyInjectionContainer();
            dic.Add<I1, C1>(DiLifecycle.Create);

            var c5 = dic.New<C5_Private>(new object[] { 99, 1 });
            Assert.AreEqual((99 + 1) * 4, c5.Get());
        }

        [TestMethod]
        public void NewTest_Manual_C5_Minimum()
        {
            var dic = new DependencyInjectionContainer();
            dic.Add<I1, C1>(DiLifecycle.Create);

            var c5 = dic.New<C5_Minimum>(new object[] { 99, 1 });
            Assert.AreEqual((99 + 1) * 1, c5.Get());
        }

    }
}

