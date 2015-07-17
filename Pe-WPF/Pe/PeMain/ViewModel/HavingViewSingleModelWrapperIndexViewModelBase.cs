namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;

	/// <summary>
	/// 何やってんのかもうわっけわからんけどテンプレートとクリップボードで共有したいのさ。
	/// </summary>
	/// <typeparam name="TModel"></typeparam>
	/// <typeparam name="TView"></typeparam>
	/// <typeparam name="TCollectionModel"></typeparam>
	/// <typeparam name="TItemModel"></typeparam>
	/// <typeparam name="TItemViewModel"></typeparam>
	public abstract class HavingViewSingleModelWrapperIndexViewModelBase<TModel, TView, TCollectionModel, TItemModel, TItemViewModel> : HavingViewSingleModelWrapperViewModelBase<TModel, TView>, IHavingClipboardWatcher, IHavingVariableConstants, IHavingNonProcess, IHavingAppSender
		where TModel: IModel
		where TView: UIElement
		where TCollectionModel : IndexItemCollectionModel<TItemModel>, new()
		where TItemModel : IndexItemModelBase
		where TItemViewModel : SingleModelWrapperViewModelBase<TItemModel>
	{
		public HavingViewSingleModelWrapperIndexViewModelBase(TModel model, TView view, IndexSettingModelBase<TCollectionModel, TItemModel> indexModel, INonProcess nonProcess, IClipboardWatcher clipboardWatcher, VariableConstants variableConstants, IAppSender appSender)
			: base(model, view)
		{
			NonProcess = nonProcess;
			ClipboardWatcher = clipboardWatcher;
			VariableConstants = variableConstants;
			AppSender = appSender;

			IndexModel = indexModel;

			IndexPairList = new MVMPairCreateDelegationCollection<TItemModel, TItemViewModel>(
				IndexModel.Items,
				default(object),
				CreateIndexViewModel
			);
		}

		#region property

		protected IndexSettingModelBase<TCollectionModel, TItemModel> IndexModel { get; private set; }

		protected MVMPairCreateDelegationCollection<TItemModel, TItemViewModel> IndexPairList { get; private set; }

		public ObservableCollection<TItemViewModel> IndexItems { get { return IndexPairList.ViewModelList; } }

		#endregion

		#region function

		protected abstract TItemViewModel CreateIndexViewModel(TItemModel model, object data);

		#endregion

		#region IHavingNonProcess

		public INonProcess NonProcess { get; private set; }

		#endregion

		#region IHavingClipboardWatcher

		public IClipboardWatcher ClipboardWatcher { get; private set; }

		#endregion

		#region IHavingVariableConstants

		public VariableConstants VariableConstants { get; private set; }

		#endregion

		#region IHavingAppSender

		public IAppSender AppSender { get; private set; }

		#endregion
	}
}
