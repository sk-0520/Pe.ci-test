using System.Text.RegularExpressions;
using ContentTypeTextNet.Pe.Main.Models.Command;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Command
{
    public class HitValuesCreatorTest
    {
        #region function

        [Theory]
        [InlineData(1, @"", "")]
        [InlineData(1, @"abc", "abc")]
        [InlineData(2, @"abc", "abcabc")]
        [InlineData(1, @"abc.*", "abcabc")]
        [InlineData(4, @"a", "aaaa")]
        [InlineData(2, @"a|b", "abc")]
        public void GetMatchesTest(int expected, string pattern, string input)
        {
            var hvc = new HitValuesCreator(Test.LoggerFactory);
            var actual = hvc.GetMatches(input, new Regex(pattern));
            Assert.Equal(expected, actual.Count);
        }


        [Theory]
        // GetScore に依存
        [InlineData(800, "abc", "abc")]
        [InlineData(0, "Abc", "abc")]
        [InlineData(800, "aBc", "abc")]
        public void CalcScoreTest(int expected, string input, string source)
        {
            var simpleRegexFactory = new SimpleRegexFactory(Test.LoggerFactory);
            var regex = simpleRegexFactory.CreateFilterRegex(input);

            var hvc = new HitValuesCreator(Test.LoggerFactory);
            var matchers = hvc.GetMatches(source, regex);
            var ranges = hvc.ConvertRanges(matchers);
            var hitValues = hvc.ConvertHitValues(source, ranges);
            var actual = hvc.CalcScore(source, hitValues);
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
