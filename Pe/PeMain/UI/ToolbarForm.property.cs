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
using ContentTypeTextNet.Pe.Application.Data;
using ContentTypeTextNet.Pe.Library.Utility;

namespace ContentTypeTextNet.Pe.Application.UI
{
	/// <summary>
	/// Description of ToolbarForm_property.
	/// </summary>
	partial class ToolbarForm
	{
		CommonData CommonData { get; set; }
		
		ToolbarGroupItem SelectedGroupItem { get; set; }
		public ToolbarItem UseToolbarItem { get; private set; }
		
		override public DesktopDockType DesktopDockType
		{
			get { return base.DesktopDockType; }
			set
			{
				if(CommonData != null ) {
					var pos = UseToolbarItem.ToolbarPosition;
					
					Padding = CommonData.Skin.GetToolbarTotalPadding(UseToolbarItem.ToolbarPosition, Size);
					if(this.toolLauncher != null) {
						if(ToolbarPositionUtility.IsHorizonMode(pos)) {
							this.toolLauncher.LayoutStyle =  ToolStripLayoutStyle.HorizontalStackWithOverflow;
						} else {
							this.toolLauncher.LayoutStyle =  ToolStripLayoutStyle.VerticalStackWithOverflow;
						}
					}
				}
				base.DesktopDockType = value;
			}
		}
	}
}
