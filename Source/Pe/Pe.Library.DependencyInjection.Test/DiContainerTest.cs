using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.CommonTest;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.DependencyInjection.Test
{
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

            [DiInjection]
            private C5_Private(int a, I1 i1, int b, I1 i2, I1 i3, I1 i4)
                : base(a, b, new[] { i1, i2, i3, i4 })
            { }
        }

        class C5_Minimum: C5
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

#pragma warning disable 169, 649
        class C6
        {
            private I1? fieldUnset_private;
            public I1? fieldUnset_public;
            [DiInjection]
            private I1? fieldSet_private;
            [DiInjection]
            public I1? fieldSet_public;

            private I1? PropertyUnset_private { get; set; }
            public I1? PropertyUnset_public { get; set; }
            [DiInjection]
            private I1? PropertySet_private { get; set; }
            [DiInjection]
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

        [Fact]
        public void NestTest()
        {
            var dic = new DiContainer();
            dic.Register<INest1, Nest1>(DiLifecycle.Transient);
            dic.Register<INest2, Nest2>(DiLifecycle.Transient);
            dic.Register<INest3, Nest3>(DiLifecycle.Transient);
            dic.Register<INest4, Nest4>(DiLifecycle.Transient);

            var root = dic.New<Root>();

            Assert.True(root.Nest1.Nest2.Nest3.Nest4.True);
            Assert.True(root.Nest2.Nest3.Nest4.True);
            Assert.True(root.Nest3.Nest4.True);
            Assert.True(root.Nest4.True);

            Assert.False(root.Nest2 == root.Nest1.Nest2);
            Assert.False(root.Nest3 == root.Nest1.Nest2.Nest3);
            Assert.False(root.Nest4 == root.Nest1.Nest2.Nest3.Nest4);
        }

        [Fact]
        public void NestTest_Singleton()
        {
            var dic = new DiContainer();
            dic.Register<INest1, Nest1>(DiLifecycle.Singleton);
            dic.Register<INest2, Nest2>(DiLifecycle.Singleton);
            dic.Register<INest3, Nest3>(DiLifecycle.Singleton);
            dic.Register<INest4, Nest4>(DiLifecycle.Singleton);

            var root = dic.New<Root>();

            Assert.True(root.Nest1.Nest2.Nest3.Nest4.True);
            Assert.True(root.Nest2.Nest3.Nest4.True);
            Assert.True(root.Nest3.Nest4.True);
            Assert.True(root.Nest4.True);

            Assert.True(root.Nest2 == root.Nest1.Nest2);
            Assert.True(root.Nest3 == root.Nest1.Nest2.Nest3);
            Assert.True(root.Nest4 == root.Nest1.Nest2.Nest3.Nest4);
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



        class D1
        {
            I1? I1_1 { get; set; }
            I1? I1_2 { get; set; }
            public I1? I1_3 { get; set; }
            public I1? I1_4 { get; set; }
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
            [DiInjection]
            public I1? I1_1;
            [DiInjection]
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

        class Wrap
        {
            public Wrap(string s) => S = s;
            public string S { get; set; } = string.Empty;

        }

        class NamedClass
        {
            [DiInjection]
            public Wrap A { get; set; } = new Wrap(string.Empty);
            [DiInjection("name")]
            public Wrap B { get; set; } = new Wrap(string.Empty);
            //BUGS: A とおなじ扱いでいいんだけどなんかむずいぞ
            //[Inject("notfound")]
            //public Wrap C { get; set; } = new Wrap(string.Empty);
        }

        class NamedClass2
        {
            public NamedClass2(string a, [DiInjection("named")] string b, [DiInjection("notfound")] string c)
            {
                A = a;
                B = b;
                C = c;
            }

            public string A { get; } = string.Empty;
            public string B { get; } = string.Empty;
            public string C { get; } = string.Empty;
        }


        public class CallClass
        {
            public int Action0CallCount { get; private set; }
            public int Action1LastValue { get; private set; }

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
                Action0CallCount += 1;
            }

            public void Action_1(int value)
            {
                Action1LastValue = value;
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

        #endregion

        [Fact]
        public void GetTest_Create()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);

            var i1_1 = dic.Get<I1>();
            Assert.Equal(3, i1_1.Func(1, 2));

            var i1_2 = dic.Get<I1>();
            Assert.Equal(30, i1_2.Func(10, 20));

            Assert.False(i1_1 == i1_2);
        }

        [Fact]
        public void GetTest_Singleton()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Singleton);

            var i1_1 = dic.Get<I1>();
            Assert.Equal(3, i1_1.Func(1, 2));

            var i1_2 = dic.Get<I1>();
            Assert.Equal(30, i1_2.Func(10, 20));

            Assert.True(i1_1 == i1_2);
        }

        [Fact]
        public void GetTest_Singleton2()
        {
            using(var dic1 = new DiContainer()) {
                dic1.Register<ActionDisposer, ActionDisposer>(DiLifecycle.Singleton, () => new ActionDisposer(d => { Assert.False(d); }));
            }

            ActionDisposer ad2;
            using(var dic2 = new DiContainer()) {
                dic2.Register<ActionDisposer, ActionDisposer>(DiLifecycle.Singleton, () => new ActionDisposer(d => { Assert.True(d); }));
                ad2 = dic2.Get<ActionDisposer>();
            }

        }

        [Fact]
        public void GetTest_Name_Create()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);
            dic.Register<I1, C1_other>("other", DiLifecycle.Transient);

            Assert.Equal(3, dic.Get<I1>().Func(1, 2));
            Assert.Equal(-1, dic.Get<I1>("other").Func(1, 2));


            dic.Register<string, string>("NAMELESS");
            dic.Register<string, string>("name", "NAMED");

            Assert.Equal("NAMELESS", dic.Get<string>());
            Assert.Equal("NAMED", dic.Get<string>("name"));

            dic.Register<I0, C0>(DiLifecycle.Transient, () => new C0(a => a + 2));
            dic.Register<I0, C0>("b", DiLifecycle.Transient, () => new C0(a => a * 2));

            Assert.Equal(5, dic.Get<I0>().Func(3));
            Assert.Equal(6, dic.Get<I0>("b").Func(3));

        }

        [Fact]
        public void NewTest_I1()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);
            dic.Register<I1, C1_other>("name", DiLifecycle.Transient);

            // 引数のない人はそのまんま生成される
            var i1 = dic.New<I1>();
            Assert.Equal(10, i1.Func(4, 6));

            var i2 = dic.New<I1>("name");
            Assert.Equal(-2, i2.Func(4, 6));
        }

        [Fact]
        public void NewTest_I1_2()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(new C1());

            var i1 = dic.New<I1>();
            Assert.Equal(10, i1.Func(4, 6));
            var i1_2 = dic.New<C1>();
            Assert.Equal(100, i1_2.Func(40, 60));
        }

        [Fact]
        public void NewTest_C1()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);

            var c1 = dic.New<C1>();
            Assert.Equal(4, c1.Func(2, 2));
        }

        [Fact]
        public void NewTest_C1toC1()
        {
            var dic = new DiContainer();
            dic.Register<C1, C1>(DiLifecycle.Transient);

            var c1 = dic.New<C1>();
            Assert.Equal(4, c1.Func(2, 2));
        }

