namespace ContentTypeTextNet.Pe.Test.Library.PeMainTest.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
	using NUnit.Framework;

	[TestFixture]
	class DisplayTextUtilityTest
	{
		[TestCase("", "")]
		[TestCase("", null)]
		//[TestCase(null, null)]
		[TestCase("name", "name")]
		[TestCase("name", "name")]
		//[TestCase("id", "")]
		//[TestCase("id", " ")]
		//[TestCase("id", null)]
		public void GetDisplayName_LauncherItemModel_Test(string test, string name)
		{
			var model = new LauncherItemModel() {
				Name = name,
			};
			var result = DisplayTextUtility.GetDisplayName(model);
			Assert.AreEqual(test, result);
		}
	}
}
