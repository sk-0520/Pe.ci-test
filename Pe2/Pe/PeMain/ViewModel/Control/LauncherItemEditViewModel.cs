namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;

	public class LauncherItemEditViewModel: LauncherSimpleViewModel
	{
		public LauncherItemEditViewModel(LauncherItemModel model, LauncherIconCaching launcherIconCaching, INonProcess nonPorocess)
			: base(model, launcherIconCaching, nonPorocess)
		{ }
	}
}