#if false
        [Fact]
        public void NewTest_C1toI1()
        {
            var dic = new DiContainer();
            Assert.Throws<ArgumentException>(() => dic.Register<C1, I1>(DiLifecycle.Transient));
        }
#endif

        [Fact]
        public void NewTest_C2()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);

            // 引数から頑張ってパラメータ割り当て
            var c2 = dic.New<C2>();
            Assert.Equal(-1, c2.Plus(1, -2));
        }

        [Fact]
        public void NewTest_Manual_C3()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);

            Assert.Throws<DiException>(() => dic.New<C3>(new object[] { 1 }));

            var c3 = dic.New<C3>(new object[] { 1, 10 });
            Assert.Equal(11, c3.Get());
        }

        [Fact]
        public void NewTest_Manual_C4()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);

            Assert.Throws<DiException>(() => dic.New<C4>(new object[] { 1 }));

            var c4 = dic.New<C4>(new object[] { 99, 1 });
            Assert.Equal(100, c4.Get());
        }

        [Fact]
        public void NewTest_Manual_C5_LongLong()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);

            var c5 = dic.New<C5_LongLong>(new object[] { 99, 1 });
            Assert.Equal((99 + 1) * 3, c5.Get());
        }

        [Fact]
        public void NewTest_Manual_C5_Private()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);

            var c5 = dic.New<C5_Private>(new object[] { 99, 1 });
            Assert.Equal((99 + 1) * 4, c5.Get());
        }

        [Fact]
        public void NewTest_Manual_C5_Minimum()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);

            var c5 = dic.New<C5_Minimum>(new object[] { 99, 1 });
            Assert.Equal((99 + 1) * 1, c5.Get());
        }

        [Fact]
        public void InjectTest_C6()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);

            var c = dic.New<C6>();
            dic.Inject(c);
            var p = new PrivateObject(c);
            Assert.Null(c.fieldUnset_public);
            Assert.Null(p.GetField("fieldUnset_private"));
            Assert.NotNull(c.fieldSet_public);
            Assert.NotNull(p.GetField("fieldSet_private"));

            Assert.Null(c.PropertyUnset_public);
            Assert.Null(p.GetProperty("PropertyUnset_private"));
            Assert.NotNull(c.PropertySet_public);
            Assert.NotNull(p.GetProperty("PropertySet_private"));
        }

