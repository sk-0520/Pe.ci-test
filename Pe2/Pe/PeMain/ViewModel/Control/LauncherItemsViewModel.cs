namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public class LauncherItemsViewModel: SingleModelWrapperViewModelBase<LauncherItemCollectionModel>, IHavingNonProcess, IHavingLauncherIconCaching
	{
		public LauncherItemsViewModel(LauncherItemCollectionModel model, LauncherIconCaching launcherIconCaching, INonProcess nonProcess)
			: base(model)
		{
			LauncherIconCaching = launcherIconCaching;
			NonProcess = nonProcess;
		}

		#region property

		#region IHavingLauncherIconCaching

		public LauncherIconCaching LauncherIconCaching { get; set; }

		#endregion

		#region IHavingClipboardWatcher

		public IClipboardWatcher ClipboardWatcher { get; set; }

		#endregion

		#region IHavingNonProcess

		public INonProcess NonProcess { get; private set; }

		#endregion

		public ObservableCollection<LauncherSimpleViewModel> Items 
		{ 
			get
			{
				return new ObservableCollection<LauncherSimpleViewModel>(Model.Items.Select(i => new LauncherSimpleViewModel(i, LauncherIconCaching, NonProcess)));
			}
		}

		#endregion
	}
}
