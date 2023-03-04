using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Standard.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Standard.DependencyInjection.Test
{
    [TestClass]
    public class DiContainerTest
    {
        #region define

        interface I0
        {
            int Func(int a);
        }

        class C0: I0
        {
            public C0(Func<int, int> f)
            {
                F = f;
            }
            Func<int, int> F { get; }

            public int Func(int a) => F(a);

        }

        interface I1
        {
            int Func(int a, int b);
        }

        interface Idmy1
        {
            int Func(int a, int b);
        }

        class C1: I1
        {
            public int Func(int a, int b) => a + b;
        }
        class C1_other: I1
        {
            public int Func(int a, int b) => a - b;
        }

        class C1_Func: I1
        {
            public C1_Func(Func<int, int, int> func)
            {
                F = func;
            }

            Func<int, int, int> F { get; }
            public int Func(int a, int b) => F(a, b);
        }

        class Cdmy1: Idmy1
        {
            public int Func(int a, int b) => a + b;
        }

        class C2
        {
            public C2(I1 i1) => I1 = i1;

            I1 I1 { get; }

            public int Plus(int a, int b) => I1.Func(a, b);
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
            public int Get() => I1.Func(A, B);
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
            public int Get() => I1.Sum(i => i.Func(A, B));
        }

        class C5_LongLong: C5
        {
            public C5_LongLong(int a, I1 i1, int b)
                : base(a, b, new[] { i1 })
            { }

            public C5_LongLong(int a, I1 i1, int b, I1 i2, I1 i3)
                : base(a, b, new[] { i1, i2, i3, })
            { }

            private C5_LongLong(int a, I1 i1, int b, I1 i2, I1 i3, I1 i4)
                : base(a, b, new[] { i1, i2, i3, i4 })
            { }
        }

        class C5_Private: C5
        {
            public C5_Private(int a, I1 i1, int b)
                : base(a, b, new[] { i1 })
            { }

            public C5_Private(int a, I1 i1, int b, I1 i2, I1 i3)
                : base(a, b, new[] { i1, i2, i3, })
            { }

            [Inject]
            private C5_Private(int a, I1 i1, int b, I1 i2, I1 i3, I1 i4)
                : base(a, b, new[] { i1, i2, i3, i4 })
            { }
        }

        class C5_Minimum: C5
        {
            [Inject]
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

#pragma warning disable 169, 649
        class C6
        {
            private I1? fieldUnset_private;
            public I1? fieldUnset_public;
            [Inject]
            private I1? fieldSet_private;
            [Inject]
            public I1? fieldSet_public;

            private I1? PropertyUnset_private { get; set; }
            public I1? PropertyUnset_public { get; set; }
            [Inject]
            private I1? PropertySet_private { get; set; }
            [Inject]
            public I1? PropertySet_public { get; set; }
        }

#if ENABLED_STRUCT
        struct S1
        {
            private I1 fieldUnset_private;
            public I1 fieldUnset_public;
            [DiInjection]
            private I1 fieldSet_private;
            [DiInjection]
            public I1 fieldSet_public;

            private I1 PropertyUnset_private { get; set; }
            public I1 PropertyUnset_public { get; set; }
            [DiInjection]
            private I1 PropertySet_private { get; set; }
            [DiInjection]
            public I1 PropertySet_public { get; set; }
        }
#endif
#pragma warning restore 169, 649

        #endregion

        [TestMethod]
        public void GetTest_Create()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);

            var i1_1 = dic.Get<I1>();
            Assert.AreEqual(3, i1_1.Func(1, 2));

            var i1_2 = dic.Get<I1>();
            Assert.AreEqual(30, i1_2.Func(10, 20));

            Assert.IsFalse(i1_1 == i1_2);
        }

        [TestMethod]
        public void GetTest_Singleton()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Singleton);

            var i1_1 = dic.Get<I1>();
            Assert.AreEqual(3, i1_1.Func(1, 2));

            var i1_2 = dic.Get<I1>();
            Assert.AreEqual(30, i1_2.Func(10, 20));

            Assert.IsTrue(i1_1 == i1_2);
        }

        [TestMethod]
        public void GetTest_Singleton2()
        {
            using(var dic1 = new DiContainer()) {
                dic1.Register<ActionDisposer, ActionDisposer>(DiLifecycle.Singleton, () => new ActionDisposer(d => { Assert.IsFalse(d); }));
            }

            ActionDisposer ad2;
            using(var dic2 = new DiContainer()) {
                dic2.Register<ActionDisposer, ActionDisposer>(DiLifecycle.Singleton, () => new ActionDisposer(d => { Assert.IsTrue(d); }));
                ad2 = dic2.Get<ActionDisposer>();
            }

        }

        [TestMethod]
        public void GetTest_Name_Create()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);
            dic.Register<I1, C1_other>("other", DiLifecycle.Transient);

            Assert.AreEqual(3, dic.Get<I1>().Func(1, 2));
            Assert.AreEqual(-1, dic.Get<I1>("other").Func(1, 2));


            dic.Register<string, string>("NAMELESS");
            dic.Register<string, string>("name", "NAMED");

            Assert.AreEqual("NAMELESS", dic.Get<string>());
            Assert.AreEqual("NAMED", dic.Get<string>("name"));

            dic.Register<I0, C0>(DiLifecycle.Transient, () => new C0(a => a + 2));
            dic.Register<I0, C0>("b", DiLifecycle.Transient, () => new C0(a => a * 2));

            Assert.AreEqual(5, dic.Get<I0>().Func(3));
            Assert.AreEqual(6, dic.Get<I0>("b").Func(3));

        }

        [TestMethod]
        public void NewTest_I1()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);
            dic.Register<I1, C1_other>("name", DiLifecycle.Transient);

            // 引数のない人はそのまんま生成される
            var i1 = dic.New<I1>();
            Assert.AreEqual(10, i1.Func(4, 6));

            var i2 = dic.New<I1>("name");
            Assert.AreEqual(-2, i2.Func(4, 6));
        }

        [TestMethod]
        public void NewTest_I1_2()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(new C1());

            var i1 = dic.New<I1>();
            Assert.AreEqual(10, i1.Func(4, 6));
            var i1_2 = dic.New<C1>();
            Assert.AreEqual(100, i1_2.Func(40, 60));
        }

        [TestMethod]
        public void NewTest_C1()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);

            var c1 = dic.New<C1>();
            Assert.AreEqual(4, c1.Func(2, 2));
        }

        [TestMethod]
        public void NewTest_C1toC1()
        {
            var dic = new DiContainer();
            dic.Register<C1, C1>(DiLifecycle.Transient);

            var c1 = dic.New<C1>();
            Assert.AreEqual(4, c1.Func(2, 2));
        }

