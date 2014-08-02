/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 01/11/2014
 * 時刻: 00:13
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PeMain.Data;
using PeMain.Logic;

namespace PeMain.UI
{
	/// <summary>
	/// Description of LogForm_variable.
	/// </summary>
	public partial class LogForm
	{
		List<LogItem> _logs = new List<LogItem>();
		ImageList _imageLogType = null;
		FileLogger _fileLogger = null;
	}
}
