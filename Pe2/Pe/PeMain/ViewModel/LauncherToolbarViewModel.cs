namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.View;

	public class LauncherToolbarViewModel : HavingViewSingleModelWrapperViewModelBase<LauncherToolbarItemModel, LauncherToolbarWindow>
	{
		public LauncherToolbarViewModel(LauncherToolbarItemModel model, LauncherToolbarWindow view)
			: base(model, view)
		{ }

		
	}
}
