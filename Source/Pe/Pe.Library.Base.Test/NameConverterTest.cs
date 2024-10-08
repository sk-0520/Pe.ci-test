using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Base;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Base.Test
{
    public class NameConverterTest
    {
        #region function

        [Fact]
        public void PascalToKebab_Exception_Test()
        {
            var nc = new NameConverter();
            Assert.Throws<ArgumentNullException>(() => nc.PascalToKebab(null!));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData("a", "a")]
        [InlineData("a", "A")]
        [InlineData("abc", "Abc")]
        [InlineData("abc-def", "AbcDef")]
        [InlineData("abc-def-ghi", "AbcDefGHI")]
        [InlineData("abc-d", "ABcD")]
        [InlineData("あ", "あ")]
        [InlineData("int32", "Int32")]
        public void PascalToKebabTest(string expected, string input)
        {
            var nc = new NameConverter();
            var actual = nc.PascalToKebab(input);
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void PascalToSnake_Exception_Test()
        {
            var nc = new NameConverter();
            Assert.Throws<ArgumentNullException>(() => nc.PascalToSnake(null!));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData("a", "a")]
        [InlineData("a", "A")]
        [InlineData("abc", "Abc")]
        [InlineData("abc_def", "AbcDef")]
        [InlineData("abc_def_ghi", "AbcDefGHI")]
        [InlineData("abc_d", "ABcD")]
        [InlineData("あ", "あ")]
        [InlineData("int32", "Int32")]
        public void PascalToSnakeTest(string expected, string input)
        {
            var nc = new NameConverter();
            var actual = nc.PascalToSnake(input);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PascalToCamel_Exception_Test()
        {
            var nc = new NameConverter();
            Assert.Throws<ArgumentNullException>(() => nc.PascalToCamel(null!));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData("a", "a")]
        [InlineData("a", "A")]
        [InlineData("abc", "Abc")]
        [InlineData("abcDef", "AbcDef")]
        [InlineData("abcDefGhi", "AbcDefGHI")]
        [InlineData("abcD", "ABcD")]
        [InlineData("あ", "あ")]
        [InlineData("int32", "Int32")]
        public void PascalToCamelTest(string expected, string input)
        {
            var nc = new NameConverter();
            var actual = nc.PascalToCamel(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData("A", "a")]
        [InlineData("Abc", "abc")]
        [InlineData("AbcDef", "abc-def")]
        [InlineData("AbcDef", "abc-def-")]
        [InlineData("AbcDef", "abc--def-")]
        [InlineData("AbcDef", "-abc--def-")]
        [InlineData("AbcDef", "---abc--def-")]
        [InlineData("AbcDef", "---abc--def---------")]
        [InlineData("ABC", "a-b-c")]
        [InlineData("ABC", "a-B-c")]
        [InlineData("ABC", "aB-c")]
        [InlineData("ABc", "a-Bc")]
        [InlineData("A_b_cD_e", "a_b_c-d_e")]
        public void KebabToPascalTest(string expected, string input)
        {
            var nc = new NameConverter();
            var actual = nc.KebabToPascal(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData("A", "a")]
        [InlineData("Abc", "abc")]
        [InlineData("AbcDef", "abc_def")]
        [InlineData("AbcDef", "abc_def_")]
        [InlineData("AbcDef", "abc__def_")]
        [InlineData("AbcDef", "_abc__def_")]
        [InlineData("AbcDef", "___abc__def_")]
        [InlineData("AbcDef", "___abc__def_________")]
        [InlineData("ABC", "a_b_c")]
        [InlineData("ABC", "a_B_c")]
        [InlineData("ABC", "aB_c")]
        [InlineData("ABc", "a_Bc")]
        [InlineData("A-b-cD-e", "a-b-c_d-e")]
        public void SnakeToPascalTest(string expected, string input)
        {
            var nc = new NameConverter();
            var actual = nc.SnakeToPascal(input);
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
