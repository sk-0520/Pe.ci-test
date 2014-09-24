/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/23
 * 時刻: 13:17
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using PeMain.Data;
using PeMain.Logic;
using PeMain.Properties;
using PeUtility;
using PInvoke.Windows;

namespace PeMain.UI
{
	/// <summary>
	/// Description of ToolbarForm_functions.
	/// </summary>
	public partial class ToolbarForm
	{
		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;
			this._isRunning = false;
			
			ApplySetting();
			
			this._isRunning = true;
		}
		
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
			
			CommonData.Skin.Start(this);
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
						DesktopDockType = ToolbarPositionUtility.ToDockType(UseToolbarItem.ToolbarPosition);
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
		void ApplySettingVisible()
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
			
			SelectedGroup(CommonData.MainSetting.Toolbar.ToolbarGroup.Groups.First());
			
			// 表示
			ApplySettingPosition();
			ApplySettingVisible();
			ApplySettingTopmost();
		}
		
		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			//pevent.Graphics.Clear()
			if(CommonData.Skin.IsDefaultDrawToolbarWindowBackground) {
				base.OnPaintBackground(pevent);
			} else {
				CommonData.Skin.DrawToolbarWindowBackground(pevent.Graphics, pevent.ClipRectangle, this == Form.ActiveForm, UseToolbarItem.ToolbarPosition);
			}
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
			mainButton.ToolTipText = CommonData.Language["toolbar/main/tips", new Dictionary<string, string>() {{SystemLanguageName.groupName, groupItem.Name}}];

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
		
		void OpenDir(string path)
		{
			try {
				Executer.OpenDirectory(path, CommonData.Logger, CommonData.Language, null);
			} catch(Exception ex) {
				CommonData.Logger.Puts(LogType.Warning, ex.Message, ex);
			}
		}
		
		void CopyText(string text)
		{
			Clipboard.SetText(text);
		}
		
		void OpenProperty(string path)
		{
			Executer.OpenProperty(path, Handle);
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
			openParentDirItem.Click += (object sender, EventArgs e) => { OpenDir(Path.GetDirectoryName(launcherItem.Command)); };
			// 作業ディレクトリを開く
			openWorkDirItem.Name = menuNamePath_openWorkDir;
			openWorkDirItem.Text = CommonData.Language["toolbar/menu/file/path/open-work-dir"];
			openWorkDirItem.Click += (object sender, EventArgs e) => { OpenDir(Path.GetDirectoryName(launcherItem.WorkDirPath)); };
			// コマンドコピー
			copyCommandItem.Name = menuNamePath_copyCommand;
			copyCommandItem.Text = CommonData.Language["toolbar/menu/file/path/copy-command"];
			copyCommandItem.Click += (object sender, EventArgs e) => { CopyText(launcherItem.Command); };
			// 親ディレクトリをコピー
			copyParentDirItem.Name = menuNamePath_copyParentDir;
			copyParentDirItem.Text = CommonData.Language["toolbar/menu/file/path/copy-parent-dir"];
			copyParentDirItem.Click += (object sender, EventArgs e) => { CopyText(Path.GetDirectoryName(launcherItem.Command)); };
			// 作業ディレクトリをコピー
			copyWorkDirItem.Name = menuNamePath_copyWorkDir;
			copyWorkDirItem.Text = CommonData.Language["toolbar/menu/file/path/copy-work-dir"];
			copyWorkDirItem.Click += (object sender, EventArgs e) => { CopyText(launcherItem.WorkDirPath); };
			// プロパティ
			propertyItem.Name = menuNamePath_property;
			propertyItem.Text = CommonData.Language["toolbar/menu/file/path/property"];
			propertyItem.Click += (object sender, EventArgs e) => { OpenProperty(launcherItem.Command); };
			
			// メニュー構築
			parentItem.DropDownItems.AddRange(itemList.ToArray());
			parentItem.DropDownOpening += (object sender, EventArgs e) => {
				// コマンド有無
				var commandEnabled = launcherItem.IsExists;
				copyCommandItem.Enabled = commandEnabled;
				propertyItem.Enabled = commandEnabled;
				// 親ディレクトリ有無
				var parentDirPath = Path.GetDirectoryName(launcherItem.Command);
				var parentDirEnabled = !string.IsNullOrEmpty(parentDirPath) && Path.GetPathRoot(parentDirPath) != parentDirPath && Directory.Exists(parentDirPath);
				openParentDirItem.Enabled = parentDirEnabled;
				copyParentDirItem.Enabled = parentDirEnabled;
				// 作業ディレクトリ有無
				var workDirEnabled = !string.IsNullOrEmpty(launcherItem.WorkDirPath) && Directory.Exists(launcherItem.WorkDirPath);
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
			}
			
			// アクセス権から使用可・不可
			//if(isDir) {
			//	var access = Directory.GetAccessControl(path);
			//} else {
			//	var access = File.GetAccessControl(path);
			//}
			
			if(isDir) {
				menuItem.DropDownOpening += (object sender, EventArgs e) => {
					LoadFileList(menuItem, path, showHiddenFile, showExtension);
				};
			}
			menuItem.Click += (object sender, EventArgs e) => {
				try {
					Executer.RunCommand(path);
				} catch(Exception ex) {
					CommonData.Logger.Puts(LogType.Warning, ex.Message, ex);
				}
			};
			return menuItem;
		}
		
		void LoadFileList(ToolStripMenuItem parentItem, string parentDirPath, bool showHiddenFile, bool showExtension)
		{
			if(parentItem.HasDropDownItems) {
				return;
			}
			
			try {
				var dirList = Directory.GetDirectories(parentDirPath);
				var fileList = Directory.GetFiles(parentDirPath);
				var menuList = new List<ToolStripMenuItem>(dirList.Length + fileList.Length);
				if(dirList.Length + fileList.Length > 0) {
					// TODO: ディレクトリとファイルで処理重複
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
				} else {
					var menuItem = new ToolStripMenuItem();
					menuItem.Text = CommonData.Language["toolbar/menu/file/ls/not-child-files"];
					menuItem.Image = SystemIcons.Information.ToBitmap();
					menuItem.Enabled = false;
					menuList.Add(menuItem);
				}
				parentItem.DropDownItems.AddRange(menuList.ToArray());
			} catch(UnauthorizedAccessException ex) {
				var menuItem = new ToolStripMenuItem();
				menuItem.Text = ex.Message;
				menuItem.Image = SystemIcons.Warning.ToBitmap();
				menuItem.Enabled = false;
				parentItem.DropDownItems.Add(menuItem);
			}
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
			executeItem.Click += (object sender, EventArgs e) => {
				ExecuteItem(launcherItem);
			};
			// 指定実行
			executeExItem.Name = menuNameExecuteEx;
			executeExItem.Text = CommonData.Language["toolbar/menu/file/execute-ex"];
			executeExItem.Click += (object sender, EventArgs e) => {
				ExecuteExItem(launcherItem);
			};
			// パス関係
			pathItem.Name = menuNamePath;
			pathItem.Text = CommonData.Language["toolbar/menu/file/path"];
			AttachmentFileLauncherPathSubMenu(pathItem, launcherItem);
			//pathItem.DropDownItems.AddRange(CreateFileLauncherMenuPathItems(launcherItem));
			// ファイル一覧
			fileItem.Name = menuNameFiles;
			fileItem.Text = CommonData.Language["toolbar/menu/file/ls"];
			fileItem.DropDownOpening += (object sender, EventArgs e) => {
				var showHiddenFile = SystemEnv.IsHiddenFileShow();
				var showExtension = SystemEnv.IsExtensionShow();
				LoadFileList(fileItem, Path.GetDirectoryName(launcherItem.Command), showHiddenFile, showExtension);
			};
			
			// メニュー設定
			parentItem.DropDownItems.AddRange(menuList.ToArray());
			
			parentItem.DropDownOpening += (object sender, EventArgs e) => {
				if(launcherItem.IsExists) {
					executeItem.Enabled = true;
					executeExItem.Enabled = launcherItem.IsExecteFile;
				} else {
					executeItem.Enabled = false;
					executeExItem.Enabled = false;
				}
				
				var parentPath = Path.GetDirectoryName(launcherItem.Command);
				fileItem.Enabled = Directory.Exists(parentPath);
			};
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
			itemList.Add(posFloatItem);
			itemList.Add(posTopItem);
			itemList.Add(posBottomItem);
			itemList.Add(posLeftItem);
			itemList.Add(posRightItem);
			itemList.Add(new ToolStripSeparator());
			itemList.Add(topmostItem);
			itemList.Add(autoHideItem);
			
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
			
			// メニュー設定
			parentItem.DropDownItems.AddRange(itemList.ToArray());
			
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
					pair.Key.Checked = UseToolbarItem.ToolbarPosition == pair.Value;
				}
				
				// 最前面表示
				topmostItem.Checked = UseToolbarItem.Topmost;
				
				// 自動的に隠す
				autoHideItem.Checked = AutoHide;
			};
		}

		static void SetButtonLayout(ToolStripDropDownItem toolItem, ISkin skin, IconScale iconSize, bool showText, int textWidth)
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
		
		ToolStripItem CreateLauncherButton(LauncherItem item)
		{
			ToolStripDropDownItem toolItem = null;
			
			if(item == null) {
				var iconSize = UseToolbarItem.IconScale.ToSize();
				toolItem = new ToolStripDropDownButton();
				using(var icon = new Icon(global::PeMain.Properties.Images.ToolbarMain, iconSize)) {
					var img = new Bitmap(iconSize.Width, iconSize.Height);
					using(var g = Graphics.FromImage(img)) {
						g.DrawIcon(icon, new Rectangle(Point.Empty, UseToolbarItem.IconScale.ToSize()));
						#if DEBUG
						using(var b = new SolidBrush(Color.FromArgb(64, Color.Red))) {
							g.FillRectangle(b, new Rectangle(Point.Empty, UseToolbarItem.IconScale.ToSize()));
						}
						#endif
					}
					toolItem.Image = img;
				}
				
				
			} else {
				var clickItem = new ToolStripSplitButton();
				clickItem.ButtonClick += new EventHandler(button_ButtonClick);
				toolItem = clickItem;
				toolItem.Text = item.Name;
				toolItem.ToolTipText = item.Name;
				var icon = item.GetIcon(UseToolbarItem.IconScale, item.IconIndex);
				if(icon != null) {
					toolItem.Image = icon.ToBitmap();
				}
			}
			
			toolItem.MouseHover += new EventHandler(toolItem_MouseHover);
			//toolItem.AutoSize = true;
			SetButtonLayout(toolItem, CommonData.Skin, UseToolbarItem.IconScale, UseToolbarItem.ShowText, UseToolbarItem.TextWidth);
			//var buttonLayout = GetButtonLayout();
			//button.Margin  = new Padding(0);
			//button.Padding = new Padding(0);
			//button.Padding = buttonLayout.Padding;
			//if(MainSetting.Toolbar.ShowText) {
			//	toolItem.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
			//} else {
			//	toolItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
			//}
			//button.Size = buttonLayout.ClientSize;
			toolItem.Visible = true;
			if(item != null) {
				toolItem.Tag = item;
				if(item.LauncherType == LauncherType.File) {
					AttachmentFileLauncherMenu(toolItem, item);
				}
			} else {
				AttachmentToolbarMenu(toolItem);
			}
			toolItem.DropDownOpening += OpeningRootMenu;
			toolItem.DropDownClosed += CloseRootMenu;

			var hasMenuItem = (ToolStripDropDownItem)toolItem;
			if(hasMenuItem != null) {
				hasMenuItem.DropDownOpening += clickItem_DropDownOpening;
			}
			//button.DropDownItemClicked += new ToolStripItemClickedEventHandler(button_DropDownItemClicked);
			
			return toolItem;
		}
		
		// TODO: 領域ぎりぎりの場合にメニュー位置が他のディスプレイに表示される
		void OpeningDropDown(ToolStripDropDownItem toolItem)
		{
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
		
		
		bool ExecuteItem(LauncherItem launcherItem)
		{
			try {
				if(launcherItem.LauncherType == LauncherType.File) {
					launcherItem.Execute(CommonData, this);
					launcherItem.Increment();
					return true;
				}
			} catch(Exception ex) {
				CommonData.Logger.Puts(LogType.Warning, ex.Message, ex);
			}
			
			return false;
		}
		
		void ExecuteExItem(LauncherItem launcherItem)
		{
			using(var form = new ExecuteForm()) {
				form.SetParameter(launcherItem);
				form.SetCommonData(CommonData);
				form.TopMost = TopMost;
				if(form.ShowDialog(this) == DialogResult.OK) {
					var editedItem = form.EditedLauncherItem;
					if(ExecuteItem(editedItem)) {
						launcherItem.Increment(editedItem.Option, editedItem.WorkDirPath);
					}
				}
			}
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

		DropData ProcessDropEffect(DragEventArgs e)
		{
			var result = new DropData();
			var localPoint = this.toolLauncher.PointToClient(new Point(e.X, e.Y));
			
			if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
				result.Files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
				result.ToolStripItem = GetOverButton(localPoint);
				if(result.ToolStripItem != null) {
					result.LauncherItem = result.ToolStripItem.Tag as LauncherItem;
				}
				
				if(result.ToolStripItem != null) {
					if(result.LauncherItem.IsExtExec && result.LauncherItem.IsExecteFile) {
						e.Effect = DragDropEffects.Move;
					} else {
						e.Effect = DragDropEffects.None;
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
			
			return result;
		}
		
		void ExecuteDropData(DropData dropData)
		{
			if(dropData.ToolStripItem != null) {
				// ボタン上
			} else {
				// 追加
				Debug.Assert(dropData.Files.Count() == 1);
				var item = LauncherItem.FileLoad(dropData.Files.First());
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
		
		public void ReceiveChangedLauncherItems(ToolbarItem toolbarItem, ToolbarGroupItem toolbarGroupItem)
		{
			// 他のツールバーから通知を受け取った場合に反映処理を行う
			Debug.Assert(toolbarItem != UseToolbarItem);
			SelectedGroup(SelectedGroupItem);
		}
		
		protected override void HiddenView(Rectangle area)
		{
			if(AutoHide) {
				if(!this._menuOpening) {
					base.HiddenView(area);
				} else {
					//SwitchHidden();
				}
			}
		}

	}
}
