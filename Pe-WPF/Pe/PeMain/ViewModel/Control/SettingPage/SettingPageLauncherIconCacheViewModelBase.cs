namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public abstract class SettingPageLauncherIconCacheViewModelBase : SettingPageViewModelBase, IHavingLauncherIconCaching
	{
		public SettingPageLauncherIconCacheViewModelBase(LauncherIconCaching launcherIconCaching, IAppNonProcess nonProcess, SettingNotifiyItem settingNotifiyItem)
			: base(nonProcess, settingNotifiyItem)
		{
			CheckUtility.EnforceNotNull(launcherIconCaching);

			LauncherIconCaching = launcherIconCaching;
		}

		#region IHavingLauncherIconCaching

		public LauncherIconCaching LauncherIconCaching { get; private set; }

		#endregion

	}
}