#if false
        [TestMethod]
        public void NewTest_C1toI1()
        {
            var dic = new DiContainer();
            Assert.ThrowsException<ArgumentException>(() => dic.Register<C1, I1>(DiLifecycle.Transient));
        }
#endif

        [TestMethod]
        public void NewTest_C2()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);

            // 引数から頑張ってパラメータ割り当て
            var c2 = dic.New<C2>();
            Assert.AreEqual(-1, c2.Plus(1, -2));
        }

        [TestMethod]
        public void NewTest_Manual_C3()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);

            Assert.ThrowsException<DiException>(() => dic.New<C3>(new object[] { 1 }));

            var c3 = dic.New<C3>(new object[] { 1, 10 });
            Assert.AreEqual(11, c3.Get());
        }

        [TestMethod]
        public void NewTest_Manual_C4()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);

            Assert.ThrowsException<DiException>(() => dic.New<C4>(new object[] { 1 }));

            var c4 = dic.New<C4>(new object[] { 99, 1 });
            Assert.AreEqual(100, c4.Get());
        }

        [TestMethod]
        public void NewTest_Manual_C5_LongLong()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);

            var c5 = dic.New<C5_LongLong>(new object[] { 99, 1 });
            Assert.AreEqual((99 + 1) * 3, c5.Get());
        }

        [TestMethod]
        public void NewTest_Manual_C5_Private()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);

            var c5 = dic.New<C5_Private>(new object[] { 99, 1 });
            Assert.AreEqual((99 + 1) * 4, c5.Get());
        }

        [TestMethod]
        public void NewTest_Manual_C5_Minimum()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);

            var c5 = dic.New<C5_Minimum>(new object[] { 99, 1 });
            Assert.AreEqual((99 + 1) * 1, c5.Get());
        }

