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

	public class LauncherItemSettingViewModel: SettingPageLauncherIconCacheViewModelBase, IHavingAppSender
	{
		#region variable

		LauncherListItemsViewModel _launcherItems;

		#endregion

		public LauncherItemSettingViewModel(LauncherItemSettingModel launcherItemSetting, IAppNonProcess appNonProcess, IAppSender appSender, SettingNotifiyItem settingNotifiyItem)
			: base(appNonProcess, settingNotifiyItem)
		{
			LauncherItemSetting = launcherItemSetting;
			AppSender = appSender;
		}

		#region proerty

		LauncherItemSettingModel LauncherItemSetting { get; set; }

		public LauncherListItemsViewModel LauncherItems
		{
			get
			{
				if(this._launcherItems == null) {
					this._launcherItems = new LauncherListItemsViewModel(
						LauncherItemSetting.Items,
						AppNonProcess,
						AppSender
					);
				}

				return this._launcherItems;
			}
		}

		#endregion

		#region IHavingAppSender

		public IAppSender AppSender { get; private set; }

		#endregion
	}
}
