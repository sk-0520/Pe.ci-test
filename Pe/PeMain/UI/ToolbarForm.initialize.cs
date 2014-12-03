/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/23
 * 時刻: 13:16
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Windows.Forms;

namespace ContentTypeTextNet.Pe.Application.UI
{
	/// <summary>
	/// Description of ToolbarForm_initialize.
	/// </summary>
	partial class ToolbarForm
	{
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
	}
}
