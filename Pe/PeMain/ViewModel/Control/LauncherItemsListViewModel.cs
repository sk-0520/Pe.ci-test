namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Data;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public class LauncherItemsListViewModel : SingleModelWrapperViewModelBase<LauncherItemCollectionModel>, IHavingAppNonProcess, IHavingAppSender
	{
		#region variable

		string _filterText;

		#endregion

		public LauncherItemsListViewModel(LauncherItemCollectionModel model, IAppNonProcess appNonProcess, IAppSender appSender)
			: base(model)
		{
			AppNonProcess = appNonProcess;
			AppSender = appSender;

			LauncherItemPairList = new MVMPairCreateDelegationCollection<LauncherItemModel, LauncherListItemViewModel>(
				Model,
				default(object),
				CreateItemViewModel
			);

			Items = CollectionViewSource.GetDefaultView(LauncherItemPairList.ViewModelList);
			Items.Filter = o => {
				var s = FilterText ?? string.Empty;
				var vm = (LauncherListItemViewModel)o;
				return vm.Model.Name.StartsWith(s, StringComparison.CurrentCultureIgnoreCase);
			};
			Items.Refresh();
		}


		#region property

		internal MVMPairCreateDelegationCollection<LauncherItemModel, LauncherListItemViewModel> LauncherItemPairList { get; private set; }

		public string FilterText
		{
			get { return this._filterText; }
			set
			{
				SetVariableValue(ref this._filterText, value);
				Items.Refresh();
				if (Items.IsEmpty && !string.IsNullOrWhiteSpace(this._filterText)) {
					this._filterText = string.Empty;
					Items.Refresh();
					this._filterText = value;
				}
			}
		}

		public ICollectionView Items { get; private set;}

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
