namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Controls;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public abstract class SettingPageLauncherIconCacheViewModelBase<TView>: SettingPageViewModelBase<TView>
		where TView: UserControl
	{
		public SettingPageLauncherIconCacheViewModelBase(TView view, IAppNonProcess appNonProcess, SettingNotifyData settingNotifiyItem)
			: base(view, appNonProcess, settingNotifiyItem)
		{ }
	}
}
