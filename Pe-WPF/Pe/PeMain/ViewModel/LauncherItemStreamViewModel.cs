namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.View;

	public class LauncherItemStreamViewModel : LauncherItemSimpleViewModel, IHavingView<LauncherItemStreamWindow>
	{
		public LauncherItemStreamViewModel(LauncherItemModel model, LauncherItemStreamWindow view, LauncherIconCaching launcherIconCaching, INonProcess nonPorocess)
			: base(model, launcherIconCaching, nonPorocess)
		{
			View = view;
		}
		#region IHavingView

		public LauncherItemStreamWindow View { get; private set; }

		public bool HasView { get { return View != null;} }

		#endregion
	}

}
