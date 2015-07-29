namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public class LauncherItemsViewModel: SingleModelWrapperViewModelBase<LauncherItemCollectionModel>, IHavingAppNonProcess
	{
		public LauncherItemsViewModel(LauncherItemCollectionModel model, IAppNonProcess appNonProcess)
			: base(model)
		{
			AppNonProcess = appNonProcess;
		}

		#region property

		#region IHavingClipboardWatcher

		public IClipboardWatcher ClipboardWatcher { get; set; }

		#endregion

		#region IHavingAppNonProcess

		public IAppNonProcess AppNonProcess { get; private set; }

		#endregion

		public CollectionModel<LauncherItemModel> Items 
		{ 
			get
			{
				return Model;
			}
		}

		#endregion
	}
}
