namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;

	public class LauncherToolbarSettingViewModel : LauncherToolbarViewModelBase<UIElement>
	{
		public LauncherToolbarSettingViewModel(LauncherToolbarItemModel model, LauncherIconCaching launcherIconCaching, INonProcess nonProcess)
			: base(model, default(UIElement), launcherIconCaching, nonProcess)
		{ }
	}
}
