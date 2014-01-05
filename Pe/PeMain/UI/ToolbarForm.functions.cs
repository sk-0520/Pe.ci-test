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
using System.Linq;
using System.Windows.Forms;
using PeMain.Properties;
using PeMain.Setting;
using PeUtility;

namespace PeMain.UI
{
	/// <summary>
	/// Description of ToolbarForm_functions.
	/// </summary>
	public partial class ToolbarForm
	{
		static ToolbarPosition ToToolbarPosition(DockType value)
		{
			return new Dictionary<DockType, ToolbarPosition>() {
				{ DockType.Left,   ToolbarPosition.DesktopLeft },
				{ DockType.Top,    ToolbarPosition.DesktopTop },
				{ DockType.Right,  ToolbarPosition.DesktopRight },
				{ DockType.Bottom, ToolbarPosition.DesktopBottom },
			}[value];
		}
		static DockType ToDockType(ToolbarPosition value)
		{
			return new Dictionary<ToolbarPosition, DockType>() {
				{ToolbarPosition.DesktopLeft,   DockType.Left },
				{ToolbarPosition.DesktopTop,    DockType.Top },
				{ToolbarPosition.DesktopRight,  DockType.Right },
				{ToolbarPosition.DesktopBottom, DockType.Bottom },
			}[value];
		}
		
		static bool IsCaptionSidePos(ToolbarPosition pos)
		{
			return pos.IsIn(
				ToolbarPosition.DesktopFloat,
				ToolbarPosition.DesktopTop,
				ToolbarPosition.DesktopBottom,
				ToolbarPosition.WindowTop,
				ToolbarPosition.WindowBottom
			);
		}
		
		Rectangle GetCaptionBarRect(ToolbarPosition pos)
		{
			var padding = Padding;
			var captionSize = SystemInformation.SmallCaptionButtonSize.Height / 2;
			
			if(IsCaptionSidePos(pos)) {
				return new Rectangle(
					padding.Left,
					padding.Top,
					captionSize,
					ClientSize.Height - padding.Vertical
				);
			} else {
				return new Rectangle(
					padding.Left,
					padding.Top,
					ClientSize.Width - padding.Horizontal,
					captionSize
				);
			}
		}

		
		public void SetSettingData(Language language, MainSetting mainSetting)
		{
			Language = language;
			ToolbarSetting = mainSetting.Toolbar;
			LauncherSetting = mainSetting.Launcher;
			
			ApplySetting();
		}
		
		
		void ApplySetting()
		{
			Debug.Assert(ToolbarSetting != null);
			
			ApplyLanguage();
			
			Font = ToolbarSetting.FontSetting.Font;
			
			// グループメニュー基盤構築
			this.menuGroup.Items.Clear();
			foreach(var groupName in ToolbarSetting.ToolbarGroup.Groups) {
				var menuItem = new ToolStripMenuItem();
				
				menuItem.Text = groupName.Name;
				menuItem.Tag = groupName;

				menuItem.Click += new EventHandler(ToolbarForm_MenuItem_Click);
				
				this.menuGroup.Items.Add(menuItem);
			}
			if(ToolbarSetting.ToolbarGroup.Groups.Count > 0) {
				SelectedGroup(ToolbarSetting.ToolbarGroup.Groups.First());
			}
			
			// 表示
			if(IsDockingMode && ToolbarSetting.Visible) {
				DockType = ToDockType(ToolbarSetting.ToolbarPosition);
			} else {
				DockType = DockType.None;
			}
			
			ItemSizeToFormSize();
			Visible = ToolbarSetting.Visible;
		}
		
		/// <summary>
		/// 表示タイプからウィンドウをそれっぽいサイズに変更
		/// </summary>
		void ItemSizeToFormSize()
		{
			// TODO: これから
		}
		
		void SelectedGroup(ToolbarGroupItem groupItem)
		{
			var toolItem = this.menuGroup.Items
				.Cast<ToolStripMenuItem>()
				.Transform(item => item.Checked = false)
				.Single(item => (ToolbarGroupItem)item.Tag == groupItem)
			;
			
			toolItem.Checked = true;
			
			// 表示アイテム設定
			var iconSize = ToolbarSetting.IconSize.ToHeight();
			//this.toolLauncher.ImageScalingSize = new Size(iconSize, iconSize);
			this.toolLauncher.ButtonSize = new Size(iconSize, iconSize);
			var toolButtonList = new List<ToolBarButton>();
			this._toolbarImageList.Images.Clear();
			foreach(var itemName in groupItem.ItemNames) {
				var launcherItem = LauncherSetting.Items.SingleOrDefault(item => item.IsNameEqual(itemName));
				if(launcherItem != null) {
					var itemButton = CreateLauncherButton(launcherItem);
					this._toolbarImageList.Images.Add(launcherItem.Name, launcherItem.GetIcon(ToolbarSetting.IconSize));
					toolButtonList.Add(itemButton);
				}
			}
			
			this.toolLauncher.Buttons.Clear();
			this.toolLauncher.ImageList = null;
			this.toolLauncher.ImageList = this._toolbarImageList;
			this.toolLauncher.Buttons.AddRange(toolButtonList.ToArray());
		}
		
		ContextMenu CreateFileLauncherMenuItems(LauncherItem item)
		{
			var menuItemList = new List<MenuItem>();
			
			var executeItem = new MenuItem();
			var executeExItem = new MenuItem();
			var pathItem = new MenuItem();
			var fileItem = new MenuItem();
			menuItemList.Add(executeItem);
			menuItemList.Add(executeExItem);
			menuItemList.Add(new MenuItem());
			menuItemList.Add(pathItem);
			menuItemList.Add(fileItem);
			
			executeItem.Text = Language["toolbar/menu/file/execute"];
			executeExItem.Text = Language["toolbar/menu/file/execute-ex"];
			pathItem.Text = Language["toolbar/menu/file/path"];
			fileItem.Text = Language["toolbar/menu/file/ls"];
			
			var itemMenu = new ContextMenu();
			itemMenu.MenuItems.AddRange(menuItemList.ToArray());
			return itemMenu;
		}
		
		ToolBarButton CreateLauncherButton(LauncherItem item)
		{
			Debug.Assert(item != null);
			
			var button = new ToolBarButton();
			
			//button.Text = item.Name;
			button.ToolTipText = item.Name;
			button.ImageKey = item.Name;
			button.Style = ToolBarButtonStyle.DropDownButton;
			//button.AutoSize = false;
			//var buttonLayout = GetButtonLayout();
			//button.Margin  = new Padding(0);
			//button.Padding = new Padding(0);
			//button.Padding = buttonLayout.Padding;
			if(ToolbarSetting.ShowText) {
				//button.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
				button.Text = item.Name;
			} else {
				//button.DisplayStyle = ToolStripItemDisplayStyle.Image;
			}
			//button.Size = buttonLayout.ClientSize;
			button.Tag = item;
			button.Visible = true;
			if(item.LauncherType == LauncherType.File) {
				button.DropDownMenu = CreateFileLauncherMenuItems(item);
			}
			//button.ButtonClick += new EventHandler(button_ButtonClick);
			//button.DropDownItemClicked += new ToolStripItemClickedEventHandler(button_DropDownItemClicked);
			
			return button;
		}


	}
}