#if PrivateObject
        [TestMethod]
        public void InjectTest_C6()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);

            var c = dic.New<C6>();
            dic.Inject(c);
            var p = new PrivateObject(c);
            Assert.IsNull(c.fieldUnset_public);
            Assert.IsNull(p.GetField("fieldUnset_private"));
            Assert.IsNotNull(c.fieldSet_public);
            Assert.IsNotNull(p.GetField("fieldSet_private"));

            Assert.IsNull(c.PropertyUnset_public);
            Assert.IsNull(p.GetProperty("PropertyUnset_private"));
            Assert.IsNotNull(c.PropertySet_public);
            Assert.IsNotNull(p.GetProperty("PropertySet_private"));
        }
#endif

#if ENABLED_STRUCT
        [TestMethod]
        public void InjectTest_S1()
        {
            var dic = new DependencyInjectionContainer();
            dic.Add<I1, C1>(DiLifecycle.Create);

            // わっからん！
            //var s = dic.New<S1>();
            var s = new S1();
            dic.Inject(ref s);
            var p = new PrivateObject(s);
            Assert.IsNull(s.fieldUnset_public);
            Assert.IsNull(p.GetField("fieldUnset_private"));
            Assert.IsNotNull(s.fieldSet_public);
            Assert.IsNotNull(p.GetField("fieldSet_private"));

            Assert.IsNull(s.PropertyUnset_public);
            Assert.IsNull(p.GetProperty("PropertyUnset_private"));
            Assert.IsNotNull(s.PropertySet_public);
            Assert.IsNotNull(p.GetProperty("PropertySet_private"));
        }
