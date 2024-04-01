using ContentTypeTextNet.Pe.Main.Models.Logic;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Logic
{
    public class SimpleRegexFactoryTest
    {
        #region function

        [Theory]
        [InlineData(true, @"", "")]
        [InlineData(true, @"abc", "abc")]
        [InlineData(true, @"Abc", "Abc")]
        [InlineData(true, @"AbC", "AbC")]
        [InlineData(false, @"AbC", "Abc")]
        [InlineData(true, @"a*c", "ac")]
        [InlineData(true, @"a*c", "abc")]
        [InlineData(true, @"a*c", "AC")]
        [InlineData(true, @"a*c", "AZC")]
        [InlineData(false, @"A*c", "abc")]
        [InlineData(true, @"A*c", "Abc")]
        [InlineData(false, @"A*c", "AC")]
        [InlineData(true, @"A*c", "Ac")]
        [InlineData(false, @"A*c", "AZC")]
        [InlineData(true, @"A*c", "AZc")]
        [InlineData(true, @"/", "/")]
        [InlineData(true, @"/", "a/")]
        [InlineData(true, @"/", "/a")]
        [InlineData(false, @"/", "")]
        [InlineData(false, @"/", " ")]
        [InlineData(true, @"/^a", "a")]
        [InlineData(false, @"/^a", " a")]
        public void CreateFilterRegexTest(bool expected, string pattern, string input)
        {
            var regex = new SimpleRegexFactory(Test.LoggerFactory).CreateFilterRegex(pattern);
            var actual = regex.IsMatch(input);
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
