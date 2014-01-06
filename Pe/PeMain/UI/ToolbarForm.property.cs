/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/23
 * 時刻: 13:17
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
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
		
		public bool IsHorizonMode
		{
			get 
			{
				return ToolbarSetting.ToolbarPosition.IsIn(
					ToolbarPosition.DesktopFloat,
					ToolbarPosition.DesktopTop,
					ToolbarPosition.DesktopBottom,
					ToolbarPosition.WindowTop,
					ToolbarPosition.WindowBottom
				);
			}
		}
	}
}
