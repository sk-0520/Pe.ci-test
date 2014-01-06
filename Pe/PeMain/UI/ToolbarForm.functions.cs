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
			if(IsDockingMode) {
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
			this.toolLauncher.ImageScalingSize = new Size(iconSize, iconSize);
			var toolButtonList = new List<ToolStripSplitButton>();
			foreach(var itemName in groupItem.ItemNames) {
				var launcherItem = LauncherSetting.Items.SingleOrDefault(item => item.IsNameEqual(itemName));
				if(launcherItem != null) {
					var itemButton = CreateLauncherButton(launcherItem);
					toolButtonList.Add(itemButton);
				}
			}
			toolLauncher.Items
				.Cast<ToolStripItem>()
				.Where(item => item.Image != null)
				.Transform(item => item.Image.Dispose())
			;
			toolLauncher.Items.Clear();
			toolLauncher.Items.AddRange(toolButtonList.ToArray());
		}
		
		ToolStripItem[] CreateFileLauncherMenuItems(LauncherItem item)
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
			
			executeItem.Text = Language["toolbar/menu/file/execute"];
			executeExItem.Text = Language["toolbar/menu/file/execute-ex"];
			pathItem.Text = Language["toolbar/menu/file/path"];
			fileItem.Text = Language["toolbar/menu/file/ls"];
			
			return result.ToArray();
		}
		
		ToolStripSplitButton CreateLauncherButton(LauncherItem item)
		{
			Debug.Assert(item != null);
			
			var button = new ToolStripSplitButton();
			
			button.Text = item.Name;
			button.ToolTipText = item.Name;
			button.Image = item.GetIcon(ToolbarSetting.IconSize).ToBitmap();
			button.TextImageRelation = TextImageRelation.ImageBeforeText;
			button.AutoSize = true;
			//var buttonLayout = GetButtonLayout();
			//button.Margin  = new Padding(0);
			//button.Padding = new Padding(0);
			//button.Padding = buttonLayout.Padding;
			if(ToolbarSetting.ShowText) {
				button.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
			} else {
				button.DisplayStyle = ToolStripItemDisplayStyle.Image;
			}
			//button.Size = buttonLayout.ClientSize;
			button.Tag = item;
			button.Visible = true;
			if(item.LauncherType == LauncherType.File) {
				button.DropDownItems.AddRange(CreateFileLauncherMenuItems(item));
			}
			button.ButtonClick += new EventHandler(button_ButtonClick);
			//button.DropDownItemClicked += new ToolStripItemClickedEventHandler(button_DropDownItemClicked);
			
			return button;
		}


	}
}
