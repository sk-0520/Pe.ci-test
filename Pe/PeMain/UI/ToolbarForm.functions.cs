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
using PeMain.Logic;
using PeMain.Properties;
using PeMain.Data;
using PeUtility;

namespace PeMain.UI
{
	/// <summary>
	/// Description of ToolbarForm_functions.
	/// </summary>
	public partial class ToolbarForm
	{
		static ToolbarPosition ToToolbarPosition(DesktopDockType value)
		{
			return new Dictionary<DesktopDockType, ToolbarPosition>() {
				{ DesktopDockType.Left,   ToolbarPosition.DesktopLeft },
				{ DesktopDockType.Top,    ToolbarPosition.DesktopTop },
				{ DesktopDockType.Right,  ToolbarPosition.DesktopRight },
				{ DesktopDockType.Bottom, ToolbarPosition.DesktopBottom },
			}[value];
		}
		static DesktopDockType ToDockType(ToolbarPosition value)
		{
			return new Dictionary<ToolbarPosition, DesktopDockType>() {
				{ToolbarPosition.DesktopLeft,   DesktopDockType.Left },
				{ToolbarPosition.DesktopTop,    DesktopDockType.Top },
				{ToolbarPosition.DesktopRight,  DesktopDockType.Right },
				{ToolbarPosition.DesktopBottom, DesktopDockType.Bottom },
			}[value];
		}
		
		public static bool IsHorizonMode(ToolbarPosition pos)
		{
			return pos.IsIn(
				ToolbarPosition.DesktopFloat,
				ToolbarPosition.DesktopTop,
				ToolbarPosition.DesktopBottom,
				ToolbarPosition.WindowTop,
				ToolbarPosition.WindowBottom
			);
		}
		
		Padding GetBorderPadding()
		{
			var frame = SystemInformation.Border3DSize;
			return new Padding(frame.Width, frame.Height, frame.Width, frame.Height);
		}
		
		Rectangle GetCaptionArea(ToolbarPosition pos)
		{
			var padding = GetBorderPadding();
			var point = new Point(padding.Left, padding.Top);
			var size = new Size();
			
			if(IsHorizonMode(pos)) {
				size.Width = SystemInformation.SmallCaptionButtonSize.Height / 2;
				size.Height = Height - Padding.Vertical;
			} else {
				size.Width = Width - Padding.Horizontal;
				size.Height = SystemInformation.SmallCaptionButtonSize.Height / 2;
			}
			
			return new Rectangle(point, size);
		}
		
		void SetPaddingArea(ToolbarPosition pos)
		{
			var borderPadding = GetBorderPadding();
			var captionArea = GetCaptionArea(pos);
			var captionPlus = new Size();
			if(IsHorizonMode(pos)) {
				captionPlus.Width = captionArea.Width;
			} else {
				captionPlus.Height =captionArea.Height; 
			}
			var padding = new Padding(
				borderPadding.Left + captionPlus.Width,
				borderPadding.Top  + captionPlus.Height,
				borderPadding.Right,
				borderPadding.Bottom
			);
			Padding = padding;
		}
		
		public void SetSettingData(Language language, MainSetting mainSetting)
		{
			Language = language;
			this.MainSetting = mainSetting;
			//MainSetting.Toolbar = mainSetting.Toolbar;
			//LauncherSetting = mainSetting.Launcher;
			
			ApplySetting();
		}
		
		void ApplySettingTopmost()
		{
			TopMost = UseToolbarItem.Topmost;
		}
		void ApplySettingPosition()
		{
			if(UseToolbarItem.Visible) {
				ItemSizeToFormSize();
				
				if(IsDockingMode) {
					DesktopDockType = ToDockType(UseToolbarItem.ToolbarPosition);
					if(IsHorizonMode(UseToolbarItem.ToolbarPosition)) {
						this.toolLauncher.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
						this.toolLauncher.Dock = DockStyle.Fill;
					} else {
						this.toolLauncher.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
						this.toolLauncher.Dock = DockStyle.Left;
					}
				} else {
					DesktopDockType = DesktopDockType.None;
					this.toolLauncher.Dock = DockStyle.Top;
					if(UseToolbarItem.ToolbarPosition == ToolbarPosition.DesktopFloat) {
						Location = UseToolbarItem.FloatLocation;
					}
				}
			}
		}
		
