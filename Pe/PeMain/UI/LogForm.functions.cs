/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 01/11/2014
 * 時刻: 00:17
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Windows.Forms;
using PeMain.Data;

namespace PeMain.UI
{
	/// <summary>
	/// Description of LogForm_functions.
	/// </summary>
	public partial class LogForm
	{
		public void Puts(LogType logType, string title, object detail, int frame = 2)
		{
			var logItem = new LogItem(logType, title, detail, frame);
			this._logs.Add(logItem);
			this.listLog.VirtualListSize = this._logs.Count;
			this.listLog.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			
			if(LogSetting.ShowAdd && !Visible) {
				Visible = true;
			}
		}
		
		public void SetSettingData(Language language, MainSetting mainSetting)
		{
			Language = language;
			LogSetting = mainSetting.Log;
			
			ApplySetting();
		}
		
		
		
		void ApplySetting()
		{
			Debug.Assert(LogSetting != null);
			
			ApplyLanguage();
		}

	}
}
