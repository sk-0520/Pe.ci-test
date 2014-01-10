/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 01/11/2014
 * 時刻: 00:16
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using PeMain.Data;

namespace PeMain.UI
{
	/// <summary>
	/// Description of LogForm.
	/// </summary>
	public partial class LogForm
	{
		void Initialize(IEnumerable<LogItem> initLog)
		{
			this._logs.AddRange(initLog);
		}
	}
}
