namespace PeDataTest.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using NUnit.Framework;

	[TestFixture]
	class IconItemModelTest
	{
		[Test]
		public void DeepCloneTest()
		{
			var src = new IconItemModel() {
				Path = "path",
				Index = 1,
			};

			var cp = (IconItemModel)src.DeepClone();

			Assert.AreEqual(cp.Path, src.Path);
			Assert.AreEqual(cp.Index, src.Index);

			src.Path = "test";
			src.Index = 2;

			Assert.AreNotEqual(cp.Path, src.Path);
			Assert.AreNotEqual(cp.Index, src.Index);
		}
	}
}
