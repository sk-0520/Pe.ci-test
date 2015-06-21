namespace ContentTypeTextNet.Pe.Test.Library.PeMainTest.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using NUnit.Framework;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;

	[TestFixture]
	class LauncherToolbarViewModelTest
	{
		[TestCase("a", null, "a")]
		[TestCase("a", "", "a")]
		[TestCase("a", "", "a", "b", "c")]
		[TestCase("b", "b", "a", "b", "c")]
		[TestCase("a", "e", "a", "b", "c")]
		public void SelectedGroupTest(string result, string defaultGroup, params string[] groups)
		{
			var m = new LauncherToolbarItemModel();
			m.Toolbar.DefaultGroupId = defaultGroup;
			foreach (var group in groups) {
				var lg = new LauncherGroupItemModel() {
					Id = group,
				};
				m.GroupItems.Add(lg);
			}
			var vm = new LauncherToolbarViewModel(m, null, null);
			Assert.AreEqual(result, vm.SelectedGroup.Id);
		}
	}
}
