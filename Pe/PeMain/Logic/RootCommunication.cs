/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/05
 * 時刻: 2:01
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using PeMain.Data;

namespace PeMain.Logic
{
	/// <summary>
	/// Description of RootCommunication.
	/// </summary>
	public interface RootCommunication
	{
		void ShowTips(string text);
	
		void ChangeLauncherItems(ToolbarItem toolbarItem, HashSet<LauncherItem> items);
	}
}
