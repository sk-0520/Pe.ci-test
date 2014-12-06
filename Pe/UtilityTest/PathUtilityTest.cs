using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ContentTypeTextNet.Pe.Library.Utility;

namespace ContentTypeTextNet.Pe.Test.UtilityTest
{
	[TestFixture]
	public class PathUtilityTest
	{
		[TestCase(false, "")]
		[TestCase(false, "a.txt")]
		[TestCase(false, "a.xexe")]
		[TestCase(false, ".htaccess")]
		[TestCase(false, "exe")]
		[TestCase(false, "com")]
		[TestCase(false, "bat")]
		[TestCase(true, ".exe")]
		[TestCase(true, ".com")]
		[TestCase(true, ".bat")]
		[TestCase(true, "a.exe")]
		[TestCase(true, "b.com")]
		[TestCase(true, "v.bat")]
		public void IsExecutePath(bool result, string s)
		{
			Assert.IsTrue(result == PathUtility.IsExecutePath(s));
		}
	}
}
