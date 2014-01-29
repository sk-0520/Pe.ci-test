/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/13
 * 時刻: 5:59
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PeMain.UI
{
	public partial class StreamForm
	{
		void ApplyLanguage()
		{
			Debug.Assert(Language != null);
			
			var map = new Dictionary<string, string>() {
				{ "ITEM", LauncherItem.Name },
			};
			
			Text = Language["window/stream", map];
		}
	}
}
