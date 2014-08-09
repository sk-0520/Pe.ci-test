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
	public partial class LogForm
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
			DialogUtility.SetDefaultText(this, CommonData.Language);
			
			this.toolLog_save.SetLanguage(CommonData.Language);
			this.toolLog_clear.SetLanguage(CommonData.Language);
			
			this.listLog_columnTimestamp.SetLanguage(CommonData.Language);
			this.listLog_columnTitle.SetLanguage(CommonData.Language);
			
			this.listStack_columnFile.SetLanguage(CommonData.Language);
			this.listStack_columnLine.SetLanguage(CommonData.Language);
			this.listStack_columnFunction.SetLanguage(CommonData.Language);
		}
	}
}
