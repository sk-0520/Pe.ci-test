/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 01/11/2014
 * 時刻: 00:27
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;

namespace PeMain.UI
{
	/// <summary>
	/// Description of LogForm_language.
	/// </summary>
	public partial class LogForm
	{
		void ApplyLanguage()
		{
			Debug.Assert(Language != null);
			
			Text = Language["window/log"];
			
			this.toolLog_save.Text = Language["log/command/save"];
			this.toolLog_save.ToolTipText = Language["log/tips/save"];
			this.toolLog_clear.Text = Language["log/command/clear"];
			this.toolLog_clear.ToolTipText = Language["log/tips/save"];
			
			this.headerTimestamp.Text = Language["log/header/timestamp"];
			this.headerTitle.Text = Language["log/header/title"];
			this.headerFile.Text = Language["log/header/file"];
			this.headerLine.Text = Language["log/header/line"];
			this.headerFunction.Text = Language["log/header/method"];
		}
	}
}
