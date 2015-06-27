namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;

	public class LauncherItemsViewModel: SingleModelWrapperViewModelBase<LauncherItemCollectionModel>, IHavingNonProcess
	{
		public LauncherItemsViewModel(LauncherItemCollectionModel model, INonProcess nonProcess)
			: base(model)
		{
			NonProcess = nonProcess;
		}

		#region property

		public LauncherIconCaching LauncherIcons { get; set; }

		#region IHavingNonProcess

		public INonProcess NonProcess { get; private set; }

		#endregion

		#endregion
	}
}