		void ApplyScreen()
		{
			Debug.Assert(MainSetting != null);
			Debug.Assert(ParentScreen != null);
			
			UseToolbarItem = null;
			foreach(var item in MainSetting.Toolbar.Items) {
				if(item.IsNameEqual(ParentScreen.DeviceName)) {
					UseToolbarItem = item;
					break;
				}
			}
			if(UseToolbarItem == null) {
				// 新規
				var toolbarItem = new ToolbarItem();
				toolbarItem.Name = ParentScreen.DeviceName;
				MainSetting.Toolbar.Items.Add(toolbarItem);
				UseToolbarItem = toolbarItem;
			}
		}
		void ApplySettingFont()
		{
			Debug.Assert(MainSetting != null);
			
			if(!UseToolbarItem.FontSetting.IsDefault) {
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
			Debug.Assert(MainSetting != null);
			
			ApplyLanguage();
			ApplyScreen();
			ApplySettingFont();
			
			Font = UseToolbarItem.FontSetting.Font;
			if(MainSetting.Toolbar.ToolbarGroup.Groups.Count == 0) {
				// グループが存在しなければグループを作っておく
				var toolbarGroupItem = new ToolbarGroupItem();
				toolbarGroupItem.Name = Language["group/new"];
				MainSetting.Toolbar.ToolbarGroup.Groups.Add(toolbarGroupItem);
			}
			
			// グループメニュー基盤構築
			this._menuGroup.MenuItems.Clear();
			foreach(var groupName in MainSetting.Toolbar.ToolbarGroup.Groups) {
				var menuItem = new MenuItem();
				
				menuItem.Text = groupName.Name;
				menuItem.Tag = groupName;

				menuItem.Click += new EventHandler(ToolbarForm_MenuItem_Click);
				
				this._menuGroup.MenuItems.Add(menuItem);
			}
			
			SelectedGroup(MainSetting.Toolbar.ToolbarGroup.Groups.First());
			
			// 表示
			ApplySettingVisible();
			ApplySettingPosition();
			ApplySettingTopmost();
		}
		
		/// <summary>
		/// 表示タイプからウィンドウをそれっぽいサイズに変更
		/// </summary>
		void ItemSizeToFormSize()
		{
			var floatSize = UseToolbarItem.FloatSize;
			SetPaddingArea(UseToolbarItem.ToolbarPosition);
			
			var buttonLayout = GetButtonLayout(UseToolbarItem.IconSize, UseToolbarItem.ShowText, UseToolbarItem.TextWidth);
			
			var minSize = buttonLayout.Size;
			minSize.Width += this.toolLauncher.Margin.Horizontal + Margin.Horizontal;
			minSize.Height += this.toolLauncher.Margin.Vertical + Margin.Vertical;
			MinimumSize = minSize;
			
			//Size = new Size(minSize.Width, minSize.Height);
			
			if(IsDockingMode) {
				BarSize = new Size(minSize.Width, minSize.Height);
			} else {
				if(IsHorizonMode(UseToolbarItem.ToolbarPosition)) {
					Size = new Size(floatSize.Width, minSize.Height);
				} else {
					Size = new Size(minSize.Width, floatSize.Height);
				}
			}
		}
		
		void SetToolButtons(IconSize iconSize, IEnumerable<ToolStripItem> buttons)
		{
			this.toolLauncher.ImageScalingSize = iconSize.ToSize();
			
			// アイコン解放
			this.toolLauncher.Items
				.Cast<ToolStripItem>()
				.Where(item => item.Image != null)
				.ForEach(item => item.Image.Dispose())
			;
			
			this.toolLauncher.Items.Clear();
			this.toolLauncher.Items.AddRange(buttons.ToArray());
		}
		
		void SelectedGroup(ToolbarGroupItem groupItem)
		{
			var toolItem = this._menuGroup.MenuItems
				.Cast<MenuItem>()
				.ForEach(item => item.Checked = false)
				.Single(item => (ToolbarGroupItem)item.Tag == groupItem)
			;
			SelectedGroupItem = groupItem;
			
			toolItem.Checked = true;
			
			// 表示アイテム生成
			var toolButtonList = new List<ToolStripItem>();
			toolButtonList.Add(CreateLauncherButton(null));
			foreach(var itemName in groupItem.ItemNames) {
				var launcherItem = this.MainSetting.Launcher.Items.SingleOrDefault(item => item.IsNameEqual(itemName));
				if(launcherItem != null) {
					var itemButton = CreateLauncherButton(launcherItem);
					toolButtonList.Add(itemButton);
				}
			}
			SetToolButtons(UseToolbarItem.IconSize, toolButtonList);
		}
		
		void OpenDir(string path)
		{
			try {
				Process.Start(path);
			} catch(Exception ex) {
				Logger.Puts(LogType.Warning, ex.Message, ex);
			}
		}
		
		void CopyText(string text)
		{
			Clipboard.SetText(text);
		}
		
		void OpenProperty(string path)
		{
			Logger.Puts(LogType.Information, "P/Iから取ってくるのだるいんで後回し", path);
		}
		
		ToolStripItem[] CreateFileLauncherMenuPathItems(LauncherItem launcherItem)
		{
			var result = new List<ToolStripItem>();
			
			var openParentDirItem = new ToolStripMenuItem();
			var openWorkDirItem = new ToolStripMenuItem();
			var copyCommandItem = new ToolStripMenuItem();
			var copyParentDirItem = new ToolStripMenuItem();
			var copyWorkDirItem = new ToolStripMenuItem();
			var propertyItem = new ToolStripMenuItem();
			result.Add(openParentDirItem);
			result.Add(openWorkDirItem);
			result.Add(new ToolStripSeparator());
			result.Add(copyCommandItem);
			result.Add(copyParentDirItem);
			result.Add(copyWorkDirItem);
			result.Add(new ToolStripSeparator());
			result.Add(propertyItem);
			
			// 親ディレクトリを開く
			openParentDirItem.Name = menuNamePath_openParentDir;
			openParentDirItem.Text = Language["toolbar/menu/file/path/open-parent-dir"];
			openParentDirItem.Click += (object sender, EventArgs e) => { OpenDir(Path.GetDirectoryName(launcherItem.Command)); };
			// 作業ディレクトリを開く
			openWorkDirItem.Name = menuNamePath_openWorkDir;
			openWorkDirItem.Text = Language["toolbar/menu/file/path/open-work-dir"];
			openWorkDirItem.Click += (object sender, EventArgs e) => { OpenDir(Path.GetDirectoryName(launcherItem.WorkDirPath)); };
			// コマンドコピー
			copyCommandItem.Name = menuNamePath_copyCommand;
			copyCommandItem.Text = Language["toolbar/menu/file/path/copy-command"];
			copyCommandItem.Click += (object sender, EventArgs e) => { CopyText(launcherItem.Command); };
			// 親ディレクトリをコピー
			copyParentDirItem.Name = menuNamePath_copyParentDir;
			copyParentDirItem.Text = Language["toolbar/menu/file/path/copy-parent-dir"];
			copyParentDirItem.Click += (object sender, EventArgs e) => { CopyText(Path.GetDirectoryName(launcherItem.Command)); };
			// 作業ディレクトリをコピー
			copyWorkDirItem.Name = menuNamePath_copyWorkDir;
			copyWorkDirItem.Text = Language["toolbar/menu/file/path/copy-work-dir"];
			copyWorkDirItem.Click += (object sender, EventArgs e) => { CopyText(launcherItem.WorkDirPath); };
			// プロパティ
			propertyItem.Name = menuNamePath_property;
			propertyItem.Text = Language["toolbar/menu/file/path/property"];
			propertyItem.Click += (object sender, EventArgs e) => { OpenProperty(launcherItem.Command); };
			
			return result.ToArray();
		}
		
		ToolStripMenuItem GetFileListItem(string path, bool isDir)
		{
			var menuItem = new ToolStripMenuItem();
			menuItem.Text = Path.GetFileName(path);
			using(var icon = IconLoader.Load(path, UseToolbarItem.IconSize, 0)) {
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
					LoadFileList(menuItem, path);
				};
			}
			menuItem.Click += (object sender, EventArgs e) => {
				try {
					Process.Start(path);
				} catch(Exception ex) {
					Logger.Puts(LogType.Warning, ex.Message, ex);
				}
			};
			return menuItem;
		}
		
