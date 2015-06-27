namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.Data;

	public class LauncherItemSettingViewModel: ViewModelBase
	{
		public LauncherItemSettingViewModel(LauncherItemCollectionModel launcherItemCollection, LauncherIconCaching launcherIconCaching)
			: base()
		{
			LauncherItemCollection = launcherItemCollection;
			LauncherIconCaching = launcherIconCaching;
		}

		#region proerty

		LauncherItemCollectionModel LauncherItemCollection { get; set; }
		LauncherIconCaching LauncherIconCaching { get; set; }

		#endregion
	}
}
