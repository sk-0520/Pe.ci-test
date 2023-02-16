using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Standard.Base.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Standard.Base.Test.Models
{
    [TestClass]
    public class NameConverterTest
    {
        #region function

        [TestMethod]
        public void PascalToKebab_Exception_Test()
        {
            var nc = new NameConverter();
            Assert.ThrowsException<ArgumentNullException>(() => nc.PascalToKebab(null!));
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow(" ", " ")]
        [DataRow("a", "a")]
        [DataRow("a", "A")]
        [DataRow("abc", "Abc")]
        [DataRow("abc-def", "AbcDef")]
        [DataRow("abc-def-ghi", "AbcDefGHI")]
        [DataRow("abc-d", "ABcD")]
        [DataRow("あ", "あ")]
        [DataRow("int32", "Int32")]
        public void PascalToKebabTest(string expected, string input)
        {
            var nc = new NameConverter();
            var actual = nc.PascalToKebab(input);
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void PascalToSnake_Exception_Test()
        {
            var nc = new NameConverter();
            Assert.ThrowsException<ArgumentNullException>(() => nc.PascalToSnake(null!));
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow(" ", " ")]
        [DataRow("a", "a")]
        [DataRow("a", "A")]
        [DataRow("abc", "Abc")]
        [DataRow("abc_def", "AbcDef")]
        [DataRow("abc_def_ghi", "AbcDefGHI")]
        [DataRow("abc_d", "ABcD")]
        [DataRow("あ", "あ")]
        [DataRow("int32", "Int32")]
        public void PascalToSnakeTest(string expected, string input)
        {
            var nc = new NameConverter();
            var actual = nc.PascalToSnake(input);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PascalToCamel_Exception_Test()
        {
            var nc = new NameConverter();
            Assert.ThrowsException<ArgumentNullException>(() => nc.PascalToCamel(null!));
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow(" ", " ")]
        [DataRow("a", "a")]
        [DataRow("a", "A")]
        [DataRow("abc", "Abc")]
        [DataRow("abcDef", "AbcDef")]
        [DataRow("abcDefGhi", "AbcDefGHI")]
        [DataRow("abcD", "ABcD")]
        [DataRow("あ", "あ")]
        [DataRow("int32", "Int32")]
        public void PascalToCamelTest(string expected, string input)
        {
            var nc = new NameConverter();
            var actual = nc.PascalToCamel(input);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow(" ", " ")]
        [DataRow("A", "a")]
        [DataRow("Abc", "abc")]
        [DataRow("AbcDef", "abc-def")]
        [DataRow("AbcDef", "abc-def-")]
        [DataRow("AbcDef", "abc--def-")]
        [DataRow("AbcDef", "-abc--def-")]
        [DataRow("AbcDef", "---abc--def-")]
        [DataRow("AbcDef", "---abc--def---------")]
        [DataRow("ABC", "a-b-c")]
        [DataRow("ABC", "a-B-c")]
        [DataRow("ABC", "aB-c")]
        [DataRow("ABc", "a-Bc")]
        [DataRow("A_b_cD_e", "a_b_c-d_e")]
        public void KebabToPascalTest(string expected, string input)
        {
            var nc = new NameConverter();
            var actual = nc.KebabToPascal(input);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow(" ", " ")]
        [DataRow("A", "a")]
        [DataRow("Abc", "abc")]
        [DataRow("AbcDef", "abc_def")]
        [DataRow("AbcDef", "abc_def_")]
        [DataRow("AbcDef", "abc__def_")]
        [DataRow("AbcDef", "_abc__def_")]
        [DataRow("AbcDef", "___abc__def_")]
        [DataRow("AbcDef", "___abc__def_________")]
        [DataRow("ABC", "a_b_c")]
        [DataRow("ABC", "a_B_c")]
        [DataRow("ABC", "aB_c")]
        [DataRow("ABc", "a_Bc")]
        [DataRow("A-b-cD-e", "a-b-c_d-e")]
        public void SnakeToPascalTest(string expected, string input)
        {
            var nc = new NameConverter();
            var actual = nc.SnakeToPascal(input);
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