#if ENABLED_STRUCT
        [Fact]
        public void InjectTest_S1()
        {
            var dic = new DependencyInjectionContainer();
            dic.Add<I1, C1>(DiLifecycle.Create);

            // わっからん！
            //var s = dic.New<S1>();
            var s = new S1();
            dic.Inject(ref s);
            var p = new PrivateObject(s);
            Assert.Null(s.fieldUnset_public);
            Assert.Null(p.GetField("fieldUnset_private"));
            Assert.NotNull(s.fieldSet_public);
            Assert.NotNull(p.GetField("fieldSet_private"));

            Assert.Null(s.PropertyUnset_public);
            Assert.Null(p.GetProperty("PropertyUnset_private"));
            Assert.NotNull(s.PropertySet_public);
            Assert.NotNull(p.GetProperty("PropertySet_private"));
        }
#endif



        [Fact]
        public void ScopeTest()
        {
            var dic1 = new DiContainer();

            dic1.Register<I1, CScopeA>(DiLifecycle.Transient);
            Assert.Equal(10, dic1.New<I1>().Func(3, 7));

            using(var dic2 = dic1.Scope()) {
                Assert.Equal(10, dic2.New<I1>().Func(3, 7));

                dic2.Register<I1, CScopeB>(DiLifecycle.Transient);
                Assert.Equal(-4, dic2.New<I1>().Func(3, 7));
                Assert.Equal(10, dic1.New<I1>().Func(3, 7));

                Assert.Throws<ArgumentException>(() => dic2.Register<I1, CScopeB>(DiLifecycle.Transient));

                using(var dic3 = dic2.Scope()) {
                    Assert.Equal(-4, dic3.New<I1>().Func(3, 7));

                    dic3.Register<I1, CScopeC>(DiLifecycle.Transient);
                    Assert.Equal(21, dic3.New<I1>().Func(3, 7));
                    Assert.Equal(-4, dic2.New<I1>().Func(3, 7));
                    Assert.Equal(10, dic1.New<I1>().Func(3, 7));

                    Assert.Throws<ArgumentException>(() => dic3.Register<I1, CScopeC>(DiLifecycle.Transient));

                    using(var dic4 = dic3.Scope()) {
                        Assert.Equal(21, dic4.New<I1>().Func(3, 7));

                        dic4.Register<I1, CScopeD>(DiLifecycle.Transient);
                        Assert.Equal(2, dic4.New<I1>().Func(10, 5));
                        Assert.Equal(21, dic3.New<I1>().Func(3, 7));
                        Assert.Equal(-4, dic2.New<I1>().Func(3, 7));
                        Assert.Equal(10, dic1.New<I1>().Func(3, 7));

                        Assert.Throws<ArgumentException>(() => dic4.Register<I1, CScopeD>(DiLifecycle.Transient));

                    }
                }
            }
        }


        [Fact]
        public void RegisterMemberTest()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);

            dic.RegisterMember<D1, I1>("I1_1");
            Assert.Throws<NullReferenceException>(() => dic.RegisterMember<D1, I1>("I1_0"));
            Assert.Throws<ArgumentException>(() => dic.RegisterMember<D1, I1>("I1_1"));
        }

        [Fact]
        public void UnregisterTest()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);
            var i1 = dic.New<I1>();
            Assert.NotNull(i1);

            Assert.False(dic.Unregister<C1>());
            Assert.True(dic.Unregister<I1>());
            Assert.Throws<DiException>(() => dic.New<I1>());
        }

        [Fact]
        public void Inject_Name_Test()
        {
            var dic = new DiContainer();
            dic.Register<Wrap, Wrap>(new Wrap("a"));
            dic.Register<Wrap, Wrap>("name", new Wrap("b"));

            var nc = dic.New<NamedClass>();
            Assert.Equal(string.Empty, nc.A.S);
            Assert.Equal(string.Empty, nc.B.S);
            //Assert.Equal(string.Empty, nc.C.S);

            dic.Inject(nc);
            Assert.Equal("a", nc.A.S);
            Assert.Equal("b", nc.B.S);
            //Assert.Equal("a", nc.C.S);

            var nc2 = dic.Build<NamedClass>();
            Assert.Equal("a", nc2.A.S);
            Assert.Equal("b", nc2.B.S);
            //Assert.Equal("a", nc2.C.S);

        }

        [Fact]
        public void New_Name_Test()
        {
            var dic = new DiContainer();
            dic.Register<string, string>("a");
            dic.Register<string, string>("named", "b");

            var nc = dic.New<NamedClass2>();
            Assert.Equal("a", nc.A);
            Assert.Equal("b", nc.B);
            Assert.Equal("a", nc.C);
        }


        [Fact]
        public void CallMethod_0_Test()
        {
            var dic = new DiContainer();
            var callClass = new CallClass();

            var methodInt = callClass.GetType().GetMethod(nameof(callClass.FuncInt_0))!;
            var actualInt = dic.CallMethod(string.Empty, callClass, methodInt, Array.Empty<object>());
            Assert.Equal(10, actualInt);

            var methodStr = callClass.GetType().GetMethod(nameof(callClass.FuncStr_0))!;
            var actualStr = dic.CallMethod(string.Empty, callClass, methodStr, Array.Empty<object>());
            Assert.Equal("str", actualStr);

            var methodVoid = callClass.GetType().GetMethod(nameof(callClass.Action_0))!;
            var actualVoid = dic.CallMethod(string.Empty, callClass, methodVoid, Array.Empty<object>());
            Assert.Equal(1, callClass.Action0CallCount);
            Assert.Null(actualVoid);
        }

        [Fact]
        public void CallMethod_1_Test()
        {
            var dic = new DiContainer();
            dic.Register<string, string>("a");
            dic.Register<string, string>("named", "b");

            var callClass = dic.New<CallClass>();

            dic.Call(callClass, nameof(callClass.Action_1), 123);
            Assert.Equal(123, callClass.Action1LastValue);

            var methodStr = callClass.GetType().GetMethod(nameof(callClass.FuncStr_1))!;

            var actualEmptyName1 = dic.CallMethod(string.Empty, callClass, methodStr, Array.Empty<object>());
            Assert.Equal("a:a", actualEmptyName1);

            var actualDefinedName1 = dic.CallMethod("named", callClass, methodStr, Array.Empty<object>());
            Assert.Equal("b:b", actualDefinedName1);

            var actualEmptyName2 = dic.CallMethod(string.Empty, callClass, methodStr, new[] { "A" });
            Assert.Equal("A:A", actualEmptyName2);

            var actualDefinedName2 = dic.CallMethod("named", callClass, methodStr, new[] { "B" });
            Assert.Equal("B:B", actualDefinedName2);
        }

        [Fact]
        public void CallMethod_2_Test()
        {
            var dic = new DiContainer();
            dic.Register<string, string>("a");
            dic.Register<string, string>("named", "b");

            var callClass = dic.New<CallClass>();
            var methodStr = callClass.GetType().GetMethod(nameof(callClass.FuncStr_2))!;

            var actualEmptyName1 = dic.CallMethod(string.Empty, callClass, methodStr, Array.Empty<object>());
            Assert.Equal("a:a", actualEmptyName1);

            var actualDefinedName1 = dic.CallMethod("named", callClass, methodStr, Array.Empty<object>());
            Assert.Equal("b:b", actualDefinedName1);

            var actualEmptyName2 = dic.CallMethod(string.Empty, callClass, methodStr, new[] { "A" });
            Assert.Equal("A:a", actualEmptyName2);

            var actualDefinedName2 = dic.CallMethod("named", callClass, methodStr, new[] { "B" });
            Assert.Equal("B:b", actualDefinedName2);

            var actualEmptyName3 = dic.CallMethod(string.Empty, callClass, methodStr, new[] { "A", "A'" });
            Assert.Equal("A:A'", actualEmptyName3);

            var actualDefinedName3 = dic.CallMethod("named", callClass, methodStr, new[] { "B", "B'" });
            Assert.Equal("B:B'", actualDefinedName3);

            //あかん
            //var actualEmptyName4 = dic.CallMethod(string.Empty, callClass, methodStr, new object[] { new DiDefaultParameter(typeof(string)), "A" });
            //Assert.Equal("a:A", actualEmptyName4);

            //var actualDefinedName4 = dic.CallMethod("named", callClass, methodStr, new object[] { new DiDefaultParameter(typeof(string)), "B" });
            //Assert.Equal("b:B", actualDefinedName4);
        }
    }
}
