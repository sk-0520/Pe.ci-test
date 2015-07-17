namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Property;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
	using ContentTypeTextNet.Pe.PeMain.View;

	public class TemplateViewModel : HavingViewSingleModelWrapperViewModelBase<TemplateSettingModel, TemplateWindow>, IHavingClipboardWatcher, IHavingVariableConstants, IHavingNonProcess, IHavingAppSender
	{
		#region variable

		TemplateItemViewModel _selectedViewModel;

		#endregion

		public TemplateViewModel(TemplateSettingModel model, TemplateWindow view, TemplateIndexSettingModel indexModel, INonProcess nonProcess, IClipboardWatcher clipboardWatcher, VariableConstants variableConstants, IAppSender appSender)
			: base(model, view)
		{
			IndexModel = indexModel;
			NonProcess = nonProcess;
			ClipboardWatcher = clipboardWatcher;
			VariableConstants = variableConstants;
			AppSender = appSender;

			InitializeIndexPairList();
		}

		#region property

		TemplateIndexSettingModel IndexModel { get; set; }

		MVMPairCreateDelegationCollection<TemplateIndexItemModel, TemplateItemViewModel> IndexPairList { get; set; }

		public ObservableCollection<TemplateItemViewModel> IndexItems { get { return IndexPairList.ViewModelList; } }

		public TemplateItemViewModel SelectedViewModel
		{
			get { return this._selectedViewModel; }
			set 
			{ 
				var prevViewModel = this._selectedViewModel;
				if (SetVariableValue(ref this._selectedViewModel, value)) {
					if (prevViewModel != null) {
						SaveItemViewModel(prevViewModel);
					}
				}
			}
		}

		#endregion

		#region command

		public ICommand CreateTemplateItem
		{
			get
			{
				var result = CreateCommand(
					o => {
						var indexModel = SettingUtility.CreateTemplateIndexItem(IndexModel.Items, NonProcess);
						var pair = IndexPairList.Insert(0, indexModel, null);
						SelectedViewModel = pair.ViewModel;
					}
				);

				return result;
			}
		}

		public ICommand RemoveTemplateItem
		{
			get
			{
				var result = CreateCommand(
					o => {
						var nowViewModel = SelectedViewModel;
						if (nowViewModel == null) {
							return;
						}

						IndexPairList.Remove(nowViewModel);
						//IndexModel.Items.Remove(nowViewModel.Model);
						//IndexItems.Remove(nowViewModel);
					}
				);

				return result;
			}
		}

		public ICommand ListItemSelectionChangedCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						if (HasView) {
							View.pageSource.IsSelected = true;
						}
					}
				);

				return result;
			}
		}

		public ICommand WindowDeactivCommnad
		{
			get
			{
				var result = CreateCommand(
					o => {
						if(SelectedViewModel != null && SelectedViewModel.IsChanged) {
							SelectedViewModel.SaveBody();
						}
					}
				);

				return result;
			}
		}

		public ICommand MoveUpItemCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						MoveItem(true, o as TemplateItemViewModel);
					}
				);

				return result;
			}
		}

		public ICommand MoveDownItemCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						MoveItem(false, o as TemplateItemViewModel);
					}
				);

				return result;
			}
		}

		#endregion

		#region function

		void MoveItem(bool moveUp, TemplateItemViewModel itemViewModel)
		{
			if (itemViewModel != null) {
				var model = itemViewModel.Model;
				var index = IndexModel.Items.IndexOf(model);
				var next = 0;
				if (moveUp) {
					if (index == 0) {
						return;
					}
					next = -1;
				} else {
					if (index == IndexModel.Items.Count - 1) {
						return;
					}
					next = +1;
				}

				IndexPairList.SwapIndex(index, index + next);
				//IndexModel.Items.SwapIndex(index, index + next);
				//IndexItems.SwapIndex(index, index + next);

				SelectedViewModel = itemViewModel;
			}
		}

		void InitializeIndexPairList()
		{
			IndexPairList = new MVMPairCreateDelegationCollection<TemplateIndexItemModel, TemplateItemViewModel>(
				IndexModel.Items,
				default(object),
				CreateIndexViewModel
			);
			
			//IndexItems = new CollectionModel<TemplateItemViewModel>(items);
		}

		TemplateItemViewModel CreateIndexViewModel(TemplateIndexItemModel model, object data)
		{
			var result = new TemplateItemViewModel(
				model,
				AppSender,
				ClipboardWatcher,
				NonProcess,
				VariableConstants
			);

			return result;
		}

		void SaveItemViewModel(TemplateItemViewModel vm)
		{
			if (vm.IsChanged) {
				vm.SaveBody();
			}
		}

		#endregion

		#region IWindowStatus

		public double WindowLeft
		{
			get { return WindowStatusProperty.GetWindowLeft(Model); }
			set { WindowStatusProperty.SetWindowLeft(Model, value, OnPropertyChanged); }
		}

		public double WindowTop
		{
			get { return WindowStatusProperty.GetWindowTop(Model); }
			set { WindowStatusProperty.SetWindowTop(Model, value, OnPropertyChanged); }
		}

		public double WindowWidth
		{
			get { return WindowStatusProperty.GetWindowWidth(Model); }
			set { WindowStatusProperty.SetWindowWidth(Model, value, OnPropertyChanged); }
		}

		public double WindowHeight
		{
			get { return WindowStatusProperty.GetWindowHeight(Model); }
			set { WindowStatusProperty.SetWindowHeight(Model, value, OnPropertyChanged); }
		}

		public WindowState WindowState
		{
			get { return WindowStatusProperty.GetWindowState(Model); }
			set { WindowStatusProperty.SetWindowState(Model, value, OnPropertyChanged); }
		}

		#region IVisible

		public Visibility Visibility
		{
			get { return VisibleVisibilityProperty.GetVisibility(Model); }
			set { VisibleVisibilityProperty.SetVisibility(Model, value, OnPropertyChanged); }
		}

		public bool Visible
		{
			get { return VisibleVisibilityProperty.GetVisible(Model); }
			set { VisibleVisibilityProperty.SetVisible(Model, value, OnPropertyChanged); }
		}

		#endregion

		#region ITopMost

		public bool TopMost
		{
			get { return TopMostProperty.GetTopMost(Model); }
			set { TopMostProperty.SetTopMost(Model, value, OnPropertyChanged); }
		}

		#endregion

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

		#region HavingViewSingleModelWrapperViewModelBase

		protected override void InitializeView()
		{
			Debug.Assert(HasView);

			View.UserClosing += View_UserClosing;

			base.InitializeView();
		}

		protected override void UninitializeView()
		{
			Debug.Assert(HasView);

			View.UserClosing -= View_UserClosing;

			base.UninitializeView();
		}

		#endregion

		private void View_UserClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
			Visible = false;
		}

	}
}
