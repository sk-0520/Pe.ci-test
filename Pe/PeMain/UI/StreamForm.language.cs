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
			
			this.pageStream.Text = Language["stream/tab/stream"];
			this.pageProcess.Text = Language["stream/tab/process"];
			this.pageProperty.Text = Language["stream/tab/property"];
			
			this.toolStream_save.Text = Language["stream/command/save"];
			this.toolStream_save.ToolTipText = Language["stream/tips/save"];
			this.toolStream_clear.Text = Language["stream/command/clear"];
			this.toolStream_clear.ToolTipText = Language["stream/tips/clear"];
			this.toolStream_refresh.Text = Language["stream/tips/refesh"];
			this.toolStream_kill.Text = Language["stream/tips/kill"];
		}
	}
}
