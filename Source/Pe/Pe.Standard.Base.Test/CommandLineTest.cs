using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Standard.Base;
using Xunit;

namespace ContentTypeTextNet.Pe.Standard.Base.Test
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

    public class CommandLineUtilityTest
    {
        class Cts
        {
            [CommandLine(longKey: "sbyte")]
            public sbyte SByte { get; set; }
            [CommandLine(longKey: "byte")]
            public byte Byte { get; set; }
            [CommandLine(longKey: "int16")]
            public short Int16 { get; set; }
            [CommandLine(longKey: "uint16")]
            public ushort UInt16 { get; set; }
            [CommandLine(longKey: "int32")]
            public int Int32 { get; set; }
            [CommandLine(longKey: "uint32")]
            public uint UInt32 { get; set; }
            [CommandLine(longKey: "int64")]
            public long Int64 { get; set; }
            [CommandLine(longKey: "uint64")]
            public ulong UInt64 { get; set; }
            [CommandLine(longKey: "single")]
            public float Single { get; set; }
            [CommandLine(longKey: "double")]
            public double Double { get; set; }
            [CommandLine(longKey: "decimal")]
            public decimal Decimal { get; set; }
            [CommandLine(longKey: "boolean")]
            public bool Boolean { get; set; }
            [CommandLine(longKey: "boolean-switch", hasValue: false)]
            public bool BooleanSwitch { get; set; }
            [CommandLine(longKey: "char")]
            public char Char { get; set; }
            [CommandLine(longKey: "string")]
            public string String { get; set; } = string.Empty;
        }


        class MappingItem
        {
            public MappingItem(string key, string value, Func<bool> test)
            {
                Key = key;
                Value = value;
                Test = test;
            }
            public string Key { get; }
            public string Value { get; }
            public Func<bool> Test { get; }
        }

        [Fact]
        public void MappingTest()
        {
            var cts = new Cts();
            var data = new[] {
                new MappingItem("/sbyte", sbyte.MaxValue.ToString(CultureInfo.InvariantCulture), () => cts.SByte == sbyte.MaxValue),
                new MappingItem("/byte", byte.MaxValue.ToString(CultureInfo.InvariantCulture), () => cts.Byte == byte.MaxValue),
                new MappingItem("/int16", short.MaxValue.ToString(CultureInfo.InvariantCulture), () => cts.Int16 == short.MaxValue),
                new MappingItem("/uint16", ushort.MaxValue.ToString(CultureInfo.InvariantCulture), () => cts.UInt16 == ushort.MaxValue),
                new MappingItem("/int32", int.MaxValue.ToString(CultureInfo.InvariantCulture), () => cts.Int32 == int.MaxValue),
                new MappingItem("/uint32", uint.MaxValue.ToString(CultureInfo.InvariantCulture), () => cts.UInt32 == uint.MaxValue),
                new MappingItem("/int64", long.MaxValue.ToString(CultureInfo.InvariantCulture), () => cts.Int64 == long.MaxValue),
                new MappingItem("/uint64", ulong.MaxValue.ToString(CultureInfo.InvariantCulture), () => cts.UInt64 == ulong.MaxValue),
                new MappingItem("/single", float.MaxValue.ToString("r", CultureInfo.InvariantCulture), () => cts.Single == float.MaxValue),
                new MappingItem("/double", double.MaxValue.ToString("r",CultureInfo.InvariantCulture), () => cts.Double == double.MaxValue),
                new MappingItem("/decimal", decimal.MaxValue.ToString(CultureInfo.InvariantCulture), () => cts.Decimal == decimal.MaxValue),
                new MappingItem("/boolean", true.ToString(), () => cts.Boolean),
                new MappingItem("/char", char.MaxValue.ToString(), () => cts.Char == char.MaxValue),
                new MappingItem("/boolean-switch", string.Empty, () => cts.BooleanSwitch),
                new MappingItem("/string", "String", () => cts.String == "String"),
           };

            var commandLineConverter = new CommandLineConverter<Cts>(new CommandLine(data.Select(d => new[] { d.Key, d.Value }).SelectMany(i => i), false), cts);
            commandLineConverter.Mapping();
            foreach(var item in data) {
                Assert.True(item.Test(), $"{item.Key}, {item.Value}");
            }
        }

        class Class
        {
            [CommandLine(longKey: "int32")]
            public int A { get; set; }

            [CommandLine(longKey: "array")]
            public string[] B { get; set; } = Array.Empty<string>();

            [CommandLine(longKey: "ienumerable")]
            public string[] C { get; set; } = Array.Empty<string>();

            [CommandLine(longKey: "list")]
            public List<string> D { get; set; } = new List<string>();

            [CommandLine(longKey: "rolist1")]
            public IReadOnlyList<string> E { get; set; } = new List<string>();

            [CommandLine(longKey: "rolist2")]
            public IReadOnlyList<string> F { get; set; } = Array.Empty<string>();

            [CommandLine(longKey: "empty")]
            public string Empty { get; set; } = string.Empty;
        }

        [Fact]
        public void MappingTest_class()
        {
            var actual = new Class();
            var commandLineConverter = new CommandLineConverter<Class>(
                new CommandLine(new[] {
                    "--int32", "1",
                    "--array", "a",
                    "--array", "b",
                    "--ienumerable", "aa",
                    "--ienumerable", "bb",
                    "--list", "aaa",
                    "--list", "bbb",
                    "--rolist1", "aaaa",
                    "--rolist1", "bbbb",
                    "--rolist2", "aaaaa",
                    "--rolist2", "bbbbb",
                },
                false),
                actual
            );
            var result = commandLineConverter.Mapping();
            Assert.True(result);
            Assert.Equal(1, actual.A);
            Assert.Equal(actual.B, new[] { "a", "b" });
            Assert.Equal(actual.C, new[] { "aa", "bb" });
            Assert.Equal(actual.D, new[] { "aaa", "bbb" });
            Assert.Equal(actual.E.ToList(), new[] { "aaaa", "bbbb" });
            Assert.Equal(actual.F.ToList(), new[] { "aaaaa", "bbbbb" });
        }
    }
}