#endif

        #region nest

        interface INest1
        {
            INest2 Nest2 { get; }
        }

        interface INest2
        {
            INest3 Nest3 { get; }
        }

        interface INest3
        {
            INest4 Nest4 { get; }
        }

        interface INest4
        {
            bool True { get; }
        }

        class Root
        {
            public Root(INest1 nest1, INest2 nest2, INest3 nest3, INest4 nest4)
            {
                Nest1 = nest1;
                Nest2 = nest2;
                Nest3 = nest3;
                Nest4 = nest4;
            }

            public INest1 Nest1 { get; }
            public INest2 Nest2 { get; }
            public INest3 Nest3 { get; }
            public INest4 Nest4 { get; }
        }

        class Nest1: INest1
        {
            public Nest1(INest2 nest2)
            {
                Nest2 = nest2;
            }

            public INest2 Nest2 { get; }
        }

        class Nest2: INest2
        {
            public Nest2(INest3 nest3)
            {
                Nest3 = nest3;
            }

            public INest3 Nest3 { get; }
        }

        class Nest3: INest3
        {
            public Nest3(INest4 nest4)
            {
                Nest4 = nest4;
            }

            public INest4 Nest4 { get; }
        }

        class Nest4: INest4
        {
            public bool True => true;
        }

        [TestMethod]
        public void NestTest()
        {
            var dic = new DiContainer();
            dic.Register<INest1, Nest1>(DiLifecycle.Transient);
            dic.Register<INest2, Nest2>(DiLifecycle.Transient);
            dic.Register<INest3, Nest3>(DiLifecycle.Transient);
            dic.Register<INest4, Nest4>(DiLifecycle.Transient);

            var root = dic.New<Root>();

            Assert.IsTrue(root.Nest1.Nest2.Nest3.Nest4.True);
            Assert.IsTrue(root.Nest2.Nest3.Nest4.True);
            Assert.IsTrue(root.Nest3.Nest4.True);
            Assert.IsTrue(root.Nest4.True);

            Assert.IsFalse(root.Nest2 == root.Nest1.Nest2);
            Assert.IsFalse(root.Nest3 == root.Nest1.Nest2.Nest3);
            Assert.IsFalse(root.Nest4 == root.Nest1.Nest2.Nest3.Nest4);
        }

        [TestMethod]
        public void NestTest_Singleton()
        {
            var dic = new DiContainer();
            dic.Register<INest1, Nest1>(DiLifecycle.Singleton);
            dic.Register<INest2, Nest2>(DiLifecycle.Singleton);
            dic.Register<INest3, Nest3>(DiLifecycle.Singleton);
            dic.Register<INest4, Nest4>(DiLifecycle.Singleton);

            var root = dic.New<Root>();

            Assert.IsTrue(root.Nest1.Nest2.Nest3.Nest4.True);
            Assert.IsTrue(root.Nest2.Nest3.Nest4.True);
            Assert.IsTrue(root.Nest3.Nest4.True);
            Assert.IsTrue(root.Nest4.True);

            Assert.IsTrue(root.Nest2 == root.Nest1.Nest2);
            Assert.IsTrue(root.Nest3 == root.Nest1.Nest2.Nest3);
            Assert.IsTrue(root.Nest4 == root.Nest1.Nest2.Nest3.Nest4);
        }

        #endregion

        class CScopeA: I1
        {
            public int Func(int a, int b) => a + b;
        }
        class CScopeB: I1
        {
            public int Func(int a, int b) => a - b;
        }
        class CScopeC: I1
        {
            public int Func(int a, int b) => a * b;
        }
        class CScopeD: I1
        {
            public int Func(int a, int b) => a / b;
        }

        [TestMethod]
        public void ScopeTest()
        {
            var dic1 = new DiContainer();

            dic1.Register<I1, CScopeA>(DiLifecycle.Transient);
            Assert.AreEqual(10, dic1.New<I1>().Func(3, 7));

            using(var dic2 = dic1.Scope()) {
                Assert.AreEqual(10, dic2.New<I1>().Func(3, 7));

                dic2.Register<I1, CScopeB>(DiLifecycle.Transient);
                Assert.AreEqual(-4, dic2.New<I1>().Func(3, 7));
                Assert.AreEqual(10, dic1.New<I1>().Func(3, 7));

                Assert.ThrowsException<ArgumentException>(() => dic2.Register<I1, CScopeB>(DiLifecycle.Transient));

                using(var dic3 = dic2.Scope()) {
                    Assert.AreEqual(-4, dic3.New<I1>().Func(3, 7));

                    dic3.Register<I1, CScopeC>(DiLifecycle.Transient);
                    Assert.AreEqual(21, dic3.New<I1>().Func(3, 7));
                    Assert.AreEqual(-4, dic2.New<I1>().Func(3, 7));
                    Assert.AreEqual(10, dic1.New<I1>().Func(3, 7));

                    Assert.ThrowsException<ArgumentException>(() => dic3.Register<I1, CScopeC>(DiLifecycle.Transient));

                    using(var dic4 = dic3.Scope()) {
                        Assert.AreEqual(21, dic4.New<I1>().Func(3, 7));

                        dic4.Register<I1, CScopeD>(DiLifecycle.Transient);
                        Assert.AreEqual(2, dic4.New<I1>().Func(10, 5));
                        Assert.AreEqual(21, dic3.New<I1>().Func(3, 7));
                        Assert.AreEqual(-4, dic2.New<I1>().Func(3, 7));
                        Assert.AreEqual(10, dic1.New<I1>().Func(3, 7));

                        Assert.ThrowsException<ArgumentException>(() => dic4.Register<I1, CScopeD>(DiLifecycle.Transient));

                    }
                }
            }
        }

        class D1
        {
            I1? I1_1 { get; set; }
            I1? I1_2 { get; set; }
            public I1? I1_3 { get; set; }
            public I1? I1_4 { get; set; }
        }

        [TestMethod]
        public void RegisterMemberTest()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);

            dic.RegisterMember<D1, I1>("I1_1");
            Assert.ThrowsException<NullReferenceException>(() => dic.RegisterMember<D1, I1>("I1_0"));
            Assert.ThrowsException<ArgumentException>(() => dic.RegisterMember<D1, I1>("I1_1"));
        }

#if PrivateObject
        [TestMethod]
        public void DirtyRegisterTest_New()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);

            dic.DirtyRegister<D1, I1>("I1_1");
            dic.DirtyRegister<D1, I1>(nameof(D1.I1_3));

            var d = new D1();
            dic.Inject(d);

            var p = new PrivateObject(d);

            Assert.IsNotNull(p.GetProperty("I1_1"));
            Assert.IsNull(p.GetProperty("I1_2"));
            Assert.IsNotNull(d.I1_3);
            Assert.IsNull(d.I1_4);
        }
#endif
        [TestMethod]
        public void UnregisterTest()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);
            var i1 = dic.New<I1>();
            Assert.IsNotNull(i1);

            Assert.IsFalse(dic.Unregister<C1>());
            Assert.IsTrue(dic.Unregister<I1>());
            Assert.ThrowsException<DiException>(() => dic.New<I1>());
        }

        interface ID2
        {
            I1 I1_2 { get; }

            I1 I1_4 { get; }

            I1 I1_6 { get; }
        }

