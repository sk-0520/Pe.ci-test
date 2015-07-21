namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.IF;
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

	public class ClipboardViewModel: HavingViewSingleModelWrapperIndexViewModelBase<ClipboardSettingModel, ClipboardWindow, ClipboardIndexItemCollectionModel, ClipboardIndexItemModel, ClipboardItemViewModel>
	{
		#region variable

		ClipboardItemViewModel _selectedViewModel;
		ImageScale _imageScale;

		#endregion

		public ClipboardViewModel(ClipboardSettingModel model, ClipboardWindow view, ClipboardIndexSettingModel indexModel, INonProcess nonProcess, IClipboardWatcher clipboardWatcher, VariableConstants variableConstants, IAppSender appSender)
			: base(model, view, indexModel, nonProcess, clipboardWatcher, variableConstants, appSender)
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

		#endregion

		#region function

		protected override ClipboardItemViewModel CreateIndexViewModel(ClipboardIndexItemModel model, object data)
		{
			var result = new ClipboardItemViewModel(
				model,
				AppSender,
				ClipboardWatcher,
				NonProcess,
				VariableConstants
			);

			return result;
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
