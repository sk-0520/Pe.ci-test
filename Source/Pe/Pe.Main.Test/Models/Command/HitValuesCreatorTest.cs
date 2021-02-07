using System.Text.RegularExpressions;
using ContentTypeTextNet.Pe.Main.Models.Command;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Command
{
    [TestClass]
    public class HitValuesCreatorTest
    {
        #region function

        [TestMethod]
        [DataRow(1, @"", "")]
        [DataRow(1, @"abc", "abc")]
        [DataRow(2, @"abc", "abcabc")]
        [DataRow(1, @"abc.*", "abcabc")]
        [DataRow(4, @"a", "aaaa")]
        [DataRow(2, @"a|b", "abc")]
        public void GetMatchesTest(int expected, string pattern, string input)
        {
            var hvc = new HitValuesCreator(Test.LoggerFactory);
            var actual = hvc.GetMatches(input, new Regex(pattern));
            Assert.AreEqual(expected, actual.Count);
        }


        [TestMethod]
        // GetScore に依存
        [DataRow(800, "abc", "abc")]
        [DataRow(0, "Abc", "abc")]
        [DataRow(800, "aBc", "abc")]
        public void CalcScoreTest(int expected, string input, string source)
        {
            var simpleRegexFactory = new SimpleRegexFactory(Test.LoggerFactory);
            var regex = simpleRegexFactory.CreateFilterRegex(input);

            var hvc = new HitValuesCreator(Test.LoggerFactory);
            var matchers = hvc.GetMatches(source, regex);
            var ranges = hvc.ConvertRanges(matchers);
            var hitValues = hvc.ConvertHitValues(source, ranges);
            var actual = hvc.CalcScore(source, hitValues);
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
