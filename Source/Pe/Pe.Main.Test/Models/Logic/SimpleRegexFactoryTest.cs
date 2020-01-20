using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Logic
{
    [TestClass]
    public class SimpleRegexFactoryTest
    {
        #region function

        [TestMethod]
        [DataRow(true, @"", "")]
        [DataRow(true, @"abc", "abc")]
        [DataRow(true, @"Abc", "Abc")]
        [DataRow(true, @"AbC", "AbC")]
        [DataRow(false, @"AbC", "Abc")]
        [DataRow(true, @"a*c", "ac")]
        [DataRow(true, @"a*c", "abc")]
        [DataRow(true, @"a*c", "AC")]
        [DataRow(true, @"a*c", "AZC")]
        [DataRow(false, @"A*c", "abc")]
        [DataRow(true, @"A*c", "Abc")]
        [DataRow(false, @"A*c", "AC")]
        [DataRow(true, @"A*c", "Ac")]
        [DataRow(false, @"A*c", "AZC")]
        [DataRow(true, @"A*c", "AZc")]
        [DataRow(true, @"/", "/")]
        [DataRow(true, @"/", "a/")]
        [DataRow(true, @"/", "/a")]
        [DataRow(false, @"/", "")]
        [DataRow(false, @"/", " ")]
        [DataRow(true, @"/^a", "a")]
        [DataRow(false, @"/^a", " a")]
        public void CreateFilterRegex(bool result, string pattern, string input)
        {
            var regex = new SimpleRegexFactory(Test.LoggerFactory).CreateFilterRegex(pattern);
            var actual = regex.IsMatch(input);
            Assert.AreEqual(result, actual, $"{pattern} - {input}");
        }

        #endregion
    }
}
