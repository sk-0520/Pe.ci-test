using System;
using System.Collections.Generic;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.KeyAction
{
    public class KeyOptionConverterTest
    {
        #region define

        class KeyOptionConverter: KeyOptionConverterBase
        {
            public new KeyActionOptionAttribute GetAttribute<TEnum>(TEnum value)
                where TEnum : struct, Enum
            {
                return base.GetAttribute(value);
            }
        }

        enum Option
        {
            A,
            [KeyActionOption(typeof(int), "NAME")]
            B,
        }

        #endregion

        #region function

        [Fact]
        public void GetAttributeTest()
        {
            var koc = new KeyOptionConverter();
            Assert.Throws<InvalidOperationException>(() => koc.GetAttribute(Option.A));
            Assert.Throws<InvalidOperationException>(() => koc.GetAttribute((Option)(-1)));
            var actual = koc.GetAttribute(Option.B);
            Assert.Equal("NAME", actual.OptionName);
            Assert.Equal(typeof(int), actual.ToType);
        }

        #endregion
    }


    public class DisableOptionConverterTest
    {
        [Theory]
        [InlineData(true, "true")]
        [InlineData(true, "TRUE")]
        [InlineData(false, "false")]
        [InlineData(false, "FALSE")]
        public void ToForeverTest(bool expected, string input)
        {
            var doc = new DisableOptionConverter();
            var map = new Dictionary<string, string>() {
                [nameof(KeyActionDisableOption.Forever)] = input,
            };
            var actual = doc.ToForever(map);
            Assert.Equal(expected, actual);
        }
    }
}
