namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.PeMain.Data;

	public class LauncherItemsViewModel: ViewModelBase
	{
		public LauncherItemsViewModel()
			: base()
		{ }

		#region property

		public LauncherIconCaching LauncherIcons { get; set; }

		#endregion
	}
}
