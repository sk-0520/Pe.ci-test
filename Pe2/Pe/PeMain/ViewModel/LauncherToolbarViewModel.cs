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
			MenuWidth = GetMenuWidth();
			ButtonSize = CalcButtonSize();
			IconSize = CalcIconSize();
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
			get { return CalcViewWidth(DockType); }
			set
			{
				if (DockType == DockType.None) {
					Model.Toolbar.FloatToolbarArea.WidthButtonCount = CalcButtonWidthCount(value, DockType);
					OnPropertyChanged();
				}
			}
		}
		public double FloatHeight
		{
			get { return CalcViewHeight(DockType); }
			set
			{
				if (DockType == DockType.None) {
					Model.Toolbar.FloatToolbarArea.HeightButtonCount = CalcButtonHeightCount(value, DockType);
					OnPropertyChanged();
				}
			}
		}

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
			NativeMethods.MoveWindow(View.Handle, podRect.Left, podRect.Top, podRect.Width, podRect.Height, true);
		}

		#endregion

		public Size IconSize { get; set; }
		public Size ButtonSize { get; set; }
		public double MenuWidth { get; set; }

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

		public ObservableCollection<LauncherGroupItemModel> GroupItems { get { return new ObservableCollection<LauncherGroupItemModel>( Model.GroupItems.Items); } }

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

		public IEnumerable<LauncherViewModel> LauncherItems 
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
					list2.Add(new LauncherViewModel(new LauncherItemModel() {
						Id = "test3",
						Name = "name3",
						LauncherKind = LauncherKind.Directory,
						Command = @"%windir%\"
					}));
					list2.Add(new LauncherViewModel(new LauncherItemModel() {
						Id = "test4",
						Name = "name4",
						LauncherKind = LauncherKind.Command,
						Command = @"ping"
					}));
					this._launcherItems = list2;
					OnPropertyChanged();
				}

				return this._launcherItems;
			}
		}

		public ImageSource ToolbarImage { get { return null; } }
		public string ToolbarText { get { return SelectedGroup.Name; } }
		public Visibility TextVisible { get { return Model.Toolbar.TextVisible ? Visibility.Visible: Visibility.Collapsed; } }

		#endregion

		#region command

		public ICommand PositionChangeCommand
		{
			get
			{
				var result = new DelegateCommand(
					o => {
						var dockType = (DockType)o;
						View.Docking(dockType);
					}
				);

				return result;
			}
		}

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
			return new Size(40 + MenuWidth + (Model.Toolbar.Visible ? Model.Toolbar.TextWidth: 0), 40);
		}

		double CalcViewWidth(DockType dockType)
		{
			return Model.Toolbar.FloatToolbarArea.WidthButtonCount * ButtonSize.Width;
		}
		int CalcButtonWidthCount(double viewWidth, DockType dockType)
		{
			return (int)(viewWidth / ButtonSize.Width);
		}
		double CalcViewHeight(DockType dockType)
		{
			return Model.Toolbar.FloatToolbarArea.HeightButtonCount * ButtonSize.Height;
		}
		int CalcButtonHeightCount(double viewHeight, DockType dockType)
		{
			return (int)(viewHeight / ButtonSize.Height);
		}

		#endregion

	}
}
