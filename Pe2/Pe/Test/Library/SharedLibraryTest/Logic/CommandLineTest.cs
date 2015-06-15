namespace ContentTypeTextNet.Test.Library.SharedLibraryTest.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using NUnit.Framework;

	[TestFixture]
	public class CommandLineTest
	{
		[TestCase("a", "a")]
		public void ToCommandLineArgumentsTest(string arg, params string[] test)
		{
			Assert.IsTrue(CommandLine.ToCommandLineArguments(arg).SequenceEqual(test));
		}

		[TestCase(1, "a")]
		[TestCase(1, "/a")]
		[TestCase(1, "/a=1")]
		[TestCase(2, "/a=1", "/b")]
		[TestCase(2, "/a=1", "/b=2")]
		[TestCase(3, "/a=1", "/b=2", "/c")]
		[TestCase(3, "/a=1", "/b=2", "/c=3")]
		[TestCase(4, "/a=1", "/b=2", "/c=3", "4")]
		[TestCase(3, "/a=1", "/b=2", "\"/c=3 4\"")]
		public void LengthTest(int test, params string[] values)
		{
			var cl = new CommandLine(values.ToArray());
			Assert.IsTrue(test == cl.Length);
		}

		[TestCase(true, "a", "/a")]
		[TestCase(true, "a", "/a=val")]
		public void HasOptionTest(bool test, string option, params string[] values)
		{
			var cl = new CommandLine(values);
			Assert.IsTrue(cl.HasOption(option) == test);
		}


		[TestCase(false, "a", "/a")]
		[TestCase(true, "a", "/a=")]
		[TestCase(true, "a", "/a=val")]
		[TestCase(false, "a", "/a", "/a=val")]
		public void HasValueTest(bool test, string option, params string[] values)
		{
			var cl = new CommandLine(values);
			Assert.IsTrue(cl.HasValue(option) == test);
		}
		[TestCase(false, "a", 0, "/a")]
		[TestCase(true, "a", 0, "/a=")]
		[TestCase(true, "a", 0, "/a=val")]
		[TestCase(false, "a", 0, "/a", "/a=val")]
		[TestCase(true, "a", 1, "/a", "/a=val")]
		[TestCase(true, "a", 1, "/a", "/b", "/a=val")]
		public void HasValueTest(bool test, string option, int index, params string[] values)
		{
			var cl = new CommandLine(values);
			Assert.IsTrue(cl.HasValue(option, index) == test);
		}


		[TestCase("a", "1", "/a=1", "/b=2", "/c=3", "4", "d=5", "/d=50")]
		[TestCase("b", "2", "/a=1", "/b=2", "/c=3", "4", "d=5", "/d=50")]
		[TestCase("c", "3", "/a=1", "/b=2", "/c=3", "4", "d=5", "/d=50")]
		[TestCase("d", "50", "/a=1", "/b=2", "/c=3", "4", "d=5", "/d=50")]
		[TestCase("a", "abc", "/a=abc")]
		[TestCase("a", "abc", "/a=\"abc\"")]
		[TestCase("a", "abc", "/a=abc", "/a=def")]
		public void GetValueTest(string option, string test, params string[] values)
		{
			var cl = new CommandLine(values.ToArray());
			Assert.IsTrue(cl.GetValue(option) == test);
		}

		[TestCase("a", "abc", 0, "/a=abc", "/a=def")]
		[TestCase("a", "def", 1, "/a=abc", "/a=def")]
		public void GetValuesTest(string option, string test, int index, params string[] values)
		{
			var cl = new CommandLine(values.ToArray());
			Assert.IsTrue(cl.GetValues(option).ElementAt(index) == test);
		}
	}
}
