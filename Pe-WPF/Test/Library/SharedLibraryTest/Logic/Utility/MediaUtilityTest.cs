namespace ContentTypeTextNet.Test.Library.SharedLibraryTest.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using NUnit.Framework;

	[TestFixture]
	class MediaUtilityTest
	{
		[TestCase(0x00, 0x00, 0x00, 0x00, 0x00, 0xff, 0xff, 0xff)]
		[TestCase(0x00, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00)]
		[TestCase(0x00, 0x10, 0xff, 0xff, 0x00, 0xef, 0x00, 0x00)]
		public void GetNegativeColorTest(byte testA, byte testR, byte testG, byte testB, byte argA, byte argR, byte argG, byte argB)
		{
			var test = Color.FromArgb(testA, testR, testG, testB);
			var arg = Color.FromArgb(argA, argR, argG, argB);

			var result = MediaUtility.GetNegativeColor(arg);

			Assert.AreEqual(test, result);
		}
	}
}
