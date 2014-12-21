/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/26
 * 時刻: 20:24
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	partial class App
	{
		/// <summary>
		/// Description of Pe_MessageWindow_property.
		/// </summary>
		partial class MessageWindow
		{
			CommonData CommonData { get; set; }
			public ILogger StartupLogger { get; set; }
			//IntPtr NextWndHandle { get; set; }
			bool ClipboardRegisted { get; set; }
		}
	}
}
