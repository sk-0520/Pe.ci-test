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
	using System.Windows.Input;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.PInvoke.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.Attribute;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Property;
	using ContentTypeTextNet.Pe.PeMain.View;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;

	public class LauncherToolbarViewModel: HavingViewSingleModelWrapperViewModelBase<LauncherToolbarItemModel, LauncherToolbarWindow>, IApplicationDesktopToolbarData, IVisualStyleData, IHavingNonProcess
	{
		#region variable

		LauncherGroupItemModel _selectedGroup = null;
		ObservableCollection<LauncherViewModel> _launcherItems = null;
		double _captionSize;

		#endregion

		public LauncherToolbarViewModel(LauncherToolbarItemModel model, LauncherToolbarWindow view, INonProcess nonProcess)
			: base(model, view)
		{
			MenuWidth = GetMenuWidth();
			IconSize = CalcIconSize();
			ButtonSize = CalcButtonSize();
			this._captionSize = 10;
			BarSize = ButtonSize;

			NonProcess = nonProcess;
		}

		#region property

		public LauncherIconCaching LauncherIcons { get; set; }

		#region IHavingNonPorocess

		public INonProcess NonProcess { get; private set; }

		#endregion

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

		#region float window

		public double FloatLeft
		{
			get { return Model.Toolbar.FloatToolbarArea.Left; }
			set 
			{
				if (DockType == DockType.None && Model.Toolbar.FloatToolbarArea.Left != value) {
					Model.Toolbar.FloatToolbarArea.Left = value;
					OnPropertyChanged();
				}
			}
		}
		public double FloatTop
		{
			get { return Model.Toolbar.FloatToolbarArea.Top; }
			set
			{
				if (DockType == DockType.None && Model.Toolbar.FloatToolbarArea.Top != value) {
					Model.Toolbar.FloatToolbarArea.Top = value;
					OnPropertyChanged();
				}
			}
		}
		public double FloatWidth
		{
			get { return CalcViewWidth(DockType, Orientation); }
			set
			{
				if (DockType == DockType.None) {
					Model.Toolbar.FloatToolbarArea.WidthButtonCount = CalcButtonWidthCount(value, DockType, Orientation);
					OnPropertyChanged();
					OnPropertyChanged("CaptionHeight");
				}
			}
		}
		public double FloatHeight
		{
			get { return CalcViewHeight(DockType, Orientation); }
			set
			{
				if (DockType == DockType.None) {
					Model.Toolbar.FloatToolbarArea.HeightButtonCount = CalcButtonHeightCount(value, DockType, Orientation);
					OnPropertyChanged();
					OnPropertyChanged("CaptionWidth");
				}
			}
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
			set 
			{
				if(Model.Toolbar.DockType != value) {
					Model.Toolbar.DockType = value; 
					OnPropertyChanged();
					OnPropertyChanged("Orientation");
					OnPropertyChanged("CaptionVisibility");
					OnPropertyChanged("CaptionWidth");
					OnPropertyChanged("CaptionHeight");
					OnPropertyChanged("CaptionCursor");
				}
			}
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

		public void ChangingWindowMode()
		{
			var logicalRect = new Rect(
				FloatLeft,
				FloatTop,
				FloatWidth,
				FloatHeight
			);
			var deviceRect = UIUtility.ToDevicePixel(View, logicalRect);
			var podRect = PodStructUtility.Convert(deviceRect);
			//NativeMethods.MoveWindow(View.Handle, podRect.Left, podRect.Top, podRect.Width, podRect.Height, false);
			NativeMethods.SetWindowPos(View.Handle, IntPtr.Zero, podRect.Left, podRect.Top, podRect.Width, podRect.Height, SWP.SWP_NOSENDCHANGING | SWP.SWP_NOREDRAW);
			//View.InvalidateVisual();
			//View.UpdateLayout()
		}

		#endregion

		#region IVisualStyleData

		public bool EnabledVisualStyle { get; set; }
		public Color VisualPlainColor { get; set; }
		public Color VisualAlphaColor { get; set; }

		#endregion

		public Size IconSize { get; set; }
		public Size ButtonSize { get; set; }
		public double MenuWidth { get; set; }

		public bool NowFloatWindow { get { return DockType == DockType.None; } }
		public bool CanWindowDrag { get { return NowFloatWindow; } }
		public Cursor CaptionCursor
		{
			get
			{
				if (NowFloatWindow) {
					return Cursors.SizeAll;
				} else {
					return Cursors.Arrow;
				}
			}
		}

		public Visibility CaptionVisibility
		{
			get { return Visibility.Visible; }
		}
		public double CaptionWidth
		{
			get 
			{
				if (Orientation == Orientation.Horizontal) {
					return this._captionSize;
				} else {
					return HasView ? View.Width : ButtonSize.Width;
				}
			}
		}
		public double CaptionHeight
		{
			get
			{
				if (Orientation == Orientation.Vertical) {
					return this._captionSize;
				} else {
					return HasView ? View.Height: ButtonSize.Height;
				}
			}
		}

		Size GetCaptionSize(Orientation orientation)
		{
			if (orientation == Orientation.Horizontal) {
				return new Size(this._captionSize, 0);
			} else {
				return new Size(0, this._captionSize);
			}
		}

		public Orientation Orientation
		{
			get
			{
				switch (DockType) {
					case DockType.Left:
					case DockType.Right:
						return Orientation.Vertical;

					case DockType.Top:
					case DockType.Bottom:
					case DockType.None:
						return Orientation.Horizontal;

					default:
						throw new NotImplementedException();
				}
			}
		}

		public IEnumerable<LauncherGroupItemModel> GroupItems { get { return Model.GroupItems.Items; } }

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
			set
			{
				if(this._selectedGroup != value) {
					this._selectedGroup = value;
					OnPropertyChanged();
					OnPropertyChanged("GroupItems");
					var oldItems = this._launcherItems;
					this._launcherItems = null;
					OnPropertyChanged("LauncherItems");
					foreach(var oldItem in oldItems) {
						oldItem.Dispose();
					}
				}
			}
		}

		//

		public IEnumerable<LauncherViewModel> LauncherItems 
		{
			get
			{
				if(this._launcherItems == null) {
					var list = GetLauncherItems(SelectedGroup)
						.Select(m => new LauncherViewModel(m, this.LauncherIcons, NonProcess) {
							IconScale = Model.Toolbar.IconScale,
						});
					;

					this._launcherItems = new ObservableCollection<LauncherViewModel>(list);
				}

				return this._launcherItems;
			}
		}

		public ImageSource ToolbarImage { get { return Resource.GetLauncherToolbarMainIcon(Model.Toolbar.IconScale); } }
		public string ToolbarText { get { return DisplayTextUtility.GetDisplayName(SelectedGroup); } }
		public Visibility TextVisible { get { return Model.Toolbar.TextVisible ? Visibility.Visible: Visibility.Collapsed; } }

		public string ScreenName { get { return ScreenUtility.GetScreenName(DockScreen); } }
		public ImageSource ScreenPosition
		{
			get
			{
				return null;
			}
		}

		#endregion

		#region HavingViewSingleModelWrapperViewModelBase

		//protected override void InitializeView()
		//{
		//	base.InitializeView();

		//	View.MouseDown += View_MouseDown;
		//}

		//protected override void UninitializeView()
		//{
		//	View.MouseDown -= View_MouseDown;
			
		//	base.UninitializeView();
		//}

		//void View_MouseDown(object sender, MouseButtonEventArgs e)
		//{
		//	if (DockType == DockType.None) {
		//		if (e.LeftButton == MouseButtonState.Pressed) {
		//			View.DragMove();
		//		}
		//	}
		//}

		#endregion

		#region command

		public ICommand PositionChangeCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						var dockType = (DockType)o;
						View.Docking(dockType);
					},
					o => {
						var dockType = (DockType)o;
						return dockType != DockType;
					}
				);

				return result;
			}
		}

		public ICommand GroupChangeCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						var group = (LauncherGroupItemModel)o;
						NonProcess.Logger.Debug(group.ToString());
						SelectedGroup = group;
					}
				);

				return result;
			}
		}

		#endregion

		#region function

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

		Size CalcIconSize()
		{
			return Model.Toolbar.IconScale.ToSize();
		}

		double GetMenuWidth()
		{
			return 20;
		}

		Size CalcButtonSize()
		{
			//TODO: どれくらいのサイズがいいかね。
			var padding = 10.0;

			var mainButtonSize = IconSize;
			return new Size(mainButtonSize.Width + padding + MenuWidth + (Model.Toolbar.Visible ? Model.Toolbar.TextWidth : 0), mainButtonSize.Height + padding);
		}

		double CalcViewWidth(DockType dockType, Orientation orientation)
		{
			var captionSize = GetCaptionSize(orientation);
			return Model.Toolbar.FloatToolbarArea.WidthButtonCount * ButtonSize.Width + captionSize.Width;
		}
		int CalcButtonWidthCount(double viewWidth, DockType dockType, Orientation orientation)
		{
			var captionSize = GetCaptionSize(orientation);
			return (int)((viewWidth - captionSize.Width) / ButtonSize.Width);
		}
		double CalcViewHeight(DockType dockType, Orientation orientation)
		{
			var captionSize = GetCaptionSize(orientation);
			return Model.Toolbar.FloatToolbarArea.HeightButtonCount * ButtonSize.Height + captionSize.Height;
		}
		int CalcButtonHeightCount(double viewHeight, DockType dockType, Orientation orientation)
		{
			var captionSize = GetCaptionSize(orientation);
			return (int)((viewHeight - captionSize.Height) / ButtonSize.Height);
		}

		#endregion


	}
}
