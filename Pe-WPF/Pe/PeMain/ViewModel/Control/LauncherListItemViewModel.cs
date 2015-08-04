namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public class LauncherListItemViewModel : LauncherItemViewModelBase
	{
		public LauncherListItemViewModel(LauncherItemModel model, IAppNonProcess nonPorocess, IAppSender appSender)
			: base(model, nonPorocess, appSender)
		{ }
	}
}
