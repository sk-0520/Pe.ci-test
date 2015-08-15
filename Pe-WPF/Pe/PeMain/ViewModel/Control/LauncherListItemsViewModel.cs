namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public class LauncherListItemsViewModel : SingleModelWrapperViewModelBase<LauncherItemCollectionModel>, IHavingAppNonProcess, IHavingAppSender
	{
		public LauncherListItemsViewModel(LauncherItemCollectionModel model, IAppNonProcess appNonProcess, IAppSender appSender)
			: base(model)
		{
			AppNonProcess = appNonProcess;
			AppSender = appSender;

			LauncherItemPairList = new MVMPairCreateDelegationCollection<LauncherItemModel, LauncherListItemViewModel>(
				Model,
				default(object),
				CreateItemViewModel
			);
		}


		#region property

		internal MVMPairCreateDelegationCollection<LauncherItemModel, LauncherListItemViewModel> LauncherItemPairList { get; private set; }

		public CollectionModel<LauncherListItemViewModel> Items
		{
			get
			{
				return LauncherItemPairList.ViewModelList;
			}
		}

		#endregion

		#region function

		LauncherListItemViewModel CreateItemViewModel(LauncherItemModel model, object data)
		{
			return new LauncherListItemViewModel(model, AppNonProcess, AppSender);
		}

		#endregion

		#region IHavingClipboardWatcher

		public IClipboardWatcher ClipboardWatcher { get; set; }

		#endregion

		#region IHavingAppNonProcess

		public IAppNonProcess AppNonProcess { get; private set; }

		#endregion

		#region IHavingAppSender

		public IAppSender AppSender { get; private set; }

		#endregion
	}
}
