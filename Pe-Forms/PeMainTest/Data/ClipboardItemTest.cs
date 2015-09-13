namespace ContentTypeTextNet.Pe.Test.ApplicationTest.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using NUnit.Framework;

	[TestFixture]
	class ClipboardItemTest
	{
		[Test]
		public void DeepCloneTest_Name()
		{
			var ci = new ClipboardItem() {
				Name = "test",
			};
			var dc = (ClipboardItem)ci.DeepClone();
			Assert.AreEqual(ci.Name, dc.Name);
		}
	}
}
