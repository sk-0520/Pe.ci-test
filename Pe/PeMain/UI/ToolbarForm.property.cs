/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/23
 * 時刻: 13:17
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
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
		ToolbarSetting ToolbarSetting { get { return this._mainSetting != null ? this._mainSetting.Toolbar: null; } }
		
		public bool IsDockingMode { get { return ToolbarSetting.ToolbarPosition.IsIn(ToolbarPosition.DesktopLeft, ToolbarPosition.DesktopTop, ToolbarPosition.DesktopRight, ToolbarPosition.DesktopBottom); } }
		
		override public DockType DockType
		{
			get { return base.DockType; }
			set 
			{
				var pos = ToolbarPosition.DesktopFloat;
				if(ToolbarSetting != null) {
					pos = ToolbarSetting.ToolbarPosition;
				}
				
				SetPaddingArea(pos);
				base.DockType = value;
			}
		}

	}
}
