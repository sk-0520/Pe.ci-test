namespace ContentTypeTextNet.Pe.PeMain.UI
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Drawing;
	using System.Drawing.Drawing2D;
	using System.IO;
	using System.Linq;
	using System.Runtime.InteropServices;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Kind;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using ContentTypeTextNet.Pe.PeMain.UI.Ex;
	using ContentTypeTextNet.Pe.PeMain.UI.Skin;
	using ObjectDumper;

	/// <summary>
	/// ツールバー。
	/// </summary>
	public partial class ToolbarForm: CommonAppbarForm
	{
		#region define
		const string menuNameMainPosDesktopFloat = "desktop_float";
		const string menuNameMainPosDesktopTop = "desktop_top";
		const string menuNameMainPosDesktopBottom = "desktop_bottom";
		const string menuNameMainPosDesktopLeft = "desktop_left";
		const string menuNameMainPosDesktopRight = "desktop_right";
		const string menuNameMainTopmost = "topmost";
		const string menuNameMainAutoHide = "autohide";
		const string menuNameMainHidden = "hidden";
		const string menuNameMainGroupSeparator = "group_sep";
		const string menuNameMainGroupItem = "group_item_";

		const string menuNameExecute = "exec";
		const string menuNameExecuteEx = "ex";
		const string menuNamePath = "path";
		const string menuNameFiles = "ls";

		const string menuNamePath_openParentDir = "open_parent_dir";
		const string menuNamePath_openWorkDir = "open_work_dir";
		const string menuNamePath_copyCommand = "copy_command";
		const string menuNamePath_copyParentDir = "copy_parrent_dir";
		const string menuNamePath_copyWorkDir = "copy_work_dir";
		const string menuNamePath_property = "property";

		const string menuNameFiles_open = "ls_dir_open";
		const string menuNameFiles_sep = "ls_dir_sep";

		const string menuNameApplicationExecute = "execute";
		const string menuNameApplicationClose = "close";
		const string menuNameApplicationHelp = "help";

		#endregion ////////////////////////////////////

		#region static
		#endregion ////////////////////////////////////

		#region variable
		ContextMenu _menuGroup = null;
		bool _menuOpening = false;

		ToolStripItem _dragStartItem;
		CustomToolTipForm _tipsLauncher;

		IDictionary<IconScale, Image> _waitImage = new Dictionary<IconScale, Image>();

		#endregion ////////////////////////////////////

		public ToolbarForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Initialize();
		}

		#region property
		//CommonData CommonData { get; set; }

		ToolbarGroupItem SelectedGroupItem { get; set; }
		public ToolbarItem UsingToolbarItem { get; private set; }

		override public DesktopDockType DesktopDockType
		{
			get { return base.DesktopDockType; }
			set
			{
				if(CommonData != null) {
					var pos = UsingToolbarItem.ToolbarPosition;

					Padding = CommonData.Skin.GetToolbarTotalPadding(UsingToolbarItem.ToolbarPosition, Size);
					if(this.toolLauncher != null) {
						if(ToolbarPositionUtility.IsHorizonMode(pos)) {
							this.toolLauncher.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
						} else {
							this.toolLauncher.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
						}
					}
				}
				base.DesktopDockType = value;
			}
		}
		#endregion ////////////////////////////////////

		#region ISetCommonData
		//public void SetCommonData(CommonData commonData)
		//{
		//	CommonData = commonData;
		//	this.Initialized = false;

		//	this._tipsLauncher.SetCommonData(CommonData);
		//	ApplySetting();

		//	this.Initialized = true;
		//}
		#endregion ////////////////////////////////////

		#region override

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			//pevent.Graphics.Clear()
			if(CommonData.Skin.IsDefaultDrawToolbarWindowBackground) {
				base.OnPaintBackground(e);
			} else {
				CommonData.Skin.DrawToolbarWindowBackground(e.Graphics, e.ClipRectangle, this == Form.ActiveForm, UsingToolbarItem.ToolbarPosition);
			}
		}

		protected override void ToShow()
		{
			base.ToShow();
			ApplySettingTopmost();
			UIUtility.ShowFront(this);
		}

		protected override void HiddenView(bool animation, Rectangle area)
		{
			if(AutoHide) {
				if(!this._menuOpening) {
					var foreGroundWnd = NativeMethods.GetForegroundWindow();
					base.HiddenView(animation, area);
					TopMost = true;
					UIUtility.ShowFront(this);
					if(foreGroundWnd != null) {
						NativeMethods.BringWindowToTop(foreGroundWnd);
					}
				} else {
					//SwitchHidden();
				}
			}
		}

		protected override void WndProc(ref Message m)
		{
			if(UsingToolbarItem.ToolbarPosition == ToolbarPosition.DesktopFloat) {
				switch(m.Msg) {
					case (int)WM.WM_SYSCOMMAND:
						{
							switch(m.WParam.ToInt32() & 0xfff0) {
								case (int)SC.SC_MINIMIZE:
								case (int)SC.SC_MAXIMIZE:
								case (int)SC.SC_RESTORE:
									return;
								default:
									break;
							}
						}
						break;

					case (int)WM.WM_NCPAINT:
						{
							if(CommonData != null) {
								//var hDC = NativeMethods.GetWindowDC(Handle);
								//try {
								//	using(var g = Graphics.FromHdc(hDC)) {
								//		DrawNoClient(g, new Rectangle(Point.Empty, Size), this == Form.ActiveForm);
								//	}
								//} finally {
								//	NativeMethods.ReleaseDC(Handle, hDC);
								//}
								using(var hDC = new UnmanagedControlDeviceContext(this)) {
									using(var g = hDC.CreateGraphics()) {
										DrawNoClient(g, new Rectangle(Point.Empty, Size), this == Form.ActiveForm);
									}
								}
							}
						}
						break;

					case (int)WM.WM_NCHITTEST:
						{
							var point = PointToClient(WindowsUtility.ScreenPointFromLParam(m.LParam));
							var padding = Padding;

							var hitTest = HT.HTNOWHERE;
							var captionArea = CommonData.Skin.GetToolbarCaptionArea(UsingToolbarItem.ToolbarPosition, Size);
							if(captionArea.Contains(point)) {
								hitTest = HT.HTCAPTION;
							} else {
								var leftArea = new Rectangle(0, 0, padding.Left, Height);
								var rightArea = new Rectangle(Width - padding.Right, 0, padding.Right, Height);
								if(leftArea.Contains(point)) {
									hitTest = HT.HTLEFT;
								} else if(rightArea.Contains(point)) {
									hitTest = HT.HTRIGHT;
								}
							}
							if(hitTest != HT.HTNOWHERE) {
								m.Result = (IntPtr)hitTest;
								return;
							}
						}
						break;

					case (int)WM.WM_SETCURSOR:
						{
							if(!this._menuOpening) {
								var hittest = WindowsUtility.HTFromLParam(m.LParam);
								if(hittest == HT.HTCAPTION) {
									NativeMethods.SetCursor(NativeMethods.LoadCursor(IntPtr.Zero, IDC.IDC_SIZEALL));
									return;
								}
							}
						}
						break;

					/*
				case (int)WM.WM_CONTEXTMENU:
					{
						//Debug.WriteLine(m.WParam);
						//NativeMethods.SetCursor(NativeMethods.LoadCursor(IntPtr.Zero, IDC.IDC_ARROW));
					}
					//return;
					break;
					 */

					case (int)WM.WM_MOVING:
						{
							var rect = (RECT)Marshal.PtrToStructure(m.LParam, typeof(RECT));
							var workingArea = DockScreen.WorkingArea;

							if(rect.X < workingArea.X) {
								// 左
								rect.X = workingArea.X;
							} else if(rect.Right > workingArea.Right) {
								// 右
								rect.X = workingArea.Right - rect.Width;
							}

							if(rect.Y < workingArea.Y) {
								// 上
								rect.Y = workingArea.Y;
							} else if(rect.Bottom > workingArea.Bottom) {
								// 下
								rect.Y = workingArea.Bottom - rect.Height;
							}

							Marshal.StructureToPtr(rect, m.LParam, false);
						}
						break;

					case (int)WM.WM_DWMCOMPOSITIONCHANGED:
						{
							CommonData.Skin.RefreshStyle(this, SkinWindow.Toolbar);
						}
						break;
				}
			}
			base.WndProc(ref m);
		}

		#endregion ////////////////////////////////////

		#region initialize
		void InitializeUI()
		{
			this._menuGroup = new ContextMenu();

			ContextMenu = this._menuGroup;
			ContextMenu.Popup += OpeningRootMenu;
			ContextMenu.Collapse += CloseRootMenu;

			Visible = false;

			this._tipsLauncher = new CustomToolTipForm();

			//this.tipsLauncher.SetToolTip(this.toolLauncher, "#");
		}

		void Initialize()
		{
			InitializeUI();
		}
		#endregion ////////////////////////////////////

		#region language
		protected override void ApplyLanguage()
		{
			base.ApplyLanguage();

			UIUtility.SetDefaultText(this, CommonData.Language);
		}
		#endregion ////////////////////////////////////

		#region function
		void ApplySettingTopmost()
		{
			TopMost = UsingToolbarItem.Topmost;
		}
		
		protected override void ApplySkin()
		{
			base.ApplySkin();

			var iconScaleList = new [] { IconScale.Small, IconScale.Normal, IconScale.Big };
			foreach(var image in this._waitImage.Values) {
				image.ToDispose();
			}
			this._waitImage.Clear();
			var waitIcon = CommonData.Skin.GetIcon(SkinIcon.Wait);
			foreach(var iconScale in iconScaleList) {
				this._waitImage[iconScale] = IconUtility.ImageFromIcon(waitIcon, iconScale);
			}

			var renderer = new ToolbarRenderer();
			renderer.Skin = CommonData.Skin;
			renderer.ToolbarItem = UsingToolbarItem;
			
			this.toolLauncher.Renderer = renderer;

			CommonData.Skin.AttachmentStyle(this, SkinWindow.Toolbar);
		}
		
		void ApplySettingPosition()
		{
			/*
			if(false) {
				HiddenWaitTime = UseToolbarItem.HiddenWaitTime;
				HiddenAnimateTime = UseToolbarItem.HiddenAnimateTime;
			}
			//*/
			if(UsingToolbarItem.Visible) {
				var prevOpacity = Opacity;
				Opacity = 0;
				
				SuspendLayout();
				try {
					ItemSizeToFormSize();
					
					if(ToolbarPositionUtility.IsDockingMode(UsingToolbarItem.ToolbarPosition)) {
						AutoHide = UsingToolbarItem.AutoHide;
					}
					
					if(ToolbarPositionUtility.IsDockingMode(UsingToolbarItem.ToolbarPosition)) {
						DesktopDockType = ToolbarPositionConverter.ToDockType(UsingToolbarItem.ToolbarPosition);
						if(ToolbarPositionUtility.IsHorizonMode(UsingToolbarItem.ToolbarPosition)) {
							this.toolLauncher.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
						} else {
							this.toolLauncher.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
						}
					} else {
						DesktopDockType = DesktopDockType.None;
						if(UsingToolbarItem.ToolbarPosition == ToolbarPosition.DesktopFloat) {
							Location = UsingToolbarItem.FloatLocation;
						}
					}
					//DrawFullActivaChanged(this == Form.ActiveForm);
				} finally {
					ResumeLayout();
					Opacity = prevOpacity;
				}
			}
		}
		
		void ApplyScreen()
		{
			Debug.Assert(CommonData != null);
			Debug.Assert(CommonData.Skin != null);
			Debug.Assert(DockScreen != null);
			
			UsingToolbarItem = null;
			foreach(var item in CommonData.MainSetting.Toolbar.Items) {
				if(item.IsNameEqual(DockScreen.DeviceName)) {
					UsingToolbarItem = item;
					break;
				}
			}
			if(UsingToolbarItem == null) {
				// 新規
				var toolbarItem = new ToolbarItem();
				toolbarItem.Name = DockScreen.DeviceName;
				CommonData.MainSetting.Toolbar.Items.Add(toolbarItem);
				toolbarItem.FloatLocation = DockScreen.WorkingArea.Location;
				UsingToolbarItem = toolbarItem;
			}
		}

		void ApplySettingFont()
		{
			Debug.Assert(CommonData != null);
			Debug.Assert(CommonData.MainSetting != null);
			
			if(UsingToolbarItem.FontSetting != null && !UsingToolbarItem.FontSetting.IsDefault) {
				this.toolLauncher.Font = UsingToolbarItem.FontSetting.Font;
			}
		}

		public void ApplySettingVisible()
		{
			var floatSize = UsingToolbarItem.FloatSize;
			if(!Visible && UsingToolbarItem.ToolbarPosition == ToolbarPosition.DesktopFloat) {
				Size = floatSize;
			}
			Visible = UsingToolbarItem.Visible;
		}
		
		protected override void ApplySetting()
		{
			ApplyScreen();
			ApplySettingFont();

			base.ApplySetting();

			this._tipsLauncher.SetCommonData(CommonData);

			Font = UsingToolbarItem.FontSetting.Font;
			if(CommonData.MainSetting.Toolbar.ToolbarGroup.Groups.Count == 0) {
				// グループが存在しなければグループを作っておく
				var toolbarGroupItem = new ToolbarGroupItem();
				toolbarGroupItem.Name = CommonData.Language["new/group-item"];
				CommonData.MainSetting.Toolbar.ToolbarGroup.Groups.Add(toolbarGroupItem);
			}
			
			// グループメニュー基盤構築
			this._menuGroup.MenuItems.Clear();
			foreach(var groupName in CommonData.MainSetting.Toolbar.ToolbarGroup.Groups) {
				var menuItem = new MenuItem();
				
				menuItem.Text = groupName.Name;
				menuItem.Tag = groupName;

				menuItem.Click += new EventHandler(ToolbarForm_MenuItem_Click);
				
				this._menuGroup.MenuItems.Add(menuItem);
			}
			var firstGroup = CommonData.MainSetting.Toolbar.ToolbarGroup.Groups.First();
			var initGroup = CommonData.MainSetting.Toolbar.ToolbarGroup.Groups.FirstOrDefault(g => ToolbarItem.CheckNameEqual(g.Name, UsingToolbarItem.DefaultGroup));
			
			SelectedGroup(initGroup ?? firstGroup);
			
			// 表示
			ApplySettingPosition();
			ApplySettingVisible();
			ApplySettingTopmost();
			
			HiddenAnimateTime = UsingToolbarItem.HiddenAnimateTime;
			HiddenWaitTime = UsingToolbarItem.HiddenWaitTime;
		}
		
		/// <summary>
		/// 表示タイプからウィンドウをそれっぽいサイズに変更
		/// </summary>
		void ItemSizeToFormSize()
		{
			var floatSize = UsingToolbarItem.FloatSize;
			Padding = CommonData.Skin.GetToolbarTotalPadding(UsingToolbarItem.ToolbarPosition, Size);
			
			var buttonLayout = CommonData.Skin.GetToolbarButtonLayout(UsingToolbarItem.IconScale, UsingToolbarItem.ShowText, UsingToolbarItem.TextWidth);
			var edgeSize = CommonData.Skin.GetToolbarWindowEdgePadding(UsingToolbarItem.ToolbarPosition);
			var borderPadding = CommonData.Skin.GetToolbarBorderPadding(UsingToolbarItem.ToolbarPosition);
			this.toolLauncher.Padding = borderPadding;
			var minSize = new Size(edgeSize.Horizontal + buttonLayout.Size.Width, edgeSize.Vertical + buttonLayout.Size.Height);
			minSize.Width += this.toolLauncher.Margin.Horizontal + borderPadding.Horizontal;
			minSize.Height += this.toolLauncher.Margin.Vertical + borderPadding.Vertical;
			
			
			//Size = new Size(minSize.Width, minSize.Height);
			
			if(ToolbarPositionUtility.IsDockingMode(UsingToolbarItem.ToolbarPosition)) {
				BarSize = new Size(minSize.Width, minSize.Height);
				MinimumSize = Size.Empty;
			} else {
				if(ToolbarPositionUtility.IsHorizonMode(UsingToolbarItem.ToolbarPosition)) {
					Size = new Size(floatSize.Width, minSize.Height);
				} else {
					Size = new Size(minSize.Width, floatSize.Height);
				}
				MinimumSize = minSize;
			}
		}

		IList<IDisposable> DisposeFileToolMenuItem(ToolStripDropDownItem parentItem)
		{
			var menuItems = parentItem.DropDownItems.OfType<ToolStripMenuItem>().ToArray();
			var result = new List<IDisposable>(menuItems.Length);
			parentItem.DropDownItems.Clear();

			foreach(var menuItem in menuItems) {
				result.AddRange(DisposeFileToolMenuItem(menuItem));

				var fileImageItem = menuItem as FileImageToolStripMenuItem;
				if(fileImageItem != null) {
					result.Add(fileImageItem.FileImage);
					fileImageItem.FileImage = null;
				} else {
					result.Add(menuItem.Image);
				}
				menuItem.Image = null;
			}
			foreach(var menuItem in menuItems) {
				menuItem.DropDownItems.Clear();
				result.Add(menuItem);
			}

			return result;
		}

		void DisposeToolButtons()
		{
			//Debug.WriteLine("くりあ");
			var toolItems = this.toolLauncher.Items.Cast<ToolStripItem>().ToArray();
			this.toolLauncher.Items.Clear();
			var diposeList = new List<IDisposable>();
			foreach(var toolItem in toolItems) {
				var dropdownMenuItem = toolItem as ToolStripDropDownItem;
				if(dropdownMenuItem != null) {
					diposeList.AddRange(DisposeFileToolMenuItem(dropdownMenuItem));
				}
				//Debug.WriteLine("DisposeToolButtons: " + toolItem.Text);
				diposeList.Add(toolItem.Image);
				diposeList.Add(toolItem);
				toolItem.Image = null;
			}
			diposeList.ForEach(d => d.ToDispose());
			diposeList.Clear();
		}
		
		void SetToolButtons(IconScale iconScale, IEnumerable<ToolStripItem> buttons)
		{
			this.toolLauncher.ImageScalingSize = iconScale.ToSize();

			DisposeToolButtons();
			
			this.toolLauncher.Items.AddRange(buttons.ToArray());
		}
		
		void SelectedGroup(ToolbarGroupItem groupItem)
		{
			var toolItems = this._menuGroup.MenuItems.Cast<MenuItem>();
			foreach(var item in toolItems) {
				item.Checked = false;
			}
			var toolItem = toolItems.Single(item => (ToolbarGroupItem)item.Tag == groupItem);
			SelectedGroupItem = groupItem;
			
			toolItem.Checked = true;
			
			// 表示アイテム生成
			var toolButtonList = new List<ToolStripItem>();
			var mainButton = CreateLauncherButton(null);
			mainButton.Text = groupItem.Name;
			//mainButton.ToolTipText = CommonData.Language["toolbar/main/tips", new Dictionary<string, string>() {{AppLanguageName.groupName, groupItem.Name}}];

			toolButtonList.Add(mainButton);
			foreach(var itemName in groupItem.ItemNames) {
				var launcherItem = CommonData.MainSetting.Launcher.Items.SingleOrDefault(item => item.IsNameEqual(itemName));
				if(launcherItem != null) {
					var itemButton = CreateLauncherButton(launcherItem);
					toolButtonList.Add(itemButton);
				}
			}
			SetToolButtons(UsingToolbarItem.IconScale, toolButtonList);
		}

		void OpenParentDirectory(LauncherItem launcherItem)
		{
			try {
				var expandPath = Environment.ExpandEnvironmentVariables(launcherItem.Command);
				Executor.OpenDirectoryWithFileSelect(expandPath, CommonData, null);
			} catch(Exception ex) {
				CommonData.Logger.Puts(LogType.Warning, ex.Message, ex);
			}
		}

		void OpenDirectory(string path)
		{
			try {
				var expandPath = Environment.ExpandEnvironmentVariables(path);
				Executor.OpenDirectory(expandPath, CommonData, null);
			} catch(Exception ex) {
				CommonData.Logger.Puts(LogType.Warning, ex.Message, ex);
			}
		}
		
		void CopyText(string text)
		{
			ClipboardUtility.CopyText(text, CommonData);
		}
		
		void AttachmentFileLauncherPathSubMenu(ToolStripMenuItem parentItem, LauncherItem launcherItem)
		{
			var itemList = new List<ToolStripItem>();

			var openParentDirItem = new LauncherToolStripMenuItem(CommonData) {
				LauncherItem = launcherItem,
			};
			var openWorkDirItem = new LauncherToolStripMenuItem(CommonData) {
				LauncherItem = launcherItem,
			};
			var copyCommandItem = new LauncherToolStripMenuItem(CommonData) {
				LauncherItem = launcherItem,
			};
			var copyParentDirItem = new LauncherToolStripMenuItem(CommonData) {
				LauncherItem = launcherItem,
			};
			var copyWorkDirItem = new LauncherToolStripMenuItem(CommonData) {
				LauncherItem = launcherItem,
			};
			var propertyItem = new LauncherToolStripMenuItem(CommonData) {
				LauncherItem = launcherItem,
			};

			itemList.Add(openParentDirItem);
			itemList.Add(openWorkDirItem);
			itemList.Add(new DisableCloseToolStripSeparator());
			itemList.Add(copyCommandItem);
			itemList.Add(copyParentDirItem);
			itemList.Add(copyWorkDirItem);
			itemList.Add(new DisableCloseToolStripSeparator());
			itemList.Add(propertyItem);
			
			// 親ディレクトリを開く
			openParentDirItem.Name = menuNamePath_openParentDir;
			openParentDirItem.Text = CommonData.Language["toolbar/menu/file/path/open-parent-dir"];
			//openParentDirItem.Click += (object sender, EventArgs e) => OpenDir(launcherItem);
			openParentDirItem.Click += FileLauncherItemPathMenu_OpenParentDirectory;
			// 作業ディレクトリを開く
			openWorkDirItem.Name = menuNamePath_openWorkDir;
			openWorkDirItem.Text = CommonData.Language["toolbar/menu/file/path/open-work-dir"];
			//openWorkDirItem.Click += (object sender, EventArgs e) => OpenDir(launcherItem.WorkDirPath);
			openWorkDirItem.Click += FileLauncherItemPathMenu_OpenWorkDirectory;
			// コマンドコピー
			copyCommandItem.Name = menuNamePath_copyCommand;
			copyCommandItem.Text = CommonData.Language["toolbar/menu/file/path/copy-command"];
			//copyCommandItem.Click += (object sender, EventArgs e) => CopyText(launcherItem.Command);
			copyCommandItem.Click += FileLauncherItemPathMenu_CopyCommand;
			// 親ディレクトリをコピー
			copyParentDirItem.Name = menuNamePath_copyParentDir;
			copyParentDirItem.Text = CommonData.Language["toolbar/menu/file/path/copy-parent-dir"];
			//copyParentDirItem.Click += (object sender, EventArgs e) => CopyText(Path.GetDirectoryName(launcherItem.Command));
			copyParentDirItem.Click += FileLauncherItemPathMenu_CopyParentDirectory;
			// 作業ディレクトリをコピー
			copyWorkDirItem.Name = menuNamePath_copyWorkDir;
			copyWorkDirItem.Text = CommonData.Language["toolbar/menu/file/path/copy-work-dir"];
			//copyWorkDirItem.Click += (object sender, EventArgs e) => CopyText(launcherItem.WorkDirPath);
			copyWorkDirItem.Click += FileLauncherItemPathMenu_CopyWorkDirectory;
			// プロパティ
			propertyItem.Name = menuNamePath_property;
			propertyItem.Text = CommonData.Language["toolbar/menu/file/path/property"];
			//propertyItem.Click += (object sender, EventArgs e) => OpenProperty(launcherItem.Command);
			propertyItem.Click += FileLauncherItemPathMenu_OpenProperty;
			
			// メニュー構築
			parentItem.DropDownItems.AddRange(itemList.ToArray());
			//parentItem.DropDownOpening += (object sender, EventArgs e) => {
			//	// コマンド有無
			//	var commandEnabled = launcherItem.IsExists;
			//	copyCommandItem.Enabled = commandEnabled;
			//	propertyItem.Enabled = commandEnabled;
			//	// 親ディレクトリ有無
			//	var parentDirPath = Path.GetDirectoryName(Environment.ExpandEnvironmentVariables(launcherItem.Command));
			//	var parentDirEnabled = !string.IsNullOrEmpty(parentDirPath) && Path.GetPathRoot(parentDirPath) != parentDirPath && Directory.Exists(parentDirPath);
			//	openParentDirItem.Enabled = parentDirEnabled;
			//	copyParentDirItem.Enabled = parentDirEnabled;
			//	// 作業ディレクトリ有無
			//	var workDirEnabled = !string.IsNullOrEmpty(launcherItem.WorkDirPath) && Directory.Exists(Environment.ExpandEnvironmentVariables(launcherItem.WorkDirPath));
			//	openWorkDirItem.Enabled = workDirEnabled;
			//	copyWorkDirItem.Enabled = workDirEnabled;
			//};
			parentItem.DropDownOpening += FileLauncherItemPathMenu_DropDownOpening;
		}
		
		ToolStripMenuItem CreateFileListMenuItem(CommonData commonData, string path, bool isDir, bool showExtension, bool isHiddenFile)
		{
			var menuItem = new FileImageToolStripMenuItem(commonData) {
				Path = path,
				Image = this._waitImage[UsingToolbarItem.IconScale],
				ImageScaling = ToolStripItemImageScaling.None,
			};

			if(!isDir && !showExtension) {
				menuItem.Text = Path.GetFileNameWithoutExtension(path);
			} else {
				menuItem.Text = Path.GetFileName(path);
			}
			//*
			// 至上命題: UIスレッドに結合される前に処理完了せよ！
			Task.Run(() => {
				try {
					/*
					var waitCount = 0;
					while(waitCount <= Literal.loadIconRetryCount) {
						using(var icon = IconUtility.Load(path, UsingToolbarItem.IconScale, 0)) {
							if(icon != null) {
								if(isHiddenFile) {
									using(var image = icon.ToBitmap()) {
										return DrawUtility.Opacity(image, Literal.hiddenFileOpacity);
									}
								} else {
									return icon.ToBitmap();
								}
							} else {
								commonData.Logger.PutsDebug(menuItem.Path, () => string.Format("Toolbar: wait {0}ms, count: {1}", Literal.loadIconRetryTime.TotalMilliseconds, waitCount));
								Thread.Sleep(Literal.loadIconRetryTime);
								waitCount++;
							}
						}
					}
					*/
					var icon = AppUtility.LoadIcon(new IconPath(path, 0), UsingToolbarItem.IconScale, Literal.loadIconRetryTime, Literal.loadIconRetryCount, commonData.Logger);
					if(icon != null) {
						using(icon) {
							if(isHiddenFile) {
								using(var image = icon.ToBitmap()) {
									return DrawUtility.Opacity(image, Literal.hiddenFileOpacity);
								}
							} else {
								return icon.ToBitmap();
							}
						}
					}
				} catch(Exception ex) {
					commonData.Logger.Puts(LogType.Warning, menuItem.Path, ex);
				}

				return null;
			}).ContinueWith(t => {
				try {
					menuItem.FileImage = t.Result;
				} catch(Exception ex) {
					commonData.Logger.Puts(LogType.Error, menuItem.Path, ex);
				} finally {
					t.Dispose();
				}
			});
			//*/
			// スレッド危ないわ
			//try {
			//	using(var icon = IconUtility.Load(path, UsingToolbarItem.IconScale, 0)) {
			//		if(icon != null) {
			//			if(isHiddenFile) {
			//				using(var image = icon.ToBitmap()) {
			//					menuItem.FileImage = DrawUtility.Opacity(image, Literal.hiddenFileOpacity);
			//				}
			//			} else {
			//				menuItem.FileImage = icon.ToBitmap();
			//			}
			//		} else {
			//			commonData.Logger.Puts(LogType.Error, menuItem.Path, "icon is null");
			//		}
			//	}
			//} catch(/*Aggregate*/Exception ex) {
			//	commonData.Logger.Puts(LogType.Warning, menuItem.Path, ex);
			//}

			if(isDir) {
				AttachmentDirectoryOpen(menuItem, path);
			} else {
				menuItem.Click += FileListMenu_Click;
			}

			return menuItem;
		}

		/// <summary>
		/// ファイル一覧メニューにディレクトリを開く共通項目の設定
		/// </summary>
		/// <param name="parentItem">項目を設定する親メニューアイテム</param>
		/// <param name="dirPath">基ディレクトリパス</param>
		void AttachmentDirectoryOpen(ToolStripDropDownItem parentItem, string dirPath)
		{
			// ここを開く
			var openItem = new FileToolStripMenuItem(CommonData) {
				Name = menuNameFiles_open,
				Text = CommonData.Language["toolbar/menu/file/ls/open"],
				Path = dirPath,
			};
			openItem.Click += FileListMenu_Click;

			// 罫線
			var sepItem = new DisableCloseToolStripSeparator() {
				Name = menuNameFiles_sep,
			};

			var menuList = new ToolStripItem[] {
				openItem,
				sepItem,
			};

			parentItem.DropDownItems.AddRange(menuList);

			parentItem.DropDownOpening += FileListMenu_DropDownOpening;
			parentItem.DropDownClosed += FileListMenu_DropDownClosed;
		}

		/// <summary>
		/// ファイル一覧メニューに指定ディレクトリ以下のファイル・ディレクトリ一覧を設定する
		/// </summary>
		/// <param name="parentItem">項目を設定する親メニューアイテム</param>
		/// <param name="appendOpen">ディレクトリを開く共通メニューを追加するか</param>
		/// <param name="dirPath">基ディレクトリパス</param>
		/// <param name="showHiddenFile">隠しファイルを表示するか</param>
		/// <param name="showExtension">拡張子を表示するか</param>
		/// <returns></returns>
		bool AttachmentFileList(ToolStripDropDownItem parentItem, bool appendOpen, string dirPath, bool showHiddenFile, bool showExtension)
		{
			if(!Directory.Exists(dirPath)) {
				CommonData.Logger.Puts(LogType.Warning, CommonData.Language["common/message/notfound-dir"], dirPath);;
				return false;
			}

			try {
				Cursor = Cursors.AppStarting;

				if(appendOpen) {
					AttachmentDirectoryOpen(parentItem, dirPath);
				}

				var menuList = new List<ToolStripItem>();
				try {
					// ディレクトリ以下のファイルを列挙
					var pathItemList = new DirectoryInfo(dirPath).EnumerateFileSystemInfos()
						.Where(fs => fs.Exists)
						.Select(fs => new {
							Path = fs.FullName,
							Name = fs.Name,
							IsDirectory = fs.Attributes.HasFlag(FileAttributes.Directory),
							IsHiddenFile = fs.Attributes.HasFlag(FileAttributes.Hidden)
						})
						.Where(f => showHiddenFile ? true : !f.IsHiddenFile)
						.OrderByDescending(f => f.IsDirectory)
						.ThenBy(fs => fs.Name)
					;

					foreach(var pathItem in pathItemList) {
						var menuItem = CreateFileListMenuItem(CommonData, pathItem.Path, pathItem.IsDirectory, showExtension, pathItem.IsHiddenFile);
						menuList.Add(menuItem);
					}
					if(menuList.Count == 0) {
						var menuItem = new ToolStripMenuItem();
						menuItem.Text = CommonData.Language["toolbar/menu/file/ls/not-child-files"];
						menuItem.Image = SystemIcons.Information.ToBitmap();
						menuItem.Enabled = false;

						menuList.Add(menuItem);
					}
				} catch(IOException ex) {
					var menuItem = new ToolStripMenuItem();
					menuItem.Text = ex.Message;
					menuItem.Image = SystemIcons.Error.ToBitmap();
					menuItem.Enabled = false;
					menuList.Add(menuItem);
				} catch(UnauthorizedAccessException ex) {
					var menuItem = new ToolStripMenuItem();
					menuItem.Text = ex.Message;
					menuItem.Image = SystemIcons.Warning.ToBitmap();
					menuItem.Enabled = false;
					menuList.Add(menuItem);
				}

				parentItem.ImageScaling = ToolStripItemImageScaling.None;
				parentItem.DropDownItems.AddRange(menuList.ToArray());
				ToolStripUtility.AttachmentOpeningMenuInScreen(parentItem);
			} finally {
				Cursor = Cursors.Default;
			}
			
			return true;
		}

		void AttachmentFileLauncherMenu(ToolStripDropDownItem parentItem, LauncherItem launcherItem)
		{
			// 通常実行
			var executeItem = new LauncherToolStripMenuItem(CommonData) {
				LauncherItem = launcherItem,
				Name = menuNameExecute,
				Text = CommonData.Language["toolbar/menu/file/execute"],
			};
			executeItem.Click += FileLauncherItemMenu_Execute;

			// 指定実行
			var executeExItem = new LauncherToolStripMenuItem(CommonData) {
				LauncherItem = launcherItem,
				Name = menuNameExecuteEx,
				Text = CommonData.Language["toolbar/menu/file/execute-ex"],
			};
			executeExItem.Click += FileLauncherItemMenu_ExecuteEx;

			// パス関係
			var pathItem = new LauncherToolStripMenuItem(CommonData) {
				LauncherItem = launcherItem,
				Name = menuNamePath,
				Text = CommonData.Language["toolbar/menu/file/path"],
			};
			AttachmentFileLauncherPathSubMenu(pathItem, launcherItem);

			// ファイル一覧
			var fileItem = new ToolStripMenuItem() {
				Name = menuNameFiles,
				Text = CommonData.Language["toolbar/menu/file/ls"],
			};

			var menuList = new ToolStripItem[] {
				executeItem,
				executeExItem,
				new DisableCloseToolStripSeparator(),
				pathItem,
				fileItem,
			};
			
			// メニュー設定
			ToolStripUtility.AttachmentOpeningMenuInScreen(menuList);
			parentItem.DropDownItems.AddRange(menuList);
			parentItem.DropDownOpening += FileLauncherItemMenu_DropDownOpening;
		}

		string MakeGroupItemName(string groupName)
		{
			return menuNameMainGroupItem + groupName;
		}
		
		/// <summary>
		/// TODO: += 
		/// </summary>
		/// <param name="parentItem"></param>
		void AttachmentToolbarMenu(ToolStripDropDownItem parentItem)
		{
			// フロート
			var posFloatItem = new ToolStripMenuItem() {
				Name = menuNameMainPosDesktopFloat,
				Text = ToolbarPosition.DesktopFloat.ToText(CommonData.Language),
			};
			posFloatItem.Click += (object sender, EventArgs e) => {
				UsingToolbarItem.ToolbarPosition = ToolbarPosition.DesktopFloat;
				ApplySettingPosition();
			};

			// デスクトップ：上
			var posTopItem = new ToolStripMenuItem() {
				Name = menuNameMainPosDesktopTop,
				Text = ToolbarPosition.DesktopTop.ToText(CommonData.Language),
			};
			posTopItem.Click += (object sender, EventArgs e) => {
				UsingToolbarItem.ToolbarPosition = ToolbarPosition.DesktopTop;
				ApplySettingPosition();
			};

			// デスクトップ：下
			var posBottomItem = new ToolStripMenuItem() {
				Name = menuNameMainPosDesktopBottom,
				Text = ToolbarPosition.DesktopBottom.ToText(CommonData.Language),
			};
			posBottomItem.Click += (object sender, EventArgs e) => {
				UsingToolbarItem.ToolbarPosition = ToolbarPosition.DesktopBottom;
				ApplySettingPosition();
			};

			// デスクトップ：左
			var posLeftItem = new ToolStripMenuItem() {
				Name = menuNameMainPosDesktopLeft,
				Text = ToolbarPosition.DesktopLeft.ToText(CommonData.Language),
			};
			posLeftItem.Click += (object sender, EventArgs e) => {
				UsingToolbarItem.ToolbarPosition = ToolbarPosition.DesktopLeft;
				ApplySettingPosition();
			};

			// デスクトップ：右
			var posRightItem = new ToolStripMenuItem() {
				Name = menuNameMainPosDesktopRight,
				Text = ToolbarPosition.DesktopRight.ToText(CommonData.Language),
			};
			posRightItem.Click += (object sender, EventArgs e) => {
				UsingToolbarItem.ToolbarPosition = ToolbarPosition.DesktopRight;
				ApplySettingPosition();
			};

			// 最前面表示
			var topmostItem = new ToolStripMenuItem() {
				Name = menuNameMainTopmost,
				Text = CommonData.Language["common/menu/topmost"],
			};
			topmostItem.Click += (object sender, EventArgs e) => {
				UsingToolbarItem.Topmost = !topmostItem.Checked;
				ApplySettingTopmost();
			};

			// 自動的に隠す
			var autoHideItem = new ToolStripMenuItem() {
				Name = menuNameMainAutoHide,
				Text = CommonData.Language["toolbar/menu/main/auto-hide"],
			};
			autoHideItem.Click += (object sender, EventArgs e) => {
				UsingToolbarItem.AutoHide = !autoHideItem.Checked;
				ApplySettingPosition();
				if(DesktopDockType != DesktopDockType.None) {
					UsingToolbarItem.AutoHide = AutoHide;
				} else {
					UsingToolbarItem.AutoHide = false;
				}
			};

			// 非表示
			var hiddenItem = new ToolStripMenuItem() {
				Name = menuNameMainHidden,
				Text = CommonData.Language["toolbar/menu/main/hidden"],
			};
			hiddenItem.Click += (object sender, EventArgs e) => {
				UsingToolbarItem.Visible = false;
				ApplySettingVisible();
			};

			// グループ関連メニュー
			var itemGroupSeparator = new DisableCloseToolStripSeparator() {
				Name = menuNameMainGroupSeparator,
			};

			var menuList = new List<ToolStripItem>() {
				posFloatItem,
				posTopItem,
				posBottomItem,
				posLeftItem,
				posRightItem,
				new DisableCloseToolStripSeparator(),
				topmostItem,
				autoHideItem,
				new DisableCloseToolStripSeparator(),
				hiddenItem,
				itemGroupSeparator,
			};

			foreach(var group in CommonData.MainSetting.Toolbar.ToolbarGroup.Groups) {
				var itemGroup = new ToolStripMenuItem();
				itemGroup.Text = group.Name;
				itemGroup.Name = MakeGroupItemName(group.Name);
				itemGroup.Tag = group;
				itemGroup.CheckState = CheckState.Indeterminate;
				itemGroup.Click += (object sender, EventArgs e) => SelectedGroup(group);
				menuList.Add(itemGroup);
			}
			
			// メニュー設定
			var items = menuList.ToArray();
			// #3
			foreach(var item in items) {
				item.ImageScaling = ToolStripItemImageScaling.None;
			}
			parentItem.DropDownItems.AddRange(items);
			
			parentItem.DropDownOpening += (object sender, EventArgs e) => {
				// ウィンドウ位置
				var windowPosNameKey = new Dictionary<ToolStripMenuItem, ToolbarPosition>() {
					{posFloatItem,  ToolbarPosition.DesktopFloat},
					{posTopItem,    ToolbarPosition.DesktopTop},
					{posBottomItem, ToolbarPosition.DesktopBottom},
					{posLeftItem,   ToolbarPosition.DesktopLeft},
					{posRightItem,  ToolbarPosition.DesktopRight},
				};
				foreach(var pair in windowPosNameKey) {
					pair.Key.CheckState = CheckState.Indeterminate;
					
					pair.Key.Checked = UsingToolbarItem.ToolbarPosition == pair.Value;
				}
				
				// 最前面表示
				topmostItem.Checked = UsingToolbarItem.Topmost;
				
				// 自動的に隠す
				autoHideItem.Checked = AutoHide;
				autoHideItem.Enabled = IsDocking;
				
				// グループ
				foreach(var groupItem in parentItem.DropDownItems.OfType<ToolStripMenuItem>().Where(i => i.Name.StartsWith(menuNameMainGroupItem, StringComparison.Ordinal))) {
					groupItem.Checked = groupItem.Tag == SelectedGroupItem;
				}
			};
		}

		void AttachmentEmbeddedLauncherMenu(ToolStripDropDownItem parentItem, LauncherItem launcherItem)
		{
			// 起動
			var execItem = new ToolStripMenuItem() {
				Name = menuNameApplicationExecute,
				Text = CommonData.Language["toolbar/menu/application/execute"],
			};
			execItem.Click += (object sender, EventArgs e) => {
				ExecuteItem(launcherItem);
			};

			// 終了
			var closeItem = new ToolStripMenuItem() {
				Name = menuNameApplicationClose,
				Text = CommonData.Language["toolbar/menu/application/close"],
			};
			closeItem.Click += (object sender, EventArgs e) => {
				try {
					CommonData.ApplicationSetting.KillApplicationItem(launcherItem);
				} catch(Exception ex) {
					var message = string.Format("{0} - {1}", launcherItem.Name, launcherItem.Command);
					CommonData.Logger.Puts(LogType.Warning, message, ex);
				}
			};

			// ヘルプ
			var helpItem = new ToolStripMenuItem() {
				Name = menuNameApplicationHelp,
				Text = CommonData.Language["toolbar/menu/application/help"],
			};
			helpItem.Click += (object sender, EventArgs e) => {
				var applicationItem = CommonData.ApplicationSetting.GetApplicationItem(launcherItem);
				try {
					Executor.RunCommand(applicationItem.HelpPath, CommonData);
				} catch(Exception ex) {
					var message = string.Format("{0} - {1}", launcherItem.Name, launcherItem.Command);
					CommonData.Logger.Puts(LogType.Warning, ex.Message, applicationItem.HelpPath);
				}
			};

			var menuList = new ToolStripItem[] {
				execItem,
				closeItem,
				new DisableCloseToolStripSeparator(),
				helpItem,
			};

			parentItem.DropDownItems.AddRange(menuList);
			parentItem.DropDownOpening += (object sender, EventArgs e) => {
				var applicationItem = CommonData.ApplicationSetting.GetApplicationItem(launcherItem);
				var isRunning = CommonData.ApplicationSetting.ExecutingItems.Any(i => i.ApplicationItem == applicationItem);
				execItem.Enabled = !isRunning;
				closeItem.Enabled = isRunning;
				helpItem.Enabled = !string.IsNullOrWhiteSpace(applicationItem.File.Help);
			};
		}

		static void SetButtonLayout(ToolStripItem toolItem, ISkin skin, IconScale iconSize, bool showText, int textWidth)
		{
			var toolSplit = toolItem as ToolStripSplitButton;
			var buttonLayout = skin.GetToolbarButtonLayout(iconSize, showText, textWidth);
			
			toolItem.Margin = Padding.Empty;
			toolItem.Padding = Padding.Empty;
			toolItem.AutoSize = false;
			toolItem.Size = buttonLayout.Size;
			if(toolSplit != null) {
				toolSplit.DropDownButtonWidth = buttonLayout.MenuWidth;
			}
			toolItem.ImageScaling = ToolStripItemImageScaling.None;
			if(showText) {
				toolItem.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
				toolItem.ImageAlign = ContentAlignment.MiddleLeft;
				toolItem.TextAlign = ContentAlignment.MiddleLeft;
			} else {
				toolItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
				toolItem.ImageAlign = ContentAlignment.MiddleCenter;
				toolItem.TextAlign = ContentAlignment.MiddleLeft;
			}
		}
		
		/// <summary>
		/// メインボタン生成。
		/// </summary>
		/// <returns></returns>
		ToolStripDropDownButton CreateMainLauncherButton()
		{
			var iconSize = UsingToolbarItem.IconScale.ToSize();
			var toolItem = new ToolStripDropDownButton();
			using(var icon = new Icon(CommonData.Skin.GetIcon(SkinIcon.ToolbarMain), iconSize)) {
				var img = new Bitmap(iconSize.Width, iconSize.Height);
				using(var g = Graphics.FromImage(img)) {
					g.DrawIcon(icon, new Rectangle(Point.Empty, UsingToolbarItem.IconScale.ToSize()));
					#if DEBUG
					DrawUtility.MarkingDebug(g, new Rectangle(Point.Empty, UsingToolbarItem.IconScale.ToSize()));
					#endif
				}
				toolItem.Image = img;
			}
			
			AttachmentToolbarMenu(toolItem);
			
			return toolItem;
		}
		
		/// <summary>
		/// ファイルアイテムボタン生成。
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		ToolStripSplitButton CreateFileItemLauncherButton(LauncherItem item)
		{
			var toolItem = new LauncherToolStripSplitButton(CommonData) {
				LauncherItem = item,
			};
			toolItem.ButtonClick += LauncherTypeFile_ButtonClick;
			
			AttachmentFileLauncherMenu(toolItem, item);
			
			return toolItem;
		}
		
		ToolStripDropDownButton CreateDirectoryItemLauncherButton(LauncherItem item)
		{
			var toolItem = new ToolStripDropDownButton();
			var showHiddenFile = SystemEnvironment.IsHiddenFileShow();
			var showExtension = SystemEnvironment.IsExtensionShow();
			AttachmentFileList(toolItem, true, Environment.ExpandEnvironmentVariables(item.Command), showHiddenFile, showExtension);

			return toolItem;
		}

		ToolStripButton CreateCommandItemLauncherButton(LauncherItem item)
		{
			var toolItem = new ToolStripButton();

			toolItem.Click += LauncherTypeFile_ButtonClick;

			return toolItem;
		}

		ToolStripSplitButton CreateEmbeddedItemLauncherButton(LauncherItem item)
		{
			var toolItem = new ToolStripSplitButton();
			toolItem.ButtonClick += LauncherTypeFile_ButtonClick;

			AttachmentEmbeddedLauncherMenu(toolItem, item);

			return toolItem;
		}

		/// <summary>
		/// ランチャーアイテムボタンの生成。
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		ToolStripItem CreateItemLauncherButton(LauncherItem item)
		{
			Debug.Assert(item != null);
			ToolStripItem toolItem;
			
			switch(item.LauncherType) {
				case LauncherType.File:
					toolItem = CreateFileItemLauncherButton(item);
					break;
				
				case LauncherType.Directory:
					toolItem = CreateDirectoryItemLauncherButton(item);
					break;

				case LauncherType.URI:
				case LauncherType.Command:
					toolItem = CreateCommandItemLauncherButton(item);
					break;

				case LauncherType.Embedded:
					toolItem = CreateEmbeddedItemLauncherButton(item);
					break;

				default:
					throw new NotImplementedException(item.LauncherType.ToString());
			}

			toolItem.AutoToolTip = false;
			toolItem.Tag = item;
			
			toolItem.Text = item.Name;
			//toolItem.ToolTipText = item.Name;
			var icon = item.GetIcon(UsingToolbarItem.IconScale, item.IconItem.Index, CommonData.ApplicationSetting, CommonData.Logger);
			if(icon != null) {
				toolItem.Image = icon.ToBitmap();
			}
			
			toolItem.MouseDown += LauncherButton_MouseDown;
			/*
			toolItem.MouseDown += (object sender, MouseEventArgs e) => {
				if(Control.ModifierKeys == Keys.Alt) {
					this._dragStartItem = toolItem;
					Debug.WriteLine(this._dragStartItem);
					this.toolLauncher.DoDragDrop(toolItem, DragDropEffects.Copy);
				}
			};
			 */
			return toolItem;
		}
		
		ToolStripItem CreateLauncherButton(LauncherItem item)
		{
			ToolStripItem toolItem = null;
			
			if(item == null) {
				toolItem = CreateMainLauncherButton();
			} else {
				toolItem = CreateItemLauncherButton(item);
			}
			
			SetButtonLayout(toolItem, CommonData.Skin, UsingToolbarItem.IconScale, UsingToolbarItem.ShowText, UsingToolbarItem.TextWidth);
			toolItem.Visible = true;

			toolItem.MouseHover += ToolItem_MouseHover;
			toolItem.MouseLeave += toolItem_MouseLeave;
			var dropdownItem = toolItem as ToolStripDropDownItem;
			if(dropdownItem != null) {
				dropdownItem.DropDownOpening += OpeningRootMenu;
				dropdownItem.DropDownClosed += CloseRootMenu;
				dropdownItem.DropDownOpening += ToolStripUtility.EventDropDownItemOpeningMenuInScreen;
			}
			
			return toolItem;
		}

		bool ExecuteItem(LauncherItem launcherItem)
		{
			try {
				Executor.RunItem(launcherItem, CommonData);
				launcherItem.Increment(null, null);
				return true;
			} catch(Exception ex) {
				CommonData.Logger.Puts(LogType.Warning, ex.Message, ex);
			}
			
			return false;
		}
		
		void ExecuteExItem(LauncherItem launcherItem, IEnumerable<string> exOptions)
		{
			var form = new ExecuteForm();
			form.SetParameter(launcherItem, exOptions);
			form.SetCommonData(CommonData);
			//form.TopMost = TopMost;
			CommonData.RootSender.AppendWindow(form);
			form.Show();
			form.FormClosed += (IRootSender, e) => {
				if(form.DialogResult == DialogResult.OK) {
					var editedItem = form.EditedLauncherItem;
					if(ExecuteItem(editedItem)) {
						launcherItem.Increment(editedItem.Option, editedItem.WorkDirPath);
					}
				}
			};
		}
		
		ToolStripItem GetOverButton(Point localPoint)
		{
			ToolStripItem overItem = null;
			foreach(ToolStripItem toolItem in this.toolLauncher.Items) {
				//Debug.WriteLine(toolItem.Bounds);
				if(toolItem.Bounds.Contains(localPoint.X, localPoint.Y)) {
					overItem = toolItem;
					break;
				}
			}
			
			return overItem;
		}

		DropData ProcessDropEffect(object sender, DragEventArgs e)
		{
			var result = new DropData();
			var localPoint = this.toolLauncher.PointToClient(new Point(e.X, e.Y));
			
			result.ToolStripItem = GetOverButton(localPoint);
			if(result.ToolStripItem != null) {
				result.LauncherItem = result.ToolStripItem.Tag as LauncherItem;
			}
			result.DropType = DropType.None;
			
			if(this._dragStartItem == null) {
				if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
					result.DropType = DropType.Files;
					result.Files = (string[])e.Data.GetData(DataFormats.FileDrop, false);

					
					if(result.ToolStripItem != null) {
						if(result.LauncherItem.IsDirectory) {
							e.Effect = DragDropEffects.None;
						} else {
							e.Effect = DragDropEffects.Move;
						}
					} else {
						if(result.Files.Count() == 1) {
							e.Effect = DragDropEffects.Copy;
						} else {
							e.Effect = DragDropEffects.None;
						}
					}
				} else {
					e.Effect = DragDropEffects.None;
				}
			} else {
				Debug.Assert(this._dragStartItem != null);
				
				e.Effect = DragDropEffects.Move;
				
				result.DropType = DropType.Button;
				result.SrcToolStripItem = this._dragStartItem;
				if(result.ToolStripItem is ToolStripOverflowButton) {
					// 表示領域外への入口
					e.Effect = DragDropEffects.None;
				} else if(result.ToolStripItem == null) {
					// ツールバー上のアイテムなし領域
					e.Effect = DragDropEffects.Move;
				} else if(result.ToolStripItem == result.SrcToolStripItem || result.LauncherItem == null) {
					// 同一アイテム or メインメニュー
					e.Effect = DragDropEffects.None;
				}
			}
			
			return result;
		}
		
		void ExecuteDropData(DropData dropData)
		{
			if(dropData.ToolStripItem != null) {
				// ボタン上
				Debug.Assert(dropData.Files.Any());
				ExecuteExItem(dropData.LauncherItem, dropData.Files);
			} else {
				// 追加
				Debug.Assert(dropData.Files.Count() == 1);
				
				var path = dropData.Files.First();
				var useShortcut = false;
				var forceLauncherType = false;
				var forceType = LauncherType.None;
				var checkDirectory = false;
				if(PathUtility.IsShortcutPath(path)) {
					var result = MessageBox.Show(CommonData.Language["common/dialog/d-d/shortcut/message"], CommonData.Language["common/dialog/d-d/shortcut/caption"], MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
					switch(result) {
						case DialogResult.Yes:
							try {
								using(var sf = new ShortcutFile(path)) {
									var expandPath = Environment.ExpandEnvironmentVariables(sf.TargetPath);
									checkDirectory = Directory.Exists(expandPath);
									path = sf.TargetPath;
								}
							} catch(ArgumentException ex) {
								CommonData.Logger.Puts(LogType.Warning, ex.Message, ex);
							}
							break;
							
						case DialogResult.No:
							useShortcut = true;
							break;
							
						default:
							return;
					}
				}
				
				if(checkDirectory || Directory.Exists(path)) {
					var result = MessageBox.Show(CommonData.Language["toolbar/dialog/d-d/directory/message"], CommonData.Language["toolbar/dialog/d-d/directory/caption"], MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
					switch(result) {
						case DialogResult.Yes:
							forceLauncherType = false;
							break;
							
						case DialogResult.No:
							forceLauncherType = true;
							forceType = LauncherType.File;
							break;
							
						default:
							return;
					}
				}
				var item = LauncherItem.LoadFile(path, useShortcut, forceLauncherType, forceType);
				var name = LauncherItem.GetUniqueName(item, CommonData.MainSetting.Launcher.Items);
				var newItem = true;
				if(item.Name != name) {
					// D&Dアイテムが既に登録済みアイテム名と被った場合はただの複製を考慮する
					var haveItem = CommonData.MainSetting.Launcher.Items.Single(i => item.IsNameEqual(i.Name));
					newItem = !haveItem.IsValueEqual(item);
				}
				if(newItem) {
					item.Name = name;
					CommonData.MainSetting.Launcher.Items.Add(item);
				}
				SelectedGroupItem.ItemNames.Add(item.Name);
				SelectedGroup(SelectedGroupItem);
				
				// 他のツールバーにアイテム変更を教える
				CommonData.RootSender.ChangedLauncherGroupItems(UsingToolbarItem, SelectedGroupItem);
			}
		}
		
		/// <summary>
		/// 該当のインデックスに挿入する形で処理する。
		/// 
		/// 例外として自身の次アイテムに挿入する場合は位置が変わらないためその次に挿入する。
		/// </summary>
		/// <param name="dropData"></param>
		void ChangeDropDataLauncherItemPosition(DropData dropData)
		{
			Debug.Assert(dropData.DropType == DropType.Button);
			//Debug.WriteLine("{0} * {1}", dropData.SrcToolStripItem, dropData.ToolStripItem);
			
			try {
				this.toolLauncher.SuspendLayout();
				
				if(dropData.ToolStripItem == null) {
					// 最終項目
					this.toolLauncher.Items.Remove(dropData.SrcToolStripItem);
					this.toolLauncher.Items.Add(dropData.SrcToolStripItem);
				} else {
					Debug.Assert(dropData.ToolStripItem != null);
					Debug.Assert(dropData.SrcToolStripItem != null);
					
					// 次の項目か
					var arrow = ToolbarPositionUtility.IsHorizonMode(UsingToolbarItem.ToolbarPosition) ? ArrowDirection.Right: ArrowDirection.Down;
					var nextItem = this.toolLauncher.GetNextItem(dropData.SrcToolStripItem, arrow);
					var isNext = nextItem == dropData.ToolStripItem;
					var itemList = this.toolLauncher.Items.Cast<ToolStripItem>().ToList();
					var newIndex = itemList.IndexOf(dropData.ToolStripItem);
					var srcIndex = itemList.IndexOf(dropData.SrcToolStripItem);
					if(isNext) {
						// 入れ替えっこ
						this.toolLauncher.Items.Insert(newIndex, dropData.SrcToolStripItem);
					} else {
						// 指定位置に挿入
						this.toolLauncher.Items.RemoveAt(srcIndex);
						this.toolLauncher.Items.Insert(newIndex, dropData.SrcToolStripItem);
					}
				}
			} finally {
				this.toolLauncher.ResumeLayout();
			}
			
			// 現在の並びをデータとして取得
			var groupItemNames = new List<string>(SelectedGroupItem.ItemNames.Count);
			foreach(var item in this.toolLauncher.Items.Cast<ToolStripItem>()) {
				var launcherItem = item.Tag as LauncherItem;
				if(launcherItem == null) {
					continue;
				}
				groupItemNames.Add(launcherItem.Name);
			}
			SelectedGroupItem.ItemNames = groupItemNames;
		}
		
		public void ReceiveChangedLauncherItems(ToolbarItem toolbarItem, ToolbarGroupItem toolbarGroupItem)
		{
			// 他のツールバーから通知を受け取った場合に反映処理を行う
			Debug.Assert(toolbarItem != UsingToolbarItem);
			SelectedGroup(SelectedGroupItem);
		}
		

		#endregion ////////////////////////////////////

		#region draw
		void DrawEdge(Graphics g, Rectangle drawArea, bool active)
		{
			if(CommonData.Skin.IsDefaultDrawToolbarWindowEdge) {
				var edgePadding = CommonData.Skin.GetToolbarWindowEdgePadding(UsingToolbarItem.ToolbarPosition);
				
				// 境界線
				var light = active ? SystemBrushes.ControlLight: SystemBrushes.ControlLightLight;
				var dark = active ? SystemBrushes.ControlDarkDark: SystemBrushes.ControlDark;
				
				// 下
				g.FillRectangle(dark, 0, drawArea.Height - edgePadding.Bottom, drawArea.Width, edgePadding.Bottom);
				// 右
				g.FillRectangle(dark, drawArea.Width - edgePadding.Right, 0, edgePadding.Right, drawArea.Height);
				// 左
				g.FillRectangle(dark, 0, 0, edgePadding.Left, drawArea.Height);
				// 上
				g.FillRectangle(dark, 0, 0, drawArea.Width, edgePadding.Top);
			} else {
				CommonData.Skin.DrawToolbarWindowEdge(g, drawArea, active, UsingToolbarItem.ToolbarPosition);
			}
		}
		
		void DrawCaption(Graphics g, Rectangle drawArea, bool active)
		{
			if(CommonData.Skin.IsDefaultDrawToolbarWindowCaption) {
				Color headColor;
				Color tailColor;
				if(active) {
					headColor = SystemColors.GradientActiveCaption;
					tailColor = SystemColors.ActiveCaption;
				} else {
					headColor = SystemColors.GradientInactiveCaption;
					tailColor = SystemColors.InactiveCaption;
				}
				var mode = ToolbarPositionUtility.IsHorizonMode(UsingToolbarItem.ToolbarPosition) ? LinearGradientMode.Vertical: LinearGradientMode.Horizontal;
				using(var brush = new LinearGradientBrush(drawArea, headColor, tailColor, mode)) {
					g.FillRectangle(brush, drawArea);
				}
			} else {
				CommonData.Skin.DrawToolbarWindowCaption(g, drawArea, active, UsingToolbarItem.ToolbarPosition);
			}
		}
		
		void DrawNoClient(Graphics g, Rectangle drawArea, bool active)
		{
			if(!CommonData.Skin.IsDefaultDrawToolbarWindowBackground) {
				CommonData.Skin.DrawToolbarWindowBackground(g, drawArea, active, UsingToolbarItem.ToolbarPosition);
			}
			
			var captionArea = CommonData.Skin.GetToolbarCaptionArea(UsingToolbarItem.ToolbarPosition, ClientSize);
			if(!captionArea.Size.IsEmpty) {
				DrawCaption(g, captionArea, active);
			}
			DrawEdge(g, drawArea, active);
		}
		
		void DrawFull(Graphics g, Rectangle drawArea, bool active)
		{
			DrawNoClient(g, drawArea, active);
			this.toolLauncher.Refresh();
		}
		
		void DrawFullActivaChanged(bool active)
		{
			using(var g = CreateGraphics()) {
				using(var bmp = new Bitmap(Width, Height, g)) {
					using(var memG = Graphics.FromImage(bmp)) {
						var rect = new Rectangle(Point.Empty, Size);
						DrawFull(memG, rect, active);
						if(!CommonData.Skin.IsDefaultDrawToolbarWindowBackground) {
							g.CompositingMode = CompositingMode.SourceCopy;
						}
						g.DrawImage(bmp, 0, 0);
					}
				}
			}
		}
		#endregion ////////////////////////////////////

		void ToolbarForm_MenuItem_Click(object sender, EventArgs e)
		{
			var menuItem = (MenuItem)sender;
			var group = (ToolbarGroupItem)menuItem.Tag;
			SelectedGroup(group);
		}
		
		void LauncherTypeFile_ButtonClick(object sender, EventArgs e)
		{
			var toolItem = (ToolStripItem)sender;
			var launcherItem = (LauncherItem)toolItem.Tag;
			this._tipsLauncher.HideItem();
			ExecuteItem(launcherItem);
		}
		
		void ToolbarForm_Paint(object sender, PaintEventArgs e)
		{
			DrawFull(e.Graphics, ClientRectangle, Form.ActiveForm == this);
		}
		
		void ToolbarForm_SizeChanged(object sender, EventArgs e)
		{
			if(this.Initialized && UsingToolbarItem.ToolbarPosition == ToolbarPosition.DesktopFloat) {
				UsingToolbarItem.FloatSize = Size;
			}
		}
		void ToolbarForm_LocationChanged(object sender, EventArgs e)
		{
			if(this.Initialized && UsingToolbarItem.ToolbarPosition == ToolbarPosition.DesktopFloat) {
				UsingToolbarItem.FloatLocation = Location;
			}
		}
		
		void ToolLauncherDragEnter(object sender, DragEventArgs e)
		{
			ProcessDropEffect(sender, e);
		}
		
		void ToolLauncherDragOver(object sender, DragEventArgs e)
		{
			ProcessDropEffect(sender, e);
		}
		
		void ToolLauncherDragDrop(object sender, DragEventArgs e)
		{
			var dropData = ProcessDropEffect(sender, e);
			if(dropData.DropType == DropType.Files) {
				ExecuteDropData(dropData);
			} else {
				Debug.Assert(dropData.DropType == DropType.Button);
				ChangeDropDataLauncherItemPosition(dropData);
				this._dragStartItem = null;
			}
		}
		
		void ToolbarFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if(e.CloseReason == CloseReason.UserClosing) {
				e.Cancel = true;
				Visible = false;
			}
		}
		
		
		void ToolbarFormShown(object sender, EventArgs e)
		{
			// この子のおかげでちかちかする。でも実装してるからなんか理由あのかもしれんけど調べる気にもならん。
			ApplySettingPosition();
		}
		
		void ToolbarForm_Activated(object sender, EventArgs e)
		{
			DrawFullActivaChanged(true);
		}
		
		void ToolbarForm_Deactivate(object sender, EventArgs e)
		{
			DrawFullActivaChanged(false);
		}
		
		void OpeningRootMenu(object sender, EventArgs e)
		{
			Cursor = Cursors.Default;
			
			this._menuOpening = true;
			this._tipsLauncher.Hide();
			var toolItem = sender as ToolStripDropDownItem;
			if(toolItem != null) {
				switch(UsingToolbarItem.ToolbarPosition) {
					case ToolbarPosition.DesktopFloat:
						toolItem.DropDownDirection = ToolStripDropDownDirection.Default;
						break;
						
					case ToolbarPosition.DesktopTop:
						toolItem.DropDownDirection = ToolStripDropDownDirection.Default;
						break;
						
					case ToolbarPosition.DesktopBottom:
						toolItem.DropDownDirection = ToolStripDropDownDirection.Default;
						break;
						
					case ToolbarPosition.DesktopLeft:
						toolItem.DropDownDirection = ToolStripDropDownDirection.Right;
						break;
						
					case ToolbarPosition.DesktopRight:
						toolItem.DropDownDirection = ToolStripDropDownDirection.Left;
						break;
						
					default:
						throw new NotImplementedException();
				}
			}
		}
		
		void CloseRootMenu(object sender, EventArgs e)
		{
			this._menuOpening = false;
			SwitchHidden();
		}
		
		void ToolItem_MouseHover(object sender, EventArgs e)
		{
			var toolItem = (ToolStripItem)sender;

			if(this._menuOpening) {
				// メニュー表示中はなんもしない
				return;
			}
			this._tipsLauncher.ShowItem(DockScreen, toolItem, SelectedGroupItem, UsingToolbarItem);
		}

		void toolItem_MouseLeave(object sender, EventArgs e)
		{
			this._tipsLauncher.HideItem();
		}
		
		void ToolbarForm_AppbarFullScreen(object sender, AppbarFullScreenEvent e)
		{
			if(e.FullScreen) {
				TopMost = false;
				NativeMethods.SetWindowPos(Handle, (IntPtr)HWND.HWND_BOTTOM, 0, 0, 0, 0, SWP.SWP_NOMOVE | SWP.SWP_NOSIZE | SWP.SWP_NOACTIVATE);
			} else {
				ApplySettingTopmost();
			}
		}
		
		void LauncherButton_MouseDown(object sender, MouseEventArgs e)
		{
			if(Control.ModifierKeys == Keys.Alt) {
				this._dragStartItem = (ToolStripItem)sender;
				this.toolLauncher.DoDragDrop(sender, DragDropEffects.Move);
			} else if(e.Button == System.Windows.Forms.MouseButtons.Middle) {
				// #148
				var toolItem = (ToolStripItem)sender;
				var launcherItem = toolItem.Tag as LauncherItem;
				if(launcherItem == null) {
					return;
				}
				var menuTypes = new [] {
					LauncherType.File,
					LauncherType.Directory,
					LauncherType.Embedded
				};
				if(menuTypes.Any(l => l == launcherItem.LauncherType)) {
					var menuItem = (ToolStripDropDownItem)toolItem;
					menuItem.ShowDropDown();
				}
			}
		}

		#region File Launcher Menu

		void FileLauncherItemPathMenu_OpenParentDirectory(object sender, EventArgs e)
		{
			var menuItem = (LauncherToolStripMenuItem)sender;
			OpenParentDirectory(menuItem.LauncherItem);
		}

		void FileLauncherItemPathMenu_OpenWorkDirectory(object sender, EventArgs e)
		{
			var menuItem = (LauncherToolStripMenuItem)sender;
			OpenDirectory(menuItem.LauncherItem.WorkDirPath);
		}

		void FileLauncherItemPathMenu_CopyCommand(object sender, EventArgs e)
		{
			var menuItem = (LauncherToolStripMenuItem)sender;
			CopyText(menuItem.LauncherItem.Command);
		}

		void FileLauncherItemPathMenu_CopyParentDirectory(object sender, EventArgs e)
		{
			var menuItem = (LauncherToolStripMenuItem)sender;
			CopyText(Path.GetDirectoryName(menuItem.LauncherItem.Command));
		}

		void FileLauncherItemPathMenu_CopyWorkDirectory(object sender, EventArgs e)
		{
			var menuItem = (LauncherToolStripMenuItem)sender;
			CopyText(menuItem.LauncherItem.WorkDirPath);
		}

		void FileLauncherItemPathMenu_OpenProperty(object sender, EventArgs e)
		{
			var menuItem = (LauncherToolStripMenuItem)sender;
			var expandPath = Environment.ExpandEnvironmentVariables(menuItem.LauncherItem.Command);

			Executor.OpenProperty(expandPath, Handle);
		}

		void FileLauncherItemPathMenu_DropDownOpening(object sender, EventArgs e)
		{
			var menuItem = (LauncherToolStripMenuItem)sender;
			var launcherItem = menuItem.LauncherItem;

			var openParentDirItem = (ToolStripItem)menuItem.DropDownItems[menuNamePath_openParentDir];
			var openWorkDirItem = (ToolStripItem)menuItem.DropDownItems[menuNamePath_openWorkDir];
			var copyCommandItem = (ToolStripItem)menuItem.DropDownItems[menuNamePath_copyCommand];
			var copyParentDirItem = (ToolStripItem)menuItem.DropDownItems[menuNamePath_copyParentDir];
			var copyWorkDirItem = (ToolStripItem)menuItem.DropDownItems[menuNamePath_copyWorkDir];
			var propertyItem = (ToolStripItem)menuItem.DropDownItems[menuNamePath_property];

			// コマンド有無
			var commandEnabled = launcherItem.IsExists;
			copyCommandItem.Enabled = commandEnabled;
			propertyItem.Enabled = commandEnabled;
			// 親ディレクトリ有無
			var parentDirPath = Path.GetDirectoryName(Environment.ExpandEnvironmentVariables(launcherItem.Command));
			var parentDirEnabled = !string.IsNullOrEmpty(parentDirPath) && Path.GetPathRoot(parentDirPath) != parentDirPath && Directory.Exists(parentDirPath);
			openParentDirItem.Enabled = parentDirEnabled;
			copyParentDirItem.Enabled = parentDirEnabled;
			// 作業ディレクトリ有無
			var workDirEnabled = !string.IsNullOrEmpty(launcherItem.WorkDirPath) && Directory.Exists(Environment.ExpandEnvironmentVariables(launcherItem.WorkDirPath));
			openWorkDirItem.Enabled = workDirEnabled;
			copyWorkDirItem.Enabled = workDirEnabled;
		}

		void FileLauncherItemMenu_Execute(object sender, EventArgs e)
		{
			var menuItem = (LauncherToolStripMenuItem)sender;
			ExecuteItem(menuItem.LauncherItem);
		}

		void FileLauncherItemMenu_ExecuteEx(object sender, EventArgs e)
		{
			var menuItem = (LauncherToolStripMenuItem)sender;
			ExecuteExItem(menuItem.LauncherItem, null);
		}

		void FileLauncherItemMenu_DropDownOpening(object sender, EventArgs e)
		{
			var menuItem = (LauncherToolStripSplitButton)sender;
			var launcherItem = menuItem.LauncherItem;

			var executeItem = (ToolStripMenuItem)menuItem.DropDownItems[menuNameExecute];
			var executeExItem = (ToolStripMenuItem)menuItem.DropDownItems[menuNameExecuteEx];
			var pathItem = (ToolStripMenuItem)menuItem.DropDownItems[menuNamePath];
			var fileItem = (ToolStripMenuItem)menuItem.DropDownItems[menuNameFiles];

			if(launcherItem.IsExists) {
					executeItem.Enabled = true;
					//executeExItem.Enabled = launcherItem.IsExecteFile;
				} else {
					executeItem.Enabled = false;
					//executeExItem.Enabled = false;
				}
				try {
					var expandPath = Environment.ExpandEnvironmentVariables(launcherItem.Command);
					var expandParentPath = Path.GetDirectoryName(expandPath);
					fileItem.Enabled = Directory.Exists(expandParentPath);
				} catch(ArgumentException ex) {
					// #41の影響により#77考慮不要
					CommonData.Logger.Puts(LogType.Information, CommonData.Language["toolbar/loging/unfile"], ex);
					pathItem.Enabled = false;
					fileItem.Enabled = false;
					executeItem.Enabled = true;
				}
				try {
					if(!fileItem.HasDropDownItems) {
						var showHiddenFile = SystemEnvironment.IsHiddenFileShow();
						var showExtension = SystemEnvironment.IsExtensionShow();
						var parentDirPath = Path.GetDirectoryName(Environment.ExpandEnvironmentVariables(launcherItem.Command));
						AttachmentFileList(fileItem, false, parentDirPath, showHiddenFile, showExtension);
					}
				} catch(Exception ex) {
					CommonData.Logger.Puts(LogType.Warning, ex.Message, ex);
				}
		}

		#endregion

		#region File List Menu

		void FileListMenu_DropDownOpening(object sender, EventArgs e)
		{
			var toolItem = (ToolStripDropDownItem)sender;
			var openItem = toolItem.DropDownItems[menuNameFiles_open];
			if(openItem.Image == null) {
				toolItem.DropDownItems[menuNameFiles_open].Image = toolItem.Image;
			}

			// 下位ファイル群が生成済みであればサヨナラ
			var skip = false;
			if(toolItem.DropDownItems.ContainsKey(menuNameFiles_sep)) {
				var separatorItem = toolItem.DropDownItems[menuNameFiles_sep];
				skip = toolItem.DropDownItems.Cast<ToolStripItem>().SkipWhile(t => t != separatorItem).Skip(1).Any();
			} else {
				skip = toolItem.DropDownItems.Count > 0;
			}

			if(skip) {
				CommonData.Logger.PutsDebug("opening: skip", () => string.Join(Environment.NewLine, toolItem.DropDownItems.Cast<ToolStripItem>().Select(t => t.Text)));
				return;
			}

			// 下位ファイル群生成
			var fileMenuItem = toolItem as FileToolStripMenuItem;
			if(fileMenuItem != null) {
				CommonData.Logger.PutsDebug(fileMenuItem.Text, () => fileMenuItem.DropDownItems.Count);
				var showHiddenFile = SystemEnvironment.IsHiddenFileShow();
				var showExtension = SystemEnvironment.IsExtensionShow();
				AttachmentFileList(fileMenuItem, false, Environment.ExpandEnvironmentVariables(fileMenuItem.Path), showHiddenFile, showExtension);
			} else {
				// 起こりえないけど改修実装なんで入れとく
				CommonData.Logger.PutsDebug("cast error: FileToolStripMenuItem", () => toolItem.DumpToString(toolItem.Text));
			}
		}

		void FileListMenu_DropDownClosed(object sender, EventArgs e)
		{
			var toolItem = (ToolStripDropDownItem)sender;
			toolItem.DropDownItems[menuNameFiles_open].Image = null;
		}

		void FileListMenu_Click(object sender, EventArgs e)
		{
			var fileMenuItem = sender as FileToolStripMenuItem;
			if(fileMenuItem != null) {
				try {
					var path = fileMenuItem.Path;
					if(File.Exists(path)) {
						Executor.OpenFile(path, CommonData);
					} else {
						Executor.OpenDirectory(path, CommonData, null);
					}
				} catch(Exception ex) {
					CommonData.Logger.Puts(LogType.Warning, ex.Message, ex);
				}
			} else {
				// 起こりえないけど改修実装なんで入れとく
				CommonData.Logger.PutsDebug("cast error: FileToolStripMenuItem", () => sender.DumpToString(sender.GetType().ToString()));
			}
		}

		#endregion
	}
}
