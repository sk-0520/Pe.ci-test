using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Standard.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Standard.Test.Models
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
        [DataRow(new[] { "-a", "A" }, 'a', "aaa", "A")]
        [DataRow(new[] { "--aaa", "A" }, 'a', "aaa", "A")]
        [DataRow(new[] { "--aaa", "AA", "-a", "A" }, 'a', "aaa", "AA")]
        [DataRow(new[] { "/a=A" }, 'a', "aaa", "A")]
        [DataRow(new[] { "/aaa=A" }, 'a', "aaa", "A")]
        [DataRow(new[] { "/aaa=AA", "/a=A" }, 'a', "aaa", "AA")]
        [DataRow(new[] { "-a=A" }, 'a', "aaa", "A")]
        [DataRow(new[] { "--aaa=A" }, 'a', "aaa", "A")]
        [DataRow(new[] { "--aaa=AA", "-a=A" }, 'a', "aaa", "AA")]
        [DataRow(new[] { "/a=\"A\"" }, 'a', "aaa", "A")]
        [DataRow(new[] { "/aaa=\"A\"" }, 'a', "aaa", "A")]
        [DataRow(new[] { "/aaa=\"AA\"", "/a=\"A\"" }, 'a', "aaa", "AA")]
        [DataRow(new[] { "-a=\"A\"" }, 'a', "aaa", "A")]
        [DataRow(new[] { "--aaa=\"A\"" }, 'a', "aaa", "A")]
        [DataRow(new[] { "--aaa=\"AA\"", "-a=\"A\"" }, 'a', "aaa", "AA")]
        public void ExecuteTest_Simple(string[] args, char shortKey, string longKey, string expected)
        {
            var commandLine = new CommandLine(args, false);
            var commanadKey = commandLine.Add(shortKey, longKey, true);

            Assert.IsTrue(commandLine.Parse());
            var value = commandLine.Values[commanadKey];
            Assert.IsTrue(value.First == expected);
        }

        [TestMethod]
        [DataRow(new[] { "/a" }, 'a', "aaa", true)]
        [DataRow(new[] { "/aaa" }, 'a', "aaa", true)]
        [DataRow(new[] { "-a" }, 'a', "aaa", true)]
        [DataRow(new[] { "--aaa" }, 'a', "aaa", true)]
        public void ExecuteTest_Switch(string[] args, char shortKey, string longKey, bool expected)
        {
            var commandLine = new CommandLine(args, false);
            var commanadKey = commandLine.Add(shortKey, longKey, false);

            Assert.IsTrue(commandLine.Parse());
            var has = commandLine.Switches.Contains(commanadKey);
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
            public SByte SByte { get; set; }
            [CommandLine(longKey: "byte")]
            public Byte Byte { get; set; }
            [CommandLine(longKey: "int16")]
            public Int16 Int16 { get; set; }
            [CommandLine(longKey: "uint16")]
            public UInt16 UInt16 { get; set; }
            [CommandLine(longKey: "int32")]
            public Int32 Int32 { get; set; }
            [CommandLine(longKey: "uint32")]
            public UInt32 UInt32 { get; set; }
            [CommandLine(longKey: "int64")]
            public Int64 Int64 { get; set; }
            [CommandLine(longKey: "uint64")]
            public UInt64 UInt64 { get; set; }
            [CommandLine(longKey: "single")]
            public Single Single { get; set; }
            [CommandLine(longKey: "double")]
            public Double Double { get; set; }
            [CommandLine(longKey: "decimal")]
            public Decimal Decimal { get; set; }
            [CommandLine(longKey: "boolean")]
            public Boolean Boolean { get; set; }
            [CommandLine(longKey: "boolean-switch", hasValue: false)]
            public Boolean BooleanSwitch { get; set; }
            [CommandLine(longKey: "char")]
            public Char Char { get; set; }
            [CommandLine(longKey: "string")]
            public String String { get; set; } = string.Empty;
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
                new MappingItem("/int16", Int16.MaxValue.ToString(CultureInfo.InvariantCulture), () => cts.Int16 == Int16.MaxValue),
                new MappingItem("/uint16", UInt16.MaxValue.ToString(CultureInfo.InvariantCulture), () => cts.UInt16 == UInt16.MaxValue),
                new MappingItem("/int32", Int32.MaxValue.ToString(CultureInfo.InvariantCulture), () => cts.Int32 == Int32.MaxValue),
                new MappingItem("/uint32", UInt32.MaxValue.ToString(CultureInfo.InvariantCulture), () => cts.UInt32 == UInt32.MaxValue),
                new MappingItem("/int64", Int64.MaxValue.ToString(CultureInfo.InvariantCulture), () => cts.Int64 == Int64.MaxValue),
                new MappingItem("/uint64", UInt64.MaxValue.ToString(CultureInfo.InvariantCulture), () => cts.UInt64 == UInt64.MaxValue),
                new MappingItem("/single", Single.MaxValue.ToString("r", CultureInfo.InvariantCulture), () => cts.Single == Single.MaxValue),
                new MappingItem("/double", Double.MaxValue.ToString("r",CultureInfo.InvariantCulture), () => cts.Double == Double.MaxValue),
                new MappingItem("/decimal", Decimal.MaxValue.ToString(CultureInfo.InvariantCulture), () => cts.Decimal == Decimal.MaxValue),
                new MappingItem("/boolean", true.ToString(), () => cts.Boolean),
                new MappingItem("/char", Char.MaxValue.ToString(), () => cts.Char == Char.MaxValue),
                new MappingItem("/boolean-switch", string.Empty, () => cts.BooleanSwitch),
                new MappingItem("/string", "String", () => cts.String == "String"),
           };

            var commandLineConverter = new CommandLineConverter<Cts>(new CommandLine(data.Select(d => new[] { d.Key, d.Value }).SelectMany(i => i), false), cts);
            commandLineConverter.Mapping();
            foreach(var item in data) {
                Assert.IsTrue(item.Test(), $"{item.Key}, {item.Value}");
            }
        }
    }
}
