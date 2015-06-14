namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;
	using ContentTypeTextNet.Library.SharedLibrary.Attribute;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
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

		string _selectedGroup = null;
		IEnumerable<LauncherViewModel> _launcherItems = null;

		#endregion

		public LauncherToolbarViewModel(LauncherToolbarItemModel model, LauncherToolbarWindow view)
			: base(model, view)
		{
			MessageString = "appbar";
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
		public string MessageString { get; set; }

		/// <summary>
		/// 他ウィンドウがフルスクリーン表示。
		/// </summary>
		public bool NowFullScreen { get; set; }
		public bool IsDocking { get; set; }
		/// <summary>
		/// ドッキング種別。
		/// </summary>
		public DockType DockType { get; set; }
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

		public string SelectedGroup
		{
			get { return this._selectedGroup; }
			set
			{
				if(this._selectedGroup != value) {
					this._selectedGroup = value;
					this._selectedGroup = null;
				}
			}
		}

		public IEnumerable<LauncherViewModel> LauncherItems 
		{
			get
			{
				if(this._launcherItems == null) {
					this._launcherItems = GetLauncherItems(SelectedGroup)
						.Select(m => new LauncherViewModel(m))
					;
				}

				return this._launcherItems;
			}
		}

		#endregion

		#region functino

		protected override void InitializeView()
		{
			View.Loaded += View_Loaded;
			base.InitializeView();
		}

		IEnumerable<LauncherItemModel> GetLauncherItems(string groupId)
		{
			var groupItem = Model.GroupItems[groupId];
			if(groupItem.GroupKind == GroupKind.LauncherItems) {
				return Model.GroupItems[groupId].LauncherItems
					.Where(i => Model.LauncherItems.Contains(i))
					.Select(i => Model.LauncherItems[i])
				;
			}
			// 当面はランチャーアイテムのみ
			throw new NotImplementedException();
		}

		#endregion

		void View_Loaded(object sender, RoutedEventArgs e)
		{
			//DockType = Library.PeData.Define.DockType.Right;
			View.Docking(Library.PeData.Define.DockType.Right);
			//DockType = Library.PeData.Define.DockType.Left;
			//OnPropertyChanged("Orientation");
		}
	}
}
