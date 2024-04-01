using System;
using System.Collections.Generic;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Logic
{
    public class PowerShellArgumentsTest
    {
        #region function

        [Fact]
        public void Create_Exception_Test()
        {
            var psa = new PowerShellArguments();
            Assert.Throws<ArgumentException>(() => psa.Create("k y", "value"));
            Assert.Throws<ArgumentException>(() => psa.Create("k ", "value"));
            Assert.Throws<ArgumentException>(() => psa.Create(" k", "value"));
            Assert.Throws<ArgumentException>(() => psa.Create(" ", "value"));
            Assert.Throws<ArgumentException>(() => psa.Create("", "value"));
        }

        [Theory]
        [InlineData("-k", "\"\"", "k", "")]
        [InlineData("-k", "v", "k", "v")]
        [InlineData("-k", "v", "-k", "v")]
        [InlineData("-k", "\"v v\"", "k", "v v")]
        [InlineData("-k", "\"v\"\" \"\"v\"", "k", "v\" \"v")]
        public void CreateTest(string expectedKey, string expectedValue, string inputKey, string inputValue)
        {
            var psa = new PowerShellArguments();
            var p = KeyValuePair.Create(inputKey, inputValue);
            var actual1 = psa.Create(p);
            var actual2 = psa.Create(inputKey, inputValue);

            Assert.Equal(expectedKey, actual1.Key);
            Assert.Equal(expectedValue, actual1.Value);
            Assert.Equal(expectedKey, actual2.Key);
            Assert.Equal(expectedValue, actual2.Value);
        }

        [Fact]
        public void CreateParameters()
        {
            var psa = new PowerShellArguments();
            var parameters = psa.CreateParameters(false, new[]{
                KeyValuePair.Create("key1", "value1"),
                KeyValuePair.Create("-key2", "value2"),
                KeyValuePair.Create("-key3", "value 3"),
            });
            Assert.Equal("-key1", parameters[0]);
            Assert.Equal("value1", parameters[1]);

            Assert.Equal("-key2", parameters[2]);
            Assert.Equal("value2", parameters[3]);

            Assert.Equal("-key3", parameters[4]);
            Assert.Equal("\"value 3\"", parameters[5]);
        }

        [Theory]
        [InlineData("\"\"\"\"\"\"", "")]
        [InlineData("\"\"\"ab\"\"\"", "ab")]
        [InlineData("\"\"\"a b\"\"\"", "a b")]
        [InlineData("\"\"\"a\"\"b\"\"\"", "a\"b")]
        public void ToRemainingValueTest(string expected, string input)
        {
            var psa = new PowerShellArguments();
            var actual = psa.ToRemainingValue(input);
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
