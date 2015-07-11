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
	using System.ComponentModel;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms;
	using System.Diagnostics;
	using ContentTypeTextNet.Library.SharedLibrary.IF.WindowsViewExtend;
	using System.Windows.Controls.Primitives;
	using System.Windows.Media.Imaging;

	public class LauncherToolbarViewModel : HavingViewSingleModelWrapperViewModelBase<LauncherToolbarDataModel, LauncherToolbarWindow>, IApplicationDesktopToolbarData, IVisualStyleData, IHavingNonProcess, IHavingClipboardWatcher, IWindowAreaCorrectionData, IWindowHitTestData, IHavingLauncherIconCaching
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
			var deviceBorderSize = SystemInformation.BorderSize;
			var logicalBorderSize = UIUtility.ToLogicalPixel(visual, deviceBorderSize);

			var map = new Dictionary<DockType, Thickness>() {
				{ DockType.None, new Thickness(logicalBorderSize.Width, logicalBorderSize.Height, logicalBorderSize.Width, logicalBorderSize.Height) },
				{ DockType.Left, new Thickness(0, 0, logicalBorderSize.Width, 0) },
				{ DockType.Top, new Thickness(0, 0, 0, logicalBorderSize.Height) },
				{ DockType.Right, new Thickness(logicalBorderSize.Width, 0, 0, 0)},
				{ DockType.Bottom, new Thickness(0, logicalBorderSize.Height, 0, 0)},
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

		static PlacementMode GetDropDownPlacement(DockType dockeType)
		{
			var map = new Dictionary<DockType, PlacementMode>() {
				{ DockType.None, PlacementMode.Bottom },
				{ DockType.Left, PlacementMode.Right },
				{ DockType.Top, PlacementMode.Bottom },
				{ DockType.Right, PlacementMode.Left },
				{ DockType.Bottom, PlacementMode.Top },
			};
			return map[dockeType];
		}

		#endregion

		#region variable

		LauncherGroupItemModel _selectedGroup = null;
		ObservableCollection<LauncherButtonViewModel> _launcherItems = null;
		double _captionWidth;
		Thickness _borderThickness;
		Brush _borderBrush;
		Size _minSize;

		Rect _showLogicalBarArea;
		bool _isHidden;

		#endregion

		public LauncherToolbarViewModel(LauncherToolbarDataModel model, LauncherToolbarWindow view, ScreenModel screen, LauncherIconCaching launcherIconCaching, INonProcess nonProcess, IClipboardWatcher clipboardWatcher)
			: base(model, view)
		{
			DockScreen = screen;
			LauncherIconCaching = launcherIconCaching;
			NonProcess = nonProcess;
			ClipboardWatcher = clipboardWatcher;

			this._captionWidth = GetCaptionWidth();
			MenuWidth = GetMenuWidth();
			IconSize = GetIconSize(Model.Toolbar.IconScale);
			ButtonSize = GetButtonSize(IconSize, MenuWidth, Model.Toolbar.TextVisible, Model.Toolbar.TextWidth);

			//BorderThickness = GetBorderThickness(Model.Toolbar.DockType, View);
			if (HasView) {
				BorderBrush = View.Background;

				var backgroundPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(
					Window.BackgroundProperty, typeof(Brush)
				);
				if (backgroundPropertyDescriptor != null) {
					backgroundPropertyDescriptor.AddValueChanged(View, OnBackgroundChanged);
				}

				CalculateWindowStatus(DockType);
			}
		}

		#region property

		public Thickness BorderThickness
		{
			get { return this._borderThickness; }
			set
			{
				//if (this._borderThickness != value) {
				//	this._borderThickness = value;
				//	OnPropertyChanged();
				//}
				SetVariableValue(ref this._borderThickness, value);
			}
		}

		public Brush BorderBrush
		{
			get { return this._borderBrush; }
			set
			{
				//if (this._borderBrush != value) {
				//	this._borderBrush = value;
				//	OnPropertyChanged();
				//}
				SetVariableValue(ref this._borderBrush, value);
			}
		}

		public Visibility HideVisibility
		{
			get
			{
				return IsHidden
					? Visibility.Collapsed
					: Visibility.Visible
				;
			}
		}

		public Size IconSize { get; set; }
		public Size ButtonSize { get; set; }
		public double MenuWidth { get; set; }

		public bool NowFloatWindow { get { return DockType == DockType.None; } }
		//public bool CanWindowDrag { get { return NowFloatWindow; } }

		public Visibility CaptionVisibility
		{
			get { return NowFloatWindow ? Visibility.Visible : Visibility.Collapsed; }
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
					return HasView ? View.Height : ButtonSize.Height;
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

		public IEnumerable<LauncherGroupItemModel> GroupItems { get { return Model.GroupItems; } }

		public LauncherGroupItemModel SelectedGroup
		{
			get
			{
				if (this._selectedGroup == null) {
					if (Model.Toolbar.DefaultGroupId != null) {
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
				//if(this._selectedGroup != value) {
				//	this._selectedGroup = value;
				//	OnPropertyChanged();
				//	OnPropertyChanged("GroupItems");
				//	var oldItems = this._launcherItems;
				//	this._launcherItems = null;
				//	OnPropertyChanged("LauncherItems");
				//	foreach(var oldItem in oldItems) {
				//		oldItem.Dispose();
				//	}
				//}
				if (SetVariableValue(ref this._selectedGroup, value)) {
					OnPropertyChanged("GroupItems");
					var oldItems = this._launcherItems;
					this._launcherItems = null;
					OnPropertyChanged("LauncherItems");
					foreach (var oldItem in oldItems) {
						oldItem.Dispose();
					}
				}
			}
		}

		public IEnumerable<LauncherButtonViewModel> LauncherItems
		{
			get
			{
				if (this._launcherItems == null) {
					var list = GetLauncherItems(SelectedGroup)
						.Select(m => new LauncherButtonViewModel(m, this.LauncherIconCaching, NonProcess, ClipboardWatcher) {
							IconScale = Model.Toolbar.IconScale,
						});
					;

					this._launcherItems = new ObservableCollection<LauncherButtonViewModel>(list);
				}

				return this._launcherItems;
			}
		}

		public ImageSource ToolbarImage { get { return GetAppIcon(); } }
		public string ToolbarText { get { return DisplayTextUtility.GetDisplayName(SelectedGroup); } }
		public Color ToolbarHotTrack { get { return GetAppIconColor(); } }
		public Visibility TextVisible { get { return Model.Toolbar.TextVisible ? Visibility.Visible : Visibility.Collapsed; } }

		public string ScreenName { get { return ScreenUtility.GetScreenName(DockScreen); } }
		public ImageSource ScreenPosition
		{
			get
			{
				return null;
			}
		}

		public PlacementMode DropDownPlacement
		{
			get { return GetDropDownPlacement(DockType); }
		}

		#endregion

		#region IHavingLauncherIconCaching

		public LauncherIconCaching LauncherIconCaching { get; private set; }

		#endregion

		#region IHavingNonPorocess

		public INonProcess NonProcess { get; private set; }

		#endregion

		#region IHavingClipboardWatcher

		public IClipboardWatcher ClipboardWatcher { get; private set; }

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

		#region window

		public double WindowLeft
		{
			get
			{
				if(DockType == DockType.None) {
					return Model.Toolbar.FloatToolbar.Left;
				} else {
					return IsHidden
						? HideLogicalBarArea.Left
						: ShowLogicalBarArea.Left;
				}
			}
			set
			{
				if(DockType == DockType.None && Model.Toolbar.FloatToolbar.Left != value) {
					Model.Toolbar.FloatToolbar.Left = value;
					OnPropertyChanged();
				} else if(!IsHidden && ShowLogicalBarArea.X != value) {
					this._showLogicalBarArea.X = value;
					OnPropertyChanged();
				}
			}
		}
		public double WindowTop
		{
			get
			{
				if(DockType == DockType.None) {
					return Model.Toolbar.FloatToolbar.Top;
				} else {
					return IsHidden
						? HideLogicalBarArea.Top
						: ShowLogicalBarArea.Top;
				}
			}
			set
			{
				if(DockType == DockType.None && Model.Toolbar.FloatToolbar.Top != value) {
					Model.Toolbar.FloatToolbar.Top = value;
					OnPropertyChanged();
				} else if(!IsHidden && ShowLogicalBarArea.Y != value) {
					this._showLogicalBarArea.Y = value;
					OnPropertyChanged();
				}
			}
		}
		public double WindowWidth
		{
			get
			{
				if(DockType == DockType.None) {
					return CalculateViewWidth(DockType, Orientation, BorderThickness, this._captionWidth);
				} else {
					return IsHidden
						? HideLogicalBarArea.Width
						: ShowLogicalBarArea.Width;
				}
			}
			set
			{
				if(DockType == DockType.None) {
					Model.Toolbar.FloatToolbar.WidthButtonCount = CalculateButtonWidthCount(DockType, Orientation, BorderThickness, this._captionWidth, value);
					OnPropertyChanged();
					OnPropertyChanged("CaptionHeight");
				} else if(!IsHidden && ShowLogicalBarArea.Width != value) {
					this._showLogicalBarArea.Width = value;
					OnPropertyChanged();
				}
			}
		}
		public double WindowHeight
		{
			get
			{
				if(DockType == DockType.None) {
					return CalculateViewHeight(DockType, Orientation, BorderThickness, this._captionWidth);
				} else {
					return IsHidden
						? HideLogicalBarArea.Height
						: ShowLogicalBarArea.Height;
				}
			}
			set
			{
				if(DockType == DockType.None) {
					Model.Toolbar.FloatToolbar.HeightButtonCount = CalculateButtonHeightCount(DockType, Orientation, BorderThickness, this._captionWidth, value);
					OnPropertyChanged();
					OnPropertyChanged("CaptionWidth");
				} else if(!IsHidden && ShowLogicalBarArea.Height != value) {
					this._showLogicalBarArea.Height = value;
					OnPropertyChanged();
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
					if(HasView) {
						CalculateWindowStatus(value);
					}

					Model.Toolbar.DockType = value;
					OnPropertyChanged();
					View.InvalidateArrange();
					OnPropertyChanged("Orientation");
					OnPropertyChanged("CaptionVisibility");
					OnPropertyChanged("CaptionWidth");
					OnPropertyChanged("CaptionHeight");
					OnPropertyChanged("DropDownPlacement");
					View.UpdateLayout();
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
				//if (Model.Toolbar.AutoHide != value) {
				//	Model.Toolbar.AutoHide = value;
				//	OnPropertyChanged();
				//	if (HasView) {
				//		View.Docking(DockType, AutoHide);
				//	}
				//}
				if(SetPropertyValue(Model.Toolbar, value)) {
					if(HasView) {
						View.Docking(DockType, AutoHide);
					}
				}
			}
		}
		/// <summary>
		/// 隠れているか。
		/// </summary>
		public bool IsHidden
		{
			get { return this._isHidden; }
			set
			{
				//if (this._isHidden != value) {
				//	this._isHidden = value;
				//	OnPropertyChanged();
				//	OnPropertyChanged("HideVisibility");
				//}
				if(SetVariableValue(ref this._isHidden, value)) {
					OnPropertyChanged("HideVisibility");
				}
			}
		}
		/// <summary>
		/// バーの論理サイズ
		/// </summary>
		[PixelKind(Px.Logical)]
		public Size BarSize { get; set; }
		/// <summary>
		/// 表示中の論理バーサイズ。
		/// </summary>
		[PixelKind(Px.Logical)]
		public Rect ShowLogicalBarArea { get { return this._showLogicalBarArea; } set { this._showLogicalBarArea = value; } }
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
		public TimeSpan HideWaitTime
		{
			get { return Model.Toolbar.HideWaitTime; }
			set
			{
				//if (Model.Toolbar.HideWaitTime != value) {
				//	Model.Toolbar.HideWaitTime = value;
				//	OnPropertyChanged();
				//}
				SetPropertyValue(Model.Toolbar, value);
			}
		}
		/// <summary>
		/// 自動的に隠す際のアニメーション時間。
		/// </summary>
		public TimeSpan HideAnimateTime
		{
			get { return Model.Toolbar.HideAnimateTime; }
			set
			{
				//if (Model.Toolbar.HideAnimateTime != value) {
				//	Model.Toolbar.HideAnimateTime = value;
				//	OnPropertyChanged();
				//}
				SetPropertyValue(Model.Toolbar, value);
			}
		}
		/// <summary>
		/// ドッキングに使用するスクリーン。
		/// </summary>
		public ScreenModel DockScreen { get; private set; }

		#endregion

		#region IVisualStyleData

		public bool UsingVisualStyle { get { return true; } }
		public bool EnabledVisualStyle { get; set; }
		public Color VisualPlainColor { get; set; }
		public Color VisualAlphaColor { get; set; }

		#endregion

		#region IWindowAreaCorrectionData

		/// <summary>
		/// ウィンドウサイズの倍数制御を行うか。
		/// </summary>
		public bool UsingMultipleResize { get { return NowFloatWindow && Visible; } }
		/// <summary>
		/// ウィンドウサイズの倍数制御に使用する元となる論理サイズ。
		/// </summary>
		[PixelKind(Px.Logical)]
		public Size MultipleSize
		{
			//TODO: 手抜き
			get { return ButtonSize; }
		}
		/// <summary>
		/// タイトルバーとかボーダーを含んだ領域。
		/// </summary>
		[PixelKind(Px.Logical)]
		public Thickness MultipleThickness
		{
			get
			{
				var captionSize = GetCaptionSize(Orientation, this._captionWidth);
				var result = new Thickness(
					BorderThickness.Left + captionSize.Width,
					BorderThickness.Top + captionSize.Height,
					BorderThickness.Right,
					BorderThickness.Bottom
				);
				return result;
			}
		}

		/// <summary>
		/// 移動制限を行うか。
		/// </summary>
		public bool UsingMoveLimitArea { get { return CaptionVisibility == Visibility.Visible; } }
		/// <summary>
		/// 移動制限に使用する論理領域。
		/// </summary>
		[PixelKind(Px.Logical)]
		public Rect MoveLimitArea
		{
			get
			{
				if(HasView) {
					return UIUtility.ToLogicalPixel(View, DockScreen.DeviceWorkingArea);
				} else {
					NonProcess.Logger.SafeDebug("device pixel");
					return DockScreen.DeviceWorkingArea;
				}
			}
		}

		/// <summary>
		/// 最大化・最小化を抑制するか。
		/// </summary>
		public bool UsingMaxMinSuppression { get { return true; } }


		#endregion

		#region IWindowHitTestData

		/// <summary>
		/// ヒットテストを行うか
		/// </summary>
		public bool UsingHitTest { get { return NowFloatWindow; } }

		/// <summary>
		/// タイトルバーとして認識される領域。
		/// </summary>
		[PixelKind(Px.Logical)]
		public Rect CaptionArea
		{
			get
			{
				var result = new Rect(BorderThickness.Left, BorderThickness.Top, CaptionWidth, CaptionHeight);
				return result;
			}
		}
		/// <summary>
		/// サイズ変更に使用する境界線。
		/// </summary>
		[PixelKind(Px.Logical)]
		public Thickness ResizeThickness { get { return BorderThickness; } }

		#endregion

		#region HavingViewSingleModelWrapperViewModelBase

		protected override void InitializeView()
		{
			Debug.Assert(HasView);

			View.Loaded += View_Loaded;
			View.UserClosing += View_UserClosing;

			base.InitializeView();
		}

		protected override void UninitializeView()
		{
			Debug.Assert(HasView);

			View.UserClosing -= View_UserClosing;
			View.Loaded -= View_Loaded;

			base.UninitializeView();
		}

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
			Debug.Assert(HasView);

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
			if (groupItem.GroupKind == GroupKind.LauncherItems) {
				return groupItem.LauncherItems
					.Where(i => Model.LauncherItems.Contains(i)) //TODO: Xアイコン表示をどうしよう
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

		BitmapSource GetAppIcon()
		{
			return Resource.GetLauncherToolbarMainIcon(Model.Toolbar.IconScale);
		}

		Color GetAppIconColor()
		{
			return MediaUtility.GetPredominantColorFromBitmapSource(GetAppIcon());
		}

		public void ChangingWindowMode(DockType dockType)
		{
			DockType = dockType;
			var logicalRect = new Rect(
				WindowLeft,
				WindowTop,
				WindowWidth,
				WindowHeight
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

		void OnBackgroundChanged(object sender, EventArgs e)
		{
			var viewBrush = View.Background as SolidColorBrush;
			if (viewBrush != null) {
				BorderBrush = viewBrush;
			}
		}

		void View_Loaded(object sender, RoutedEventArgs e)
		{
			var value = WindowsUtility.GetWindowLong(View.Handle, (int)GWL.GWL_STYLE).ToInt32();
			var resetValue = value & ~(int)(WS.WS_MAXIMIZEBOX | WS.WS_MINIMIZEBOX);
			WindowsUtility.SetWindowLong(View.Handle, (int)GWL.GWL_STYLE, new IntPtr(resetValue));
		}

		void View_UserClosing(object sender, CancelEventArgs e)
		{
			Debug.Assert(HasView);

			e.Cancel = true;
			Visible = false;
		}

	}
}
