namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Library.SharedLibrary.Data;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Define;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Property;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
	using ContentTypeTextNet.Pe.PeMain.View;
	using Microsoft.Win32;

	public class ClipboardViewModel: HavingViewSingleModelWrapperIndexViewModelBase<ClipboardSettingModel, ClipboardWindow, ClipboardIndexItemCollectionModel, ClipboardIndexItemModel, ClipboardItemViewModel>
	{
		#region variable

		ClipboardItemViewModel _selectedViewModel;
		ImageScale _imageScale;

		#endregion

		public ClipboardViewModel(ClipboardSettingModel model, ClipboardWindow view, ClipboardIndexSettingModel indexModel, IAppNonProcess appNonProcess, IAppSender appSender)
			: base(model, view, indexModel, appNonProcess, appSender)
		{ }

		#region property

		public bool Enabled
		{
			get { return Model.Enabled; }
			set { SetModelValue(value); }
		}

		public ClipboardItemViewModel SelectedViewModel
		{
			get { return this._selectedViewModel; }
			set
			{
				var prevViewModel = this._selectedViewModel;
				if(SetVariableValue(ref this._selectedViewModel, value)) {
					if(this._selectedViewModel != null) {
						if(HasView) {
							// TODO: View依存
							var map = new Dictionary<ClipboardType, TabItem>() {
								{ ClipboardType.Text, View.pageText },
								{ ClipboardType.Rtf, View.pageRtf },
								{ ClipboardType.Html, View.pageHtml },
								{ ClipboardType.Image, View.pageImage },
								{ ClipboardType.File, View.pageFiles },
							};
							var type = ClipboardUtility.GetSingleClipboardType(this._selectedViewModel.Model.Type);
							foreach(var tab in map.Values) {
								tab.IsSelected = false;
							}
							map[type].IsSelected = true;
						}
						if (prevViewModel != null) {
							prevViewModel.Unload();
						}
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

		public ImageScale ImageScale
		{
			get { return this._imageScale; }
			set { SetVariableValue(ref this._imageScale, value); }
		}

		#endregion

		#region command

		public ICommand RemoveItemCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						var nowViewModel = SelectedViewModel;
						if(nowViewModel == null) {
							return;
						}

						IndexPairList.Remove(nowViewModel);
						AppSender.SendRemoveIndex(IndexKind.Clipboard, nowViewModel.Model.Id);
					}
				);

				return result;
			}
		}

		public ICommand ClipboardClearCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						Clipboard.Clear();
					}
				);

				return result;
			}
		}

		public ICommand SaveItemCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						if (SelectedViewModel == null) {
							return;
						}

						SaveFileFromDialog(SelectedViewModel);
					}
				);

				return result;
			}
		}

		#endregion

		#region function

		protected override ClipboardItemViewModel CreateIndexViewModel(ClipboardIndexItemModel model, object data)
		{
			var result = new ClipboardItemViewModel(
				model,
				AppSender,
				AppNonProcess
			);

			return result;
		}

		bool SaveFileFromDialog(ClipboardItemViewModel vm)
		{
			var srcFilters = new [] {
				new DialogFilterValueItem<ClipboardType>(ClipboardType.Text, "text", "*.txt"),
				new DialogFilterValueItem<ClipboardType>(ClipboardType.Rtf, "rtf", "*.rtf"),
				new DialogFilterValueItem<ClipboardType>(ClipboardType.Html, "html", "*.html"),
				new DialogFilterValueItem<ClipboardType>(ClipboardType.Image, "image", "*.png"),
			};
			var filter = new DialogFilterList();

			var bestType = ClipboardUtility.GetSingleClipboardType(vm.Model.Type);
			var types = ClipboardUtility.GetClipboardTypeList(vm.Model.Type);
			var defIndex = 0;
			var tempIndex = 0;
			foreach (var type in types.Where(t => t != ClipboardType.File)) {
				var filterItem = srcFilters.FirstOrDefault(f => f.Value == type);
				if (filterItem != null) {
					filter.Add(filterItem);
					tempIndex += 1;
					if (filterItem.Value == bestType) {
						defIndex = tempIndex;
					}
				}
			}

			if (!filter.Any()) {
				AppNonProcess.Logger.Information("type list: 0");
				return false;
			}

			var dialog = new SaveFileDialog() {
				Filter = filter.FilterText,
				FilterIndex = defIndex,
				AddExtension = true,
				CheckPathExists = true,
				ValidateNames = true,
			};

			var dialogResult = dialog.ShowDialog();
			if (dialogResult.GetValueOrDefault()) {
				var type = ((DialogFilterValueItem<ClipboardType>)filter[dialog.FilterIndex - 1]).Value;
				SaveFile(dialog.FileName, vm, type);
				return true;
			} else {
				return false;
			}
		}

		bool SaveFile(string path, ClipboardItemViewModel vm, ClipboardType saveType)
		{
			Debug.Assert(saveType != ClipboardType.File);

			var map = new Dictionary<ClipboardType, Action>() {
				{ ClipboardType.Text, () => File.WriteAllText(path, vm.Text) },
				{ ClipboardType.Rtf, () => File.WriteAllText(path, vm.Rtf) },
				{ ClipboardType.Html, () => File.WriteAllText(path, vm.HtmlCode) },
				{ ClipboardType.Image, () => {
					using(var stream = new FileStream(path, FileMode.Create, FileAccess.Write)) {
						var encoder = new PngBitmapEncoder();
						encoder.Frames.Add(BitmapFrame.Create(vm.Image));
						encoder.Save(stream);
					}
				} },
			};

			try {
				map[saveType]();
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

		public bool Visible
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
			Visible = false;
		}
	}
}
