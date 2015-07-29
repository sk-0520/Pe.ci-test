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
	using ContentTypeTextNet.Pe.Library.PeData.Setting;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public class LauncherItemSettingViewModel: SettingPageLauncherIconCacheViewModelBase
	{
		#region variable

		LauncherItemsViewModel _launcherItems;

		#endregion

		public LauncherItemSettingViewModel(LauncherItemSettingModel launcherItemSetting, LauncherIconCaching launcherIconCaching, IAppNonProcess nonProcess, SettingNotifiyItem settingNotifiyItem)
			: base(launcherIconCaching, nonProcess, settingNotifiyItem)
		{
			LauncherItemSetting = launcherItemSetting;
		}

		#region proerty

		LauncherItemSettingModel LauncherItemSetting { get; set; }

		public LauncherItemsViewModel LauncherItems
		{
			get
			{
				if(this._launcherItems == null) {
					this._launcherItems = new LauncherItemsViewModel(
						LauncherItemSetting.Items,
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
