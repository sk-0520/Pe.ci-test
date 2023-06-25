using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Standard.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Standard.Base.Test
{
    [TestClass]
    public class CommandLineTest
    {
        [TestMethod]
        [DataRow(new[] { "a" }, true, "a", 0)]
        [DataRow(new[] { "A", "a" }, true, "A", 1)]
        [DataRow(new[] { "A", "a" }, false, "", 2)]
        [DataRow(new string[] { }, true, "", 0)]
        [DataRow(new string[] { }, false, "", 0)]
        public void ConstructorTest(string[] args, bool withCommand, string expectedProgramName, int expectedArgumentCount)
        {
            var commandLine = new CommandLine(args, withCommand);
            Assert.IsTrue(commandLine.CommandName == expectedProgramName);
            Assert.IsTrue(commandLine.Arguments.Count == expectedArgumentCount);
        }

        [TestMethod]
        [DataRow(true, 'a', "aa")]
        [DataRow(true, '\0', "aa")]
        [DataRow(true, 'a', "")]
        [DataRow(false, '\0', "")]
        [DataRow(false, '\0', "a")]
        public void AddTest(bool expected, char shortKey, string longKey)
        {
            var commandLine = new CommandLine();
            try {
                var key = commandLine.Add(shortKey, longKey);
                Assert.IsTrue(expected);
            } catch(ArgumentException ex) {
                Assert.IsFalse(expected, ex.ToString());
            }
        }

        [TestMethod]
        [DataRow(false, 'a', "")]
        [DataRow(false, '\0', "aa")]
        [DataRow(false, '\0', "bb")]
        [DataRow(false, 'c', "")]
        [DataRow(true, 'b', "aaa")]
        public void AddTest_Exists(bool expected, char shortKey, string longKey)
        {
            var commandLine = new CommandLine();
            commandLine.Add('a', "aa");
            commandLine.Add('\0', "bb");
            commandLine.Add('c', "");

            try {
                var key = commandLine.Add(shortKey, longKey);
                Assert.IsTrue(expected);
            } catch(ArgumentException ex) {
                Assert.IsFalse(expected, ex.ToString());
            }
        }

        [TestMethod]
        [DataRow(new[] { "/a", "A" }, 'a', "aaa", "A")]
        [DataRow(new[] { "/aaa", "A" }, 'a', "aaa", "A")]
        [DataRow(new[] { "/aaa", "AA", "/a", "A" }, 'a', "aaa", "AA")]
        //[DataRow(new[] { "-a", "A" }, 'a', "aaa", "A")]
        [DataRow(new[] { "--aaa", "A" }, 'a', "aaa", "A")]
        [DataRow(new[] { "--aaa", "AA", "-a", "A" }, 'a', "aaa", "AA")]
        [DataRow(new[] { "/a=A" }, 'a', "aaa", "A")]
        [DataRow(new[] { "/aaa=A" }, 'a', "aaa", "A")]
        [DataRow(new[] { "/aaa=AA", "/a=A" }, 'a', "aaa", "AA")]
        //[DataRow(new[] { "-a=A" }, 'a', "aaa", "A")]
        [DataRow(new[] { "--aaa=A" }, 'a', "aaa", "A")]
        [DataRow(new[] { "--aaa=AA", "-a=A" }, 'a', "aaa", "AA")]
        //[DataRow(new[] { "/a=\"A\"" }, 'a', "aaa", "A")]
        [DataRow(new[] { "/aaa=\"A\"" }, 'a', "aaa", "A")]
        [DataRow(new[] { "/aaa=\"AA\"", "/a=\"A\"" }, 'a', "aaa", "AA")]
        //[DataRow(new[] { "-a=\"A\"" }, 'a', "aaa", "A")]
        [DataRow(new[] { "--aaa=\"A\"" }, 'a', "aaa", "A")]
        [DataRow(new[] { "--aaa=\"AA\"", "-a=\"A\"" }, 'a', "aaa", "AA")]
        public void ExecuteTest_Simple(string[] args, char shortKey, string longKey, string expected)
        {
            var commandLine = new CommandLine(args, false);
            var commandKey = commandLine.Add(shortKey, longKey, true);

            Assert.IsTrue(commandLine.Parse());
            var value = commandLine.Values[commandKey];
            Assert.IsTrue(value.First == expected);
        }

        [TestMethod]
        [DataRow(new[] { "/a" }, 'a', "aaa", true)]
        [DataRow(new[] { "/aaa" }, 'a', "aaa", true)]
        //[DataRow(new[] { "-a" }, 'a', "aaa", true)]
        [DataRow(new[] { "--aaa" }, 'a', "aaa", true)]
        public void ExecuteTest_Switch(bool expected, string[] args, char shortKey, string longKey)
        {
            var commandLine = new CommandLine(args, false);
            var commandKey = commandLine.Add(shortKey, longKey, false);

            Assert.IsTrue(commandLine.Parse());
            var has = commandLine.Switches.Contains(commandKey);
            Assert.IsTrue(has == expected);
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow("a", "a")]
        [DataRow("a", " a")]
        [DataRow("a", "a ")]
        [DataRow("a", " a ")]
        [DataRow("\"a a\"", "a a")]
        [DataRow("a\"\"b", "a\"b")]
        [DataRow("a\"\"\"\"\"\"b", "a\"\"\"b")]
        [DataRow("\"a \"\"\"\"\"\" b\"", "a \"\"\" b")]
        public void Escape(string expected, string input)
        {
            var actual = CommandLine.Escape(input);
            Assert.AreEqual(expected, actual);
        }
    }

    [TestClass]
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

        [TestMethod]
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
                Assert.IsTrue(item.Test(), $"{item.Key}, {item.Value}");
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

        [TestMethod]
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
            Assert.IsTrue(result);
            Assert.AreEqual(actual.A, 1);
            CollectionAssert.AreEqual(actual.B, new[] { "a", "b" });
            CollectionAssert.AreEqual(actual.C, new[] { "aa", "bb" });
            CollectionAssert.AreEqual(actual.D, new[] { "aaa", "bbb" });
            CollectionAssert.AreEqual(actual.E.ToList(), new[] { "aaaa", "bbbb" });
            CollectionAssert.AreEqual(actual.F.ToList(), new[] { "aaaaa", "bbbbb" });
        }
    }
}
