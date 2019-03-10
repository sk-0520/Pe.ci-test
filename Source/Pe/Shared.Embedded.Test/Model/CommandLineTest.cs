using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shared.Embedded.Test.Model
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
        public void ConstructorTest(string[] args, bool firstIsProgram, string resultProgramName, int resultArgumentCount)
        {
            var commandLine = new CommandLine(args, firstIsProgram);
            Assert.IsTrue(commandLine.ProgramName == resultProgramName);
            Assert.IsTrue(commandLine.Arguments.Count == resultArgumentCount);
        }

        [TestMethod]
        [DataRow(true, 'a', "aa")]
        [DataRow(true, '\0', "aa")]
        [DataRow(true, 'a', "")]
        [DataRow(false, '\0', "")]
        [DataRow(false, '\0', "a")]
        public void AddTest(bool result, char shortKey, string longKey)
        {
            var commandLine = new CommandLine();
            try {
                var key = commandLine.Add(shortKey, longKey);
                Assert.IsTrue(result);
            } catch(ArgumentException ex) {
                Assert.IsFalse(result, ex.ToString());
            }
        }

        [TestMethod]
        [DataRow(false, 'a', "")]
        [DataRow(false, '\0', "aa")]
        [DataRow(false, '\0', "bb")]
        [DataRow(false, 'c', "")]
        [DataRow(true, 'b', "aaa")]
        public void AddTest_Exists(bool result, char shortKey, string longKey)
        {
            var commandLine = new CommandLine();
            commandLine.Add('a', "aa");
            commandLine.Add('\0', "bb");
            commandLine.Add('c', "");

            try {
                var key = commandLine.Add(shortKey, longKey);
                Assert.IsTrue(result);
            } catch(ArgumentException ex) {
                Assert.IsFalse(result, ex.ToString());
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
        public void ExecuteTest_Simple(string[] args, char shortKey, string longKey, string result)
        {
            var commandLine = new CommandLine(args, false);
            var commanadKey = commandLine.Add(shortKey, longKey, true);

            Assert.IsTrue(commandLine.Parse());
            var value = commandLine.Values[commanadKey];
            Assert.IsTrue(value.First == result);
        }

        [TestMethod]
        [DataRow(new[] { "/a" }, 'a', "aaa", true)]
        [DataRow(new[] { "/aaa" }, 'a', "aaa", true)]
        [DataRow(new[] { "-a" }, 'a', "aaa", true)]
        [DataRow(new[] { "--aaa" }, 'a', "aaa", true)]
        public void ExecuteTest_Switch(string[] args, char shortKey, string longKey, bool result)
        {
            var commandLine = new CommandLine(args, false);
            var commanadKey = commandLine.Add(shortKey, longKey, false);

            Assert.IsTrue(commandLine.Parse());
            var has = commandLine.Switch.Contains(commanadKey);
            Assert.IsTrue(has == result);
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
            public String String { get; set; }
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
                new MappingItem("/sbyte", sbyte.MaxValue.ToString(), () => cts.SByte == sbyte.MaxValue),
                new MappingItem("/byte", byte.MaxValue.ToString(), () => cts.Byte == byte.MaxValue),
                new MappingItem("/int16", Int16.MaxValue.ToString(), () => cts.Int16 == Int16.MaxValue),
                new MappingItem("/uint16", UInt16.MaxValue.ToString(), () => cts.UInt16 == UInt16.MaxValue),
                new MappingItem("/int32", Int32.MaxValue.ToString(), () => cts.Int32 == Int32.MaxValue),
                new MappingItem("/uint32", UInt32.MaxValue.ToString(), () => cts.UInt32 == UInt32.MaxValue),
                new MappingItem("/int64", Int64.MaxValue.ToString(), () => cts.Int64 == Int64.MaxValue),
                new MappingItem("/uint64", UInt64.MaxValue.ToString(), () => cts.UInt64 == UInt64.MaxValue),
                new MappingItem("/single", Single.MaxValue.ToString("r"), () => cts.Single == Single.MaxValue),
                new MappingItem("/double", Double.MaxValue.ToString("r"), () => cts.Double == Double.MaxValue),
                new MappingItem("/decimal", Decimal.MaxValue.ToString(), () => cts.Decimal == Decimal.MaxValue),
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