#pragma warning disable 169, 649
        class D2: ID2
        {
            [Inject]
            public I1? I1_1;
            [Inject]
#pragma warning disable CS8613 // 戻り値の型における参照型の Null 許容性が、暗黙的に実装されるメンバーと一致しません。
#pragma warning disable CS8618 // Null 非許容フィールドが初期化されていません。
            public I1 I1_2 { get; set; }
#pragma warning restore CS8618 // Null 非許容フィールドが初期化されていません。
#pragma warning restore CS8613 // 戻り値の型における参照型の Null 許容性が、暗黙的に実装されるメンバーと一致しません。

            public I1? I1_3;
#pragma warning disable CS8618 // Null 非許容フィールドが初期化されていません。
            public I1 I1_4 { get; set; }
#pragma warning restore CS8618 // Null 非許容フィールドが初期化されていません。

            public I1? I1_5;
#pragma warning disable CS8618 // Null 非許容フィールドが初期化されていません。
            public I1 I1_6 { get; set; }
#pragma warning restore CS8618 // Null 非許容フィールドが初期化されていません。
        }
#pragma warning restore 169, 649

        [TestMethod]
        public void BuildTest()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);
            dic.Register<ID2, D2>(DiLifecycle.Transient);

            dic.RegisterMember<D2, I1>("I1_3");
            dic.RegisterMember<D2, I1>(nameof(D1.I1_4));

            var d = (D2)dic.Build<ID2>();
            Assert.IsNotNull(d.I1_1);
            Assert.IsNotNull(d.I1_2);
            Assert.IsNotNull(d.I1_3);
            Assert.IsNotNull(d.I1_4);
            Assert.IsNull(d.I1_5);
            Assert.IsNull(d.I1_6);
        }

        class Wrap
        {
            public Wrap(string s) => S = s;
            public string S { get; set; } = string.Empty;

        }

        class NamedClass
        {
            [Inject]
            public Wrap A { get; set; } = new Wrap(string.Empty);
            [Inject("name")]
            public Wrap B { get; set; } = new Wrap(string.Empty);
            //BUGS: A とおなじ扱いでいいんだけどなんかむずいぞ
            //[Inject("notfound")]
            //public Wrap C { get; set; } = new Wrap(string.Empty);
        }


        [TestMethod]
        public void Inject_Name_Test()
        {
            var dic = new DiContainer();
            dic.Register<Wrap, Wrap>(new Wrap("a"));
            dic.Register<Wrap, Wrap>("name", new Wrap("b"));

            var nc = dic.New<NamedClass>();
            Assert.AreEqual(string.Empty, nc.A.S);
            Assert.AreEqual(string.Empty, nc.B.S);
            //Assert.AreEqual(string.Empty, nc.C.S);

            dic.Inject(nc);
            Assert.AreEqual("a", nc.A.S);
            Assert.AreEqual("b", nc.B.S);
            //Assert.AreEqual("a", nc.C.S);

            var nc2 = dic.Build<NamedClass>();
            Assert.AreEqual("a", nc2.A.S);
            Assert.AreEqual("b", nc2.B.S);
            //Assert.AreEqual("a", nc2.C.S);

        }

        class NamedClass2
        {
            public NamedClass2(string a, [Inject("named")] string b, [Inject("notfound")] string c)
            {
                A = a;
                B = b;
                C = c;
            }

            public string A { get; } = string.Empty;
            public string B { get; } = string.Empty;
            public string C { get; } = string.Empty;
        }
        [TestMethod]
        public void New_Name_Test()
        {
            var dic = new DiContainer();
            dic.Register<string, string>("a");
            dic.Register<string, string>("named", "b");

            var nc = dic.New<NamedClass2>();
            Assert.AreEqual("a", nc.A);
            Assert.AreEqual("b", nc.B);
            Assert.AreEqual("a", nc.C);
        }

        public class CallClass
        {
            public int FuncInt_0()
            {
                return 10;
            }
            public string FuncStr_0()
            {
                return "str";
            }
            public void Action_0()
            {
                //nop
            }

            public string FuncStr_1(string s)
            {
                return s + ":" + s;
            }

            public string FuncStr_2(string a, string b)
            {
                return a + ":" + b;
            }
        }

        [TestMethod]
        public void CallMethod_0_Test()
        {
            var dic = new DiContainer();
            var callClass = new CallClass();

            var methodInt = callClass.GetType().GetMethod(nameof(callClass.FuncInt_0))!;
            var acutalInt = dic.CallMethod(string.Empty, callClass, methodInt, Array.Empty<object>());
            Assert.AreEqual(10, acutalInt);

            var methodStr = callClass.GetType().GetMethod(nameof(callClass.FuncStr_0))!;
            var acutalStr = dic.CallMethod(string.Empty, callClass, methodStr, Array.Empty<object>());
            Assert.AreEqual("str", acutalStr);

            var methodVoid = callClass.GetType().GetMethod(nameof(callClass.Action_0))!;
            var acutalVoid = dic.CallMethod(string.Empty, callClass, methodVoid, Array.Empty<object>());
            Assert.IsNull(acutalVoid);
        }

        [TestMethod]
        public void CallMethod_1_Test()
        {
            var dic = new DiContainer();
            dic.Register<string, string>("a");
            dic.Register<string, string>("named", "b");

            var callClass = dic.New<CallClass>();
            var methodStr = callClass.GetType().GetMethod(nameof(callClass.FuncStr_1))!;

            var acutalEmptyName1 = dic.CallMethod(string.Empty, callClass, methodStr, Array.Empty<object>());
            Assert.AreEqual("a:a", acutalEmptyName1);

            var acutalDefinedName1 = dic.CallMethod("named", callClass, methodStr, Array.Empty<object>());
            Assert.AreEqual("b:b", acutalDefinedName1);

            var acutalEmptyName2 = dic.CallMethod(string.Empty, callClass, methodStr, new[] { "A" });
            Assert.AreEqual("A:A", acutalEmptyName2);

            var acutalDefinedName2 = dic.CallMethod("named", callClass, methodStr, new[] { "B" });
            Assert.AreEqual("B:B", acutalDefinedName2);
        }

        [TestMethod]
        public void CallMethod_2_Test()
        {
            var dic = new DiContainer();
            dic.Register<string, string>("a");
            dic.Register<string, string>("named", "b");

            var callClass = dic.New<CallClass>();
            var methodStr = callClass.GetType().GetMethod(nameof(callClass.FuncStr_2))!;

            var acutalEmptyName1 = dic.CallMethod(string.Empty, callClass, methodStr, Array.Empty<object>());
            Assert.AreEqual("a:a", acutalEmptyName1);
            var acutalEmptyName1_Call = dic.Call<string>(callClass, methodStr.Name);
            Assert.AreEqual("a:a", acutalEmptyName1_Call);

            var acutalDefinedName1 = dic.CallMethod("named", callClass, methodStr, Array.Empty<object>());
            Assert.AreEqual("b:b", acutalDefinedName1);

            var acutalEmptyName2 = dic.CallMethod(string.Empty, callClass, methodStr, new[] { "A" });
            Assert.AreEqual("A:a", acutalEmptyName2);
            var acutalEmptyName2_Call = dic.Call<string>(callClass, methodStr.Name, "A");
            Assert.AreEqual("A:a", acutalEmptyName2_Call);

            var acutalDefinedName2 = dic.CallMethod("named", callClass, methodStr, new[] { "B" });
            Assert.AreEqual("B:b", acutalDefinedName2);

            var acutalEmptyName3 = dic.CallMethod(string.Empty, callClass, methodStr, new[] { "A", "A'" });
            Assert.AreEqual("A:A'", acutalEmptyName3);
            var acutalEmptyName3_Call = dic.Call<string>(callClass, methodStr.Name, "A", "A");
            Assert.AreEqual("A:A", acutalEmptyName3_Call);

            var acutalDefinedName3 = dic.CallMethod("named", callClass, methodStr, new[] { "B", "B'" });
            Assert.AreEqual("B:B'", acutalDefinedName3);

            //あかん
            //var acutalEmptyName4 = dic.CallMethod(string.Empty, callClass, methodStr, new object[] { new DiDefaultParameter(typeof(string)), "A" });
            //Assert.AreEqual("a:A", acutalEmptyName4);

            //var acutalDefinedName4 = dic.CallMethod("named", callClass, methodStr, new object[] { new DiDefaultParameter(typeof(string)), "B" });
            //Assert.AreEqual("b:B", acutalDefinedName4);
        }
    }

}
