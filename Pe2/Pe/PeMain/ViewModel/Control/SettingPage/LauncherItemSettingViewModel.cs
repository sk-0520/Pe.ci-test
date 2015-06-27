namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public class LauncherItemSettingViewModel: ViewModelBase, IHavingNonProcess, IHavingLauncherIconCaching
	{
		#region variable

		LauncherItemsViewModel _launcherItems;

		#endregion

		public LauncherItemSettingViewModel(LauncherItemCollectionModel launcherItemCollection, LauncherIconCaching launcherIconCaching, INonProcess nonProcess)
			: base()
		{
			LauncherItemCollection = launcherItemCollection;
			LauncherIconCaching = launcherIconCaching;
			NonProcess = nonProcess;
		}

		#region proerty

		LauncherItemCollectionModel LauncherItemCollection { get; set; }

		#region IHavingLauncherIconCaching

		public LauncherIconCaching LauncherIconCaching { get; private set; }

		#endregion

		#region IHavingNonProcess

		public INonProcess NonProcess { get; private set; }

		#endregion

		public LauncherItemsViewModel LauncherItems
		{
			get
			{
				if(this._launcherItems == null) {
					this._launcherItems = new LauncherItemsViewModel(
						LauncherItemCollection,
						LauncherIconCaching,
						NonProcess
					);
				}

				return this._launcherItems;
			}
		}

		#endregion
	}
}
