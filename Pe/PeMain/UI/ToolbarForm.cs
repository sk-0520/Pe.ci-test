using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Skin;
using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	/// <summary>
	/// ツールバー。
	/// </summary>
	public partial class ToolbarForm : AppbarForm, ISetCommonData
	{
		#region define
		const string menuNameMainPosDesktopFloat = "desktop_float";
		const string menuNameMainPosDesktopTop = "desktop_top";
		const string menuNameMainPosDesktopBottom = "desktop_bottom";
		const string menuNameMainPosDesktopLeft = "desktop_left";
		const string menuNameMainPosDesktopRight = "desktop_right";
		const string menuNameMainTopmost = "topmost";
		const string menuNameMainAutoHide = "autohide";
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

		const string menuNameApplicationExecute = "execute";
		const string menuNameApplicationClose = "close";
		const string menuNameApplicationHelp = "help";

		enum DropType
		{
			None,
			Files,
			Button
		}

		struct DropData
		{
			public DropType DropType { get; set; }
			public ToolStripItem ToolStripItem { get; set; }
			public LauncherItem LauncherItem { get; set; }
			public IEnumerable<string> Files { get; set; }
			public ToolStripItem SrcToolStripItem { get; set; }
		}

		/// <summary>
		/// TODO: スキンと内部描画が入り混じっている。描画処理は整理出来たら全部スキンに回す。
		/// </summary>
		public partial class CustomToolTipForm: Form, ISetCommonData
		{
			CommonData CommonData { get; set; }

			FontSetting TitleFontSetting { get; set; }
			FontSetting MessageFontSetting { get; set; }
			IconScale IconScale { get; set; }
			Size TipPadding { get; set; }

			string _title, _message;
			Image _imageIcon;

			float _titleHeight;

			//enum FadeState
			//{
			//	None,
			//	In,
			//	Out,
			//}
			//FadeState _fadeState;

			public CustomToolTipForm()
			{
				//Opacity = 0;
				Visible = false;
				FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
				ShowInTaskbar = false;
				TopMost = true;
				Padding = new Padding(3);

				BackColor = SystemColors.Info;
				ForeColor = SystemColors.InfoText;

				//this._fadeState = FadeState.None;

				TipPadding = new Size(4, 4);
				TitleFontSetting = new FontSetting(SystemFonts.MessageBoxFont);
				MessageFontSetting = new FontSetting(SystemFonts.SmallCaptionFont);
				IconScale = IconScale.Normal;
			}

			protected override void Dispose(bool disposing)
			{
				_imageIcon.ToDispose();

				base.Dispose(disposing);
			}

			protected override bool ShowWithoutActivation { get { return true; } }

			protected override CreateParams CreateParams
			{
				get
				{
					var result = base.CreateParams;

					result.ExStyle |= (int)(WS_EX.WS_EX_NOACTIVATE | WS_EX.WS_EX_TOOLWINDOW);
					result.ClassStyle |= (int)CS.CS_DROPSHADOW;

					return result;
				}
			}

			void ApplyLanguage()
			{ }

			void ApplySetting()
			{
				ApplyLanguage();
			}

			public void SetCommonData(CommonData commonData)
			{
				CommonData = commonData;

				ApplySetting();
			}

			bool HasMessage()
			{
				return !string.IsNullOrWhiteSpace(this._message);
			}

			StringFormat CreateTitleFormat()
			{
				var sf = new StringFormat();

				sf.Alignment = StringAlignment.Near;
				sf.LineAlignment = StringAlignment.Center;
				sf.Trimming = StringTrimming.EllipsisCharacter;

				return sf;
			}
			StringFormat CreateMessageFormat()
			{
				var sf = new StringFormat();

				sf.Alignment = StringAlignment.Near;
				sf.LineAlignment = StringAlignment.Near;
				sf.Trimming = StringTrimming.EllipsisCharacter;

				return sf;
			}

			protected override void OnPaintBackground(PaintEventArgs e)
			{
				if(CommonData != null && CommonData.Skin != null) {
					if(CommonData.Skin.IsDefaultDrawToolbarToolTipBackground) {
						base.OnPaintBackground(e);
						//e.Graphics.FillEllipse(SystemBrushes.InfoText, e.ClipRectangle);
					} else {
						CommonData.Skin.DrawToolbarToolTipBackground(e.Graphics, e.ClipRectangle);
					}
				} else {
					base.OnPaintBackground(e);
				}
			}

			protected override void OnPaint(PaintEventArgs e)
			{
				var iconTop = this._titleHeight / 2 - IconScale.ToHeight() / 2;
				e.Graphics.DrawImage(this._imageIcon, new Point(Padding.Left, Padding.Top + (int)iconTop));
				var iconWidth = Padding.Left + IconScale.ToWidth() + Padding.Left;
				var titleArea = new Rectangle(iconWidth, Padding.Top, Width - iconWidth, (int)this._titleHeight);
				var messageArea = new Rectangle(Padding.Left, titleArea.Bottom, Width - Padding.Horizontal, Height - titleArea.Height - Padding.Vertical);
				using(var sf = CreateTitleFormat())
				using(var brush = new SolidBrush(Color.Black)) {
					e.Graphics.DrawString(this._title, TitleFontSetting.Font, brush, titleArea, sf);
				}
				if(HasMessage()) {
					using(var sf = CreateTitleFormat())
					using(var brush = new SolidBrush(Color.Black)) {
						e.Graphics.DrawString(this._message, MessageFontSetting.Font, brush, messageArea, sf);
					}
				}
			}

			void ToShow()
			{
				WindowsUtility.ShowNoActive(this);
			}

			void ToHide()
			{
				Visible = false;
			}

			public void HideItem()
			{
				ToHide();
			}

			public void ShowItem(Screen screen, ToolStripItem toolStripItem, ToolbarGroupItem groupItem, ToolbarItem toolbarItem)
			{
				Debug.Assert(toolStripItem != null);
				Debug.Assert(CommonData != null);

				var launcherItem = toolStripItem.Tag as LauncherItem;

				if(launcherItem != null) {
					this._imageIcon = launcherItem.GetIcon(IconScale.Normal, launcherItem.IconItem.Index, CommonData.ApplicationSetting).ToBitmap();
					this._title = launcherItem.Name;
					if(launcherItem.LauncherType == LauncherType.Embedded) {
						var applicationItem = CommonData.ApplicationSetting.GetApplicationItem(launcherItem);
						this._message = LanguageUtility.ApplicationItemToComment(CommonData.Language, applicationItem);
					} else {
						this._message = launcherItem.Note;
					}
				} else {
					this._imageIcon = IconUtility.ImageFromIcon(CommonData.Skin.GetIcon(SkinIcon.App), IconScale.Normal);
					this._title = CommonData.Language["toolbar/main/tips", new Dictionary<string, string>() { { AppLanguageName.groupName, groupItem.Name } }];
					this._message = string.Empty;
				}

				// 描画サイズ生成
				using(var g = CreateGraphics())
				using(var titleFormat = CreateTitleFormat())
				using(var messageFormat = CreateTitleFormat()) {
					var maxShowSize = new Size(screen.WorkingArea.Size.Width / 2, screen.WorkingArea.Size.Height / 2);
					var titleSize = g.MeasureString(this._title, TitleFontSetting.Font, maxShowSize, titleFormat);
					var messageSize = HasMessage() ? g.MeasureString(this._message, MessageFontSetting.Font, maxShowSize, messageFormat) : SizeF.Empty;

					this._titleHeight = Math.Max((float)IconScale.ToHeight(), titleSize.Height) + (float)(HasMessage() ? Padding.Top : 0);

					Size = new Size(
						(int)(Math.Max(titleSize.Width, messageSize.Width) + IconScale.Normal.ToWidth() + Padding.Left) + Padding.Horizontal,
						(int)(Math.Max(IconScale.ToHeight(), titleSize.Height) + messageSize.Height) + Padding.Vertical
					);
				}

				CommonData.Skin.ApplyToolbarToolTipRegion(this);

				// 表示位置設定
				var itemArea = toolStripItem.Bounds;
				if(toolStripItem.OwnerItem != null) {
					var ownerItemLocation = toolStripItem.OwnerItem.Bounds.Location;
					itemArea.Offset(ownerItemLocation);
				}
				var screenPoint = toolStripItem.Owner.PointToScreen(itemArea.Location);
				switch(toolbarItem.ToolbarPosition) {
					case ToolbarPosition.DesktopFloat:
					case ToolbarPosition.DesktopTop:
						// 下に表示
						Location = new Point(screenPoint.X, screenPoint.Y + itemArea.Height + TipPadding.Height);
						if(toolbarItem.ToolbarPosition == ToolbarPosition.DesktopFloat) {
							if(Location.Y + Size.Height > screen.WorkingArea.Height) {
								goto LABEL_TOP;
							}
						}
						break;

					case ToolbarPosition.DesktopBottom:
					LABEL_TOP:
						// 上に表示
						Location = new Point(screenPoint.X, screenPoint.Y - Height - TipPadding.Height);
						break;

					case ToolbarPosition.DesktopLeft:
						// 右に表示
						Location = new Point(screenPoint.X + itemArea.Width + TipPadding.Width, screenPoint.Y);
						break;

					case ToolbarPosition.DesktopRight:
						// 左に表示
						Location = new Point(screenPoint.X - Width - TipPadding.Width, screenPoint.Y);
						break;

					default:
						Debug.Assert(false, toolbarItem.ToolbarPosition.ToString());
						throw new NotImplementedException();
				}

				ToShow();
			}
		}

		#endregion ////////////////////////////////////

		#region static
		#endregion ////////////////////////////////////

		#region variable
		ContextMenu _menuGroup = null;
		bool _isRunning = false;
		bool _menuOpening = false;

		ToolStripItem _dragStartItem;
		CustomToolTipForm _tipsLauncher;
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
		CommonData CommonData { get; set; }

		ToolbarGroupItem SelectedGroupItem { get; set; }
		public ToolbarItem UseToolbarItem { get; private set; }

		override public DesktopDockType DesktopDockType
		{
			get { return base.DesktopDockType; }
			set
			{
				if(CommonData != null) {
					var pos = UseToolbarItem.ToolbarPosition;

					Padding = CommonData.Skin.GetToolbarTotalPadding(UseToolbarItem.ToolbarPosition, Size);
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
		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;
			this._isRunning = false;

			this._tipsLauncher.SetCommonData(CommonData);
			ApplySetting();

			this._isRunning = true;
		}
		#endregion ////////////////////////////////////

		#region override

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			//pevent.Graphics.Clear()
			if(CommonData.Skin.IsDefaultDrawToolbarWindowBackground) {
				base.OnPaintBackground(e);
			} else {
				CommonData.Skin.DrawToolbarWindowBackground(e.Graphics, e.ClipRectangle, this == Form.ActiveForm, UseToolbarItem.ToolbarPosition);
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
			if(UseToolbarItem.ToolbarPosition == ToolbarPosition.DesktopFloat) {
				switch(m.Msg) {
					case (int)WM.WM_SYSCOMMAND: {
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

					case (int)WM.WM_NCPAINT: {
							if(CommonData != null) {
								var hDC = NativeMethods.GetWindowDC(Handle);
								try {
									using(var g = Graphics.FromHdc(hDC)) {
										DrawNoClient(g, new Rectangle(Point.Empty, Size), this == Form.ActiveForm);
									}
								} finally {
									NativeMethods.ReleaseDC(Handle, hDC);
								}
							}
						}
						break;

					case (int)WM.WM_NCHITTEST: {

							var point = PointToClient(WindowsUtility.ScreenPointFromLParam(m.LParam));
							var padding = Padding;

							var hitTest = HT.HTNOWHERE;
							var captionArea = CommonData.Skin.GetToolbarCaptionArea(UseToolbarItem.ToolbarPosition, Size);
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

					case (int)WM.WM_SETCURSOR: {
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

					case (int)WM.WM_MOVING: {
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

					case (int)WM.WM_DWMCOMPOSITIONCHANGED: {
							CommonData.Skin.RefreshStyle(this);
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
		void ApplyLanguage()
		{
			Debug.Assert(CommonData != null);
			Debug.Assert(CommonData.Language != null);

			UIUtility.SetDefaultText(this, CommonData.Language);
		}
		#endregion ////////////////////////////////////

		#region function
		void ApplySettingTopmost()
		{
			TopMost = UseToolbarItem.Topmost;
		}
		
		void ApplySkin()
		{
			var renderer = new ToolbarRenderer();
			renderer.Skin = CommonData.Skin;
			renderer.ToolbarItem = UseToolbarItem;
			
			this.toolLauncher.Renderer = renderer;
			
			CommonData.Skin.AttachmentStyle(this);
		}
		
		void ApplySettingPosition()
		{
			/*
			if(false) {
				HiddenWaitTime = UseToolbarItem.HiddenWaitTime;
				HiddenAnimateTime = UseToolbarItem.HiddenAnimateTime;
			}
			//*/
			if(UseToolbarItem.Visible) {
				var prevOpacity = Opacity;
				Opacity = 0;
				
				SuspendLayout();
				try {
					ItemSizeToFormSize();
					
					if(ToolbarPositionUtility.IsDockingMode(UseToolbarItem.ToolbarPosition)) {
						AutoHide = UseToolbarItem.AutoHide;
					}
					
					if(ToolbarPositionUtility.IsDockingMode(UseToolbarItem.ToolbarPosition)) {
						DesktopDockType = ToolbarPositionConverter.ToDockType(UseToolbarItem.ToolbarPosition);
						if(ToolbarPositionUtility.IsHorizonMode(UseToolbarItem.ToolbarPosition)) {
							this.toolLauncher.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
						} else {
							this.toolLauncher.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
						}
					} else {
						DesktopDockType = DesktopDockType.None;
						if(UseToolbarItem.ToolbarPosition == ToolbarPosition.DesktopFloat) {
							Location = UseToolbarItem.FloatLocation;
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
			
			UseToolbarItem = null;
			foreach(var item in CommonData.MainSetting.Toolbar.Items) {
				if(item.IsNameEqual(DockScreen.DeviceName)) {
					UseToolbarItem = item;
					break;
				}
			}
			if(UseToolbarItem == null) {
				// 新規
				var toolbarItem = new ToolbarItem();
				toolbarItem.Name = DockScreen.DeviceName;
				CommonData.MainSetting.Toolbar.Items.Add(toolbarItem);
				toolbarItem.FloatLocation = DockScreen.WorkingArea.Location;
				UseToolbarItem = toolbarItem;
			}
		}

		void ApplySettingFont()
		{
			Debug.Assert(CommonData != null);
			Debug.Assert(CommonData.MainSetting != null);
			
			if(UseToolbarItem.FontSetting != null && !UseToolbarItem.FontSetting.IsDefault) {
				this.toolLauncher.Font = UseToolbarItem.FontSetting.Font;
			}
		}
		public void ApplySettingVisible()
		{
			var floatSize = UseToolbarItem.FloatSize;
			if(!Visible && UseToolbarItem.ToolbarPosition == ToolbarPosition.DesktopFloat) {
				Size = floatSize;
			}
			Visible = UseToolbarItem.Visible;
		}
		
		void ApplySetting()
		{
			Debug.Assert(CommonData != null);
			Debug.Assert(CommonData.MainSetting != null);
			
			ApplyLanguage();
			ApplyScreen();
			ApplySettingFont();
			ApplySkin();
			
			Font = UseToolbarItem.FontSetting.Font;
			if(CommonData.MainSetting.Toolbar.ToolbarGroup.Groups.Count == 0) {
				// グループが存在しなければグループを作っておく
				var toolbarGroupItem = new ToolbarGroupItem();
				toolbarGroupItem.Name = CommonData.Language["group/new"];
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
			var initGroup = CommonData.MainSetting.Toolbar.ToolbarGroup.Groups.FirstOrDefault(g => ToolbarItem.CheckNameEqual(g.Name, UseToolbarItem.DefaultGroup));
			
			SelectedGroup(initGroup ?? firstGroup);
			
			// 表示
			ApplySettingPosition();
			ApplySettingVisible();
			ApplySettingTopmost();
			
			HiddenAnimateTime = UseToolbarItem.HiddenAnimateTime;
			HiddenWaitTime = UseToolbarItem.HiddenWaitTime;
		}
		
		/// <summary>
		/// 表示タイプからウィンドウをそれっぽいサイズに変更
		/// </summary>
		void ItemSizeToFormSize()
		{
			var floatSize = UseToolbarItem.FloatSize;
			Padding = CommonData.Skin.GetToolbarTotalPadding(UseToolbarItem.ToolbarPosition, Size);
			
			var buttonLayout = CommonData.Skin.GetToolbarButtonLayout(UseToolbarItem.IconScale, UseToolbarItem.ShowText, UseToolbarItem.TextWidth);
			var edgeSize = CommonData.Skin.GetToolbarWindowEdgePadding(UseToolbarItem.ToolbarPosition);
			var borderPadding = CommonData.Skin.GetToolbarBorderPadding(UseToolbarItem.ToolbarPosition);
			this.toolLauncher.Padding = borderPadding;
			var minSize = new Size(edgeSize.Horizontal + buttonLayout.Size.Width, edgeSize.Vertical + buttonLayout.Size.Height);
			minSize.Width += this.toolLauncher.Margin.Horizontal + borderPadding.Horizontal;
			minSize.Height += this.toolLauncher.Margin.Vertical + borderPadding.Vertical;
			
			
			//Size = new Size(minSize.Width, minSize.Height);
			
			if(ToolbarPositionUtility.IsDockingMode(UseToolbarItem.ToolbarPosition)) {
				BarSize = new Size(minSize.Width, minSize.Height);
				MinimumSize = Size.Empty;
			} else {
				if(ToolbarPositionUtility.IsHorizonMode(UseToolbarItem.ToolbarPosition)) {
					Size = new Size(floatSize.Width, minSize.Height);
				} else {
					Size = new Size(minSize.Width, floatSize.Height);
				}
				MinimumSize = minSize;
			}
		}
		
		void SetToolButtons(IconScale iconScale, IEnumerable<ToolStripItem> buttons)
		{
			this.toolLauncher.ImageScalingSize = iconScale.ToSize();
			
			/*
			// アイコン解放
			var items = this.toolLauncher.Items
				.Cast<ToolStripItem>()
				.Where(item => item.Image != null)
			;
			foreach(var item in items) {
				item.Dispose();
			}
			 */
			
			this.toolLauncher.Items.Clear();
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
			SetToolButtons(UseToolbarItem.IconScale, toolButtonList);
		}

		void OpenDir(LauncherItem launcherItem)
		{
			try {
				var expandPath = Environment.ExpandEnvironmentVariables(launcherItem.Command);
				Executor.OpenDirectoryWithFileSelect(expandPath, CommonData, null);
			} catch(Exception ex) {
				CommonData.Logger.Puts(LogType.Warning, ex.Message, ex);
			}
		}

		void OpenDir(string path)
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
		
		void OpenProperty(string path)
		{
			var expandPath = Environment.ExpandEnvironmentVariables(path);
			Executor.OpenProperty(expandPath, Handle);
		}
		
		void AttachmentFileLauncherPathSubMenu(ToolStripMenuItem parentItem, LauncherItem launcherItem)
		{
			var itemList = new List<ToolStripItem>();
			
			var openParentDirItem = new ToolStripMenuItem();
			var openWorkDirItem = new ToolStripMenuItem();
			var copyCommandItem = new ToolStripMenuItem();
			var copyParentDirItem = new ToolStripMenuItem();
			var copyWorkDirItem = new ToolStripMenuItem();
			var propertyItem = new ToolStripMenuItem();
			itemList.Add(openParentDirItem);
			itemList.Add(openWorkDirItem);
			itemList.Add(new ToolStripSeparator());
			itemList.Add(copyCommandItem);
			itemList.Add(copyParentDirItem);
			itemList.Add(copyWorkDirItem);
			itemList.Add(new ToolStripSeparator());
			itemList.Add(propertyItem);
			
			// 親ディレクトリを開く
			openParentDirItem.Name = menuNamePath_openParentDir;
			openParentDirItem.Text = CommonData.Language["toolbar/menu/file/path/open-parent-dir"];
			openParentDirItem.Click += (object sender, EventArgs e) => OpenDir(launcherItem);
			// 作業ディレクトリを開く
			openWorkDirItem.Name = menuNamePath_openWorkDir;
			openWorkDirItem.Text = CommonData.Language["toolbar/menu/file/path/open-work-dir"];
			openWorkDirItem.Click += (object sender, EventArgs e) => OpenDir(launcherItem.WorkDirPath);
			// コマンドコピー
			copyCommandItem.Name = menuNamePath_copyCommand;
			copyCommandItem.Text = CommonData.Language["toolbar/menu/file/path/copy-command"];
			copyCommandItem.Click += (object sender, EventArgs e) => CopyText(launcherItem.Command);
			// 親ディレクトリをコピー
			copyParentDirItem.Name = menuNamePath_copyParentDir;
			copyParentDirItem.Text = CommonData.Language["toolbar/menu/file/path/copy-parent-dir"];
			copyParentDirItem.Click += (object sender, EventArgs e) => CopyText(Path.GetDirectoryName(launcherItem.Command));
			// 作業ディレクトリをコピー
			copyWorkDirItem.Name = menuNamePath_copyWorkDir;
			copyWorkDirItem.Text = CommonData.Language["toolbar/menu/file/path/copy-work-dir"];
			copyWorkDirItem.Click += (object sender, EventArgs e) => CopyText(launcherItem.WorkDirPath);
			// プロパティ
			propertyItem.Name = menuNamePath_property;
			propertyItem.Text = CommonData.Language["toolbar/menu/file/path/property"];
			propertyItem.Click += (object sender, EventArgs e) => OpenProperty(launcherItem.Command);
			
			// メニュー構築
			parentItem.DropDownItems.AddRange(itemList.ToArray());
			parentItem.DropDownOpening += (object sender, EventArgs e) => {
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
			};
		}
		
		ToolStripMenuItem GetFileListItem(string path, bool isDir, bool showHiddenFile, bool showExtension)
		{
			var menuItem = new ToolStripMenuItem();
			if(!isDir && !showExtension) {
				menuItem.Text = Path.GetFileNameWithoutExtension(path);
			} else {
				menuItem.Text = Path.GetFileName(path);
			}
			using(var icon = IconUtility.Load(path, UseToolbarItem.IconScale, 0)) {
				//using(var icon = IconUtility.Load(path, IconScale.Small, 0)) {
				menuItem.Image = icon.ToBitmap();
				menuItem.ImageScaling = ToolStripItemImageScaling.None;
			}
			
			// アクセス権から使用可・不可
			//if(isDir) {
			//	var access = Directory.GetAccessControl(path);
			//} else {
			//	var access = File.GetAccessControl(path);
			//}
			
			if(isDir) {
				menuItem.DropDownOpening += (object sender, EventArgs e) => LoadFileList(menuItem, path, showHiddenFile, showExtension);
			}
			
			menuItem.Click += (object sender, EventArgs e) => {
				try {
					if(File.Exists(path)) {
						Executor.OpenFile(path, CommonData);
					} else {
						Executor.OpenDirectory(path, CommonData, null);
					}
				} catch(Exception ex) {
					CommonData.Logger.Puts(LogType.Warning, ex.Message, ex);
				}
			};
			
			return menuItem;
		}
		
		bool LoadFileList(ToolStripDropDownItem parentItem, string parentDirPath, bool showHiddenFile, bool showExtension)
		{
			if(parentItem.HasDropDownItems) {
				return false;
			}
			
			if(!Directory.Exists(parentDirPath)) {
				CommonData.Logger.Puts(LogType.Warning, CommonData.Language["common/message/notfound-dir"], parentDirPath);;
				return false;
			}

			IList<ToolStripItem> menuList;
			
			try {
				var dirList = Directory.GetDirectories(parentDirPath);
				var fileList = Directory.GetFiles(parentDirPath);
				var pathItemList = new [] {
					new { PathList = dirList,  IsDirectory = true, },
					new { PathList = fileList, IsDirectory = false, },
				};
				menuList = new List<ToolStripItem>(dirList.Length + fileList.Length);
				if(dirList.Length + fileList.Length > 0) {
					/*
					foreach(var path in dirList) {
						var use = true;
						if(!showHiddenFile && (File.GetAttributes(path) & FileAttributes.Hidden) == FileAttributes.Hidden) {
							use = false;
						}
						if(use) {
							var menuItem = GetFileListItem(path, true, showHiddenFile, showExtension);
							menuList.Add(menuItem);
						}
					}
					foreach(var path in fileList) {
						var use = true;
						if(!showHiddenFile && (File.GetAttributes(path) & FileAttributes.Hidden) == FileAttributes.Hidden) {
							use = false;
						}
						if(use) {
							var menuItem = GetFileListItem(path, false, showHiddenFile, showExtension);
							menuList.Add(menuItem);
						}
					}
					 */
					foreach(var pathItem in pathItemList) {
						foreach(var path in pathItem.PathList) {
							var use = true;
							if(!showHiddenFile && (File.GetAttributes(path) & FileAttributes.Hidden) == FileAttributes.Hidden) {
								use = false;
							}
							if(use) {
								var menuItem = GetFileListItem(path, pathItem.IsDirectory, showHiddenFile, showExtension);
								menuList.Add(menuItem);
							}
						}
					}
					
				} else {
					var menuItem = new ToolStripMenuItem();
					menuItem.Text = CommonData.Language["toolbar/menu/file/ls/not-child-files"];
					menuItem.Image = SystemIcons.Information.ToBitmap();
					menuItem.Enabled = false;
					
					menuList.Add(menuItem);
				}
				
			} catch(UnauthorizedAccessException ex) {
				var menuItem = new ToolStripMenuItem();
				menuItem.Text = ex.Message;
				menuItem.Image = SystemIcons.Warning.ToBitmap();
				menuItem.Enabled = false;
				menuList = new [] { menuItem };
			}
			
			parentItem.DropDownItems.AddRange(menuList.ToArray());
			
			ToolStripUtility.AttachmentOpeningMenuInScreen(parentItem);
			parentItem.ShowDropDown();
			return true;
		}
		
		void AttachmentFileLauncherMenu(ToolStripDropDownItem parentItem, LauncherItem launcherItem)
		{
			var menuList = new List<ToolStripItem>();
			
			var executeItem = new ToolStripMenuItem();
			var executeExItem = new ToolStripMenuItem();
			var pathItem = new ToolStripMenuItem();
			var fileItem = new ToolStripMenuItem();
			menuList.Add(executeItem);
			menuList.Add(executeExItem);
			menuList.Add(new ToolStripSeparator());
			menuList.Add(pathItem);
			menuList.Add(fileItem);
			
			// 通常実行
			executeItem.Name = menuNameExecute;
			executeItem.Text = CommonData.Language["toolbar/menu/file/execute"];
			executeItem.Click += (object sender, EventArgs e) => ExecuteItem(launcherItem);
			// 指定実行
			executeExItem.Name = menuNameExecuteEx;
			executeExItem.Text = CommonData.Language["toolbar/menu/file/execute-ex"];
			executeExItem.Click += (object sender, EventArgs e) => ExecuteExItem(launcherItem, null);
			// パス関係
			pathItem.Name = menuNamePath;
			pathItem.Text = CommonData.Language["toolbar/menu/file/path"];
			AttachmentFileLauncherPathSubMenu(pathItem, launcherItem);
			//pathItem.DropDownItems.AddRange(CreateFileLauncherMenuPathItems(launcherItem));
			// ファイル一覧
			fileItem.Name = menuNameFiles;
			fileItem.Text = CommonData.Language["toolbar/menu/file/ls"];
			fileItem.DropDownOpening += (object sender, EventArgs e) => {
				var showHiddenFile = SystemEnvironment.IsHiddenFileShow();
				var showExtension = SystemEnvironment.IsExtensionShow();
				var expandPath = Environment.ExpandEnvironmentVariables(launcherItem.Command);
				LoadFileList(fileItem, Path.GetDirectoryName(expandPath), showHiddenFile, showExtension);
			};
			ToolStripUtility.AttachmentOpeningMenuInScreen(fileItem);
			
			// メニュー設定
			var menuItems = menuList.ToArray();
			ToolStripUtility.AttachmentOpeningMenuInScreen(menuItems);
			parentItem.DropDownItems.AddRange(menuItems);
			
			parentItem.DropDownOpening += (object sender, EventArgs e) => {
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
			};
		}

		string MakeGroupItemName(string groupName)
		{
			return menuNameMainGroupItem + groupName;
		}
		
		void AttachmentToolbarMenu(ToolStripDropDownItem parentItem)
		{
			var itemList = new List<ToolStripItem>();
			
			var posFloatItem = new ToolStripMenuItem();
			var posTopItem = new ToolStripMenuItem();
			var posBottomItem = new ToolStripMenuItem();
			var posLeftItem = new ToolStripMenuItem();
			var posRightItem = new ToolStripMenuItem();
			var topmostItem = new ToolStripMenuItem();
			var autoHideItem = new ToolStripMenuItem();
			var hiddenItem = new ToolStripMenuItem();
			var groupSeparator = new ToolStripSeparator();
			itemList.Add(posFloatItem);
			itemList.Add(posTopItem);
			itemList.Add(posBottomItem);
			itemList.Add(posLeftItem);
			itemList.Add(posRightItem);
			itemList.Add(new ToolStripSeparator());
			itemList.Add(topmostItem);
			itemList.Add(autoHideItem);
			itemList.Add(new ToolStripSeparator());
			itemList.Add(hiddenItem);
			
			// フロート
			posFloatItem.Name = menuNameMainPosDesktopFloat;
			posFloatItem.Text = ToolbarPosition.DesktopFloat.ToText(CommonData.Language);
			posFloatItem.Click += (object sender, EventArgs e) => {
				UseToolbarItem.ToolbarPosition = ToolbarPosition.DesktopFloat;
				ApplySettingPosition();
			};
			// デスクトップ：上
			posTopItem.Name = menuNameMainPosDesktopTop;
			posTopItem.Text = ToolbarPosition.DesktopTop.ToText(CommonData.Language);
			posTopItem.Click += (object sender, EventArgs e) => {
				UseToolbarItem.ToolbarPosition = ToolbarPosition.DesktopTop;
				ApplySettingPosition();
			};
			// デスクトップ：下
			posBottomItem.Name = menuNameMainPosDesktopBottom;
			posBottomItem.Text = ToolbarPosition.DesktopBottom.ToText(CommonData.Language);
			posBottomItem.Click += (object sender, EventArgs e) => {
				UseToolbarItem.ToolbarPosition = ToolbarPosition.DesktopBottom;
				ApplySettingPosition();
			};
			// デスクトップ：左
			posLeftItem.Name = menuNameMainPosDesktopLeft;
			posLeftItem.Text = ToolbarPosition.DesktopLeft.ToText(CommonData.Language);
			posLeftItem.Click += (object sender, EventArgs e) => {
				UseToolbarItem.ToolbarPosition = ToolbarPosition.DesktopLeft;
				ApplySettingPosition();
			};
			// デスクトップ：右
			posRightItem.Name = menuNameMainPosDesktopRight;
			posRightItem.Text = ToolbarPosition.DesktopRight.ToText(CommonData.Language);
			posRightItem.Click += (object sender, EventArgs e) => {
				UseToolbarItem.ToolbarPosition = ToolbarPosition.DesktopRight;
				ApplySettingPosition();
			};
			
			// 最前面表示
			topmostItem.Name = menuNameMainTopmost;
			topmostItem.Text = CommonData.Language["common/menu/topmost"];
			topmostItem.Click += (object sender, EventArgs e) => {
				UseToolbarItem.Topmost = !topmostItem.Checked;
				ApplySettingTopmost();
			};
			
			// 自動的に隠す
			autoHideItem.Name = menuNameMainAutoHide;
			autoHideItem.Text = CommonData.Language["toolbar/menu/main/auto-hide"];
			autoHideItem.Click += (object sender, EventArgs e) => {
				UseToolbarItem.AutoHide = !autoHideItem.Checked;
				ApplySettingPosition();
				if(DesktopDockType != DesktopDockType.None) {
					UseToolbarItem.AutoHide = AutoHide;
				} else {
					UseToolbarItem.AutoHide = false;
				}
			};
			hiddenItem.Text = CommonData.Language["toolbar/menu/main/hidden"];
			hiddenItem.Click += (object sender, EventArgs e) => {
				UseToolbarItem.Visible = false;
				ApplySettingVisible();
			};
			
			// グループ関連メニュー
			var itemGroupSeparator = new ToolStripSeparator();
			groupSeparator.Name = menuNameMainGroupSeparator;
			itemList.Add(itemGroupSeparator);
			foreach(var group in CommonData.MainSetting.Toolbar.ToolbarGroup.Groups) {
				var itemGroup = new ToolStripMenuItem();
				itemGroup.Text = group.Name;
				itemGroup.Name = MakeGroupItemName(group.Name);
				itemGroup.Tag = group;
				itemGroup.CheckState = CheckState.Indeterminate;
				itemGroup.Click += (object sender, EventArgs e) => SelectedGroup(group);
				itemList.Add(itemGroup);
			}
			
			// メニュー設定
			var items = itemList.ToArray();
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
					
					pair.Key.Checked = UseToolbarItem.ToolbarPosition == pair.Value;
				}
				
				// 最前面表示
				topmostItem.Checked = UseToolbarItem.Topmost;
				
				// 自動的に隠す
				autoHideItem.Checked = AutoHide;
				autoHideItem.Enabled = IsDocking;
				
				// グループ
				foreach(var groupItem in parentItem.DropDownItems.Cast<ToolStripItem>().Where(i => i.Name.StartsWith(menuNameMainGroupItem, StringComparison.Ordinal)).Cast<ToolStripMenuItem>()) {
					groupItem.Checked = groupItem.Tag == SelectedGroupItem;
				}
			};
		}

		void AttachmentEmbeddedLauncherMenu(ToolStripDropDownItem parentItem, LauncherItem launcherItem)
		{
			var itemList = new List<ToolStripItem>();

			var execItem = new ToolStripMenuItem();
			var closeItem = new ToolStripMenuItem();
			var helpItem = new ToolStripMenuItem();
			itemList.Add(execItem);
			itemList.Add(closeItem);
			itemList.Add(new ToolStripSeparator());
			itemList.Add(helpItem);

			// 起動
			execItem.Name = menuNameApplicationExecute;
			execItem.Text = CommonData.Language["toolbar/menu/application/execute"];
			execItem.Click += (object sender, EventArgs e) => {
				ExecuteItem(launcherItem);
			};
			// 終了
			closeItem.Name = menuNameApplicationClose;
			closeItem.Text = CommonData.Language["toolbar/menu/application/close"];
			closeItem.Click += (object sender, EventArgs e) => {
				try {
					CommonData.ApplicationSetting.KillApplicationItem(launcherItem);
				} catch(Exception ex) {
					var message = string.Format("{0} - {1}", launcherItem.Name, launcherItem.Command);
					CommonData.Logger.Puts(LogType.Warning, message, ex);
				}
			};
			// ヘルプ
			helpItem.Name = menuNameApplicationHelp;
			helpItem.Text = CommonData.Language["toolbar/menu/application/help"];
			helpItem.Click += (object sender, EventArgs e) => {
				var applicationItem = CommonData.ApplicationSetting.GetApplicationItem(launcherItem);
				try {
					Executor.RunCommand(applicationItem.HelpPath, CommonData);
				} catch(Exception ex) {
					var message = string.Format("{0} - {1}", launcherItem.Name, launcherItem.Command);
					CommonData.Logger.Puts(LogType.Warning, ex.Message, applicationItem.HelpPath);
				}
			};

			parentItem.DropDownItems.AddRange(itemList.ToArray());
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
			var iconSize = UseToolbarItem.IconScale.ToSize();
			var toolItem = new ToolStripDropDownButton();
			using(var icon = new Icon(CommonData.Skin.GetIcon(SkinIcon.ToolbarMain), iconSize)) {
				var img = new Bitmap(iconSize.Width, iconSize.Height);
				using(var g = Graphics.FromImage(img)) {
					g.DrawIcon(icon, new Rectangle(Point.Empty, UseToolbarItem.IconScale.ToSize()));
					#if DEBUG
					DrawUtility.MarkingDebug(g, new Rectangle(Point.Empty, UseToolbarItem.IconScale.ToSize()));
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
			var toolItem = new ToolStripSplitButton();
			toolItem.ButtonClick += LauncherTypeFile_ButtonClick;
			
			AttachmentFileLauncherMenu(toolItem, item);
			
			return toolItem;
		}
		
		ToolStripDropDownButton CreateDirectoryItemLauncherButton(LauncherItem item)
		{
			var toolItem = new ToolStripDropDownButton();
			
			toolItem.DropDownOpening += (object sender, EventArgs e) => {
				var showHiddenFile = SystemEnvironment.IsHiddenFileShow();
				var showExtension = SystemEnvironment.IsExtensionShow();
				var expandPath = Environment.ExpandEnvironmentVariables(item.Command);
				if(LoadFileList(toolItem, expandPath, showHiddenFile, showExtension)) {
					var openItem = new ToolStripMenuItem();
					openItem.Text = CommonData.Language["toolbar/menu/file/ls/open"];
					openItem.Image = toolItem.Image;
					openItem.Click += (object child_sender, EventArgs child_e) => ExecuteItem(item);
					toolItem.DropDownItems.Insert(0, openItem);
					toolItem.DropDownItems.Insert(1, new ToolStripSeparator());
				}
			};

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
			var icon = item.GetIcon(UseToolbarItem.IconScale, item.IconItem.Index, CommonData.ApplicationSetting);
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
			
			SetButtonLayout(toolItem, CommonData.Skin, UseToolbarItem.IconScale, UseToolbarItem.ShowText, UseToolbarItem.TextWidth);
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
								var sf = new ShortcutFile(path, false);
								var expandPath = Environment.ExpandEnvironmentVariables(sf.TargetPath);
								checkDirectory = Directory.Exists(expandPath);
								path = sf.TargetPath;
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
				CommonData.RootSender.ChangeLauncherGroupItems(UseToolbarItem, SelectedGroupItem);
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
					var arrow = ToolbarPositionUtility.IsHorizonMode(UseToolbarItem.ToolbarPosition) ? ArrowDirection.Right: ArrowDirection.Down;
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
			Debug.Assert(toolbarItem != UseToolbarItem);
			SelectedGroup(SelectedGroupItem);
		}
		

		#endregion ////////////////////////////////////

		#region draw
		void DrawEdge(Graphics g, Rectangle drawArea, bool active)
		{
			if(CommonData.Skin.IsDefaultDrawToolbarWindowEdge) {
				var edgePadding = CommonData.Skin.GetToolbarWindowEdgePadding(UseToolbarItem.ToolbarPosition);
				
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
				CommonData.Skin.DrawToolbarWindowEdge(g, drawArea, active, UseToolbarItem.ToolbarPosition);
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
				var mode = ToolbarPositionUtility.IsHorizonMode(UseToolbarItem.ToolbarPosition) ? LinearGradientMode.Vertical: LinearGradientMode.Horizontal;
				using(var brush = new LinearGradientBrush(drawArea, headColor, tailColor, mode)) {
					g.FillRectangle(brush, drawArea);
				}
			} else {
				CommonData.Skin.DrawToolbarWindowCaption(g, drawArea, active, UseToolbarItem.ToolbarPosition);
			}
		}
		
		void DrawNoClient(Graphics g, Rectangle drawArea, bool active)
		{
			if(!CommonData.Skin.IsDefaultDrawToolbarWindowBackground) {
				CommonData.Skin.DrawToolbarWindowBackground(g, drawArea, active, UseToolbarItem.ToolbarPosition);
			}
			
			var captionArea = CommonData.Skin.GetToolbarCaptionArea(UseToolbarItem.ToolbarPosition, ClientSize);
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
			if(this._isRunning && UseToolbarItem.ToolbarPosition == ToolbarPosition.DesktopFloat) {
				UseToolbarItem.FloatSize = Size;
			}
		}
		void ToolbarForm_LocationChanged(object sender, EventArgs e)
		{
			if(this._isRunning && UseToolbarItem.ToolbarPosition == ToolbarPosition.DesktopFloat) {
				UseToolbarItem.FloatLocation = Location;
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
				switch(UseToolbarItem.ToolbarPosition) {
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
						Debug.Assert(false, UseToolbarItem.ToolbarPosition.ToString());
						break;
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
			/*
			var cursorPoint = Cursor.Position;
			cursorPoint.Offset(SystemInformation.SmallIconSize.Width, SystemInformation.SmallIconSize.Height);
			var point = this.PointToClient(cursorPoint);
			Debug.WriteLine(toolItem.ToolTipText);
			this.tipsLauncher.Show(toolItem.ToolTipText, this, point);
			 */
			//this.tipsLauncher.SetToolTip(this.toolLauncher, toolItem.ToolTipText);
			//this.tipsLauncher.Show(toolItem.Text, this, Point.Empty);
			//this.tipsLauncher.RemoveAll();
			//this.tipsLauncher.SetToolTip(this, "#");
			//if(toolItem.OwnerItem == this.toolLauncher)
			//if(toolItem.OwnerItem == null)
			//var menuItem = toolItem as ToolStripDropDownItem;
			if(this._menuOpening) {
				// メニュー表示中はなんもしない
				return;
			}
			this._tipsLauncher.ShowItem(DockScreen, toolItem, SelectedGroupItem, UseToolbarItem);
		}

		void toolItem_MouseLeave(object sender, EventArgs e)
		{
			this._tipsLauncher.HideItem();
			//this.tipsLauncher.RemoveAll();
		}
		

		/*
		void ToolLauncher_MouseHover(object sender, EventArgs e)
		{
			var cursorPoint = Cursor.Position;
			cursorPoint.Offset(SystemInformation.SmallIconSize.Width, SystemInformation.SmallIconSize.Height);
			var point = this.PointToClient(cursorPoint);
			var toolItem = this.toolLauncher.Items.Cast<ToolStripItem>().FirstOrDefault(i => i.Bounds.Contains(point));
			if(toolItem != null) {
				//this.tipsLauncher.SetToolTip(this.toolLauncher, toolItem.ToolTipText);
				Debug.WriteLine("ToolLauncher_MouseHover");
			} else {
				this.tipsLauncher.RemoveAll();
			}
		}
		 * */
		
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
			}
		}

	}
}
