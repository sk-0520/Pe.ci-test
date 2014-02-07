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
			Debug.Assert(CommonData != null);
			Debug.Assert(CommonData.Language != null);
			
			var map = new Dictionary<string, string>() {
				{ "ITEM", LauncherItem.Name },
			};
			
			Text = CommonData.Language["window/stream", map];
			
			this.pageStream.Text = CommonData.Language["stream/tab/stream"];
			this.pageProcess.Text = CommonData.Language["stream/tab/process"];
			this.pageProperty.Text = CommonData.Language["stream/tab/property"];
			
			this.toolStream_save.Text = CommonData.Language["stream/command/save"];
			this.toolStream_save.ToolTipText = CommonData.Language["stream/tips/save"];
			this.toolStream_clear.Text = CommonData.Language["stream/command/clear"];
			this.toolStream_clear.ToolTipText = CommonData.Language["stream/tips/clear"];
			this.toolStream_refresh.Text = CommonData.Language["stream/tips/refesh"];
			this.toolStream_kill.Text = CommonData.Language["stream/tips/kill"];
		}
	}
}
