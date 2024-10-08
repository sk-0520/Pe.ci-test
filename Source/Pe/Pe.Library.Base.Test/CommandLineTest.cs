using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Base;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Base.Test
{
    public class CommandLineTest
    {
        [Theory]
        [InlineData(new[] { "a" }, true, "a", 0)]
        [InlineData(new[] { "A", "a" }, true, "A", 1)]
        [InlineData(new[] { "A", "a" }, false, "", 2)]
        [InlineData(new string[] { }, true, "", 0)]
        [InlineData(new string[] { }, false, "", 0)]
        public void ConstructorTest(string[] args, bool withCommand, string expectedProgramName, int expectedArgumentCount)
        {
            var commandLine = new CommandLine(args, withCommand);
            Assert.True(commandLine.CommandName == expectedProgramName);
            Assert.True(commandLine.Arguments.Count == expectedArgumentCount);
        }

        [Theory]
        [InlineData(true, 'a', "aa")]
        [InlineData(true, '\0', "aa")]
        [InlineData(true, 'a', "")]
        [InlineData(false, '\0', "")]
        [InlineData(false, '\0', "a")]
        public void AddTest(bool expected, char shortKey, string longKey)
        {
            var commandLine = new CommandLine();
            try {
                var key = commandLine.Add(shortKey, longKey);
                Assert.True(expected);
            } catch(ArgumentException ex) {
                Assert.False(expected, ex.ToString());
            }
        }

        [Theory]
        [InlineData(false, 'a', "")]
        [InlineData(false, '\0', "aa")]
        [InlineData(false, '\0', "bb")]
        [InlineData(false, 'c', "")]
        [InlineData(true, 'b', "aaa")]
        public void AddTest_Exists(bool expected, char shortKey, string longKey)
        {
            var commandLine = new CommandLine();
            commandLine.Add('a', "aa");
            commandLine.Add('\0', "bb");
            commandLine.Add('c', "");

            try {
                var key = commandLine.Add(shortKey, longKey);
                Assert.True(expected);
            } catch(ArgumentException ex) {
                Assert.False(expected, ex.ToString());
            }
        }

        [Theory]
        [InlineData("A", new[] { "/a", "A" }, 'a', "aaa")]
        [InlineData("A", new[] { "/aaa", "A" }, 'a', "aaa")]
        [InlineData("AA", new[] { "/aaa", "AA", "/a", "A" }, 'a', "aaa")]
        //[InlineData("A", new[] { "-a", "A" }, 'a', "aaa")]
        [InlineData(   "A", new[] { "--aaa", "A" }, 'a', "aaa")]
        [InlineData(   "AA", new[] { "--aaa", "AA", "-a", "A" }, 'a', "aaa")]
        [InlineData("A", new[] { "/a=A" }, 'a', "aaa")]
        [InlineData("A", new[] { "/aaa=A" }, 'a', "aaa")]
        [InlineData("AA", new[] { "/aaa=AA", "/a=A" }, 'a', "aaa")]
        //[InlineData("A", new[] { "-a=A" }, 'a', "aaa")]
        [InlineData("A", new[] { "--aaa=A" }, 'a', "aaa")]
        [InlineData("AA", new[] { "--aaa=AA", "-a=A" }, 'a', "aaa")]
        //[InlineData("A", new[] { "/a=\"A\"" }, 'a', "aaa")]
        [InlineData("A", new[] { "/aaa=\"A\"" }, 'a', "aaa")]
        [InlineData("AA", new[] { "/aaa=\"AA\"", "/a=\"A\"" }, 'a', "aaa")]
        //[InlineData("A", new[] { "-a=\"A\"" }, 'a', "aaa")]
        [InlineData("A", new[] { "--aaa=\"A\"" }, 'a', "aaa")]
        [InlineData("AA", new[] { "--aaa=\"AA\"", "-a=\"A\"" }, 'a', "aaa")]
        public void ExecuteTest_Simple(string expected, string[] args, char shortKey, string longKey)
        {
            var commandLine = new CommandLine(args, false);
            var commandKey = commandLine.Add(shortKey, longKey, true);

            Assert.True(commandLine.Parse());
            var value = commandLine.Values[commandKey];
            Assert.True(value.First == expected);
        }

        [Theory]
        [InlineData(true, new[] { "/a" }, 'a', "aaa")]
        [InlineData(true, new[] { "/aaa" }, 'a', "aaa")]
        //[InlineData(true, new[] { "-a" }, 'a', "aaa")]
        [InlineData(true, new[] { "--aaa" }, 'a', "aaa")]
        public void ExecuteTest_Switch(bool expected, string[] args, char shortKey, string longKey)
        {
            var commandLine = new CommandLine(args, false);
            var commandKey = commandLine.Add(shortKey, longKey, false);

            Assert.True(commandLine.Parse());
            var has = commandLine.Switches.Contains(commandKey);
            Assert.True(has == expected);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("a", "a")]
        [InlineData("a", " a")]
        [InlineData("a", "a ")]
        [InlineData("a", " a ")]
        [InlineData("\"a a\"", "a a")]
        [InlineData("a\"\"b", "a\"b")]
        [InlineData("a\"\"\"\"\"\"b", "a\"\"\"b")]
        [InlineData("\"a \"\"\"\"\"\" b\"", "a \"\"\" b")]
        public void Escape(string expected, string input)
        {
            var actual = CommandLine.Escape(input);
            Assert.Equal(expected, actual);
        }
    }
}
