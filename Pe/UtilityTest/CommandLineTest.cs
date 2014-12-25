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
		[TestCase(1,"a")]
		[TestCase(1,"/a")]
		[TestCase(1,"/a=1")]
		[TestCase(2,"/a=1", "/b")]
		[TestCase(2,"/a=1", "/b=2")]
		[TestCase(3,"/a=1", "/b=2", "/c")]
		[TestCase(3,"/a=1", "/b=2", "/c=3")]
		[TestCase(4,"/a=1", "/b=2", "/c=3", "4")]
		[TestCase(3,"/a=1", "/b=2", "\"/c=3 4\"")]
		public void Length(int result, params string[] args)
		{
			var cl = new CommandLine(args.ToArray());
			Assert.IsTrue(result == cl.Length);
		}

		[TestCase("a", "1", "/a=1", "/b=2", "/c=3", "4", "d=5", "/d=50")]
		[TestCase("b", "2", "/a=1", "/b=2", "/c=3", "4", "d=5", "/d=50")]
		[TestCase("c", "3", "/a=1", "/b=2", "/c=3", "4", "d=5", "/d=50")]
		[TestCase("d", "50", "/a=1", "/b=2", "/c=3", "4", "d=5", "/d=50")]
		[TestCase("a", "abc", "/a=abc")]
		[TestCase("a", "abc", "/a=\"abc\"")]
		public void GetValue(string option, string value, params string[] args)
		{
			var cl = new CommandLine(args.ToArray());
			Assert.IsTrue(cl.GetValue(option) == value);
		}
	}
}
