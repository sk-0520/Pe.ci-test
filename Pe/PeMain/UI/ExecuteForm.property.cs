/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/13
 * 時刻: 6:00
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.IO;
using PeMain.Data;
using PeMain.Logic;

namespace PeMain.UI
{
	public partial class ExecuteForm
	{
		CommonData CommonData { get; set; }
		LauncherItem LauncherItem { get; set;}

		public LauncherItem EditedLauncherItem { get; private set; }
	}
}
