using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.CommonTest;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.DependencyInjection.Test
{
    public class IDiContainerExtensionsTest
    {
        #region define

        interface I0
        {
            int Func(int a);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<保留中>")]
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

        interface IDmy1
        {
            int Func(int a, int b);
        }

        class C1: I1
        {
            public int Func(int a, int b) => a + b;
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "<保留中>")]
        class C1_other: I1
        {
            public int Func(int a, int b) => a - b;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<保留中>")]
        class C1_Func: I1
        {
            public C1_Func(Func<int, int, int> func)
            {
                F = func;
            }

            Func<int, int, int> F { get; }
            public int Func(int a, int b) => F(a, b);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<保留中>")]
        class CDmy1: IDmy1
        {
            public int Func(int a, int b) => a + b;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<保留中>")]
        class C2
        {
            public C2(I1 i1) => I1 = i1;

            I1 I1 { get; }

            public int Plus(int a, int b) => I1.Func(a, b);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<保留中>")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<保留中>")]
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
            protected C5(int a, int b, IEnumerable<I1> i1s)
            {
                A = a;
                B = b;
                I1 = i1s.ToArray();
            }

            int A { get; }
            int B { get; }
            I1[] I1 { get; }
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<保留中>")]
            public int Get() => I1.Sum(i => i.Func(A, B));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<保留中>")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<保留中>")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<保留中>")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<保留中>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:読み取り専用修飾子を追加します", Justification = "<保留中>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:使用されていないプライベート メンバーを削除する", Justification = "<保留中>")]
        class C6
        {
#pragma warning disable 169,649
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
#pragma warning restore
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
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<保留中>")]
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
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<保留中>")]
            public Nest1(INest2 nest2)
            {
                Nest2 = nest2;
            }

            public INest2 Nest2 { get; }
        }

        class Nest2: INest2
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<保留中>")]
            public Nest2(INest3 nest3)
            {
                Nest3 = nest3;
            }

            public INest3 Nest3 { get; }
        }

        class Nest3: INest3
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<保留中>")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<保留中>")]
        class CScopeA: I1
        {
            public int Func(int a, int b) => a + b;
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<保留中>")]
        class CScopeB: I1
        {
            public int Func(int a, int b) => a - b;
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<保留中>")]
        class CScopeC: I1
        {
            public int Func(int a, int b) => a * b;
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<保留中>")]
        class CScopeD: I1
        {
            public int Func(int a, int b) => a / b;
        }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<保留中>")]
        class D1
        {
            I1? I1_1 { get; set; }
            I1? I1_2 { get; set; }
            public I1? I1_3 { get; set; }
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3459:Unassigned members should be removed", Justification = "<保留中>")]
            public I1? I1_4 { get; set; }
        }


        interface ID2
        {
            I1 I1_2 { get; }

            I1 I1_4 { get; }

            I1 I1_6 { get; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3459:Unassigned members should be removed", Justification = "<保留中>")]
        class D2: ID2
        {
#pragma warning disable 169,649,8618
            [DiInjection]
            public I1? I1_1;
            [DiInjection]
            public I1 I1_2 { get; set; }

            public I1? I1_3;
            public I1 I1_4 { get; set; }

            public I1? I1_5;
            public I1 I1_6 { get; set; }
#pragma warning restore
        }

        class Wrap
        {
            public Wrap(string s) => S = s;
            public string S { get; set; } = string.Empty;

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<保留中>")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<保留中>")]
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

        #region function

        [Fact]
        public void BuildTest()
        {
            var dic = new DiContainer();
            dic.Register<I1, C1>(DiLifecycle.Transient);
            dic.Register<ID2, D2>(DiLifecycle.Transient);

            dic.RegisterMember<D2, I1>("I1_3");
            dic.RegisterMember<D2, I1>(nameof(D1.I1_4));

            var d = (D2)dic.Build<ID2>();
            Assert.NotNull(d.I1_1);
            Assert.NotNull(d.I1_2);
            Assert.NotNull(d.I1_3);
            Assert.NotNull(d.I1_4);
            Assert.Null(d.I1_5);
            Assert.Null(d.I1_6);
        }

        [Fact]
        public void CallMethod_notfound_throw_Test()
        {
            var dic = new DiContainer();
            var callClass = new CallClass();

            Assert.Throws<DiFunctionMethodNotFoundException>(() => dic.Call(callClass, "<MEDTHOD>"));
        }

        [Fact]
        public void CallMethod_0_Test()
        {
            var dic = new DiContainer();
            var callClass = new CallClass();

            dic.Call(callClass, nameof(callClass.Action_0));
            Assert.Equal(1, callClass.Action0CallCount);

            Assert.Throws<DiFunctionResultException>(() => dic.Call(callClass, nameof(callClass.FuncInt_0)));
        }

        [Fact]
        public void CallMethod_1_Test()
        {
            var dic = new DiContainer();
            dic.Register<string, string>("a");
            dic.Register<string, string>("named", "b");

            var callClass = dic.New<CallClass>();
            var methodStr = callClass.GetType().GetMethod(nameof(callClass.FuncStr_1))!;

            Assert.Throws<DiFunctionResultException>(() => dic.Call(callClass, nameof(callClass.FuncStr_1), Array.Empty<object>()));
        }

        [Fact]
        public void CallMethod_2_Test()
        {
            var dic = new DiContainer();
            dic.Register<string, string>("a");
            dic.Register<string, string>("named", "b");

            var callClass = dic.New<CallClass>();
            var methodStr = callClass.GetType().GetMethod(nameof(callClass.FuncStr_2))!;

            var actualEmptyName1_Call = dic.Call<string>(callClass, methodStr.Name);
            Assert.Equal("a:a", actualEmptyName1_Call);
            Assert.Throws<DiFunctionResultException>(() => dic.Call<int>(callClass, methodStr.Name));

            var actualEmptyName2_Call = dic.Call<string>(callClass, methodStr.Name, "A");
            Assert.Equal("A:a", actualEmptyName2_Call);
            Assert.Throws<DiFunctionResultException>(() => dic.Call<int>(callClass, methodStr.Name, "A"));

            var actualDefinedName2 = dic.CallMethod("named", callClass, methodStr, new[] { "B" });
            Assert.Equal("B:b", actualDefinedName2);

            var actualEmptyName3_Call = dic.Call<string>(callClass, methodStr.Name, "A", "A");
            Assert.Equal("A:A", actualEmptyName3_Call);
        }

        #endregion
    }
}
