/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/23
 * 時刻: 13:17
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Windows.Forms;
using PeMain.Data;
using PeMain.Logic;
using PeUtility;

namespace PeMain.UI
{
	/// <summary>
	/// Description of ToolbarForm_property.
	/// </summary>
	public partial class ToolbarForm
	{
		public ILogger Logger { get; set; }
		
		Language Language { get; set; }
		
		MainSetting MainSetting { get; set; }
		
		ToolbarGroupItem SelectedGroupItem { get; set; }
		ToolbarItem UseToolbarItem { get; set; }
		public Screen ParentScreen { get; set; }
		
		public bool IsDockingMode { get { return UseToolbarItem.ToolbarPosition.IsIn(ToolbarPosition.DesktopLeft, ToolbarPosition.DesktopTop, ToolbarPosition.DesktopRight, ToolbarPosition.DesktopBottom); } }
		
		override public DesktopDockType DesktopDockType
		{
			get { return base.DesktopDockType; }
			set 
			{
				var pos = ToolbarPosition.DesktopFloat;
				if(MainSetting != null) {
					pos = UseToolbarItem.ToolbarPosition;
				}
				
				SetPaddingArea(pos);
				if(this.toolLauncher != null) {
					if(IsHorizonMode(pos)) {
						this.toolLauncher.LayoutStyle =  ToolStripLayoutStyle.HorizontalStackWithOverflow;
					} else {
						this.toolLauncher.LayoutStyle =  ToolStripLayoutStyle.VerticalStackWithOverflow;
					}
				}
				base.DesktopDockType = value;
			}
		}

	}
}
