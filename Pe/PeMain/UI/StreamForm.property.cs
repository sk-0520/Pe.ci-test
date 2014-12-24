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
using ContentTypeTextNet.Pe.PeMain.Data;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	partial class StreamForm
	{
		CommonData CommonData { get; set; }
		Process Process { get; set;}
		public LauncherItem LauncherItem { get; private set; }
		public bool ProcessRunning { get { return Process != null && !Process.HasExited; } }
	}
}
