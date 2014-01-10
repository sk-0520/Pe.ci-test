/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 01/11/2014
 * 時刻: 00:14
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using PeMain.Data;

namespace PeMain.Logic
{
	/// <summary>
	/// Description of ILogger.
	/// </summary>
	public interface ILogger
	{
		void Logging(LogType logType, string title, string detail, int frame = 2);
	}
}
