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
using PeMain.Logic;

namespace PeMain.UI
{
	/// <summary>
	/// Description of LogForm_language.
	/// </summary>
	partial class LogForm
	{
		void ApplyLanguage()
		{
			Debug.Assert(CommonData != null);
			
			//Text = CommonData.Language["window/log"];
			/*
			this.toolLog_save.Text = CommonData.Language["log/command/save"];
			this.toolLog_save.ToolTipText = CommonData.Language["log/tips/save"];
			this.toolLog_clear.Text = CommonData.Language["log/command/clear"];
			this.toolLog_clear.ToolTipText = CommonData.Language["log/tips/save"];
			
			this.listLog_columnTimestamp.Text = CommonData.Language["log/header/timestamp"];
			this.listLog_columnTitle.Text = CommonData.Language["log/header/title"];
			this.listStack_columnFile.Text = CommonData.Language["log/header/file"];
			this.listStack_columnLine.Text = CommonData.Language["log/header/line"];
			this.listStack_columnFunction.Text = CommonData.Language["log/header/method"];
			*/
			UIUtility.SetDefaultText(this, CommonData.Language);
			//Text = CommonData.Language["window/log"];
			
			this.toolLog_save.Text = CommonData.Language["log/command/save"];
			this.toolLog_save.ToolTipText = CommonData.Language["log/tips/save"];
			this.toolLog_clear.Text = CommonData.Language["log/command/clear"];
			this.toolLog_clear.ToolTipText = CommonData.Language["log/tips/clear"];
			
			this.listLog_columnTimestamp.Text = CommonData.Language["log/header/timestamp"];
			this.listLog_columnTitle.Text = CommonData.Language["log/header/title"];
			
			this.listStack_columnFile.Text = CommonData.Language["log/header/method"];
			this.listStack_columnLine.Text = CommonData.Language["log/header/file"];
			this.listStack_columnFunction.Text = CommonData.Language["log/header/title"];
		}
	}
}
