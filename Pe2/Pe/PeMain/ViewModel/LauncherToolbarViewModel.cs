﻿namespace ContentTypeTextNet.Pe.PeMain.ViewModel
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
	using System.ComponentModel;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms;

	public class LauncherToolbarViewModel: HavingViewSingleModelWrapperViewModelBase<LauncherToolbarItemModel, LauncherToolbarWindow>, IApplicationDesktopToolbarData, IVisualStyleData, IHavingNonProcess
	{
		#region static

		static double GetCaptionWidth()
		{
			return 10;
		}

		static Size GetCaptionSize(Orientation orientation, double captionWidth)
		{
			if (orientation == Orientation.Horizontal) {
				return new Size(captionWidth, 0);
			} else {
				return new Size(0, captionWidth);
			}
		}

		static Thickness GetBorderThickness(DockType dockType, Visual visual)
		{
			var borderSize = SystemInformation.BorderSize;

			var map = new Dictionary<DockType, Thickness>() {
				{ DockType.None, new Thickness(borderSize.Width, borderSize.Height, borderSize.Width, borderSize.Height) },
				{ DockType.Left, new Thickness(0, 0, borderSize.Width, 0) },
				{ DockType.Top, new Thickness(0, 0, 0, borderSize.Height) },
				{ DockType.Right, new Thickness(borderSize.Width, 0, 0, 0)},
				{ DockType.Bottom, new Thickness(0, borderSize.Height, 0, 0)},
			};

			return map[dockType];
		}

		static Size GetIconSize(IconScale iconScale)
		{
			return iconScale.ToSize();
		}

		static double GetMenuWidth()
		{
			return 20;
		}

		static Size GetButtonSize(Size iconSize, double menuWidth, bool showText, double textWidth)
		{
			//TODO: どれくらいのサイズがいいかね。
			var padding = 10.0;

			var mainButtonSize = iconSize;
			return new Size(mainButtonSize.Width + padding + menuWidth + (showText ? textWidth : 0), mainButtonSize.Height + padding);
		}

		double GetHideWidth(DockType dockType)
		{
			var map = new Dictionary<DockType, double>() {
				{ DockType.Left, BorderThickness.Right },
				{ DockType.Top, BorderThickness.Bottom },
				{ DockType.Right, BorderThickness.Left },
				{ DockType.Bottom, BorderThickness.Top },
			};

			return map[dockType];
		}

		static Orientation GetOrientation(DockType dockType)
		{
			switch (dockType) {
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

		static Size GetMinSize(DockType dockType, Orientation orientation, Thickness borderThickness, Size buttonSize, double captionWidth)
		{
			var captionSize = GetCaptionSize(orientation, captionWidth);

			return new Size(
				captionSize.Width + borderThickness.GetHorizon() + buttonSize.Width,
				captionSize.Height + borderThickness.GetVertical() + buttonSize.Height
			);
		}

		#endregion

		#region variable

		LauncherGroupItemModel _selectedGroup = null;
		ObservableCollection<LauncherViewModel> _launcherItems = null;
		double _captionWidth;
		Thickness _borderThickness;
		Brush _borderBrush;
		Size _minSize;

		#endregion

		public LauncherToolbarViewModel(LauncherToolbarItemModel model, LauncherToolbarWindow view, INonProcess nonProcess)
			: base(model, view)
		{
			NonProcess = nonProcess;

			this._captionWidth = GetCaptionWidth();
			MenuWidth = GetMenuWidth();
			IconSize = GetIconSize(Model.Toolbar.IconScale);
			ButtonSize = GetButtonSize(IconSize, MenuWidth, Model.Toolbar.TextVisible, Model.Toolbar.TextWidth);

			//BorderThickness = GetBorderThickness(Model.Toolbar.DockType, View);
			BorderBrush = View.Background;

			var backgroundPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(
				Window.BackgroundProperty, typeof(Brush)
			);
			if (backgroundPropertyDescriptor != null) {
				backgroundPropertyDescriptor.AddValueChanged(View, OnBackgroundChanged);
			}


			CalculateWindowStatus(DockType);

		}

		#region property

		public LauncherIconCaching LauncherIcons { get; set; }

		public Thickness BorderThickness
		{
			get { return this._borderThickness; } 
			set
			{
				if (this._borderThickness != value) {
					this._borderThickness = value;
					OnPropertyChanged();
				}
			}
		}

		public Brush BorderBrush
		{
			get { return this._borderBrush; }
			set
			{
				if (this._borderBrush != value) {
					this._borderBrush = value;
					OnPropertyChanged();
				}
			}
		}

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
			get 
			{ 
				return IsHidden 
					? HideLogicalBarArea.Left
					: Model.Toolbar.FloatToolbar.Left;
			}
			set 
			{
				if (DockType == DockType.None && Model.Toolbar.FloatToolbar.Left != value) {
					Model.Toolbar.FloatToolbar.Left = value;
					OnPropertyChanged();
				}
			}
		}
		public double FloatTop
		{
			get { 
				return IsHidden 
					? HideLogicalBarArea.Top
					: Model.Toolbar.FloatToolbar.Top; 
			}
			set
			{
				if (DockType == DockType.None && Model.Toolbar.FloatToolbar.Top != value) {
					Model.Toolbar.FloatToolbar.Top = value;
					OnPropertyChanged();
				}
			}
		}
		public double FloatWidth
		{
			get
			{
				return IsHidden
				? HideLogicalBarArea.Width
				: CalculateViewWidth(DockType, Orientation, BorderThickness, this._captionWidth);
			}
			set
			{
				if (DockType == DockType.None) {
					Model.Toolbar.FloatToolbar.WidthButtonCount = CalculateButtonWidthCount(DockType, Orientation, BorderThickness, this._captionWidth, value);
					OnPropertyChanged();
					OnPropertyChanged("CaptionHeight");
				}
			}
		}
		public double FloatHeight
		{
			get
			{
				return IsHidden
					? HideLogicalBarArea.Height
					: CalculateViewHeight(DockType, Orientation, BorderThickness, this._captionWidth);
			}
			set
			{
				if (DockType == DockType.None) {
					Model.Toolbar.FloatToolbar.HeightButtonCount = CalculateButtonHeightCount(DockType, Orientation, BorderThickness, this._captionWidth, value);
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
					CalculateWindowStatus(value);
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
		public bool AutoHide 
		{
			get { return Model.Toolbar.AutoHide; }
			set
			{
				if (Model.Toolbar.AutoHide != value) {
					Model.Toolbar.AutoHide = value;
					OnPropertyChanged();
					if (HasView) {
						View.Docking(DockType, AutoHide);
					}
				}
			}
		}
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
		/// </summary>
		[PixelKind(Px.Logical)]
		public double HideWidth { get; set; }
		/// <summary>
		/// 表示中の隠れたバーの論理領域。
		/// </summary>
		[PixelKind(Px.Logical)]
		public Rect HideLogicalBarArea { get; set; }
		/// <summary>
		/// 自動的に隠すまでの時間。
		/// </summary>
		public TimeSpan HideWaitTime {
			get { return Model.Toolbar.HideWaitTime; }
			set
			{
				if (Model.Toolbar.HideWaitTime != value) {
					Model.Toolbar.HideWaitTime = value;
					OnPropertyChanged();
				}
			}
		}
		/// <summary>
		/// 自動的に隠す際のアニメーション時間。
		/// </summary>
		public TimeSpan HideAnimateTime {
			get { return Model.Toolbar.HideAnimateTime; }
			set
			{
				if (Model.Toolbar.HideAnimateTime != value) {
					Model.Toolbar.HideAnimateTime = value;
					OnPropertyChanged();
				}
			}
		}
		/// <summary>
		/// ドッキングに使用するスクリーン。
		/// </summary>
		public ScreenModel DockScreen { get; set; }

		public void ChangingWindowMode(DockType dockType)
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
			//DockType = dockType;
			//MinSize = GetMinSize(DockType, Orientation, BorderThickness, ButtonSize);
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
			get { return NowFloatWindow ? Visibility.Visible: Visibility.Collapsed; }
		}
		public double CaptionWidth
		{
			get 
			{
				if (Orientation == Orientation.Horizontal) {
					return this._captionWidth;
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
					return this._captionWidth;
				} else {
					return HasView ? View.Height: ButtonSize.Height;
				}
			}
		}

		public Orientation Orientation
		{
			get { return GetOrientation(DockType); }
		}

		public Size MinSize
		{
			get { return this._minSize; }
			set
			{
				if (this._minSize != value) {
					this._minSize = value;
					OnPropertyChanged();
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
						View.Docking(dockType, AutoHide);
					},
					o => {
						var dockType = (DockType)o;
						return dockType != DockType;
					}
				);

				return result;
			}
		}

		public ICommand SwitchTopMostCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						TopMost = !TopMost;
					}
				);

				return result;
			}
		}

		public ICommand SwitchAutoHideCommand
		{
			get 
			{
				var result = CreateCommand(
					o => {
						AutoHide = !AutoHide;
					},
					o => {
						return IsDocking;
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

		public ICommand ChangeVisibleCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						var visible = (bool)o;
						Visible = visible;
					}
				);

				return result;
			}
		}

		#endregion

		#region function

		void CalculateWindowStatus(DockType dockType)
		{
			BorderThickness = GetBorderThickness(dockType, View);

			BarSize = new Size(ButtonSize.Width + BorderThickness.GetHorizon(), ButtonSize.Height + BorderThickness.GetVertical());

			if (dockType != DockType.None) {
				HideWidth = GetHideWidth(dockType);
				MinSize = new Size(0, 0);
			} else {
				var orientation = GetOrientation(dockType);
				MinSize = GetMinSize(dockType, orientation, BorderThickness, ButtonSize, this._captionWidth);
			}

		}

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

		double CalculateViewWidth(DockType dockType, Orientation orientation, Thickness borderThickness, double captionWidth)
		{
			var captionSize = GetCaptionSize(orientation, captionWidth);
			return Model.Toolbar.FloatToolbar.WidthButtonCount * ButtonSize.Width + captionSize.Width + borderThickness.GetHorizon();
		}
		int CalculateButtonWidthCount(DockType dockType, Orientation orientation, Thickness borderThickness, double captionWidth, double viewWidth)
		{
			var captionSize = GetCaptionSize(orientation, captionWidth);
			return (int)((viewWidth - borderThickness.GetHorizon() - captionSize.Width) / ButtonSize.Width);
		}
		double CalculateViewHeight(DockType dockType, Orientation orientation, Thickness borderThickness, double captionWidth)
		{
			var captionSize = GetCaptionSize(orientation, captionWidth);
			return Model.Toolbar.FloatToolbar.HeightButtonCount * ButtonSize.Height + captionSize.Height + borderThickness.GetVertical();
		}
		int CalculateButtonHeightCount(DockType dockType, Orientation orientation, Thickness borderThickness, double captionWidth, double viewHeight)
		{
			var captionSize = GetCaptionSize(orientation, captionWidth);
			return (int)((viewHeight - borderThickness.GetVertical() - captionSize.Height) / ButtonSize.Height);
		}


		#endregion

		void OnBackgroundChanged(object sender, EventArgs e)
		{
			var viewBrush = View.Background as SolidColorBrush;
			if (viewBrush != null) {
				BorderBrush = viewBrush;
			}
		}

	}
}
