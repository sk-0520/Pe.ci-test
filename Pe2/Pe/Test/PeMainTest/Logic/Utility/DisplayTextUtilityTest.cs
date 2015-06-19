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
		[TestCase("", "", "")]
		[TestCase("", "", null)]
		[TestCase(null, null, null)]
		[TestCase("name", "id", "name")]
		[TestCase("name", "", "name")]
		[TestCase("id", "id", "")]
		[TestCase("id", "id", " ")]
		[TestCase("id", "id", null)]
		public void GetDisplayName_LauncherItemModel_Test(string test, string id, string name)
		{
			var model = new LauncherItemModel() {
				Id = id,
				Name = name,
			};
			var result = DisplayTextUtility.GetDisplayName(model);
			Assert.AreEqual(test, result);
		}
	}
}
