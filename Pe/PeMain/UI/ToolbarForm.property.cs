/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/23
 * 時刻: 13:17
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

using PeMain.Setting;
using PeUtility;

namespace PeMain.UI
{
	/// <summary>
	/// Description of ToolbarForm_property.
	/// </summary>
	public partial class ToolbarForm
	{
		public ToolbarSetting ToolbarSetting { get; private set; }
		public Language Language { get; private set; }
		public LauncherSetting LauncherSetting { get; private set; }
		
		public bool IsDockingMode { get { return ToolbarSetting.ToolbarPosition.IsIn(ToolbarPosition.DesktopLeft, ToolbarPosition.DesktopTop, ToolbarPosition.DesktopRight, ToolbarPosition.DesktopBottom); } }
		
		public override DockType DockType
		{
			get { return base.DockType; }
			set
			{
				// タイトルバー用のpadding設定
				var paddingSize = SystemInformation.Border3DSize;
				var pos = ToolbarSetting == null ? ToolbarPosition.DesktopFloat: ToolbarSetting.ToolbarPosition;
				var captionRect = GetCaptionBarRect(pos);
				var exPaddingSize = new Size(paddingSize.Width, paddingSize.Height);
				if(IsCaptionSidePos(pos)) {
					exPaddingSize.Width = captionRect.Width + paddingSize.Width;
				} else {
					exPaddingSize.Height = captionRect.Height + paddingSize.Height;
				}
				Padding = new Padding(
					exPaddingSize.Width,
					exPaddingSize.Height,
					paddingSize.Width,
					paddingSize.Height
				);
				
				base.DockType = value;
			}
		}
	}
}
