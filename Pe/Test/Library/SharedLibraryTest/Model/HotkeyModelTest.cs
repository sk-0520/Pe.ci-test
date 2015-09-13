namespace ContentTypeTextNet.Test.Library.SharedLibraryTest.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using NUnit.Framework;

	[TestFixture]
	class HotkeyModelTest
	{
		[Test]
		public void DeepCloneTest()
		{
			var src = new HotKeyModel() {
				Key = Key.A,
				ModifierKeys = ModifierKeys.Control
			};

			var cp = (HotKeyModel)src.DeepClone();

			Assert.AreEqual(cp.Key, src.Key);
			Assert.AreEqual(cp.ModifierKeys, src.ModifierKeys);

			src.Key = Key.B;
			src.ModifierKeys = ModifierKeys.Alt | ModifierKeys.Control;

			Assert.AreNotEqual(cp.Key, src.Key);
			Assert.AreNotEqual(cp.ModifierKeys, src.ModifierKeys);
		}
	}
}
