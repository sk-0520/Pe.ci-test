namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	public class LauncherViewModel: SingleModelWrapperViewModelBase<LauncherItemModel>
	{
		public LauncherViewModel(LauncherItemModel model)
			: base(model)
		{ }
	}
}
