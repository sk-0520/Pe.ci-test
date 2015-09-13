namespace ContentTypeTextNet.Test.Library.SharedLibraryTest.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using NUnit.Framework;

	[TestFixture]
	class EnumUtilityTest
	{
		[TestCase(IconScale.Small, IconScale.Small, IconScale.Small)]
		[TestCase(IconScale.Small, IconScale.Small, IconScale.Normal)]
		[TestCase(IconScale.Small, IconScale.Small, IconScale.Big)]
		[TestCase(IconScale.Small, IconScale.Small, IconScale.Large)]
		[TestCase(IconScale.Small, 16, IconScale.Large)]
		[TestCase(IconScale.Large, 17, IconScale.Large)]
		[TestCase(IconScale.Normal, 32, IconScale.Large)]
		public void GetNormalization_IconScale_Test(IconScale result, object test, IconScale def)
		{
			var r = EnumUtility.GetNormalization(test, def);
			Assert.AreEqual(r, result);
		}
	}
}
