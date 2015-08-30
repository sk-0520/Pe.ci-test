using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Utility;
using NUnit.Framework;

namespace ContentTypeTextNet.Pe.Test.UtilityTest
{
	[TestFixture]
	public class CommandLineTest
	{
		[TestCase("a", "a")]
		public void ToCommandLineArgumentsTest(string arg, params string[] result)
		{
			Assert.IsTrue(CommandLine.ToCommandLineArguments(arg).SequenceEqual(result));
		}

		[TestCase(1,"a")]
		[TestCase(1,"/a")]
		[TestCase(1,"/a=1")]
		[TestCase(2,"/a=1", "/b")]
		[TestCase(2,"/a=1", "/b=2")]
		[TestCase(3,"/a=1", "/b=2", "/c")]
		[TestCase(3,"/a=1", "/b=2", "/c=3")]
		[TestCase(4,"/a=1", "/b=2", "/c=3", "4")]
		[TestCase(3,"/a=1", "/b=2", "\"/c=3 4\"")]
		public void LengthTest(int result, params string[] args)
		{
			var cl = new CommandLine(args.ToArray());
			Assert.IsTrue(result == cl.Length);
		}

		[TestCase(true, "a", "/a")]
		[TestCase(true, "a", "/a=val")]
		public void HasOptionTest(bool test, string option, params string[] args)
		{
			var cl = new CommandLine(args);
			Assert.IsTrue(cl.HasOption(option) == test);
		}


		[TestCase(false, "a", "/a")]
		[TestCase(true, "a", "/a=")]
		[TestCase(true, "a", "/a=val")]
		[TestCase(false, "a", "/a", "/a=val")]
		public void HasValueTest(bool test, string option, params string[] args)
		{
			var cl = new CommandLine(args);
			Assert.IsTrue(cl.HasValue(option) == test);
		}
		[TestCase(false, "a", 0, "/a")]
		[TestCase(true, "a", 0, "/a=")]
		[TestCase(true, "a", 0, "/a=val")]
		[TestCase(false, "a", 0, "/a", "/a=val")]
		[TestCase(true, "a", 1, "/a", "/a=val")]
		[TestCase(true, "a", 1, "/a", "/b", "/a=val")]
		public void HasValueTest(bool test, string option, int index, params string[] args)
		{
			var cl = new CommandLine(args);
			Assert.IsTrue(cl.HasValue(option, index) == test);
		}


		[TestCase("a", "1", "/a=1", "/b=2", "/c=3", "4", "d=5", "/d=50")]
		[TestCase("b", "2", "/a=1", "/b=2", "/c=3", "4", "d=5", "/d=50")]
		[TestCase("c", "3", "/a=1", "/b=2", "/c=3", "4", "d=5", "/d=50")]
		[TestCase("d", "50", "/a=1", "/b=2", "/c=3", "4", "d=5", "/d=50")]
		[TestCase("a", "abc", "/a=abc")]
		[TestCase("a", "abc", "/a=\"abc\"")]
		[TestCase("a", "abc", "/a=abc", "/a=def")]
		public void GetValueTest(string option, string value, params string[] args)
		{
			var cl = new CommandLine(args.ToArray());
			Assert.IsTrue(cl.GetValue(option) == value);
		}

		[TestCase("a", "abc", 0, "/a=abc", "/a=def")]
		[TestCase("a", "def", 1, "/a=abc", "/a=def")]
		public void GetValuesTest(string option, string value, int index, params string[] args)
		{
			var cl = new CommandLine(args.ToArray());
			Assert.IsTrue(cl.GetValues(option).ElementAt(index) == value);
		}
	}
}
