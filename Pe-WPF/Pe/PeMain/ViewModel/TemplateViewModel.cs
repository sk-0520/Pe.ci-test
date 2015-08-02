namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.Data;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
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
	using Microsoft.Win32;

	public class TemplateViewModel : HavingViewSingleModelWrapperIndexViewModelBase<TemplateSettingModel, TemplateWindow, TemplateIndexItemCollectionModel, TemplateIndexItemModel, TemplateItemViewModel>
	{
		#region variable

		TemplateItemViewModel _selectedViewModel;

		#endregion

		public TemplateViewModel(TemplateSettingModel model, TemplateWindow view, TemplateIndexSettingModel indexModel, IAppNonProcess appNonProcess, IAppSender appSender)
			: base(model, view, indexModel, appNonProcess, appSender)
		{ }

		#region property

		public TemplateItemViewModel SelectedViewModel
		{
			get { return this._selectedViewModel; }
			set 
			{ 
				var prevViewModel = this._selectedViewModel;
				if (SetVariableValue(ref this._selectedViewModel, value)) {
					View.pageSource.IsSelected = true;

					if(prevViewModel != null) {
						SaveItemViewModel(prevViewModel);
						prevViewModel.Unload();
					}
				}
			}
		}

		/// <summary>
		/// リスト部の幅。
		/// </summary>
		public double ItemsListWidth
		{
			get { return Model.ItemsListWidth; }
			set { SetModelValue(value); }
		}

		/// <summary>
		/// 置き換えリスト部の幅。
		/// </summary>
		public double ReplaceListWidth
		{
			get { return Model.ReplaceListWidth; }
			set { SetModelValue(value); }
		}

		#endregion

		#region command

		public ICommand CreateItemCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						var indexModel = SettingUtility.CreateTemplateIndexItem(IndexModel.Items, AppNonProcess);
						var pair = IndexPairList.Insert(0, indexModel, null);
						SelectedViewModel = pair.ViewModel;
					}
				);

				return result;
			}
		}

		public ICommand RemoveItemCommand
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
						AppSender.SendRemoveIndex(IndexKind.Template, nowViewModel.Model.Id);
						
						//IndexModel.Items.Remove(nowViewModel.Model);
						//IndexItems.Remove(nowViewModel);
					}
				);

				return result;
			}
		}

		//public ICommand ListItemSelectionChangedCommand
		//{
		//	get
		//	{
		//		var result = CreateCommand(
		//			o => {
		//				if (HasView) {
		//					View.pageSource.IsSelected = true;
		//				}
		//			}
		//		);

		//		return result;
		//	}
		//}

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

		public ICommand SaveItemCommand
		{
			get {
				var result = CreateCommand(
					o => {
						if (SelectedViewModel == null) {
							return;
						}

						SelectedViewModel.SetReplacedValue();
						if (!string.IsNullOrEmpty(SelectedViewModel.Replaced)) {
							SaveFileFromDialog(SelectedViewModel);
						}
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

		protected override TemplateItemViewModel CreateIndexViewModel(TemplateIndexItemModel model, object data)
		{
			var result = new TemplateItemViewModel(
				model,
				AppSender,
				AppNonProcess
			);

			return result;
		}

		void SaveItemViewModel(TemplateItemViewModel vm)
		{
			if (vm.IsChanged) {
				vm.SaveBody();
			}
		}

		bool SaveFileFromDialog(TemplateItemViewModel vm)
		{
			CheckUtility.EnforceNotNullAndNotEmpty(vm.Replaced);

			var filter = new DialogFilterList() {
				{ new DialogFilterItem("text", "*.txt") },
			};

			var dialog = new SaveFileDialog() {
				Filter = filter.FilterText,
				FilterIndex = 0,
				AddExtension = true,
				CheckPathExists = true,
				ValidateNames = true,
			};

			var dialogResult = dialog.ShowDialog();
			if (dialogResult.GetValueOrDefault()) {
				return SaveFile(dialog.FileName, vm);
			} else {
				return false;
			}
		}

		bool SaveFile(string path, TemplateItemViewModel vm)
		{
			CheckUtility.EnforceNotNullAndNotEmpty(vm.Replaced);

			var writeValue = vm.Replaced;
			try {
				File.WriteAllText(path, writeValue);
				return true;
			} catch (Exception ex) {
				AppNonProcess.Logger.Error(ex);
				return false;
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

		public bool IsVisible
		{
			get { return VisibleVisibilityProperty.GetVisible(Model); }
			set { VisibleVisibilityProperty.SetVisible(Model, value, OnPropertyChanged); }
		}

		#endregion

		#region ITopMost

		public bool IsTopmost
		{
			get { return TopMostProperty.GetTopMost(Model); }
			set { TopMostProperty.SetTopMost(Model, value, OnPropertyChanged); }
		}

		#endregion

		#endregion

		#region HavingViewSingleModelWrapperIndexViewModelBase

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
			IsVisible = false;
		}


	}
}