		void LoadFileList(ToolStripMenuItem parentItem, string parentDirPath)
		{
			if(parentItem.HasDropDownItems) {
				return;
			}
			
			try {
				var dirList = Directory.GetDirectories(parentDirPath);
				var fileList = Directory.GetFiles(parentDirPath);
				var menuList = new List<ToolStripMenuItem>(dirList.Length + fileList.Length);
				if(dirList.Length + fileList.Length > 0) {
					foreach(var path in dirList) {
						var menuItem = GetFileListItem(path, true);
						menuList.Add(menuItem);
					}
					foreach(var path in fileList) {
						var menuItem = GetFileListItem(path, false);
						menuList.Add(menuItem);
					}
				} else {
					var menuItem = new ToolStripMenuItem();
					menuItem.Text = Language["toolbar/menu/file/ls/not-child-files"];
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
		
		ToolStripItem[] CreateFileLauncherMenuItems(LauncherItem launcherItem)
		{
			var result = new List<ToolStripItem>();
			
			var executeItem = new ToolStripMenuItem();
			var executeExItem = new ToolStripMenuItem();
			var pathItem = new ToolStripMenuItem();
			var fileItem = new ToolStripMenuItem();
			result.Add(executeItem);
			result.Add(executeExItem);
			result.Add(new ToolStripSeparator());
			result.Add(pathItem);
			result.Add(fileItem);
			
			// 通常実行
			executeItem.Name = menuNameExecute;
			executeItem.Text = Language["toolbar/menu/file/execute"];
			executeItem.Click += (object sender, EventArgs e) => {
				ExecuteItem(launcherItem);
			};
			// 指定実行
			executeExItem.Name = menuNameExecuteEx;
			executeExItem.Text = Language["toolbar/menu/file/execute-ex"];
			executeExItem.Click += (object sender, EventArgs e) => {
				ExecuteExItem(launcherItem);
			};
			// パス関係
			pathItem.Name = menuNamePath;
			pathItem.Text = Language["toolbar/menu/file/path"];
			pathItem.DropDownItems.AddRange(CreateFileLauncherMenuPathItems(launcherItem));
			pathItem.DropDownOpening += (object sender, EventArgs e) => {
				// コマンド有無
				var commandEnabled = launcherItem.IsExists;
				pathItem.DropDownItems[menuNamePath_copyCommand].Enabled = commandEnabled;
				pathItem.DropDownItems[menuNamePath_property].Enabled = commandEnabled;
				// 親ディレクトリ有無
				var parentDirPath = Path.GetDirectoryName(launcherItem.Command);
				//Debug.WriteLine(Path.GetPathRoot(parentDirPath) );
				//Debug.WriteLine(parentDirPath);
				var parentDirEnabled = !string.IsNullOrEmpty(parentDirPath) && Path.GetPathRoot(parentDirPath) != parentDirPath && Directory.Exists(parentDirPath);
				pathItem.DropDownItems[menuNamePath_openParentDir].Enabled = parentDirEnabled;
				pathItem.DropDownItems[menuNamePath_copyParentDir].Enabled = parentDirEnabled;
				// 作業ディレクトリ有無
				var workDirEnabled = !string.IsNullOrEmpty(launcherItem.WorkDirPath) && Directory.Exists(launcherItem.WorkDirPath);
				pathItem.DropDownItems[menuNamePath_openWorkDir].Enabled = workDirEnabled;
				pathItem.DropDownItems[menuNamePath_copyWorkDir].Enabled = workDirEnabled;
			};
			// ファイル一覧
			fileItem.Name = menuNameFiles;
			fileItem.Text = Language["toolbar/menu/file/ls"];
			fileItem.DropDownOpening += (object sender, EventArgs e) => {
				LoadFileList(fileItem, Path.GetDirectoryName(launcherItem.Command));
			};
			
			return result.ToArray();
		}
		
		ToolStripItem[] CreateToolbarMenu()
		{
			var result = new List<ToolStripItem>();
			
			var posFloatItem = new ToolStripMenuItem();
			var posTopItem = new ToolStripMenuItem();
			var posBottomItem = new ToolStripMenuItem();
			var posLeftItem = new ToolStripMenuItem();
			var posRightItem = new ToolStripMenuItem();
			var topmostItem = new ToolStripMenuItem();
			var autoHideItem = new ToolStripMenuItem();
			result.Add(posFloatItem);
			result.Add(posTopItem);
			result.Add(posBottomItem);
			result.Add(posLeftItem);
			result.Add(posRightItem);
			result.Add(new ToolStripSeparator());
			result.Add(topmostItem);
			//result.Add(autoHideItem);
			
			// フロート
			posFloatItem.Name = menuNameMainPosDesktopFloat;
			posFloatItem.Text = ToolbarPosition.DesktopFloat.ToText(Language);
			posFloatItem.Click += (object sender, EventArgs e) => {
				UseToolbarItem.ToolbarPosition = ToolbarPosition.DesktopFloat;
				ApplySettingPosition();
			};
			// デスクトップ：上
			posTopItem.Name = menuNameMainPosDesktopTop;
			posTopItem.Text = ToolbarPosition.DesktopTop.ToText(Language);
			posTopItem.Click += (object sender, EventArgs e) => {
				UseToolbarItem.ToolbarPosition = ToolbarPosition.DesktopTop;
				ApplySettingPosition();
			};
			// デスクトップ：下
			posBottomItem.Name = menuNameMainPosDesktopBottom;
			posBottomItem.Text = ToolbarPosition.DesktopBottom.ToText(Language);
			posBottomItem.Click += (object sender, EventArgs e) => {
				UseToolbarItem.ToolbarPosition = ToolbarPosition.DesktopBottom;
				ApplySettingPosition();
			};
			// デスクトップ：左
			posLeftItem.Name = menuNameMainPosDesktopLeft;
			posLeftItem.Text = ToolbarPosition.DesktopLeft.ToText(Language);
			posLeftItem.Click += (object sender, EventArgs e) => {
				UseToolbarItem.ToolbarPosition = ToolbarPosition.DesktopLeft;
				ApplySettingPosition();
			};
			// デスクトップ：右
			posRightItem.Name = menuNameMainPosDesktopRight;
			posRightItem.Text = ToolbarPosition.DesktopRight.ToText(Language);
			posRightItem.Click += (object sender, EventArgs e) => {
				UseToolbarItem.ToolbarPosition = ToolbarPosition.DesktopRight;
				ApplySettingPosition();
			};
			
			// 最前面表示
			topmostItem.Name = menuNameMainTopmost;
			topmostItem.Text = Language["common/menu/topmost"];
			topmostItem.Click += (object sender, EventArgs e) => {
				UseToolbarItem.Topmost = !topmostItem.Checked;
				ApplySettingTopmost();
			};
			
			// 
			autoHideItem.Name = menuNameMainAutoHide;
			autoHideItem.Text = Language["toolbar/menu/main/auto-hide"];
			autoHideItem.Click += (object sender, EventArgs e) => {
				UseToolbarItem.AutoHide = !autoHideItem.Checked;
				ApplySettingPosition();
			};
			
			return result.ToArray();
		}
		
		static ButtonLayout GetButtonLayout(IconSize iconSize, bool showText, int textWidth)
		{
			var iconBox = iconSize.ToSize();
			var systemBorderSize = SystemInformation.Border3DSize;
			var systemPaddingSize = SystemInformation.FixedFrameBorderSize;
			var padding = new Padding(
				systemBorderSize.Width + systemPaddingSize.Width / 2, 
				systemBorderSize.Height + systemPaddingSize.Height / 2, 
				systemBorderSize.Width + systemPaddingSize.Width / 2, 
				systemBorderSize.Height + systemPaddingSize.Height / 2
			);
			var buttonSize = new Size();
			var menuWidth = 12;
			
			buttonSize.Width = iconBox.Width + padding.Horizontal + menuWidth;
			if(showText) {
				buttonSize.Width += textWidth > 0 ? textWidth: Literal.toolbarTextWidth;
			}
			buttonSize.Height = iconBox.Height + padding.Vertical;
			
			var buttonLayout = new ButtonLayout();
			buttonLayout.Size = buttonSize;
			buttonLayout.MenuWidth = menuWidth;
			return buttonLayout;
		}

		static void SetButtonLayout(ToolStripDropDownItem toolItem, IconSize iconSize, bool showText, int textWidth)
		{
			var toolSplit = toolItem as ToolStripSplitButton;
			var buttonLayout = GetButtonLayout(iconSize, showText, textWidth);
			
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
				toolItem = new ToolStripDropDownButton();
				toolItem.Text = Language["toolbar/main/text"];
				toolItem.ToolTipText = Language["toolbar/main/text"];
				toolItem.Image = PeMain.Properties.Images.ToolbarMain;
			} else {
				toolItem = new ToolStripSplitButton();
				toolItem.Text = item.Name;
				toolItem.ToolTipText = item.Name;
				var icon = item.GetIcon(UseToolbarItem.IconSize);
				if(icon != null) {
					toolItem.Image = icon.ToBitmap();
				}
			}

			//toolItem.AutoSize = true;
			SetButtonLayout(toolItem, UseToolbarItem.IconSize, UseToolbarItem.ShowText, UseToolbarItem.TextWidth);
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
					toolItem.DropDownItems.AddRange(CreateFileLauncherMenuItems(item));
					toolItem.DropDownOpening += (object sender, EventArgs e) => {
						if(item.IsExists) {
							toolItem.DropDownItems[menuNameExecute].Enabled = true;
							toolItem.DropDownItems[menuNameExecuteEx].Enabled = item.IsExecteFile;
						} else {
							toolItem.DropDownItems[menuNameExecute].Enabled = false;
							toolItem.DropDownItems[menuNameExecuteEx].Enabled = false;
						}
						var parentPath = Path.GetDirectoryName(item.Command);
						toolItem.DropDownItems[menuNameFiles].Enabled = Directory.Exists(parentPath);
					};
				}
			} else {
				toolItem.DropDownItems.AddRange(CreateToolbarMenu());
				toolItem.DropDownOpening += delegate(object sender, EventArgs e) {
					// ウィンドウ位置
					var windowPosNameKey = new Dictionary<string, ToolbarPosition>() {
						{menuNameMainPosDesktopFloat,  ToolbarPosition.DesktopFloat},
						{menuNameMainPosDesktopTop,    ToolbarPosition.DesktopTop},
						{menuNameMainPosDesktopBottom, ToolbarPosition.DesktopBottom},
						{menuNameMainPosDesktopLeft,   ToolbarPosition.DesktopLeft},
						{menuNameMainPosDesktopRight,  ToolbarPosition.DesktopRight},
					};
					foreach(var pair in windowPosNameKey) {
						var menu = (ToolStripMenuItem)(toolItem.DropDownItems[pair.Key]);
						if(menu != null) {
							menu.Checked = UseToolbarItem.ToolbarPosition == pair.Value;
						}
					}
					
					// 最前面表示
					((ToolStripMenuItem)(toolItem.DropDownItems[menuNameMainTopmost])).Checked = UseToolbarItem.Topmost;
				};
			}
			if(item != null) {
				var clickItem = (ToolStripSplitButton)toolItem;
				clickItem.ButtonClick += new EventHandler(button_ButtonClick);
			}
			//button.DropDownItemClicked += new ToolStripItemClickedEventHandler(button_DropDownItemClicked);
			
			return toolItem;
		}
		
		bool ExecuteItem(LauncherItem launcherItem)
		{
			try {
				if(launcherItem.LauncherType == LauncherType.File) {
					launcherItem.Execute(Logger, Language, this.MainSetting, this);
					launcherItem.Increment();
					return true;
				}
			} catch(Exception ex) {
				Logger.Puts(LogType.Warning, ex.Message, ex);
			}
			
			return false;
		}
		
		void ExecuteExItem(LauncherItem launcherItem)
		{
			using(var form = new ExecuteForm()) {
				form.SetParameter(launcherItem);
				form.SetSettingData(Language, this.MainSetting);
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
					if(result.LauncherItem.IsExtExcec && result.LauncherItem.IsExecteFile) {
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
				item.Name = LauncherItem.GetUniqueName(item, this.MainSetting.Launcher.Items);
				this.MainSetting.Launcher.Items.Add(item);
				SelectedGroupItem.ItemNames.Add(item.Name);
				SelectedGroup(SelectedGroupItem);
				// TODO: 他のツールバーに教える
			}
		}
		

	}
}
