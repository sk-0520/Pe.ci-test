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
using PeMain.Setting;

namespace PeMain.UI
{
	/// <summary>
	/// Description of LauncherItemSelectControl_initialize.
	/// </summary>
	public partial class LauncherItemSelectControl
	{
		void Initialize()
		{
			_items = new List<LauncherItem>();
			
			Language = new PeMain.Setting.Language();
			ItemEdit = true;
			FilterType = LauncherItemSelecterType.Full;
			ResizeInputArea();
		}
	}
}
