namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.SharedLibrary.Attribute;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Property;
	using ContentTypeTextNet.Pe.PeMain.View;

	public class LauncherToolbarViewModel: HavingViewSingleModelWrapperViewModelBase<LauncherToolbarItemModel, LauncherToolbarWindow>, IApplicationDesktopToolbarData
	{
		#region variable

		LauncherGroupItemModel _selectedGroup = null;
		ObservableCollection<LauncherViewModel> _launcherItems = null;

		#endregion

		public LauncherToolbarViewModel(LauncherToolbarItemModel model, LauncherToolbarWindow view)
			: base(model, view)
		{
			BarSize = new Size(80, 80);
		}

		#region property

		#region ITopMost

		public bool TopMost
		{
			get { return TopMostProperty.GetTopMost(Model.Toolbar); }
			set { TopMostProperty.SetTopMost(Model.Toolbar, value, OnPropertyChanged); }
		}

		#endregion

		#region IVisible

		public Visibility Visibility
		{
			get { return VisibleVisibilityProperty.GetVisibility(Model.Toolbar); }
			set { VisibleVisibilityProperty.SetVisibility(Model.Toolbar, value, OnPropertyChanged); }
		}

		public bool Visible
		{
			get { return VisibleVisibilityProperty.GetVisible(Model.Toolbar); }
			set { VisibleVisibilityProperty.SetVisible(Model.Toolbar, value, OnPropertyChanged); }
		}

		#endregion

		#region IApplicationDesktopToolbarData

		public uint CallbackMessage { get; set; }

		/// <summary>
		/// 他ウィンドウがフルスクリーン表示。
		/// </summary>
		public bool NowFullScreen { get; set; }
		public bool IsDocking { get; set; }
		/// <summary>
		/// ドッキング種別。
		/// </summary>
		public DockType DockType 
		{
			get { return Model.Toolbar.DockType; }
			set { Model.Toolbar.DockType = value; } 
		}
		/// <summary>
		/// 自動的に隠す。
		/// </summary>
		public bool AutoHide { get; set; }
		/// <summary>
		/// 隠れているか。
		/// </summary>
		public bool IsHidden { get; set; }
		/// <summary>
		/// バーの論理サイズ
		/// </summary>
		[PixelKind(Px.Logical)]
		public Size BarSize { get; set; }
		/// <summary>
		/// 表示中の物理バーサイズ。
		/// </summary>
		[PixelKind(Px.Device)]
		public Rect ShowDeviceBarArea { get; set; }
		/// <summary>
		/// 隠れた状態のバー論理サイズ。
		/// <para>横: Widthを使用</para>
		/// <para>縦: Heightを使用</para>
		/// </summary>
		[PixelKind(Px.Logical)]
		public Size HideSize { get; set; }
		/// <summary>
		/// 自動的に隠すまでの時間。
		/// </summary>
		public TimeSpan HiddenWaitTime { get; set; }
		/// <summary>
		/// 自動的に隠す際のアニメーション時間。
		/// </summary>
		public TimeSpan HiddenAnimateTime { get; set; }
		/// <summary>
		/// ドッキングに使用するスクリーン。
		/// </summary>
		public ScreenModel DockScreen { get; set; }

		#endregion

		public Orientation Orientation
		{
			get { return DockType == DockType.Left ? Orientation.Horizontal : System.Windows.Controls.Orientation.Vertical; }
		}

		public LauncherGroupItemModel SelectedGroup
		{
			get 
			{
				if (this._selectedGroup == null) {
					if(Model.Toolbar.DefaultGroupId != null) {
						Model.GroupItems.TryGetValue(Model.Toolbar.DefaultGroupId, out this._selectedGroup);
					}
					if (this._selectedGroup == null) {
						this._selectedGroup = Model.GroupItems.First();
					}
				}

				return this._selectedGroup;
			}
		}

		//

		public ObservableCollection<LauncherViewModel> LauncherItems 
		{
			get
			{
				if(this._launcherItems == null) {
					var list = GetLauncherItems(SelectedGroup)
						.Select(m => new LauncherViewModel(m))
					;
					var list2 = new ObservableCollection<LauncherViewModel>(list);
					list2.Add(new LauncherViewModel(new LauncherItemModel() {
						Id = "test1",
						Name = "name1",
						LauncherKind = LauncherKind.File,
						Command = @"C:\Windows\System32\mspaint.exe"
					}));
					list2.Add(new LauncherViewModel(new LauncherItemModel() {
						Id = "test2",
						Name = "name2",
						LauncherKind = LauncherKind.File,
						Command = @"%windir%\system32\calc.exe"
					}));
					this._launcherItems = list2;
					OnPropertyChanged();
				}

				return this._launcherItems;
			}
		}

		public ImageSource ToolbarImage { get { return null; } }
		public string ToolbarText { get { return SelectedGroup.Name; } }

		#endregion

		#region functino

		IEnumerable<LauncherItemModel> GetLauncherItems(LauncherGroupItemModel groupItem)
		{
			if(groupItem.GroupKind == GroupKind.LauncherItems) {
				return groupItem.LauncherItems
					.Where(i => Model.LauncherItems.Contains(i))
					.Select(i => Model.LauncherItems[i])
				;
			}
			// 当面はランチャーアイテムのみ
			throw new NotImplementedException();
		}

		#endregion

	}
}
