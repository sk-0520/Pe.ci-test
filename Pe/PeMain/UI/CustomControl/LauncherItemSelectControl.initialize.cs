/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/22
 * 時刻: 16:20
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using PeMain.Data;
using PeSkin;

namespace PeMain.UI
{
	/// <summary>
	/// Description of LauncherItemSelectControl_initialize.
	/// </summary>
	partial class LauncherItemSelectControl
	{
		void InitializeUI()
		{
		}
		
		void Initialize()
		{
			this._items = new List<LauncherItem>();
			
			ItemEdit = true;
			FilterType = LauncherItemSelecterType.Full;
			IconScale = IconScale.Normal;
			
			InitializeUI();
			TuneItemHeight();
			ResizeInputArea();
		}
	}
}
