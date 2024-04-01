using ContentTypeTextNet.Pe.Main.Models.Database;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database
{
    public class EnumTransferTest
    {
        enum A
        {
            TestEnum,
            testEnum,
            TEStENUm,
        }

        enum B
        {
            [EnumTransfer("abc")]
            TestEnum,
            [EnumTransfer("abc")]
            testEnum,
            [EnumTransfer("abc")]
            TEStENUm,
        }

        [Fact]
        public void ToTest_A()
        {
            var expected = "test-enum";
            var enumTransfer = new EnumTransfer<A>();
            Assert.Equal(expected, enumTransfer.ToString(A.TestEnum));
            Assert.Equal(expected, enumTransfer.ToString(A.testEnum));
            Assert.Equal(expected, enumTransfer.ToString(A.TEStENUm));
        }

        [Fact]
        public void ToTest_B()
        {
            var expected = "abc";
            var enumTransfer = new EnumTransfer<B>();
            Assert.Equal(expected, enumTransfer.ToString(B.TestEnum));
            Assert.Equal(expected, enumTransfer.ToString(B.testEnum));
            Assert.Equal(expected, enumTransfer.ToString(B.TEStENUm));
        }

        enum C
        {
            TestMember1,
            TestMember2,
            TestMember3,
        }

        [Fact]
        public void FromTest_C()
        {
            var enumTransfer = new EnumTransfer<C>();
            Assert.Equal(C.TestMember1, enumTransfer.ToEnum("testmember1"));
            Assert.Equal(C.TestMember2, enumTransfer.ToEnum("testmember2"));
            Assert.Equal(C.TestMember3, enumTransfer.ToEnum("testmember3"));

            Assert.Equal(C.TestMember1, enumTransfer.ToEnum("test-member-1"));
            Assert.Equal(C.TestMember2, enumTransfer.ToEnum("test-member-2"));
            Assert.Equal(C.TestMember3, enumTransfer.ToEnum("test-member-3"));

            Assert.Equal(C.TestMember1, enumTransfer.ToEnum("TestMember1"));
            Assert.Equal(C.TestMember2, enumTransfer.ToEnum("TestMember2"));
            Assert.Equal(C.TestMember3, enumTransfer.ToEnum("TestMember3"));
        }

        enum D
        {
            [EnumTransfer("test1")]
            TestMember1,
            [EnumTransfer("TEST2")]
            TestMember2,
            [EnumTransfer("3")]
            TestMember3,
        }

        [Fact]
        public void FromTest_D()
        {
            var enumTransfer = new EnumTransfer<D>();
            Assert.Equal(D.TestMember1, enumTransfer.ToEnum("test1"));
            Assert.Equal(D.TestMember2, enumTransfer.ToEnum("TEST2"));
            Assert.Equal(D.TestMember3, enumTransfer.ToEnum("3"));

            Assert.Equal(D.TestMember1, enumTransfer.ToEnum("testmember1"));
            Assert.Equal(D.TestMember2, enumTransfer.ToEnum("testmember2"));
            Assert.Equal(D.TestMember3, enumTransfer.ToEnum("testmember3"));

            Assert.Equal(D.TestMember1, enumTransfer.ToEnum("test-member-1"));
            Assert.Equal(D.TestMember2, enumTransfer.ToEnum("test-member-2"));
            Assert.Equal(D.TestMember3, enumTransfer.ToEnum("test-member-3"));

            Assert.Equal(D.TestMember1, enumTransfer.ToEnum("TestMember1"));
            Assert.Equal(D.TestMember2, enumTransfer.ToEnum("TestMember2"));
            Assert.Equal(D.TestMember3, enumTransfer.ToEnum("TestMember3"));
        }

        enum E
        {
            A,
            B = 100,
            [EnumTransfer("Z")]
            C = 10,
            D,
        }
        [Fact]
        public void FromTest_E()
        {
            var enumTransfer = new EnumTransfer<E>();
            Assert.Equal(E.A, enumTransfer.ToEnum(enumTransfer.ToString(E.A)));
            Assert.Equal(E.B, enumTransfer.ToEnum(enumTransfer.ToString(E.B)));
            Assert.Equal(E.C, enumTransfer.ToEnum(enumTransfer.ToString(E.C)));
            Assert.Equal(E.D, enumTransfer.ToEnum(enumTransfer.ToString(E.D)));
        }


    }
}
