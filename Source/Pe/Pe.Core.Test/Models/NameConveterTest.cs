using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    [TestClass]
    public class NameConveterTest
    {
        #region function

        [TestMethod]
        public void PascalToKebab_Exception_Test()
        {
            var nc = new NameConveter();
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
            var nc = new NameConveter();
            var actual = nc.PascalToKebab(input);
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void PascalToSnake_Exception_Test()
        {
            var nc = new NameConveter();
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
            var nc = new NameConveter();
            var actual = nc.PascalToSnake(input);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PascalToCamel_Exception_Test()
        {
            var nc = new NameConveter();
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
            var nc = new NameConveter();
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
            var nc = new NameConveter();
            var actual = nc.KebabToPascal(input);
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
